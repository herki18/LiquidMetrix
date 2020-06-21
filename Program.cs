using System;
using LiquidMetrix.Data;
using LiquidMetrix.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LiquidMetrix
{
    class Program
    {
        static void Main(string[] args)
        {
            var gridSize = new Vector2(40, 30);

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(l => l.AddConsole())
                .Configure<LoggerFilterOptions>(c => c.MinLevel = LogLevel.Trace)
                .AddScoped<IGrid>(g => new Grid(gridSize, g.GetService<ILogger<Grid>>()))
                .AddScoped<IActionFactory, ActionFactory>()
                .AddScoped<IRoverManager, RoverManager>()
                .AddScoped<IRoverMover, RoverMover>()
                .AddScoped<IOutput, RoverTransformStringOutput>()
                .AddScoped<Simulation>()
                .BuildServiceProvider();

            var simulation = serviceProvider.GetRequiredService<Simulation>();
            
            Console.WriteLine("Rover Control Center");
            GetCommands();
            bool exit = false;
            while (!exit)
            {
                string input = Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(input))
                {
                    if (input.Contains("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        exit = true;
                    }else if (input.Contains("clear", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                    }else if (input.Contains("help", StringComparison.OrdinalIgnoreCase))
                    {
                        GetCommands();
                    }
                    else
                    {
                        simulation.Update(input);
                    }
                }
            }
        }

        private static void GetCommands()
        {
            Console.WriteLine("Supported commands:");
            Console.WriteLine("1. Example: \"0 0 N\" Create new rover and set initial position ");
            Console.WriteLine("2. Example: \"R1R3L2L1\" Create new rover and set initial position [x y Direction]");
            Console.WriteLine("3. Example: \"[Rr]overs\" Get all rovers");
            Console.WriteLine("4. Example: \"[Cc]hange 1\" Change active rover");
            Console.WriteLine("5. Example: \"[Ss]elected\" Get active rover");
            Console.WriteLine("6. Example@ \"[Ee]xit\" Exit program");
            Console.WriteLine("7. Example@ \"[Cc]lear\" Clear console");
            Console.WriteLine("7. Example@ \"[Hh]elp\" Help");
        }
    }
}
