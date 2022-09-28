using Assets.Match.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Match.Scripts.Gameplay
{
    public class BonusController : MonoBehaviour
    {
        [SerializeField] private Button _bombButton;
        [SerializeField] private Button _rocketButton;


        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject rocketPrefab;


        public bool isBombClicked;

        public bool isRocketClicked;

        private void OnEnable()
        {            
            _bombButton.onClick.AddListener(CheckBombButton);
            _rocketButton.onClick.AddListener(CheckRocketButton);
        }

        private void OnDisable()
        {
            _bombButton.onClick.RemoveListener(CheckBombButton);
            _rocketButton.onClick.RemoveListener(CheckRocketButton);
        }

        void GetBonus(int match)
        {
            if (match < 5)
            {
                return;
            }
            if (match >= 5 && match <= 7)
            {
                // анимация ракеты
                // счет бонусов +1
            }
            if (match >= 5 && match <= 7)
            {
                // анимация бомбы
                // счет бонусов +1
            }
        }

        private void CheckBombButton()
        {
            isBombClicked = true;
        }
        private void CheckRocketButton()
        {
            isRocketClicked = true;
        }



        //void ActivateBonusBomb(GameObject bomb)
        //{
        //    int x = (int)bomb.transform.position.x;
        //    int y = (int)bomb.transform.position.y;

        //    if (IsWithinBounds(boardManager._tiles, x, y))
        //    {
        //        boardManager._tiles[x, y] = bomb.GetComponent<Tile>();
        //    }


        //    //задержка
        //    //анимация
        //    //уничтожение блоков на 5х5
        //}

        void ActivateBonusRocet()
        {
            //нацепить на кнопку
            //spawn в рандомном месте
            //задержка
            //анимация
            //уничтожение блоков  горизонтально
        }

        //public Bomb SpawnBomb()
        //{
        //    GameObject newBomb = GameObject.Instantiate(bombPrefab, transform);
        //    return (newBomb.GetComponent<Bomb>());
        //}
        public Block SpawnBomb()
        {
            GameObject newBomb = GameObject.Instantiate(bombPrefab, transform);
            return (newBomb.GetComponent<Block>());
        }

        public Rocket SpawnRocket()
        {
            GameObject newRocket = GameObject.Instantiate(rocketPrefab, transform);
            return (newRocket.GetComponent<Rocket>());
        }



    }


}
