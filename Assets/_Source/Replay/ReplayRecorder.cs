using System.Collections.Generic;

namespace Replay
{
    public sealed class ReplayRecorder
    {
        private readonly List<ReplayCommand> _commands = new();
        private readonly IRng _rng;

        private bool _recording;
        private int _headerSeed;
        private int _tickRateHz;
        private int _maxTicks;

        public ReplayRecorder(IRng rng)
        {
            _rng = rng;
        }

        public bool IsRecording => _recording;

        public void StartRecording(int tickRateHz, int maxRecordedTicks)
        {
            _commands.Clear();
            _headerSeed = _rng.InitialSeed;
            _tickRateHz = tickRateHz;
            _maxTicks = maxRecordedTicks;
            _recording = true;
        }

        public void StopRecording()
        {
            _recording = false;
        }

        public void Record(in ReplayCommand command)
        {
            if (!_recording)
            {
                return;
            }

            _commands.Add(command);
        }

        public ReplayData BuildReplayData()
        {
            var data = new ReplayData
            {
                Seed = _headerSeed,
                TickRateHz = _tickRateHz,
                MaxRecordedTicks = _maxTicks
            };

            _commands.Sort((a, b) => a.Tick.CompareTo(b.Tick));
            data.Commands.AddRange(_commands);
            return data;
        }
    }
}
