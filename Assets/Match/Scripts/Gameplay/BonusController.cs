using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Enum;
using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.UI.Animations;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class BonusController : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private BoardManager _boardManager;
        [SerializeField] private GameController _gameController;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;
        [SerializeField] private ObjectsAnimation _objectsAnimation;
        [SerializeField] private BoardScriptableObject _board;

        [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private GameObject _rocketPrefab;

        [SerializeField] private GameObject _explosion;

#endregion        

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public bool IsWithinBounds(object[,] array2D, int x, int y)
        {
            return (x >= 0 && x < array2D.GetLength(0) && y >= 0 && y < array2D.GetLength(1));
        }

        public void GetBonus(int match)
        {
            if (match < 5)
                return;            
            else if (match < 8)
                DropBonus(BonusType.Rocket);
            else 
                DropBonus(BonusType.Bomb);
        }

        private async void DropBonus(BonusType bonusType)
        {
            try
            {
                int x = Random.Range(0, _board.Blocks.GetLength(0));
                int y = Random.Range(0, _board.Blocks.GetLength(1));

                await Task.Delay(300);

                _boardManager.DestroyBlock(_board.Blocks[x, y]);

                BlockController bonusBlock;
                if (bonusType == BonusType.Rocket)
                {
                    bonusBlock = Instantiate(_rocketPrefab, new Vector3(0, 0, 0), 
                        Quaternion.identity).GetComponent<BlockController>();
                }
                else
                {
                    bonusBlock = Instantiate(_bombPrefab, new Vector3(0, 0, 0), 
                        Quaternion.identity).GetComponent<BlockController>();
                }

                bonusBlock.transform.SetParent(transform);
                _board.Blocks[x, y] = bonusBlock;
                _objectsAnimation.BonusAnimation(bonusBlock.gameObject, _board.Tiles[x, y].transform.position);
                bonusBlock.SetTarget = new Point(x, y);
                bonusBlock.IsBonus = true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
                throw;
            }         
        }

        private List<BlockController> ActivateBomb(Block[,] tiles, int x, int y, int offset = 2) 
        { 
            List<BlockController> gamePieces = new List<BlockController>();        
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = y - offset; j <= y + offset; j++)
                {
                    if(IsWithinBounds(tiles, i,j))
                    {
                        gamePieces.Add(tiles[i, j].GetComponent<BlockController>());
                    }

                }
            }

            return gamePieces;
        }

        private List<BlockController> ActivateRocket(Block[,] allPieces, int row)
        {
            List<BlockController> gamePieces = new List<BlockController>();

            for (int i = 0; i < allPieces.GetLength(0); i++)
            {
                if (allPieces[i, row] != null)
                {
                    gamePieces.Add(allPieces[i, row].GetComponent<BlockController>());
                }
            }

            return gamePieces;
        }

        public void BonusValidation(BlockController matchedBlock, Block[,] allBlocks)
        {
            GameObject explosion = null;

            if (matchedBlock.IsBonus==true)
            {
                _boardManager.DestroyBlock(matchedBlock);
                
                if(matchedBlock.GetComponent<Bonus>().Type == BonusType.Bomb)
                {
                    _audioEffectsGame.PlayBombSound();
                    foreach (BlockController block in ActivateBomb(allBlocks, matchedBlock.GetX, matchedBlock.GetY))
                    {
                        _gameController.MatchWithGoal(block);
                        _boardManager.DestroyBlock(block);
                        explosion = Instantiate(_explosion, block.transform.position, Quaternion.identity);
                        Destroy(explosion, 2f);
                    }
                    _camera.transform.DOShakePosition(1f, new Vector3(0f, 0.08f, -0.01f), 6, 1, false, true)
                                                        .OnComplete(() => _boardManager.SetupCamera());
                }
                else if(matchedBlock.GetComponent<Bonus>().Type == BonusType.Rocket)
                {
                    _audioEffectsGame.PlayRocketSound();
                    foreach (BlockController block in ActivateRocket(allBlocks, matchedBlock.GetY))
                    {
                        _gameController.MatchWithGoal(block);
                        _boardManager.DestroyBlock(block);
                         explosion = Instantiate(_explosion, block.transform.position, Quaternion.identity);
                        Destroy(explosion, 2f);
                    }
                    _camera.transform.DOShakePosition(0.8f, new Vector3(0.08f, 0f, -0.01f), 6, 1, false, true)
                                                        .OnComplete(() => _boardManager.SetupCamera());
                }

                _gameController.CountingScore(allBlocks.Length);            
            }          
        }

    }
}
