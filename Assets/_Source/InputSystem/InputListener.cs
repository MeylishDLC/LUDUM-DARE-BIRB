using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
        public event Action OnJumpStarted;
        public event Action OnJumpEnded;
        public Observable<Unit> JumpStartedStream => _jumpStartedSubject;
        public Observable<Unit> JumpEndedStream => _jumpEndedSubject;

        private Controls _controls;
        private InputAction _jumpAction;
        private InputAction _moveAction;
        private readonly Subject<Unit> _jumpStartedSubject = new();
        private readonly Subject<Unit> _jumpEndedSubject = new();

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
            DisableInput();
        }
        private void OnDestroy()
        {
            CleanUp();
            _jumpStartedSubject.Dispose();
            _jumpEndedSubject.Dispose();
        }
        public Vector2 GetMovementValue()
        {
            return _moveAction.ReadValue<Vector2>();
        }
        public void DisableInput()
        {
            _moveAction.Disable();
            _jumpAction.Disable();
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
            _jumpStartedSubject.OnNext(Unit.Default);
            OnJumpStarted?.Invoke();
        }
        private void OnJumpButtonReleased(InputAction.CallbackContext context)
        {
            _jumpEndedSubject.OnNext(Unit.Default);
            OnJumpEnded?.Invoke();
        }
       
        private void CleanUp()
        {
            _jumpAction.started -= OnJumpButtonPressed;
            _jumpAction.canceled -= OnJumpButtonReleased;
        }
    }
}
