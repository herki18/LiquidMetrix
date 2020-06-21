using System;
using LiquidMetrix.Actions;
using LiquidMetrix.Enums;
using Microsoft.Extensions.Logging;

namespace LiquidMetrix.Logic
{
    public class Simulation
    {
        private readonly ILogger<Simulation> _logger;
        private readonly IActionFactory _action;
        private readonly IRoverManager _roverManager;
        private readonly IOutput _output;
        private readonly IRoverMover _roverMover;

        public Simulation(ILogger<Simulation> logger, IGrid grid, IActionFactory action, IRoverManager roverManager,
            IOutput roverTransformStringOutput)
        {
            _logger = logger;
            _action = action;
            _roverManager = roverManager;
            _output = roverTransformStringOutput;
            _roverMover = new RoverMover(grid, _roverManager);
        }

        public void Update(string input)
        {
            if (input.Contains("Rovers", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var rover in _roverManager.Rovers)
                {
                    Console.WriteLine($"Rover {rover.Key}, {_output.GetTransform(rover.Value)}");
                }
            }
            else if (input.Contains("Change", StringComparison.OrdinalIgnoreCase))
            {
                var str = input.Remove(0, 7);
                if (int.TryParse(str, out var id))
                {
                    _roverManager.SelectedRoverId = id;
                    Console.WriteLine($"Rover {_roverManager.SelectedRover.Id}, {_output.GetTransform(_roverManager.SelectedRover)}");
                }
                else
                {
                    _logger.LogWarning($"Input is in valid: {input}");
                }
            }
            else if (input.Contains("Selected", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Rover {_roverManager.SelectedRover.Id}, {_output.GetTransform(_roverManager.SelectedRover)}");
            }
            else
            {
                StatusCode statusCode = _action.Translate(input, out var action);

                if (action is SetPositionAction setPosition)
                {
                    statusCode = _roverMover.SetPosition(setPosition);
                }
                else if (action is MoveAction move)
                {
                    statusCode = _roverMover.Move(move);
                }

                if (statusCode == StatusCode.Successful)
                {
                    Console.WriteLine($"Rover {_roverManager.SelectedRover.Id} {_output.GetTransform(_roverManager.SelectedRover)}");
                }
                else
                {
                    _logger.LogWarning($"StatusCode: {statusCode}");
                }
            }
        }
    }
}
