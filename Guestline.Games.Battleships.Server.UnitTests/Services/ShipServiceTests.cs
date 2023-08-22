using Guestline.Games.Battleships.Server.Abstractions;
using Guestline.Games.Battleships.Server.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Guestline.Games.Battleships.Server.UnitTests.Services
{
    public class ShipServiceTests
    {
        private readonly Mock<IRandomWrapper> _randomMock;
        private readonly Mock<ILogger<ShipService>> _logger;
        private readonly ShipService _shipService;

        public ShipServiceTests()
        {
            _randomMock = new Mock<IRandomWrapper>();
            _logger = new Mock<ILogger<ShipService>>();

            _shipService = new ShipService(_randomMock.Object, _logger.Object);
        }

        [Fact]
        public void GetShipsForGame_WithParams_ReturnsCorrectShips()
        {
            // Arange
            int gameSize = 10;
            int numberOfBattleships = 1;
            int numberOfDestroyers = 1;

            _randomMock.SetupSequence(r => r.Next(0, gameSize))
                .Returns(0)   // 1st ship X position
                .Returns(0)   // 1st ship Y position
                .Returns(0)   // 2nd ship X position
                .Returns(1);  // 2nd ship Y position

            _randomMock.SetupSequence(r => r.Next(0, 2))
                .Returns(0)  // 1st ship orientation, 0 is horizontal and 1 is vertical
                .Returns(0); // 2st ship orientation

            // Act
            var ships = _shipService.GetShipsForGame(gameSize, numberOfDestroyers, numberOfBattleships);

            // Assert
            Assert.Equal(numberOfDestroyers + numberOfBattleships, ships.Count);

            var battleShip = ships[0];
            var destroyer = ships[1];

            Assert.Equal(5, battleShip.Positions.Count);
            Assert.Equal(4, destroyer.Positions.Count);

            Assert.True(battleShip.Positions.All(x => x.X <= 4));
            Assert.True(battleShip.Positions.All(x => x.Y == 0));

            Assert.True(destroyer.Positions.All(x => x.X <= 3));
            Assert.True(destroyer.Positions.All(x => x.Y == 1));
        }

        [Fact]
        public void GetShipsForGame_WhenOverlapping_ReturnsCorrectShips()
        {
            // Arange
            int gameSize = 10;
            int numberOfBattleships = 1;
            int numberOfDestroyers = 1;

            _randomMock.SetupSequence(r => r.Next(0, gameSize))
                .Returns(0)   // 1st ship X position
                .Returns(0)   // 1st ship Y position
                .Returns(3)   // 2nd ship X position - 1st attempt
                .Returns(0)   // 2nd ship Y position - 1st attempt
                .Returns(3)   // 2nd ship X position - 2st attempt
                .Returns(1);  // 2nd ship Y position - 2st attempt

            _randomMock.SetupSequence(r => r.Next(0, 2))
                .Returns(0)  // 1st ship orientation, 0 is horizontal and 1 is vertical
                .Returns(0); // 2st ship orientation

            // Act
            var ships = _shipService.GetShipsForGame(gameSize, numberOfDestroyers, numberOfBattleships);

            // Assert
            Assert.Equal(numberOfDestroyers + numberOfBattleships, ships.Count);

            var battleShip = ships[0];
            var destroyer = ships[1];

            Assert.Equal(5, battleShip.Positions.Count);
            Assert.Equal(4, destroyer.Positions.Count);

            Assert.True(battleShip.Positions.All(x => x.X <= 4));
            Assert.True(battleShip.Positions.All(x => x.Y == 0));

            Assert.True(destroyer.Positions.All(x => x.X >= 3 && x.X <= 7));
            Assert.True(destroyer.Positions.All(x => x.Y == 1));
        }
    }
}
