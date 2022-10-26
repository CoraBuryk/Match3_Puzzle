using UnityEngine;

namespace Assets.Match.Scripts.Models
{

    public abstract class PositionOnBoard : MonoBehaviour
    {
        private Point targetPoint;

#region Public Proprieties

        public Point SetTarget { set { targetPoint = value; } }

        public Point GetTarget { get { return targetPoint; } }

        public int GetX { get { return targetPoint.GetX; } }

        public int GetY { get { return targetPoint.GetY; } }

        public int SetX { set { targetPoint.SetX = value; } }

        public int SetY { set { targetPoint.SetY = value; } }

#endregion

    }
}
