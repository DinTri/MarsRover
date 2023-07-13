using MarsRover.Infrastructure.Interfaces;

namespace MarsRover.Infrastructure.Utils
{
    public class StringInputReader : IInputReader
    {
        public string? ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
