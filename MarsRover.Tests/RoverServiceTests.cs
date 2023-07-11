using MarsRover.Application.Services;
using MarsRover.Cmd;
using MarsRover.Core.Models;
using MarsRover.Infrastructure.Interfaces;
using MarsRover.Infrastructure.RoverActions;
using Moq;

namespace MarsRover.Tests
{
    public class RoverServiceTests
    {
        [Fact]
        public void MoveRover_ValidInstructions_ReturnsFinalPosition()
        {
            // Arrange
            var initialPosition = new Position { X = 1, Y = 2, Direction = 'N' };
            var instructions = "LMLMLMLMM";
            var expectedFinalPosition = new Position { X = 1, Y = 3, Direction = 'N' };

            var mockRover = new Mock<IRover>();
            mockRover.Setup(r => r.SetPosition(initialPosition.X, initialPosition.Y, initialPosition.Direction));
            mockRover.Setup(r => r.Move(instructions));
            mockRover.Setup(r => r.GetPosition()).Returns(expectedFinalPosition);

            var roverService = new RoverService(mockRover.Object);

            // Act
            var finalPosition = roverService.MoveRover(initialPosition, instructions);

            // Assert
            Assert.Equal(expectedFinalPosition.X, finalPosition.X);
            Assert.Equal(expectedFinalPosition.Y, finalPosition.Y);
            Assert.Equal(expectedFinalPosition.Direction, finalPosition.Direction);
            mockRover.Verify(r => r.SetPosition(initialPosition.X, initialPosition.Y, initialPosition.Direction),
                Times.Once);
            mockRover.Verify(r => r.Move(instructions), Times.Once);
            mockRover.Verify(r => r.GetPosition(), Times.Once);
        }

        [Fact]
        public void MoveRover_Input_ReturnsExpectedFinalPosition()
        {
            // Arrange
            var rover = new Rover();
            var roverService = new RoverService(rover);

            var initialPosition1 = new Position { X = 1, Y = 2, Direction = 'N' };
            var instructions1 = "LMLMLMLMM";
            var expectedFinalPosition1 = new Position { X = 1, Y = 3, Direction = 'N' };

            var initialPosition2 = new Position { X = 3, Y = 3, Direction = 'E' };
            var instructions2 = "MMRMMRMRRM";
            var expectedFinalPosition2 = new Position { X = 5, Y = 1, Direction = 'E' };

            // Act
            var finalPosition1 = roverService.MoveRover(initialPosition1, instructions1);
            var finalPosition2 = roverService.MoveRover(initialPosition2, instructions2);

            // Assert
            Assert.Equal(expectedFinalPosition1.X, finalPosition1.X);
            Assert.Equal(expectedFinalPosition1.Y, finalPosition1.Y);
            Assert.Equal(expectedFinalPosition1.Direction, finalPosition1.Direction);

            Assert.Equal(expectedFinalPosition2.X, finalPosition2.X);
            Assert.Equal(expectedFinalPosition2.Y, finalPosition2.Y);
            Assert.Equal(expectedFinalPosition2.Direction, finalPosition2.Direction);
        }

        [Fact]
        public void MoveRover_Main_Input_ReturnsExpectedOutput()
        {
            // Arrange
            var input = "5 5\n1 2 N\nLMLMLMLMM\n3 3 E\nMMRMMRMRRM";

            var roverServiceMock = new Mock<IRoverService>();
            var outputWriterMock = new Mock<IOutputWriter>();
            var inputReaderMock = new Mock<IInputReader>();

            // Set up input reader to return lines from the input string
            var inputLines = input.Split('\n');
            var lineIndex = 0;
            inputReaderMock.Setup(r => r.ReadLine())
                .Returns(() => lineIndex < inputLines.Length ? inputLines[lineIndex++] : null);

            // Set up the expected final positions
            var finalPosition1 = new Position { X = 1, Y = 3, Direction = 'N' };
            var finalPosition2 = new Position { X = 5, Y = 1, Direction = 'E' };

            // Set up the rover service to return the expected final positions
            roverServiceMock.SetupSequence(s => s.MoveRover(It.IsAny<Position>(), It.IsAny<string>()))
                .Returns(finalPosition1)
                .Returns(finalPosition2);

            var program = new Program(roverServiceMock.Object, outputWriterMock.Object, inputReaderMock.Object);

            // Act
            program.Main();

            // Assert
            outputWriterMock.Verify(o => o.WriteLine("Rover final position: 1 3 N"), Times.Once);
            outputWriterMock.Verify(o => o.WriteLine("Rover final position: 5 1 E"), Times.Once);
        }

        [Fact]
        public void MoveRover_InvalidInstructions_ThrowsArgumentException()
        {
            // Arrange
            var initialPosition = new Position { X = 1, Y = 2, Direction = 'N' };
            var invalidInstructions = "LMXYZ";

            var mockRover = new Mock<IRover>();
            mockRover.Setup(r => r.Move(invalidInstructions))
                .Throws<ArgumentException>();

            var roverService = new RoverService(mockRover.Object);

            // Act and Assert
            var exception =
                Assert.Throws<ArgumentException>(() => roverService.MoveRover(initialPosition, invalidInstructions));
            Assert.Equal("Invalid instructions", exception.Message);

            mockRover.Verify(r => r.Move(invalidInstructions), Times.Once);
            mockRover.Verify(r => r.GetPosition(), Times.Never);
        }
    }
}
