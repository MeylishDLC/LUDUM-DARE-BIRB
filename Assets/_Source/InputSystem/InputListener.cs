using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
        public event Action OnJumpStarted;
        public event Action OnJumpEnded;

        private Controls _controls;
        private InputAction _jumpAction;
        private InputAction _moveAction;

        private void Awake()
        {
            _controls = new Controls();
            SetupActions();
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
            _jumpAction.canceled += OnJumpButtonReleased;
            
            _moveAction = _controls.Player.Move; 
        }
        private void OnJumpButtonPressed(InputAction.CallbackContext context)
        {
            OnJumpStarted?.Invoke();
        }
        private void OnJumpButtonReleased(InputAction.CallbackContext context)
        {
            OnJumpEnded?.Invoke();
        }
        public Vector2 GetMovementValue()
        {
            return _moveAction.ReadValue<Vector2>();
        }
        private void CleanUp()
        {
            _jumpAction.started -= OnJumpButtonPressed;
            _jumpAction.canceled -= OnJumpButtonReleased;
        }
    }
}
