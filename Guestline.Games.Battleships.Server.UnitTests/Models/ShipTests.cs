using Guestline.Games.Battleships.Server.Models;

namespace Guestline.Games.Battleships.Server.UnitTests.Models
{
    public class ShipTests
    {
        [Fact]
        public void IsDestroyed_WhenNotAllPositionsHit_ReturnsFalse()
        {
            // Arrange
            var positions = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(1, 2),
            };
            var ship = new Ship(positions);
            ship.HitList.Add(new Position(1, 1));

            // Act
            var isDestroyed = ship.IsDestroyed;

            // Assert
            Assert.False(isDestroyed);
        }

        [Fact]
        public void IsDestroyed_WhenAllPositionsHit_ReturnsTrue()
        {
            // Arrange
            var positions = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(1, 2),
            };
            var ship = new Ship(positions);
            ship.HitList.Add(new Position(1, 1));
            ship.HitList.Add(new Position(1, 2));

            // Act
            var isDestroyed = ship.IsDestroyed;

            // Assert
            Assert.True(isDestroyed);
        }

        [Fact]
        public void Constructor_InitializesHitListAsEmpty()
        {
            // Arrange
            var positions = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(1, 2),
            };

            // Act
            var ship = new Ship(positions);

            // Assert
            Assert.Empty(ship.HitList);
        }
    }
}
