using Guestline.Games.Battleships.Server.Abstractions;
using Guestline.Games.Battleships.Server.DTOs.Requests;
using Guestline.Games.Battleships.Server.DTOs.Responses;
using Guestline.Games.Battleships.Server.Models;
using Guestline.Games.Battleships.Server.Models.Enums;

namespace Guestline.Games.Battleships.Server.Services
{
    public class BattleshipsService : IBattleshipService
    {
        private readonly IShipService _shipService;
        private readonly IMemoryCacheWrapper _memoryCacheWrapper;
        private readonly ILogger<BattleshipsService> _logger;

        private const int GameSize = 10;
        private const int NumberOfBattleships = 1;
        private const int NumberOfDestroyers = 2;

        public BattleshipsService(IShipService shipService, IMemoryCacheWrapper memoryCacheWrapper, ILogger<BattleshipsService> logger)
        {
            _shipService = shipService;
            _memoryCacheWrapper = memoryCacheWrapper;
            _logger = logger;
        }

        public StartNewGameResponse StartGame()
        {
            var gameId = Guid.NewGuid();

            var gameShips = _shipService.GetShipsForGame(GameSize, NumberOfDestroyers, NumberOfBattleships);

            _memoryCacheWrapper.Add(gameId, gameShips);

            _logger.LogInformation("Successfuly created game with id: {gameId}", gameId);
            
            return new StartNewGameResponse() { GameId = gameId };
        }

        public TryHitResponse TryHit(TryHitRequest tryHitRequest)
        {
            if (tryHitRequest == null || tryHitRequest.Position == null)
            {
                _logger.LogError("TryHitRequest is null or without position");
                throw new ArgumentNullException(nameof(tryHitRequest));
            }

            _logger.LogInformation("Recivied TryHitRequest with gameId: {gameId}, position X: {x} and position Y: {y}", tryHitRequest.GameId, tryHitRequest.Position.X, tryHitRequest.Position.Y);

            var result = HandleTryHit(tryHitRequest);

            _logger.LogInformation("TryHitRequest for gameId: {gameId} resulted as {result}", tryHitRequest.GameId, result.HitResult);

            return result;
        }

        private TryHitResponse HandleTryHit(TryHitRequest tryHitRequest)
        {
           
            var ships = _memoryCacheWrapper.Get<List<Ship>>(tryHitRequest.GameId);

            var shipHitted = ships.Where(ship => ship.Positions.Any(pos => pos == tryHitRequest.Position)).FirstOrDefault();

            if (shipHitted == null)
            {
                return new TryHitResponse(HitResult.Miss, tryHitRequest.Position);
            }

            shipHitted.HitList.Add(tryHitRequest.Position);

            if (ships.All(x => x.IsDestroyed))
            {
                return new TryHitResponse(HitResult.GameOver);
            }

            if (shipHitted.IsDestroyed)
            {
                return new TryHitResponse(HitResult.Sink, shipHitted.Positions);
            }

            return new TryHitResponse(HitResult.Hit, tryHitRequest.Position);
        }
    }
}
