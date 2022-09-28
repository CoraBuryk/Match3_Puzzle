using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Enum;
using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.UI.Animations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Assets.Match.Scripts.Gameplay
{
    public class BonusController : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private BoardManager _boardManager;
        [SerializeField] private GameController _gameController;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;
        [SerializeField] private ObjectsAnimation _objectsAnimation;

        [SerializeField] public GameObject _bombPrefab;
        [SerializeField] public GameObject _rocketPrefab;

        [SerializeField] private GameObject _explosion;
        #endregion

        public static bool IsWithinBounds(object[,] array2D, int x, int y)
        {
            return (x >= 0 && x < array2D.GetLength(0) && y >= 0 && y < array2D.GetLength(1));
        }

        public void GetBonus(int match)
        {
            if (match < 5)
                return;            
            else if (match >= 5 && match <= 7)
                DropBonus(BonusType.Rocket);
            else if(match >= 8) 
                DropBonus(BonusType.Bomb);
        }

        public async void DropBonus(BonusType bonusType)
        {
            int x = Random.Range(0, BoardManager.blocks.GetLength(0));
            int y = Random.Range(0, BoardManager.blocks.GetLength(1));

            await Task.Delay(300);

            if (bonusType == BonusType.Rocket)
            {
                _boardManager.DestroyBlock(BoardManager.blocks[x, y]);
                Block bonusBlock = Instantiate(_rocketPrefab, new Vector3(0,0,0), Quaternion.identity).GetComponent<Block>();
                bonusBlock.transform.SetParent(transform);
                BoardManager.blocks[x, y] = bonusBlock;
                _objectsAnimation.RocketAnimation(bonusBlock.gameObject, BoardManager.tiles[x, y].transform.position);
                bonusBlock.SetTarget = new Point(x, y);
                bonusBlock.IsBonus = true;
                
            }
            else
            {
                _boardManager.DestroyBlock(BoardManager.blocks[x, y]);
                Block bonusBlock = Instantiate(_bombPrefab, new Vector3(0,0,0), Quaternion.identity).GetComponent<Block>();
                bonusBlock.transform.SetParent(transform);
                BoardManager.blocks[x, y] = bonusBlock;
                _objectsAnimation.BombAnimation(bonusBlock.gameObject, BoardManager.tiles[x, y].transform.position);
                bonusBlock.SetTarget = new Point(x, y);
                bonusBlock.IsBonus = true;
                
            }

        }

        public static List<Block> ActivateBomb(Block[,] tiles, int x, int y, int offset= 2) 
        { 
            List<Block> gamePieces = new List<Block>();        
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = y - offset; j <= y + offset; j++)
                {
                    if(IsWithinBounds(tiles, i,j))
                    {
                        gamePieces.Add(tiles[i, j]);
                    }

                }
            }
            return gamePieces;
        }

        public static List<Block> ActivateRocket(Block[,] allPieces, int row)
        {
            List<Block> gamePieces = new List<Block>();

            for (int i = 0; i < allPieces.GetLength(0); i++)
            {
                if (allPieces[i, row] != null)
                {
                    gamePieces.Add(allPieces[i, row]);
                }
            }
            return gamePieces;
        }

        public void BonusValidation(Block matchedBlock, Block[,] allBlocks)
        {
            
                if (matchedBlock.IsBonus == true && matchedBlock.GetComponent<Bonus>().Type == BonusType.Bomb)
                {
                    _boardManager.DestroyBlock(matchedBlock);
                    _audioEffectsGame.PlayBombSound();
                    foreach (Block block in ActivateBomb(allBlocks, matchedBlock.GetX, matchedBlock.GetY))
                    {
                        _gameController.MatchWithGoal(block);
                        _boardManager.DestroyBlock(block);

                        GameObject explosion = Instantiate(_explosion, block.transform.position, Quaternion.identity);
                    Camera.main.transform.DOShakePosition(0.8f, new Vector3(0.1f, 0.1f, -1), 6, 90, false, true)
                        .OnComplete(() => _boardManager.SetupCamera());
                        Destroy(explosion, 2f);
                    }
                    _gameController.CountingScore(allBlocks.Length);
                }

                if (matchedBlock.IsBonus == true && matchedBlock.GetComponent<Bonus>().Type == BonusType.Rocket)
                {
                    _boardManager.DestroyBlock(matchedBlock);                   
                    _audioEffectsGame.PlayRocketSound();

                    foreach (Block block in ActivateRocket(allBlocks, matchedBlock.GetY))
                    {
                        _gameController.MatchWithGoal(block);
                        _boardManager.DestroyBlock(block);

                        GameObject explosion = Instantiate(_explosion, block.transform.position, Quaternion.identity);
                        Camera.main.transform.DOShakePosition(0.8f, new Vector3(0f, 0.1f, -1), 6, 90, false, true)
                                    .OnComplete(() => _boardManager.SetupCamera());
                    Destroy(explosion, 2f);
                }
                _gameController.CountingScore(allBlocks.Length);
            }
        }
    }
}
