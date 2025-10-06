using System;
using Cinemachine;
using InputSystem;
using SoundSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnPlayerDeath;
        
        [SerializeField] private PlayerControllerConfig config;
        [SerializeField] private PlayerView view; 
        [SerializeField] private LayerMask obstacleMask;

        private PlayerModel _model;
        private InputListener _inputListener;
        private SoundManager _soundManager;
        private CinemachineVirtualCamera _vcam;
        
        private float _jumpHoldTimer;
        private bool _isHoldingJump;
        private bool _isDying;

        [Inject]
        public void Initialize(InputListener inputListener, CinemachineVirtualCamera vcam, SoundManager soundManager)
        {
            _inputListener = inputListener;
            _vcam = vcam;
            _soundManager = soundManager;
        }
        private void Awake()
        {
            _model = new PlayerModel();
        }
        private void OnEnable()
        {
            _inputListener.OnJumpStarted += PlayJumpSound;
            _inputListener.OnJumpStarted += HandleJumpStarted;
            _inputListener.OnJumpEnded += HandleJumpEnded;
            view.OnTriggered += HandleCollision;
        }
        private void OnDisable()
        {
            _inputListener.OnJumpStarted -= PlayJumpSound;
            _inputListener.OnJumpStarted -= HandleJumpStarted;
            _inputListener.OnJumpEnded -= HandleJumpEnded;
            view.OnTriggered -= HandleCollision;
        }
        private void Update()
        {
            HandleMovement();

            if (_isHoldingJump)
            {
                _jumpHoldTimer += Time.deltaTime;
                if (_jumpHoldTimer >= config.MaxJumpHoldTime)
                {
                    _isHoldingJump = false;
                }
            }
            
            view.UpdateAnimations(view.GetVelocity());
            view.UpdateFacing(_inputListener.GetMovementValue().x);
        }
        private void FixedUpdate()
        {
            ApplyPhysics();
        }
        private void HandleMovement()
        {
            var targetSpeed = _inputListener.GetMovementValue().x * config.HorizontalMoveSpeed;
            var currentSpeed = view.GetVelocity().x;

            var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? config.Acceleration : config.Deceleration;

            var newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelRate * Time.deltaTime);

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

            var jumpForce = _model.CalculateJumpForce(config.JumpForce, Time.time);
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
            OnPlayerDeath?.Invoke();
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