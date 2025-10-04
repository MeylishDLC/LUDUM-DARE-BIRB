using System;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        private Rigidbody2D _rb;
        public event Action<Collider2D> OnTriggered;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
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
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggered?.Invoke(other);
        }
    }
}