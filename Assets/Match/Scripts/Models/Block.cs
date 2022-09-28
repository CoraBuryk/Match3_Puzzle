using Assets.Match.Scripts.Enum;
using Assets.Match.Scripts.Gameplay;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Match.Scripts.Models
{
    public class Block : PositionOnBoard
    {
        public SpriteRenderer BlockRenderer { get; set; }
        public bool IsBonus { get; set; }

        private void Awake()
        {
            BlockRenderer = GetComponent<SpriteRenderer>();
        }

        public bool TryToMatchWithRenderer(Block block)
        {
            if(!block)
                return false;
            if (block.GetComponent<SpriteRenderer>().sprite != BlockRenderer.sprite)
                return false;
            return IsNeighbourWith(block.GetTarget);
        }

        public bool IsNeighbourWith(Point blockPosition)
        {
            Point direction = blockPosition - GetTarget;
            if (direction.GetY == 1 && direction.GetX == 0)
                return true;
            if (direction.GetY == 1 && direction.GetX == -1)
                return true;
            if (direction.GetY == 0 && direction.GetX == -1)
                return true;
            if (direction.GetY == -1 && direction.GetX == 0)
                return true;
            if (direction.GetY == -1 && direction.GetX == 1)
                return true;
            if (direction.GetY == 0 && direction.GetX == 1)
                return true;
            if (direction.GetY == -1 && direction.GetX == -1)
                return true;
            if (direction.GetY == 1 && direction.GetX == 1)
                return true;
            return false;
        }

        public void SelectTile()
        {
            transform.DOShakePosition(10f, 0.01f, 8, 90f, false, false);
            transform.DOScale(0.6f, 0.1f);
        }

        public void Unselect()
        {
            transform.DOKill();
            transform.DOScale(0.5f, 0.1f);
        }

        private List<Obstacles> FindObstacles(Block block)
        {
            List<Obstacles> obstaclesBlocks = new List<Obstacles>();
            for (int x = 0; x < BoardManager.xSize; x++)
            {
                for (int y = 0; y < BoardManager.ySize; y++)
                {
                    if (BoardManager.obstacles[x, y] == null)
                        continue;

                    if (block.IsNeighbourWithObstacle(BoardManager.obstacles[x, y].GetTarget) == true)
                    {
                        obstaclesBlocks.Add(BoardManager.obstacles[x, y]);
                    }
                        
                }
            }
            return obstaclesBlocks;
        }

        public void DestroyObstacle(Block block)
        {

            foreach (Obstacles ob in block.FindObstacles(block))
            {
                if (ob.Type == ObstacleType.Rock)
                {
                    ob.GetComponent<AudioSource>().Play();
                    ob.transform.DOScale(0, 0.2f);
                    Destroy(ob.GetComponent<Block>().gameObject);
                }

                if (ob.Type == ObstacleType.Ice)
                {
                    ob.GetComponent<AudioSource>().Play();
                    ob.transform.DOScale(0, 0.2f);
                    Destroy(ob.GetComponent<Block>().gameObject);
                }
                BoardManager.tiles[ob.GetX, ob.GetY].IsObstacle = false;
                BoardManager.obstacles[ob.GetX, ob.GetY] = null;
            }
        }

        public bool IsNeighbourWithObstacle(Point blockPosition)
        {
            Point direction = blockPosition - GetTarget;
            if (direction.GetY == -1 && direction.GetX == 0)
                return true;
            if (direction.GetY == 0 && direction.GetX == -1)
                return true;
            if (direction.GetY == 1 && direction.GetX == 0)
                return true;
            if (direction.GetY == 0 && direction.GetX == 1)
                return true;
            return false;
        }
    }

}
