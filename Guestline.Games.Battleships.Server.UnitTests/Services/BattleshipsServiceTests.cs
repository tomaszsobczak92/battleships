using Guestline.Games.Battleships.Server.Abstractions;
using Guestline.Games.Battleships.Server.DTOs.Requests;
using Guestline.Games.Battleships.Server.Models.Enums;
using Guestline.Games.Battleships.Server.Models;
using Guestline.Games.Battleships.Server.Services;
using Moq;
using Microsoft.Extensions.Logging;

namespace Guestline.Games.Battleships.Server.UnitTests.Services
{
    public class BattleshipsServiceTests
    {
        private readonly Mock<IShipService> _shipServiceMock;
        private readonly Mock<IMemoryCacheWrapper> _memoryCacheWrapperMock;
        private readonly Mock<ILogger<BattleshipsService>> _logger;
        private readonly BattleshipsService _battleshipsService;

        public BattleshipsServiceTests()
        {
            _logger = new Mock<ILogger<BattleshipsService>>();
            _shipServiceMock = new Mock<IShipService>();
            _memoryCacheWrapperMock = new Mock<IMemoryCacheWrapper>();

            _battleshipsService = new BattleshipsService(_shipServiceMock.Object, _memoryCacheWrapperMock.Object, _logger.Object);
        }

        [Fact]
        public void StartGame_ShouldStartANewGame()
        {
            // Arange
            var ships = new List<Ship>();
            _shipServiceMock.Setup(ss => ss.GetShipsForGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(ships);

            // Act
            var response = _battleshipsService.StartGame();

            // Assert
            Assert.NotEqual(Guid.Empty, response.GameId);
            _memoryCacheWrapperMock.Verify(mc => mc.Add(response.GameId, ships, null), Times.Once);
        }

        [Fact]
        public void TryHit_Missed_ShouldReturnMissedResult()
        {
            // Arange
            var ships = new List<Ship>();
            _memoryCacheWrapperMock.Setup(mc => mc.Get<List<Ship>>(It.IsAny<Guid>())).Returns(ships);

            // Act
            var response = _battleshipsService.TryHit(new TryHitRequest { GameId = Guid.NewGuid(), Position = new Position(0, 0) });

            // Assert
            Assert.Equal(HitResult.Miss.ToString(), response.HitResult);
        }

        [Fact]
        public void TryHit_HitButNotDestroyed_ShouldReturnHitResult()
        {
            // Arrange
            var ships = new List<Ship> { new Ship(new HashSet<Position> { new Position(0, 0), new Position(0, 1) }) };
            _memoryCacheWrapperMock.Setup(mc => mc.Get<List<Ship>>(It.IsAny<Guid>())).Returns(ships);

            // Act
            var response = _battleshipsService.TryHit(new TryHitRequest { GameId = Guid.NewGuid(), Position = new Position(0, 0) });

            // Assert
            Assert.Equal(HitResult.Hit.ToString(), response.HitResult);
        }

        [Fact]
        public void TryHit_DestroyedShip_ShouldReturnDestroyedResult()
        {
            // Arange
            var ships = new List<Ship>
            {
                new Ship(new HashSet<Position> { new Position(0, 0) }),
                new Ship(new HashSet<Position> { new Position(1, 1) })
            };

            _memoryCacheWrapperMock.Setup(mc => mc.Get<List<Ship>>(It.IsAny<Guid>())).Returns(ships);

            // Act
            var response = _battleshipsService.TryHit(new TryHitRequest { GameId = Guid.NewGuid(), Position = new Position(0, 0) });

            // Assert
            Assert.Equal(HitResult.Sink.ToString(), response.HitResult);
        }

        [Fact]
        public void TryHit_DestroyAllShips_ShouldReturnGameOverResult()
        {
            // Arrange
            var ships = new List<Ship>
            {
                new Ship(new HashSet<Position> { new Position(0, 0) }),
                new Ship(new HashSet<Position> { new Position(1, 1) })
            };

            _memoryCacheWrapperMock.Setup(mc => mc.Get<List<Ship>>(It.IsAny<Guid>())).Returns(ships);

            _battleshipsService.TryHit(new TryHitRequest { GameId = Guid.NewGuid(), Position = new Position(0, 0) });
            
            // Act
            var response = _battleshipsService.TryHit(new TryHitRequest { GameId = Guid.NewGuid(), Position = new Position(1, 1) });

            // Assert
            Assert.Equal(HitResult.GameOver.ToString(), response.HitResult);
        }
    }
}
