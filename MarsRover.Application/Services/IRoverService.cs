using MarsRover.Core.Models;

namespace MarsRover.Application.Services
{
    public interface IRoverService
    {
        Position MoveRover(Position initialPosition, string? instructions);
    }
}
