using Guestline.Games.Battleships.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guestline.Games.Battleships.Server.UnitTests.Models
{
    public class PositionTests
    {
        [Fact]
        public void Equals_GivenSameXAndY_ReturnsTrue()
        {
            // Arrange
            var position1 = new Position(1, 2);
            var position2 = new Position(1, 2);

            // Act
            var result = position1.Equals(position2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_GivenDifferentXOrY_ReturnsFalse()
        {
            // Arrange
            var position1 = new Position(1, 2);
            var position2 = new Position(2, 2);

            // Act
            var result = position1.Equals(position2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_GivenNonPositionObject_ReturnsFalse()
        {
            // Arrange
            var position = new Position(1, 2);
            object notAPosition = new object();

            // Act
            var result = position.Equals(notAPosition);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OperatorEquals_GivenSamePositions_ReturnsTrue()
        {
            // Arrange
            var position1 = new Position(1, 2);
            var position2 = new Position(1, 2);

            // Act
            var result = position1 == position2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorNotEquals_GivenDifferentPositions_ReturnsTrue()
        {
            // Arrange
            var position1 = new Position(1, 2);
            var position2 = new Position(2, 3);

            // Act
            var result = position1 != position2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCode_GivenSameXAndY_ReturnsEqualHashCodes()
        {
            // Arrange
            var position1 = new Position(1, 2);
            var position2 = new Position(1, 2);

            // Act
            var hash1 = position1.GetHashCode();
            var hash2 = position2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }
    }
}