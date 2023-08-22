using Guestline.Games.Battleships.Server.Abstractions;
using Guestline.Games.Battleships.Server.Models;
using Guestline.Games.Battleships.Server.Models.Enums;

namespace Guestline.Games.Battleships.Server.Services
{
    public class ShipService : IShipService
    {
        private const int BattleshipSize = 5;
        private const int DestroyerSize = 4;

        private readonly IRandomWrapper _random;
        private readonly ILogger<ShipService> _logger;
        public ShipService(IRandomWrapper random, ILogger<ShipService> logger)
        {
            _random = random;
            _logger = logger;
        }

        public List<Ship> GetShipsForGame(int gameSize, int numberOfDestroyers, int numberOfBattleships)
        {
            var ships = new List<Ship>();

            CreateShips(ships, gameSize, BattleshipSize, numberOfBattleships);
            CreateShips(ships, gameSize, DestroyerSize, numberOfDestroyers);

            return ships;
        }

        private void CreateShips(List<Ship> existingShips, int gameSize, int shipSize, int shipCount)
        {
            for (int i = 0; i < shipCount; i++)
            {
                Ship? newShip;
                do
                {
                    int beginX = _random.Next(0, gameSize);
                    int beginY = _random.Next(0, gameSize);
                    var orientation = (Orientation)_random.Next(0, 2);

                    newShip = TryCreateShip(beginX, beginY, shipSize, gameSize, orientation, existingShips);
                } while (newShip == null);

                existingShips.Add(newShip);
            }
        }

        private Ship? TryCreateShip(int beginX, int beginY, int shipSize, int gameSize, Orientation orientation, List<Ship> existingShips)
        {
            if (!IsValidPlacement(beginX, beginY, shipSize, gameSize, orientation, existingShips))
                return null;

            return GenerateShip(beginX, beginY, shipSize, orientation);
        }


        private bool IsValidPlacement(int beginX, int beginY, int shipSize, int gameSize, Orientation orientation, List<Ship> existingShips)
        {
            if (orientation == Orientation.Horizontal && beginX + shipSize > gameSize)
                return false;

            if (orientation == Orientation.Vertical && beginY + shipSize > gameSize)
                return false;

            for (int i = 0; i < shipSize; i++)
            {
                int x = orientation == Orientation.Horizontal ? beginX + i : beginX;
                int y = orientation == Orientation.Vertical ? beginY + i : beginY;

                if (existingShips.Any(ship => ship.Positions.Contains(new Position(x, y))))
                    return false;
            }

            return true;
        }

        private Ship GenerateShip(int beginX, int beginY, int shipSize, Orientation orientation)
        {
            var positions = new HashSet<Position>();

            for (int i = 0; i < shipSize; i++)
            {
                int x = orientation == Orientation.Horizontal ? beginX + i : beginX;
                int y = orientation == Orientation.Vertical ? beginY + i : beginY;

                positions.Add(new Position(x, y));
            }

            _logger.LogInformation("Created ship with size {size} and orientation {orientation} starting at X position: {beginX} and Y position {beginY}", shipSize, orientation, beginX, beginY);

            return new Ship(positions);
        }
    }
}
