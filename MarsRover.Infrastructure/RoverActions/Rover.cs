using MarsRover.Core.Models;
using MarsRover.Infrastructure.Interfaces;

namespace MarsRover.Infrastructure.RoverActions
{
    public class Rover : IRover
    {
        private int _x;
        private int _y;
        private char _direction;

        public void SetPosition(int x, int y, char direction)
        {
            _x = x;
            _y = y;
            _direction = direction;
        }

        public Position GetPosition()
        {
            return new Position { X = _x, Y = _y, Direction = _direction };
        }

        public void Move(string? instructions)
        {
            if (instructions == null) return;
            foreach (char instruction in instructions)
            {
                switch (instruction)
                {
                    case 'L':
                        RotateLeft();
                        break;
                    case 'R':
                        RotateRight();
                        break;
                    case 'M':
                        MoveForward();
                        break;
                    default:
                        throw new ArgumentException("Invalid instruction");
                }
            }
        }

        private void RotateLeft()
        {
            _direction = _direction switch
            {
                'N' => 'W',
                'E' => 'N',
                'S' => 'E',
                'W' => 'S',
                _ => _direction
            };
        }

        private void RotateRight()
        {
            _direction = _direction switch
            {
                'N' => 'E',
                'E' => 'S',
                'S' => 'W',
                'W' => 'N',
                _ => _direction
            };
        }

        private void MoveForward()
        {
            switch (_direction)
            {
                case 'N':
                    _y += 1;
                    break;
                case 'E':
                    _x += 1;
                    break;
                case 'S':
                    _y -= 1;
                    break;
                case 'W':
                    _x -= 1;
                    break;
            }
        }
    }
}
