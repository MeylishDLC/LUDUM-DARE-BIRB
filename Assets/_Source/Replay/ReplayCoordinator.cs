using System;
using Controller;
using InputSystem;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Replay
{
    [DefaultExecutionOrder(-200)]
    public sealed class ReplayCoordinator : MonoBehaviour
    {
        public static ReplayData LastReplay { get; private set; }

        private const int TickRateHz = 50;
        private const int MaxDurationSeconds = 30;
        private static int MaxTicks => TickRateHz * MaxDurationSeconds;


        [SerializeField] private bool showUiOverlay = true;

        private PlayerController _player;
        private InputListener _input;
        private IRng _rng;
        private ReplayReadState _readState;

        private ReplayRecorder _recorder;
        private int _completedTick;
        private bool _isReplaying;
        private bool _recording;
        private bool _selectDemoRecorded;

        private ReplayData _activeReplay;
        private int _commandCursor;

        private IDisposable _jumpStartSub;
        private IDisposable _jumpEndSub;

        [Inject]
        private void Construct(PlayerController player, InputListener input, IRng rng, ReplayReadState readState)
        {
            _player = player;
            _input = input;
            _rng = rng;
            _readState = readState;
        }

        private void Awake()
        {
            Time.fixedDeltaTime = 1f / TickRateHz;
            _recorder = new ReplayRecorder(_rng);
        }

        private void Start()
        {
            if (ReplaySession.TryConsumePendingReplay(out var data))
            {
                BeginReplay(data);
            }
        }

        private void OnDestroy()
        {
            _jumpStartSub?.Dispose();
            _jumpEndSub?.Dispose();
        }

        private void FixedUpdate()
        {
            _completedTick++;

            if (_recording)
            {
                RecordMoveSample();
                RecordSelectDemoOnce();

                if (_completedTick >= MaxTicks)
                {
                    StopRecordingInternal();
                }
            }

            if (_isReplaying && _activeReplay != null)
            {
                ApplyCommandsForTick(_completedTick);
            }

            if (_isReplaying)
            {
                _readState.IsReplaying = true;
            }
            else
            {
                _readState.IsReplaying = false;
                _readState.ReplayMoveX = 0f;
            }
        }

        private void RecordMoveSample()
        {
            var x = _input.GetMovementValue().x;
            _recorder.Record(new ReplayCommand
            {
                Tick = _completedTick,
                Type = ReplayCommandType.Move,
                MoveX = x
            });
        }

        private void RecordSelectDemoOnce()
        {
            if (_selectDemoRecorded || _completedTick != 1)
            {
                return;
            }

            _selectDemoRecorded = true;
            _recorder.Record(new ReplayCommand
            {
                Tick = _completedTick,
                Type = ReplayCommandType.Select,
                SelectIndex = 0
            });
        }

        private void RecordJumpStart()
        {
            var tick = _completedTick + 1;
            _recorder.Record(new ReplayCommand
            {
                Tick = tick,
                Type = ReplayCommandType.Action,
                ActionKind = ReplayActionKind.JumpStarted
            });
        }

        private void RecordJumpEnd()
        {
            var tick = _completedTick + 1;
            _recorder.Record(new ReplayCommand
            {
                Tick = tick,
                Type = ReplayCommandType.Action,
                ActionKind = ReplayActionKind.JumpEnded
            });
        }

        private void BeginReplay(ReplayData data)
        {
            _activeReplay = data;
            _commandCursor = 0;
            _isReplaying = true;
            _readState.IsReplaying = true;
            _readState.ReplayMoveX = 0f;
            _input.DisableInput();
        }

        private void ApplyCommandsForTick(int tick)
        {
            var commands = _activeReplay.Commands;

            while (_commandCursor < commands.Count && commands[_commandCursor].Tick < tick)
            {
                var t = commands[_commandCursor].Tick;
                while (_commandCursor < commands.Count && commands[_commandCursor].Tick == t)
                {
                    ApplyOneCommand(commands[_commandCursor]);
                    _commandCursor++;
                }
            }

            while (_commandCursor < commands.Count && commands[_commandCursor].Tick == tick)
            {
                ApplyOneCommand(commands[_commandCursor]);
                _commandCursor++;
            }
        }

        private void ApplyOneCommand(in ReplayCommand c)
        {
            switch (c.Type)
            {
                case ReplayCommandType.Move:
                    _readState.ReplayMoveX = c.MoveX;
                    break;
                case ReplayCommandType.Action:
                    switch (c.ActionKind)
                    {
                        case ReplayActionKind.JumpStarted:
                            _player.ReplayApplyJumpStart();
                            break;
                        case ReplayActionKind.JumpEnded:
                            _player.ReplayApplyJumpEnd();
                            break;
                    }
                    break;
                case ReplayCommandType.Select:
                    break;
            }
        }

        public void RecordStart()
        {
            if (_isReplaying)
            {
                return;
            }

            StopRecordingInternal();
            _completedTick = 0;
            _selectDemoRecorded = false;
            _recorder.StartRecording(TickRateHz, MaxTicks);
            _recording = true;
            _jumpStartSub?.Dispose();
            _jumpEndSub?.Dispose();
            _jumpStartSub = _input.JumpStartedStream.Subscribe(_ => RecordJumpStart());
            _jumpEndSub = _input.JumpEndedStream.Subscribe(_ => RecordJumpEnd());
        }

        public void RecordStop()
        {
            StopRecordingInternal();
        }

        private void StopRecordingInternal()
        {
            if (!_recording)
            {
                return;
            }

            _recording = false;
            _jumpStartSub?.Dispose();
            _jumpEndSub?.Dispose();
            _jumpStartSub = null;
            _jumpEndSub = null;
            _recorder.StopRecording();
            LastReplay = _recorder.BuildReplayData();
        }

        public void ReplayPlayLast()
        {
            if (LastReplay == null || LastReplay.Commands.Count == 0)
            {
                Debug.LogWarning("Replay:: No replay recorded.");
                return;
            }

            _recording = false;
            _jumpStartSub?.Dispose();
            _jumpEndSub?.Dispose();
            _jumpStartSub = null;
            _jumpEndSub = null;

            ReplaySession.SetPendingReplayPlayback(LastReplay, LastReplay.Seed);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnGUI()
        {
            if (!showUiOverlay)
            {
                return;
            }
            const int w = 160;
            const int h = 28;
            var y = 10f;
            if (GUI.Button(new Rect(10, y, w, h), "record_start"))
            {
                RecordStart();
            }

            y += h + 4;
            if (GUI.Button(new Rect(10, y, w, h), "record_stop"))
            {
                RecordStop();
            }

            y += h + 4;
            if (GUI.Button(new Rect(10, y, w, h), "replay_play_last"))
            {
                ReplayPlayLast();
            }
        }
    }
}
