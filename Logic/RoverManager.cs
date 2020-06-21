using System.Collections.Generic;
using LiquidMetrix.Actions;
using LiquidMetrix.Data;
using Microsoft.Extensions.Logging;

namespace LiquidMetrix.Logic
{
    public interface IRoverManager
    {
        Dictionary<int, Rover> Rovers { get; }
        Rover SelectedRover { get; }
        int SelectedRoverId { get; set; }
        Rover CreateRover(SetPositionAction setPosition);
        void UpdateRover(Rover rover);
    }

    public class RoverManager : IRoverManager
    {
        private readonly ILogger<RoverManager> _logger;
        public Dictionary<int, Rover> Rovers { get; } = new Dictionary<int, Rover>();

        public Rover SelectedRover => Rovers[SelectedRoverId];

        public int SelectedRoverId
        {
            get => _selectedRoverId;
            set
            {
                if (Rovers.ContainsKey(value))
                    _selectedRoverId = value;
                else
                    _logger.LogWarning($"Rover does not exist with this id: {value}");
                    
            }
        }

        private int _latestId;
        private int _selectedRoverId;

        public RoverManager(ILogger<RoverManager> logger)
        {
            _logger = logger;
        }

        public Rover CreateRover(SetPositionAction setPosition)
        {
            var rover = new Rover(_latestId++)
            {
                Direction = setPosition.Direction,
                Position = setPosition.Position
            };
            Rovers.Add(rover.Id, rover);
            SelectedRoverId = rover.Id;
            return SelectedRover;
        }

        public void UpdateRover(Rover rover)
        {
            if (Rovers.ContainsKey(rover.Id))
            {
                Rovers[rover.Id] = rover;
                SelectedRoverId = rover.Id;
            }
        }
    }
}
