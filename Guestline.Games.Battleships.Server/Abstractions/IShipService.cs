using Guestline.Games.Battleships.Server.Models;

namespace Guestline.Games.Battleships.Server.Abstractions
{
    public interface IShipService
    {
        List<Ship> GetShipsForGame(int gameSize, int numberOfDestroyers, int numberOfBattleships);
    }
}