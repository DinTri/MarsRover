namespace MarsRover.Infrastructure.Interfaces
{
    public interface IOutputWriter
    {
        void WriteLine(string value);
        void Write(string value);
    }
}
