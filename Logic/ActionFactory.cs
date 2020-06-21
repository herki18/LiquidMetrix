using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LiquidMetrix.Actions;
using LiquidMetrix.Data;
using LiquidMetrix.Enums;
using Microsoft.Extensions.Logging;

namespace LiquidMetrix.Logic
{
    public interface IActionFactory
    {
        StatusCode Translate(string input, out IAction action);
    }

    public class ActionFactory : IActionFactory
    {
        private readonly ILogger<ActionFactory> _logger;

        public ActionFactory(ILogger<ActionFactory> logger)
        {
            _logger = logger;
        }

        public StatusCode Translate(string input, out IAction action)
        {
            input = input.Trim('[', ']');
            if (Regex.IsMatch(input, @"^\d+"))
            {
                
                var values = input.Split(" ");
                if(int.TryParse(values[0], out var x) 
                   && int.TryParse(values[1], out var y)
                   && (Regex.IsMatch(values[2], "^[NSWE]") && Enum.TryParse(typeof(Direction), values[2], out var direction)))
                {
                    action = new SetPositionAction(
                        new Vector2(x, y),
                        (Direction)direction);
                    
                    return StatusCode.Successful;
                }
                _logger.LogWarning($"Input string is Invalid {input}");
                action = null;
                return StatusCode.InvalidInput;
            }
            else if (Regex.IsMatch(input, @"^[LR]"))
            {
                var strings = Regex.Split(input, @"(?<!^)(?=[A-Z])");
                var move = new MoveAction(new List<Move>());
                foreach (var str in strings)
                {
                    var rotation = str.Substring(0, 1);
                    var stepsStr = str.Substring(1);
                    if(int.TryParse(stepsStr, out var steps))
                    {
                        move.MoveActions.Add(new Move(Enum.Parse<Rotation>(rotation), Convert.ToInt32(steps)));
                    }
                    else
                    {
                        _logger.LogWarning($"Input string is Invalid {input}");
                        action = null;
                        return StatusCode.InvalidInput;
                    }
                }

                action = move;
                return StatusCode.Successful;
            }
            _logger.LogWarning($"Input string is Invalid {input}");
            action = null;
            return StatusCode.InvalidInput;
        }
    }
}
