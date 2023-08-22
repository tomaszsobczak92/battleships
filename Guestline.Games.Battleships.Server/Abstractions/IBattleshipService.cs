using Guestline.Games.Battleships.Server.DTOs.Requests;
using Guestline.Games.Battleships.Server.DTOs.Responses;

namespace Guestline.Games.Battleships.Server.Abstractions
{
    public interface IBattleshipService
    {
        StartNewGameResponse StartGame();
        TryHitResponse TryHit(TryHitRequest tryHitRequest);
    }
}