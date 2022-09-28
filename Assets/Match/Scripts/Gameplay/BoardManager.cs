using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Enum;
using Assets.Match.Scripts.InputSystemController;
using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.ScriptableObjects;
using Assets.Match.Scripts.States;
using Assets.Match.Scripts.UI.Menu;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Match.Scripts.Gameplay
{
    public class BoardManager : StateMachine
    {
        #region Serialized Variables
        [SerializeField] private MoveController _moveController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private GameController _gameController;
        [SerializeField] private BonusController _bonusController;
        [SerializeField] private GoalController _goalController;
        [SerializeField] private VictoryPanel _victoryPanel;
        [SerializeField] private GameOverPanel _gameOverPanel;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;

        [SerializeField] private Block[] _blocksPrefabs;
        [SerializeField] private Obstacles[] ObstaclesPrefabs;
        [SerializeField] private Tiles _boardTilesPrefabs;
        [SerializeField] private LevelScriptableObject _levelScriptableObject;
        #endregion

        #region Private Variables
        private InputManager _inputManager;

        private const int _xSizeMin = 3;
        private const int _xSizeMax = 7;
        private const int _ySizeMin = 3;
        private const int _ySizeMax = 11;
        
        private Vector3 _startPosition;

        private static BoardManager _boardManager; 
        private List<Block> _selectedBlocks = new List<Block>();        
        private bool _isSelecting = false;
        private bool _isDrop = false;
        #endregion

        #region Static Variables
        public static int xSize;
        public static int ySize;
        public static Tiles[,] tiles;
        public static Block[,] blocks;
        public static Obstacles[,] obstacles;
        #endregion

        public int TotalMatch { get; set; }       

        private void Start()
        {
            _inputManager = InputManager.Instance;
            _boardManager = GetComponent<BoardManager>();

            if (_levelScriptableObject.isConfigured)
            {
                _levelScriptableObject.totalStar = 0;

                if (_levelScriptableObject.xSize >= _xSizeMin && _levelScriptableObject.xSize <= _xSizeMax)
                {
                    xSize = _levelScriptableObject.xSize;
                }

                if (_levelScriptableObject.ySize >= _ySizeMin && _levelScriptableObject.ySize <= _ySizeMax)
                {
                    ySize = _levelScriptableObject.ySize;
                }

                GenerateBoard(_levelScriptableObject);
                return;
            }
        }

        public void Update()
        {
            if (_isSelecting)
            {
                CheckForTileAtLocation(Camera.main.ScreenToWorldPoint(_inputManager.PressLocation));
            }
        }

        private Tiles MakeTile(Vector3 position, int xPos, int yPos)
        {
            Tiles tile = Instantiate(_boardTilesPrefabs, position, Quaternion.identity);
            tile.SetTarget = new Point(xPos, yPos);
            tile.transform.SetParent(transform);
            return tile;
        }

        private Obstacles MakeObstacle(Tiles positionTile, ObstacleType Type)
        {
            Obstacles obstacles = gameObject.AddComponent<Obstacles>();

            if(Type == ObstacleType.Rock)
            {
                obstacles = Instantiate(ObstaclesPrefabs[0], positionTile.transform.position, Quaternion.identity);

            }
            else if(Type == ObstacleType.Ice)
            {
                obstacles = Instantiate(ObstaclesPrefabs[1], positionTile.transform.position, Quaternion.identity);
            }

            obstacles.SetTarget = new Point(positionTile.GetX, positionTile.GetY);
            positionTile.IsObstacle = true;
            obstacles.transform.SetParent(transform);
            return obstacles;
        }

        public Block MakeBlock(Tiles postionTile, Block block)
        {
            int index = Random.Range(0, _blocksPrefabs.Length);
            Block newBlock = Instantiate(_blocksPrefabs[index], postionTile.transform.position, Quaternion.identity);

            newBlock.SetTarget = new Point(postionTile.GetX, postionTile.GetY);
            newBlock.transform.SetParent(transform);
            newBlock.IsBonus = false;

            return newBlock;
        }

        private void SetObstacles()
        {

            if (_levelScriptableObject.isObstacleLevel && _levelScriptableObject.obstaclesOnLevel.position.Length > 0)
            {
                for (int i = 0; i <= _levelScriptableObject.obstaclesOnLevel.NumberOfObstacles +1; i++)
                {
                    tiles[_levelScriptableObject.obstaclesOnLevel.position[i].x, 
                        _levelScriptableObject.obstaclesOnLevel.position[i].y].IsObstacle = true;
                }
            }

            if (_levelScriptableObject.isObstacleLevel && _levelScriptableObject.obstaclesOnLevel.position.Length == 0)
            {
               
                for (int i = 0; i <= _levelScriptableObject.obstaclesOnLevel.NumberOfObstacles +1; i++)
                {
                    int x = Random.Range(0, xSize);
                    int y = Random.Range(0, ySize);

                    tiles[x, y].IsObstacle = true;
                }
            }
        }

        private void GenerateBoard(LevelScriptableObject level)
        {
            tiles = new Tiles[level.xSize, level.ySize];
            obstacles = new Obstacles[level.xSize, level.ySize];

            SearchStartPosition();

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    tiles[x, y] = MakeTile(new Vector3(_startPosition.x + 0.8f * x, _startPosition.y + 0.8f * y, 0), x, y);                    
                }
            }

            SetObstacles();

            AddBlocks();

            SetupCamera();
            SwitchState<WaitingState>();
        }

        private void AddBlocks()
        {  
            blocks = new Block[xSize, ySize];

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (tiles[x, y] != null && tiles[x, y].IsObstacle != true)
                        blocks[x, y] = MakeBlock(tiles[x, y], _blocksPrefabs[Random.Range(0, _blocksPrefabs.Length)]);
                    else
                    {
                        obstacles[x, y] = MakeObstacle(tiles[x, y], _levelScriptableObject.obstaclesOnLevel.type);
                        blocks[x, y] = obstacles[x, y].GetComponent<Block>();
                        blocks[x, y].SetTarget = new Point(obstacles[x, y].GetX, obstacles[x,y].GetY);
                    }

                }
            }
        }

        public void SelectBlocks(bool isPressed)
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

        private void CheckForTileAtLocation(Vector2 location)
        {
            RaycastHit2D hit = Physics2D.Raycast(location, Vector2.zero);
            if (hit)
            {
                Block hitBlock = hit.collider.gameObject.GetComponent<Block>();
                Block selectedBlock = _selectedBlocks.Find(block => block == hitBlock);
                if (selectedBlock == null && _isDrop == false)
                {
                    if (_selectedBlocks.Count == 0 || _selectedBlocks[_selectedBlocks.Count - 1].TryToMatchWithRenderer(hitBlock))
                    {
                        _selectedBlocks.Add(hitBlock);
                        _audioEffectsGame.PlaySelectSound();
                        hitBlock.SelectTile();
                    }
                    else
                    {
                        int tileIndex = _selectedBlocks.IndexOf(selectedBlock);
                        if (tileIndex < _selectedBlocks.Count - 1)
                        {
                            for (int i = _selectedBlocks.Count - 1; i > tileIndex; i--)
                            {
                                _selectedBlocks[i].Unselect();
                                _selectedBlocks.Remove(_selectedBlocks[i]);
                            }
                        }
                    }
                }
            }
        }

        private void ValidateSelection()
        {
            _isSelecting = false;
            TotalMatch = _selectedBlocks.Count;

            if(TotalMatch == 1 && _levelScriptableObject.isBonusLevel == true)
            {
                foreach (Block matchedTile in _selectedBlocks)
                {
                    _bonusController.BonusValidation(matchedTile, blocks);
                }
                ClearSelectedTiles();
            }
            else if (TotalMatch >= 3)
            {
                _gameController.CountingScore(TotalMatch);
                SwitchState<TileDropState>();
                _selectedBlocks.Sort((p1, p2) => p1.GetY.CompareTo(p2.GetY));

                foreach (Block matchedTile in _selectedBlocks)
                {
                    _gameController.MatchWithGoal(matchedTile);

                    matchedTile.DestroyObstacle(matchedTile);

                    DestroyBlock(matchedTile);

                }

                _moveController.NumberOfMoves(_moveController.TotalMove - 1);
                ClearSelectedTiles();
            }
            else
            {
                ClearSelectedTiles();
            }

            SearchEmptyTile();

            if (_levelScriptableObject.isBonusLevel == true)
            {
                _bonusController.GetBonus(TotalMatch);
            }
            
            TotalMatch = 0;
            SwitchState<WaitingState>();
        }
  
        public async void SearchEmptyTile()
        {
            await Task.Delay(100);
            for (int x = 0; x < xSize; x++)
            {
                for (int y = ySize - 1; y > -1; y--)
                {
                    if (blocks[x, y] == null)
                    {   
                        _isDrop = true;
                        DropBlocks();
                        _isDrop = false;
                    }
                }
            }
            await Task.Delay(150);

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (blocks[x, y] != null)
                        continue;

                    blocks[x, y] = MakeBlock(tiles[x, y], _blocksPrefabs[Random.Range(0, _blocksPrefabs.Length)]);
                    blocks[x, y].transform.localScale = Vector3.zero;
                    blocks[x, y].transform.DOScale(0.5f, 0.2f);
                }
            }
        }       

        private void DropBlocks()
        {
            int yBelow = 0;
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (blocks[x, y] == null || blocks[x, y].GetY <= 0 || blocks[x, y - 1] != null)
                        continue;

                    if (obstacles[x, y] != null && obstacles[x, y].Type == ObstacleType.Ice)
                        continue;

                    if (obstacles[x, y - 1] != null && obstacles[x, y - 1].Type == ObstacleType.Ice)
                        continue;

                    yBelow = y - 1;
                    while (y > 0)
                    {
                        if (blocks[x, yBelow] == null)
                        {
                            if (yBelow == 0)
                            {
                                if (obstacles[x, y] != null && obstacles[x, y].Type == ObstacleType.Rock)
                                {
                                    obstacles[x, y].transform.DOMove(tiles[x, yBelow].transform.position, 0.5f);
                                    obstacles[x, y].SetY = yBelow;
                                    obstacles[x, yBelow] = obstacles[x, y];
                                    obstacles[x, y] = null;
                                    tiles[x, y].IsObstacle = false;
                                    tiles[x, yBelow].IsObstacle = true;
                                }

                                blocks[x, y].transform.DOMove(tiles[x, yBelow].transform.position, 0.5f);
                                blocks[x, y].SetY = yBelow;
                                blocks[x, yBelow] = blocks[x, y];
                                blocks[x, y] = null;
                                break;
                            }

                            yBelow--;
                        }
                        else
                        {
                            if (obstacles[x, y] != null && obstacles[x, y].Type == ObstacleType.Rock)
                            {
                                obstacles[x, y].transform.DOMove(tiles[x, yBelow + 1].transform.position, 0.5f);
                                obstacles[x, y].SetY = yBelow + 1;
                                obstacles[x, yBelow + 1] = obstacles[x, y];
                                obstacles[x, y] = null;
                                tiles[x, y].IsObstacle = false;
                                tiles[x, yBelow + 1].IsObstacle = true;
                            }

                            blocks[x, y].transform.DOMove(tiles[x, yBelow + 1].transform.position, 0.5f);
                            blocks[x, y].SetY = yBelow + 1;
                            blocks[x, yBelow + 1] = blocks[x, y];
                            blocks[x, y] = null;
                            break;
                        }
                    }
                }
            }
            _audioEffectsGame.PlayDropSound();
        }
  
        private void ClearSelectedTiles()
        {
            foreach (Block tile in _selectedBlocks)
            {
                tile.Unselect();
            }
            _selectedBlocks.Clear();
        }
        
        public void DestroyBlock(Block block)
        {
            Destroy(block.gameObject);
        }

        public void UpdateBoard()
        {
            for (var i = 0; i < xSize; i++)
            {
                for (var j = 0; j < ySize; j++)
                {
                    if (obstacles[i, j] != null)
                        continue;

                    DestroyBlock(blocks[i, j]);
                    blocks[i, j] = MakeBlock(tiles[i, j], _blocksPrefabs[Random.Range(0, _blocksPrefabs.Length)]);
                }
            }
        }

        public void SearchStartPosition()
        {
            _startPosition = new Vector3(-(xSize / 2) + 0.1f, -(ySize / 2), 0);
        }

        public void SetupCamera()
        {
            switch (xSize)
            {
                case 3:
                Camera.main.orthographicSize = 3;
                    break;
                case 4:
                    Camera.main.orthographicSize = 4.5f;
                    Camera.main.transform.position = new Vector3(-0.65f, -0.5f, -1);
                    break;
                case 5:
                    Camera.main.orthographicSize = 5f;
                    Camera.main.transform.position = new Vector3(-0.35f, -0.5f, -1);
                    break;
                case 6:
                    Camera.main.orthographicSize = 5.5f;
                    Camera.main.transform.position = new Vector3(-0.85f, -0.5f, -1);
                    break;
                case 7:
                    Camera.main.orthographicSize = 6f;
                    Camera.main.transform.position = new Vector3(-0.55f, -0.5f, -1);
                    break;
            }
        }
    }
}
