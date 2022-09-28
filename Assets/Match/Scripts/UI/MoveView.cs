using Assets.Match.Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Assets.Match.Scripts.UI
{
    public class MoveView : MonoBehaviour       
    {
        [SerializeField] private TextMeshProUGUI _move;
        [SerializeField] private MoveController _moveController;

        private void OnEnable()
        {
            _moveController.MovesChange += MovementView;
        }

        private void OnDisable()
        {
            _moveController.MovesChange -= MovementView;
        }

        private void Start()
        {
            MovementView();
        }

        public void MovementView()
        {
            _move.text = _moveController.TotalMove.ToString();
        }
    }
}
