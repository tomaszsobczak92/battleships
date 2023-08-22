using Guestline.Games.Battleships.Server.Abstractions;
using Guestline.Games.Battleships.Server.DTOs.Requests;
using Guestline.Games.Battleships.Server.DTOs.Responses;
using Microsoft.AspNetCore.SignalR;

namespace Guestline.Games.Battleships.Server.Hubs
{
    public class BattleshipsGameHub : Hub
    {
        private readonly IBattleshipService _gameService;

        public BattleshipsGameHub(IBattleshipService gameService)
        {
            _gameService = gameService;
        }

        public StartNewGameResponse StartGame()
        {
            var response = _gameService.StartGame();

            return response;
        }

        public TryHitResponse TryHit(TryHitRequest request)
        {
            var response = _gameService.TryHit(request);

            return response;
        }
    }
}
