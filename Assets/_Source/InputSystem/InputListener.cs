using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
        public event Action OnJumpStarted;

        private Controls _controls;
        private InputAction _jumpAction;
        private InputAction _moveAction;

        private void Awake()
        {
            _controls = new Controls();
            SetupActions();
        }
        private void Update()
        {
            var dir = GetMovementValue();
            if (dir.x > 0)
            {
                Debug.Log($"Move started: moving right.");
            }
            else if (dir.x < 0)
            {
                Debug.Log($"Move started: moving left.");
            }
        }

        private void OnEnable()
        {
            _jumpAction.Enable();
            _moveAction.Enable();
        }
        private void OnDisable()
        {
            _jumpAction.Disable();
            _moveAction.Disable();
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        private void SetupActions()
        {
            _jumpAction = _controls.Player.Jump;
            _jumpAction.started += OnJumpButtonPressed;
            
            _moveAction = _controls.Player.Move; 
        }
        private void OnJumpButtonPressed(InputAction.CallbackContext context)
        {
            OnJumpStarted?.Invoke();
            Debug.Log("Jump pressed");
        }
        public Vector2 GetMovementValue()
        {
            return _moveAction.ReadValue<Vector2>();
        }
        private void CleanUp()
        {
            _jumpAction.started -= OnJumpButtonPressed;
        }
    }
}
