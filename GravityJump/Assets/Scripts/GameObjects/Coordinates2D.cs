namespace GameObjects
{
    public class Coordinates2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float ZAngle { get; set; }

        public Coordinates2D(float x,float y, float zAngle)
        {
            this.X = x;
            this.Y = y;
            this.ZAngle = zAngle;
        }
    }
}
