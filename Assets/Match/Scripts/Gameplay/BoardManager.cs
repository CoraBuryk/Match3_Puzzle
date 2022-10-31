using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Enum;
using Assets.Match.Scripts.InputSystemController;
using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class BoardManager : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private MoveController _moveController;
        [SerializeField] private GameController _gameController;
        [SerializeField] private BonusController _bonusController;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;

        [SerializeField] private BlockController[] _blocksPrefabs;
        [SerializeField] private Obstacles[] _obstaclesPrefabs;
        [SerializeField] private Tiles _boardTilesPrefab;
        [SerializeField] private LevelScriptableObject _levelScriptableObject;
        [SerializeField] private BoardScriptableObject _boardScriptableObject;

#endregion

        public int TotalMatch { get; set; }


#region Private Variables

        private InputManager _inputManager;
        private Camera _mainCamera;
        private Block _currentBlock;

        private const int _xSizeMin = 3;
        private const int _xSizeMax = 7;
        private const int _ySizeMin = 3;
        private const int _ySizeMax = 11;
        
        private Vector3 _startPosition;
        private Vector3 _cameraPosition = new(0, 0, 0);
        private float _cameraSize = 0;

        private readonly List<BlockController> _selectedBlocks = new();        
        private bool _isSelecting = false;
        private bool _isDrop = false;

