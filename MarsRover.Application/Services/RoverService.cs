using MarsRover.Core.Models;
using MarsRover.Infrastructure.Interfaces;

namespace MarsRover.Application.Services
{
    public class RoverService : IRoverService
    {
        private readonly IRover _rover;

        public RoverService(IRover rover)
        {
            _rover = rover;
        }

        public Position MoveRover(Position initialPosition, string? instructions)
        {
            _rover.SetPosition(initialPosition.X, initialPosition.Y, initialPosition.Direction);

            try
            {
                _rover.Move(instructions);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Invalid instructions", ex);
            }

            return _rover.GetPosition();
        }
    }
}
