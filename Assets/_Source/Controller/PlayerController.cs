using System;
using Cinemachine;
using InputSystem;
using R3;
using Replay;
using SoundSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnPlayerDeath;
        public Observable<Unit> PlayerDeathStream => _playerDeathSubject;
        
        [SerializeField] private PlayerControllerConfig config;
        [SerializeField] private PlayerView view; 
        [SerializeField] private LayerMask obstacleMask;

        private PlayerModel _model;
        private InputListener _inputListener;
        private IReplayReadState _replayRead;
        private SoundManager _soundManager;
        private CinemachineVirtualCamera _vcam;
        private readonly Subject<Unit> _playerDeathSubject = new();
        
        private float _jumpHoldTimer;
        private bool _isHoldingJump;
        private bool _isDying;
        private float _simTime;
        private IDisposable _jumpStartedSubscription;
        private IDisposable _jumpStartedSoundSubscription;
        private IDisposable _jumpEndedSubscription;

        [Inject]
        public void Initialize(InputListener inputListener, CinemachineVirtualCamera vcam, SoundManager soundManager,
            IReplayReadState replayRead)
        {
            _inputListener = inputListener;
            _vcam = vcam;
            _soundManager = soundManager;
            _replayRead = replayRead;
        }
        private void Awake()
        {
            _model = new PlayerModel();
        }
        private void OnEnable()
        {
            if (!ReplaySession.HasPendingReplay)
            {
                _jumpStartedSoundSubscription = _inputListener.JumpStartedStream.Subscribe(_ => PlayJumpSound());
                _jumpStartedSubscription = _inputListener.JumpStartedStream.Subscribe(_ => HandleJumpStarted());
                _jumpEndedSubscription = _inputListener.JumpEndedStream.Subscribe(_ => HandleJumpEnded());
            }

            view.OnTriggered += HandleCollision;
        }
        private void OnDisable()
        {
            _jumpStartedSoundSubscription?.Dispose();
            _jumpStartedSubscription?.Dispose();
            _jumpEndedSubscription?.Dispose();
            _jumpStartedSoundSubscription = null;
            _jumpStartedSubscription = null;
            _jumpEndedSubscription = null;
            view.OnTriggered -= HandleCollision;
        }
        private void OnDestroy()
        {
            _playerDeathSubject.Dispose();
        }
        private void Update()
        {
            view.UpdateAnimations(view.GetVelocity());
            view.UpdateFacing(GetMovementXForFrame());
        }
        private void FixedUpdate()
        {
            _simTime += Time.fixedDeltaTime;

            HandleMovementFixed();

            if (_isHoldingJump)
            {
                _jumpHoldTimer += Time.fixedDeltaTime;
                if (_jumpHoldTimer >= config.MaxJumpHoldTime)
                {
                    _isHoldingJump = false;
                }
            }

            ApplyPhysics();
        }
        private float GetMovementXForFrame()
        {
            return _replayRead.IsReplaying ? _replayRead.ReplayMoveX : _inputListener.GetMovementValue().x;
        }

        private void HandleMovementFixed()
        {
            var targetSpeed = GetMovementXForFrame() * config.HorizontalMoveSpeed;
            var currentSpeed = view.GetVelocity().x;

            var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? config.Acceleration : config.Deceleration;

            var newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelRate * Time.fixedDeltaTime);

            var velocity = view.GetVelocity();
            velocity.x = newSpeed;
            view.SetVelocity(velocity);
        }
        private void ApplyPhysics()
        {
            view.ApplyGravity(config.Gravity, config.MaxFallSpeed);
        }
        private void HandleJumpStarted()
        {
            _jumpHoldTimer = 0f;
            _isHoldingJump = true;

            var jumpForce = _model.CalculateJumpForce(config.JumpForce, _simTime);
            var velocity = view.GetVelocity();
            velocity.y = jumpForce;
            view.SetVelocity(velocity);
        }
        private void HandleJumpEnded()
        {
            _isHoldingJump = false;

            var velocity = view.GetVelocity();
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
                view.SetVelocity(velocity);
            }
        }
        private void HandleCollision(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & obstacleMask.value) != 0)
            {
                Die();
            }
        }
        private void Die()
        {
            if (_isDying)
            {
                return;
            }
            _isDying = true;
            PlayHitSound();
            _inputListener.DisableInput();
            view.PlayDeathAnimation();
            _vcam.Follow = null;

            var velocity = view.GetVelocity();
            if (velocity.y > 0)
            { 
                velocity.y = 0;
            }

            velocity.y -= 10f;
            view.SetVelocity(velocity);

            view.SetGravityMultiplier(2f);
            _playerDeathSubject.OnNext(Unit.Default);
            OnPlayerDeath?.Invoke();
        }

        public void ReplayApplyJumpStart()
        {
            PlayJumpSound();
            HandleJumpStarted();
        }

        public void ReplayApplyJumpEnd()
        {
            HandleJumpEnded();
        }

        private void PlayJumpSound()
        {
            _soundManager.PlayOneShot(_soundManager.FmodEventsConfig.JumpSound);
        }
        private void PlayHitSound()
        {
            _soundManager.PlayOneShot(_soundManager.FmodEventsConfig.StickHitSound);
        }
    }
}