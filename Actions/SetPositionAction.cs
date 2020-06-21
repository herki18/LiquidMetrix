using LiquidMetrix.Data;
using LiquidMetrix.Enums;

namespace LiquidMetrix.Actions
{
    public struct SetPositionAction : IAction
    {
        public Vector2 Position { get; }
        public Direction Direction { get; }

        public SetPositionAction(Vector2 position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}
