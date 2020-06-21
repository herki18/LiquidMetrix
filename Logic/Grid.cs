using System.Collections.Generic;
using System.Linq;
using LiquidMetrix.Data;
using LiquidMetrix.Enums;
using Microsoft.Extensions.Logging;

namespace LiquidMetrix.Logic
{
    public interface IGrid
    {
        StatusCode CanMove(Vector2 position);
        void SetRoverPosition(Rover rover);
    }

    public class Grid : IGrid
    {
        private readonly ILogger<Grid> _logger;
        private Vector2 BottomLeftBound { get; }
        private Vector2 TopRightBound { get; }

        private readonly Dictionary<int, Vector2> _entities = new Dictionary<int, Vector2>();

        public Grid(Vector2 size, ILogger<Grid> logger)
        {
            _logger = logger;
            BottomLeftBound = new Vector2(0, 0);
            TopRightBound = new Vector2(size.X, size.Y);
        }

        public StatusCode CanMove(Vector2 position)
        {
            if (!(position.X >= BottomLeftBound.X && position.X <= TopRightBound.X &&
                 position.Y >= BottomLeftBound.Y && position.Y <= TopRightBound.Y))
            {
                _logger.LogWarning($"Rover position is Out of Bounds {position.ToString()}");
                return StatusCode.OutOfBounds;
            }

            if (_entities.Count != 0 && _entities.ContainsValue(position))
            {
                var rover = _entities.First(x => x.Value == position);
                _logger.LogWarning($"Rover {rover.Key} is occupying {position}");
                return StatusCode.PositionIsOccupied;
            }

            return StatusCode.Successful;
        }

        public void SetRoverPosition(Rover rover)
        {
            if (_entities.ContainsKey(rover.Id))
            {
                _entities[rover.Id] = rover.Position;
            }
            else
            {
                _entities.Add(rover.Id, rover.Position);
            }
        }
    }
}
