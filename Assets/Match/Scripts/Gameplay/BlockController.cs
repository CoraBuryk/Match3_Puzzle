using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.ScriptableObjects;
using Assets.Match.Scripts.UI.Animations;

namespace Assets.Match.Scripts.Gameplay
{

    public class BlockController : Block
    {
        [SerializeField] private BoardScriptableObject _board;

        public bool TryToMatchWithRenderer(BlockController block)
        {
            if (!block)
                return false;
            if (block.Type != Type)
                return false;
            return IsNeighbourWith(block.GetTarget);
        }

        private bool IsNeighbourWith(Point blockPosition)
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

        private List<Obstacles> FindObstacles(BlockController block)
        {
            List<Obstacles> obstaclesBlocks = new List<Obstacles>();
            for (int x = 0; x < _board.XSize; x++)
            {
                for (int y = 0; y < _board.YSize; y++)
                {
                    if (_board.Obstacles[x, y] == null)
                        continue;

                    if (block.IsNeighbourWithObstacle(_board.Obstacles[x, y].GetTarget) == true)
                    {
                        obstaclesBlocks.Add(_board.Obstacles[x, y]);
                    }

                }
            }
            return obstaclesBlocks;
        }

        public void DestroyObstacle(BlockController block)
        {
            foreach (Obstacles ob in block.FindObstacles(block))
            {
                ob.GetComponent<ObstacleAnimation>().ObstaclesAnimation(ob);

                _board.Tiles[ob.GetX, ob.GetY].IsObstacle = false;
                _board.Obstacles[ob.GetX, ob.GetY] = null;
            }
        }

        private bool IsNeighbourWithObstacle(Point blockPosition)
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
