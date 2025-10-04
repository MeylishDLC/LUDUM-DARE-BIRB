using InputSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerControllerConfig config;
        [SerializeField] private PlayerView view; 
        [SerializeField] private LayerMask groundMask;

        private PlayerModel _model;
        private InputListener _inputListener;
        
        private float _jumpHoldTimer;
        private bool _isHoldingJump;

        [Inject]
        public void Initialize(InputListener inputListener)
        {
            _inputListener = inputListener;
        }
        private void Awake()
        {
            _model = new PlayerModel();
        }
        private void OnEnable()
        {
            _inputListener.OnJumpStarted += HandleJumpStarted;
            _inputListener.OnJumpEnded += HandleJumpEnded;
        }
        private void OnDisable()
        {
            _inputListener.OnJumpStarted -= HandleJumpStarted;
            _inputListener.OnJumpEnded -= HandleJumpEnded;
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
            if (_isHoldingJump)
            {
                var t = Mathf.Clamp01(_jumpHoldTimer / config.MaxJumpHoldTime);
                var extraForce = Mathf.Lerp(config.MinJumpForce, config.JumpForce, t);

                var velocity = view.GetVelocity();
                if (velocity.y > 0)
                {
                    velocity.y += extraForce * 0.5f;
                    view.SetVelocity(velocity);
                }
            }
            _isHoldingJump = false;
        }
    }
}