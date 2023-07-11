using MarsRover.Application.Services;
using MarsRover.Core.Models;
using MarsRover.Infrastructure.Interfaces;
using MarsRover.Infrastructure.RoverActions;
using MarsRover.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace MarsRover.Cmd
{
    public class Program
    {
        private readonly IRoverService _roverService;
        private readonly IOutputWriter _outputWriter;
        private readonly IInputReader _inputReader;

        public Program(IRoverService roverService, IOutputWriter outputWriter, IInputReader inputReader)
        {
            _roverService = roverService;
            _outputWriter = outputWriter;
            _inputReader = inputReader;
        }

        public void Main()
        {
            // Read the plateau coordinates
            _outputWriter.WriteLine("Enter the data for the plateau and rovers (format: plateauX plateauY rover1X rover1Y rover1Orientation rover1Instructions rover2X rover2Y rover2Orientation rover2Instructions ...):");
            string plateauCoordinates = _inputReader.ReadLine();
            if (string.IsNullOrWhiteSpace(plateauCoordinates))
            {
                _outputWriter.WriteLine("Invalid plateau coordinates");
                return;
            }

            // Parse the plateau coordinates
            string[] plateauCoords = plateauCoordinates.Split(' ');
            if (plateauCoords.Length != 2 || !int.TryParse(plateauCoords[0], out int plateauX) || !int.TryParse(plateauCoords[1], out int plateauY))
            {
                _outputWriter.WriteLine("Invalid plateau coordinates");
                return;
            }

            // Create a list to store the final positions
            List<Position> finalPositions = new List<Position>();

            // Loop through the rovers
            while (true)
            {
                // Read the rover's position
                string roverPosition = _inputReader.ReadLine();
                if (string.IsNullOrWhiteSpace(roverPosition))
                {
                    // Reached the end of input
                    break;
                }

                // Parse the rover's position
                string[] position = roverPosition.Split(' ');
                if (position.Length != 3 || !int.TryParse(position[0], out int roverX) || !int.TryParse(position[1], out int roverY) || position[2].Length != 1)
                {
                    _outputWriter.WriteLine("Invalid rover position");
                    continue;
                }
                char roverDirection = position[2][0];

                // Create a new position object
                var initialPosition = new Position
                {
                    X = roverX,
                    Y = roverY,
                    Direction = roverDirection
                };

                // Read the instructions
                string instructions = _inputReader.ReadLine();
                if (string.IsNullOrWhiteSpace(instructions))
                {
                    _outputWriter.WriteLine("Missing instructions");
                    continue;
                }

                // Move the rover and get the final position
                Position finalPosition;
                try
                {
                    finalPosition = _roverService.MoveRover(initialPosition, instructions);
                    finalPositions.Add(finalPosition);
                }
                catch (ArgumentException ex)
                {
                    _outputWriter.WriteLine("Error: " + ex.Message);
                }
            }

            // Print the final positions
            foreach (var position in finalPositions)
            {
                _outputWriter.WriteLine($"Rover final position: {position.X} {position.Y} {position.Direction}");

            }
        }
    }

    public static class ProgramEntryPoint
    {
        public static void Main()
        {
            var serviceProvider = ConfigureServices();
            if (serviceProvider != null)
            {
                var program = serviceProvider.GetService<Program>();
                program?.Main();
            }

            DisposeServices(serviceProvider);
        }

        private static IServiceProvider? ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register the dependencies
            services.AddSingleton<IRover, Rover>();
            services.AddSingleton<IRoverService, RoverService>();
            services.AddSingleton<IOutputWriter, StringOutputWriter>();
            services.AddSingleton<IInputReader, StringInputReader>();
            services.AddSingleton<Program>();

            return services.BuildServiceProvider();
        }

        private static void DisposeServices(IServiceProvider? serviceProvider)
        {
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
