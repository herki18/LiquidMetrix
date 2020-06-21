using LiquidMetrix.Actions;
using LiquidMetrix.Data;
using LiquidMetrix.Enums;

namespace LiquidMetrix.Logic
{
    public interface IRoverMover
    {
        StatusCode SetPosition(SetPositionAction setPosition);
        StatusCode Move(MoveAction move);
    }

    public class RoverMover : IRoverMover
    {
        private readonly IGrid _grid;
        private readonly IRoverManager _roverManager;

        public RoverMover(IGrid grid, IRoverManager roverManager)
        {
            _grid = grid;
            _roverManager = roverManager;
        }

        public StatusCode SetPosition(SetPositionAction setPosition)
        {
            var statusCode = _grid.CanMove(setPosition.Position);
            if (statusCode == StatusCode.Successful)
            {
                var rover = _roverManager.CreateRover(setPosition);
                _grid.SetRoverPosition(rover);
            }

            return statusCode;
        }

        public StatusCode Move(MoveAction move)
        {
            StatusCode statusCode = StatusCode.None;
            var rover = _roverManager.SelectedRover;
            foreach (var moveAction in move.MoveActions)
            {
                if (rover.Direction != null)
                {
                    var direction = RotateRover(rover.Direction.Value, moveAction.Rotation);
                    
                    var position = Move(direction, rover.Position, moveAction.StepsToMove);

                    if (position.HasValue)
                    {
                        statusCode = _grid.CanMove(position.Value);
                        if (statusCode == StatusCode.Successful)
                        {
                            rover.Direction = direction;
                            rover.Position = position.Value;
                        }
                    }
                }
            }

            if(statusCode == StatusCode.Successful)
            {
                _roverManager.UpdateRover(rover);
                _grid.SetRoverPosition(rover);
            }

            return statusCode;
        }

        private Vector2? Move(Direction direction, Vector2 position, int steps)
        {
            switch (direction)
            {
                case Direction.N:
                {
                    return new Vector2(position.X, position.Y + steps);
                }
                case Direction.E:
                {
                    return new Vector2(position.X + steps, position.Y);
                }
                case Direction.S:
                {
                    return new Vector2(position.X, position.Y - steps);
                }
                case Direction.W:
                {
                    return new Vector2(position.X - steps, position.Y);
                }
                default:
                {
                    return null;
                }
            }
        }

        private Direction RotateRover(Direction direction, Rotation rotation)
        {
            if (rotation == Rotation.R)
            {
                if (direction == Direction.W)
                {
                    return Direction.N;
                }

                direction++;
            }
            else
            {
                if (direction == Direction.N)
                {
                    return Direction.W;
                }

                direction--;
            }

            return direction;
        }
    }
}