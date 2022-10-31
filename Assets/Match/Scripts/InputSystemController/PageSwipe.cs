using UnityEngine;
using UnityEngine.UI;

namespace Assets.Match.Scripts.InputSystemController
{

    public class PageSwipe : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        private InputManager _inputManager;
        private Vector3 _panelPosition;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private const float _minimumDistance = 0.01f;      

        private void Awake()
        {
            _inputManager = InputManager.Instance;
            _panelPosition = transform.position;
        }

        private void OnEnable()
        {
            _inputManager.OnStartTouch += SwipeStart;
            _inputManager.OnEndTouch += SwipeEnd;
            _scrollRect.onValueChanged.AddListener(DetectSwipe);
        }

        private void SwipeStart(Vector2 position)
        {
            _startPosition = position;
        }

        private void SwipeEnd(Vector2 position)
        {
            _endPosition = position;
        }

        private void DetectSwipe(Vector2 value)
        {
            if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance)
            {
                float difference = _endPosition.y - _startPosition.y;
                float newPosition = _panelPosition.y + difference;
                value = new Vector2(0, newPosition);
            }
        }

        private void OnDisable()
        {
            _inputManager.OnStartTouch -= SwipeStart;
            _inputManager.OnEndTouch -= SwipeEnd;
            _scrollRect.onValueChanged.RemoveListener(DetectSwipe);
        }
    }
}
