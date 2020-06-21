using LiquidMetrix.Enums;

namespace LiquidMetrix.Data
{
    public class Move
    {
        public Rotation Rotation;
        public int StepsToMove;

        public Move(Rotation rotation, int stepsToMove)
        {
            Rotation = rotation;
            StepsToMove = stepsToMove;
        }
    }
}