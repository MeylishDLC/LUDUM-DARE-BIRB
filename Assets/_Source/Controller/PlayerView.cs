using System;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        private static readonly int IsFlyingUp = Animator.StringToHash("IsFlyingUp");
        private static readonly int Die = Animator.StringToHash("Die");
        
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Rigidbody2D _rb;
        public event Action<Collider2D> OnTriggered;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggered?.Invoke(other);
        }
        public void SetVelocity(Vector2 velocity)
        {
            _rb.velocity = velocity;
        }
        public Vector2 GetVelocity()
        {
            return _rb.velocity;
        }
        public void ApplyGravity(float gravity, float maxFallSpeed)
        {
            if (_rb.velocity.y > -maxFallSpeed)
            {
                _rb.velocity += Vector2.down * (gravity * Time.fixedDeltaTime);
            }
        }
        public void UpdateAnimations(Vector2 velocity)
        {
            var isFlyingUp = velocity.y > 0.1f;
            animator.SetBool(IsFlyingUp, isFlyingUp);
        }
        public void UpdateFacing(float horizontalInput)
        {
            if (horizontalInput > 0.05f)
            {
                spriteRenderer.flipX = true;
            }
            else if (horizontalInput < -0.05f)
            {
                spriteRenderer.flipX = false;
            }
        }
        public void PlayDeathAnimation()
        {
            animator.SetTrigger(Die);
        }
    }
}