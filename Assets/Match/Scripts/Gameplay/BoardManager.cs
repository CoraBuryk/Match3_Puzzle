using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.InputSystemController;
using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.States;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{
    public class BoardManager : StateMachine
    {
        [SerializeField] private MoveController _moveController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private GameController _gameController;
        [SerializeField] private BonusController _bonusController;

        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject rocketPrefab;

        public static BoardManager instance;
        public GameObject[] blocks;

        public GameObject[] boardTiles;
        public bool _isSelecting = false;
        private InputManager _inputManager;
        public List<Block> selectedTiles = new List<Block>();

        public GameObject[,] tiles;

        public int TotalMatch { get; set; }

        void Awake()
        {
            instance = GetComponent<BoardManager>();
            _inputManager = InputManager.Instance;

            tiles = new GameObject[boardTiles.Length, 1];

            FillBoard();
        }

        private void FillBoard()
        {         
            for(int i = 0; i < boardTiles.Length; i++)
            {
                GameObject block = MakeBlock(boardTiles[i].transform.position);
            }
            SwitchState<WaitingState>();
        }

        GameObject MakeBlock(Vector3 position)
        {
            int index = UnityEngine.Random.Range(0, blocks.Length);
            GameObject newBlock = Instantiate(blocks[index], position, Quaternion.identity);
            return newBlock;
        }

        public void Update()
        {
            if (_isSelecting)
            {
                CheckForTileAtLocation(Camera.main.ScreenToWorldPoint(_inputManager.PressLocation));
            }
        }

        public void SelectTiles(bool isPressed)
        {
            if (isPressed)
            {
                _isSelecting = true;
            }
            else
            {
                _isSelecting = false;
                ValidateSelection();
            }
        }

        public void CheckForTileAtLocation(Vector2 location)
        {
            RaycastHit2D hit = Physics2D.Raycast(location, Vector2.zero);
            if(hit)
            {
                Block hitTile = hit.collider.gameObject.GetComponent<Block>();
                Block selectedTile = selectedTiles.Find(tile => tile == hitTile);
                if(selectedTile == null)
                {
                    if(selectedTiles.Count == 0 || selectedTiles[selectedTiles.Count - 1].TryToMatchWithRenderer(hitTile) ||
                        selectedTiles[selectedTiles.Count - 1].TryToMatchWithType(hitTile))
                    {
                        selectedTiles.Add(hitTile);
                        hitTile.SelectTile();
                    }
                    else
                    {
                        int tileIndex = selectedTiles.IndexOf(selectedTile);
                        if (tileIndex < selectedTiles.Count - 1)
                        {
                            for (int i = selectedTiles.Count - 1; i > tileIndex; i--)
                            {
                                selectedTiles[i].Unselect();
                                selectedTiles.Remove(selectedTiles[i]);
                            }
                        }
                    }
                }
            }
        }


        public void ValidateSelection()
        {
            _isSelecting = false;
            TotalMatch = selectedTiles.Count;
            if (TotalMatch >= 3)
            {
                _gameController.CountingScore(TotalMatch);
                SwitchState<TileDropState>();
                
                foreach (Block matchedTile in selectedTiles)
                {
                    DropTiles(matchedTile);
                }
                _moveController.NumberOfMoves(_moveController.TotalMove - 1);
                TotalMatch = 0;
                ClearSelectedTiles();
                SwitchState<WaitingState>();
            }
            else
            {
                ClearSelectedTiles();
            }

            if (TotalMatch == 1 && _bonusController.isBombClicked == true)
            {
                foreach (Block matchedTile in selectedTiles)
                {
                    matchedTile.DestroyBlock();
                        DropBomb(matchedTile);                   
                }
                _bonusController.isBombClicked = false;

            }
        }

        private void DropBomb(Block matchedTile)
        {
            matchedTile.DestroyBlock();
            MakeBomb(matchedTile.transform.position);

        }
        GameObject MakeBomb(Vector3 position)
        {           
            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            return bomb;
        }

        private void DropTiles(Block matchedTile)
        {
            matchedTile.DestroyBlock();
            
            foreach (GameObject ss in FindMatch(matchedTile.transform.position))
            {
                FindMatch(matchedTile.transform.position).Remove(ss);
                MakeBlock(ss.transform.position);
                ss.transform.position = new Vector2(matchedTile.transform.position.x, matchedTile.transform.position.y - 0.001f);
            }
        }


        private List<GameObject> FindMatch(Vector2 castDir)
        { 
            List<GameObject> matchingTiles = new List<GameObject>(); 
            Collider2D collision = Physics2D.OverlapBox(castDir, new Vector2(0f, 5f), 10f);
            matchingTiles.Add(collision.gameObject);
            return matchingTiles; 
        }

        private void ClearSelectedTiles()
        {
            foreach (Block tile in selectedTiles)
            {
                tile.Unselect();
            }
            selectedTiles.Clear();
        }
    }
}
