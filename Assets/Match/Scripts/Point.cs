namespace Assets.Match.Scripts
{

    [System.Serializable]

    public sealed class Point
    {       
        public int GetX { get { return x; } }

        public int GetY { get { return y; } }

        public int SetX { set { x = value; } }

        public int SetY { set { y = value; } }

        private int x; 
        private int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }
        
    }
}
