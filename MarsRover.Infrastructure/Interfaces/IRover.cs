using MarsRover.Core.Models;

namespace MarsRover.Infrastructure.Interfaces
{
    public interface IRover
    {
        void SetPosition(int x, int y, char direction);
        Position GetPosition();
        void Move(string? instructions);
    }
}