#endregion

        private void Awake()
        {
            _inputManager = InputManager.Instance;
            _mainCamera = Camera.main;           
        }

        private void OnEnable()
        {
            _inputManager.OnPress.AddListener(SelectBlocks);
        }

        private void Start()
        {
            if (_levelScriptableObject.isConfigured)
            {
                _levelScriptableObject.totalStar = 0;

                if (_levelScriptableObject.xSize >= _xSizeMin && _levelScriptableObject.xSize <= _xSizeMax)
                {
                    _boardScriptableObject.XSize = _levelScriptableObject.xSize;
                }

                if (_levelScriptableObject.ySize >= _ySizeMin && _levelScriptableObject.ySize <= _ySizeMax)
                {
                    _boardScriptableObject.YSize = _levelScriptableObject.ySize;
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
            Tiles tile = Instantiate(_boardTilesPrefab, position, Quaternion.identity);
            tile.SetTarget = new Point(xPos, yPos);
            tile.transform.SetParent(transform);
            return tile;
        }

        private Obstacles MakeObstacle(Tiles positionTile, ObstacleType Type)
        {
            Obstacles obstacles = gameObject.AddComponent<Obstacles>();

            if(Type == ObstacleType.Rock)
            {
                obstacles = Instantiate(_obstaclesPrefabs[0], positionTile.transform.position, Quaternion.identity);               

            }
            else if(Type == ObstacleType.Ice)
            {
                obstacles = Instantiate(_obstaclesPrefabs[1], positionTile.transform.position, Quaternion.identity);
            }

            obstacles.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            obstacles.SetTarget = new Point(positionTile.GetX, positionTile.GetY);
            positionTile.IsObstacle = true;
            obstacles.transform.SetParent(transform);
            return obstacles;
        }

        private BlockController MakeBlock(Tiles postionTile)
        {
            int index = Random.Range(0, _blocksPrefabs.Length);

            BlockController newBlock = Instantiate(_blocksPrefabs[index], postionTile.transform.position, Quaternion.identity);
            newBlock.GetComponent<BlockController>().SetTarget = new Point(postionTile.GetX, postionTile.GetY);
            newBlock.transform.SetParent(transform);
            newBlock.IsBonus = false;

            return newBlock;
        }

        private void SetObstacles()
        {

            if (_levelScriptableObject.isObstacleLevel && _levelScriptableObject.obstaclesOnLevel.position.Length > 0)
            {
                for (int i = 0; i < _levelScriptableObject.obstaclesOnLevel.NumberOfObstacles; i++)
                {
                    _boardScriptableObject.Tiles[_levelScriptableObject.obstaclesOnLevel.position[i].x, 
                        _levelScriptableObject.obstaclesOnLevel.position[i].y].IsObstacle = true;
                }
            }

            if (_levelScriptableObject.isObstacleLevel && _levelScriptableObject.obstaclesOnLevel.position.Length == 0)
            {
               
                for (int i = 0; i < _levelScriptableObject.obstaclesOnLevel.NumberOfObstacles; i++)
                {
                    int x = Random.Range(0, _boardScriptableObject.XSize);
                    int y = Random.Range(0, _boardScriptableObject.YSize);

                    _boardScriptableObject.Tiles[x, y].IsObstacle = true;
                }
            }
        }

        private void GenerateBoard(LevelScriptableObject level)
        {
            _boardScriptableObject.Tiles = new Tiles[level.xSize, level.ySize];
            _boardScriptableObject.Obstacles = new Obstacles[level.xSize, level.ySize];

            SearchStartPosition();

            for (int x = 0; x < _boardScriptableObject.XSize; x++)
            {
                for (int y = 0; y < _boardScriptableObject.YSize; y++)
                {
                    _boardScriptableObject.Tiles[x, y] = MakeTile(new Vector3(_startPosition.x + 0.8f * x, _startPosition.y + 0.8f * y, 0), x, y);                    
                }
            }

            SetObstacles();

            AddBlocks();

            SetupCamera();
        }

        private void AddBlocks()
        {
            _boardScriptableObject.Blocks = new Block[_boardScriptableObject.XSize, _boardScriptableObject.YSize];

            for (int x = 0; x < _boardScriptableObject.XSize; x++)
            {
                for (int y = 0; y < _boardScriptableObject.YSize; y++)
                {
                    if (_boardScriptableObject.Tiles[x, y] != null && _boardScriptableObject.Tiles[x, y].IsObstacle != true)
                        _currentBlock = MakeBlock(_boardScriptableObject.Tiles[x, y]);
                    else
                    {
                        _boardScriptableObject.Obstacles[x, y] = MakeObstacle(_boardScriptableObject.Tiles[x, y], 
                                            _levelScriptableObject.obstaclesOnLevel.type);
                        _currentBlock = _boardScriptableObject.Obstacles[x, y].GetComponent<Block>();
                        _currentBlock.GetComponent<BlockController>().SetTarget = new Point(_boardScriptableObject.Obstacles[x, y].GetX,
                                            _boardScriptableObject.Obstacles[x, y].GetY);
                    }
                    _boardScriptableObject.Blocks[x, y] = _currentBlock;
                    _boardScriptableObject.Obstacles[x, y] = _boardScriptableObject.Obstacles[x, y];
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
                BlockController hitBlock = hit.collider.gameObject.GetComponent<BlockController>();
                BlockController selectedBlock = _selectedBlocks.Find(block => block == hitBlock);
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
                foreach (BlockController matchedTile in _selectedBlocks)
                {
                    _bonusController.BonusValidation(matchedTile, _boardScriptableObject.Blocks);
                }
                ClearSelectedTiles();
            }
            else if (TotalMatch >= 3)
            {
                _gameController.CountingScore(TotalMatch);
                _selectedBlocks.Sort((p1, p2) => p1.GetY.CompareTo(p2.GetY));

                foreach (BlockController matchedTile in _selectedBlocks)
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

            _isDrop = false;

            TotalMatch = 0;
        }

        public async void SearchEmptyTile()
        {
            try
            {
                await Task.Delay(100);
                for (int x = 0; x < _boardScriptableObject.XSize; x++)
                {
                    for (int y = _boardScriptableObject.YSize - 1; y > -1; y--)
                    {
                        _currentBlock = _boardScriptableObject.Blocks[x, y];
                        if (_currentBlock == null)
                        {
                            _isDrop = true;
                            DropBlocks();
                        }

                    }
                }

                await Task.Delay(100);

                for (int x = 0; x < _boardScriptableObject.XSize; x++)
                {
                    for (int y = 0; y < _boardScriptableObject.YSize; y++)
                    {
                        _currentBlock = _boardScriptableObject.Blocks[x, y];
                        if (_currentBlock != null)
                            continue;

                        _currentBlock = MakeBlock(_boardScriptableObject.Tiles[x, y]);
                        _currentBlock.transform.localScale = Vector3.zero;
                        _currentBlock.transform.DOScale(0.5f, 0.2f);
                        _boardScriptableObject.Blocks[x, y] = _currentBlock;

                    }
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
            }

        }

        private void DropBlocks()
        {
            int yBelow;

            for (int x = 0; x < _boardScriptableObject.XSize; x++)
            {
                for (int y = 0; y < _boardScriptableObject.YSize; y++)
                {
                    _currentBlock = _boardScriptableObject.Blocks[x, y];

                    if (_currentBlock == null || _currentBlock.GetComponent<BlockController>().GetY <= 0 || 
                        _boardScriptableObject.Blocks[x, y - 1] != null)
                        continue;
                    if (_boardScriptableObject.Obstacles[x, y] != null && _boardScriptableObject.Obstacles[x, y].Type == ObstacleType.Ice)
                        continue;
                    if (_boardScriptableObject.Obstacles[x, y - 1] != null && _boardScriptableObject.Obstacles[x, y - 1].Type == ObstacleType.Ice)
                        continue;

                    yBelow = y - 1;
                    while (y > 0)
                    {
                        if (_boardScriptableObject.Blocks[x, yBelow] == null)
                        {
                            if (yBelow == 0)
                            {
                                if (_boardScriptableObject.Obstacles[x, y] != null && _boardScriptableObject.Obstacles[x, y].Type == ObstacleType.Rock)
                                {
                                    _boardScriptableObject.Obstacles[x, y].transform.DOMove(_boardScriptableObject.Tiles[x, yBelow].transform.position, 0.5f);
                                    _boardScriptableObject.Obstacles[x, y].SetY = yBelow;
                                    _boardScriptableObject.Obstacles[x, yBelow] = _boardScriptableObject.Obstacles[x, y];
                                    _boardScriptableObject.Obstacles[x, y] = null;
                                    _boardScriptableObject.Tiles[x, y].IsObstacle = false;
                                    _boardScriptableObject.Tiles[x, yBelow].IsObstacle = true;
                                }

                                _currentBlock.transform.DOMove(_boardScriptableObject.Tiles[x, yBelow].transform.position, 0.5f);
                                _currentBlock.GetComponent<BlockController>().SetY = yBelow;
                                _boardScriptableObject.Blocks[x, yBelow] = _currentBlock;
                                _boardScriptableObject.Blocks[x, y] = null;
                                break;
                            }

                            yBelow--;
                        }
                        else
                        {
                            if (_boardScriptableObject.Obstacles[x, y] != null && _boardScriptableObject.Obstacles[x, y].Type == ObstacleType.Rock)
                            {
                                _boardScriptableObject.Obstacles[x, y].transform.DOMove(_boardScriptableObject.Tiles[x, yBelow + 1].transform.position, 0.5f);
                                _boardScriptableObject.Obstacles[x, y].SetY = yBelow + 1;
                                _boardScriptableObject.Obstacles[x, yBelow + 1] = _boardScriptableObject.Obstacles[x, y];
                                _boardScriptableObject.Obstacles[x, y] = null;
                                _boardScriptableObject.Tiles[x, y].IsObstacle = false;
                                _boardScriptableObject.Tiles[x, yBelow + 1].IsObstacle = true;
                            }

                            _currentBlock.transform.DOMove(_boardScriptableObject.Tiles[x, yBelow + 1].transform.position, 0.5f);
                            _currentBlock.GetComponent<BlockController>().SetY = yBelow + 1;
                            _boardScriptableObject.Blocks[x, yBelow + 1] = _currentBlock;
                            _boardScriptableObject.Blocks[x, y] = null;
                            
                            break;
                        }
                    }
                }
            }
            _audioEffectsGame.PlayDropSound();
        }
  
        private void ClearSelectedTiles()
        {
            foreach (BlockController tile in _selectedBlocks)
            {
                tile.Unselect();
            }
            _selectedBlocks.Clear();
        }

        public void UpdateBoard()
        {
            for (var x = 0; x < _boardScriptableObject.XSize; x++)
            {
                for (var y = 0; y < _boardScriptableObject.YSize; y++)
                {
                    _currentBlock = _boardScriptableObject.Blocks[x, y];
                    if (_boardScriptableObject.Obstacles[x, y] != null)
                        continue;

                    DestroyBlock(_currentBlock);
                    _currentBlock = MakeBlock(_boardScriptableObject.Tiles[x, y]);
                    _boardScriptableObject.Blocks[x, y] = _currentBlock;
                }
            }
        }

        public void DestroyBlock(Block block)
        {
            Destroy(block.gameObject);
        }
        
        public void SearchStartPosition()
        {
            _startPosition = new Vector3(-(_boardScriptableObject.XSize / 2) + 0.1f, -(_boardScriptableObject.YSize / 2), 0);
        }

        public void SetupCamera()
        {
            switch (_boardScriptableObject.XSize)
            {
                case 3:                
                    _cameraSize = 3;
                    break;
                case 4:
                    _cameraSize = 4.5f;
                    _cameraPosition = new Vector3(-0.65f, -0.5f, -1);
                    break;
                case 5:
                    _cameraSize = 5f;
                    _cameraPosition = new Vector3(-0.35f, -0.5f, -1);
                    break;
                case 6:
                    _cameraSize = 5.5f;
                    _cameraPosition = new Vector3(-0.85f, -0.5f, -1);
                    break;
                case 7:
                    _cameraSize = 6f;
                    _cameraPosition = new Vector3(-0.55f, -0.5f, -1);
                    break;
            }

            _mainCamera.transform.position = _cameraPosition;
            _mainCamera.orthographicSize = _cameraSize;

        }

        private void OnDisable()
        {
            _inputManager.OnPress.RemoveListener(SelectBlocks);
        }

    }
}
