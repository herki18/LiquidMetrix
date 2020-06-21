using LiquidMetrix.Data;

namespace LiquidMetrix.Logic
{
    public interface IOutput
    {
        string GetTransform(Rover rover);
    }

    public class RoverTransformStringOutput : IOutput
    {
        public string GetTransform(Rover rover)
        {
            return $"[{rover.Position.X}, {rover.Position.Y}, {rover.Direction}]";
        }
    }
}