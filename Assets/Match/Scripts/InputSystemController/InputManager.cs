using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace Assets.Match.Scripts.InputSystemController
{

    public class InputManager : Singleton<InputManager>
    {

#region Events
        [Serializable] public class PressEvent : UnityEvent<bool> { }
        public PressEvent OnPress;

        public delegate void StartTouch(Vector2 position);
        public event StartTouch OnStartTouch;

        public delegate void EndTouch(Vector2 position);
        public event StartTouch OnEndTouch;

#endregion

        private PlayerControls _playerControls;
        private Camera _mainCamera;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _playerControls.Enable();
            OnStartTouch += SwipeStart;
            OnEndTouch += SwipeEnd;
        }

        private void Start()
        {
            _playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
            _playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
        }

        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            OnStartTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()));
        }

        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            OnEndTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()));
        }

        public Vector2 PressLocation
        {
            get { return _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(); }            
        }

        private void SwipeStart(Vector2 position)
        {
            position = PressLocation;
            OnPress.Invoke(true);
        }

        private void SwipeEnd(Vector2 position)
        {
            position = PressLocation;
            OnPress.Invoke(false);
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            OnStartTouch -= SwipeStart;
            OnEndTouch -= SwipeEnd;
        }

    }
}
