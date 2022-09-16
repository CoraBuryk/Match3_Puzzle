using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Match.Scripts.InputSystemController
{
    public class InputManager : Singleton<InputManager>
    {
        #region Events
        public delegate void StartTouch(Vector2 position);
        public event StartTouch OnStartTouch;
        public delegate void EndTouch(Vector2 position);
        public event StartTouch OnEndTouch;

        [Serializable] public class PressEvent : UnityEvent<bool> { }
        public PressEvent OnPress;

        #endregion

        private PlayerControls _playerControls;
        private Camera _mainCamera;

        private Vector2 _startPosition;
        private Vector2 _endPosition;

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

        private void OnDisable()
        {
            _playerControls.Disable();
            OnStartTouch -= SwipeStart;
            OnEndTouch -= SwipeEnd;
        }

        private void Start()
        {
            _playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
            _playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
        }

        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnStartTouch != null)                
            {
                OnStartTouch(Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()));                
            }
        }

        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnEndTouch != null)
            {
                OnEndTouch(Utils.ScreenToWorld(_mainCamera, _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()));                
            }
        }

        public Vector2 PressLocation
        {
            get { return _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(); }            
        }

        private void SwipeStart(Vector2 position)
        {
            position = _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
            _startPosition = position;          
            OnPress.Invoke(true);
        }

        private void SwipeEnd(Vector2 position)
        {
            position = _playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
            _endPosition = position;
            OnPress.Invoke(false);
        }
    }
}
