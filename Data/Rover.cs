using LiquidMetrix.Enums;

namespace LiquidMetrix.Data
{
    public struct Rover
    {
        public int Id { get; set; }
        public Direction? Direction { get; set; }
        public Vector2 Position { get; set; }

        public Rover(int id)
        {
            Direction = null;
            Position = Vector2.Zero;
            Id = id;
        }
    }
}
