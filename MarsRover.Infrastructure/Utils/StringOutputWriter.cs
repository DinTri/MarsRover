using MarsRover.Infrastructure.Interfaces;

namespace MarsRover.Infrastructure.Utils
{
    public class StringOutputWriter : IOutputWriter
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
