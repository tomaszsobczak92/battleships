using Guestline.Games.Battleships.Server.Models;
using Guestline.Games.Battleships.Server.Models.Enums;

namespace Guestline.Games.Battleships.Server.DTOs.Responses
{
    public record TryHitResponse
    {
        public TryHitResponse(HitResult hitResult, IEnumerable<Position> positions)
        {
            HitResult = hitResult.ToString();
            Positions = positions;
        }

        public TryHitResponse(HitResult hitResult)
        {
            HitResult = hitResult.ToString();
        }

        public TryHitResponse(HitResult hitResult, Position position)
        {
            HitResult = hitResult.ToString();
            Positions = new[] { position };
        }

        public TryHitResponse()
        {

        }

        public string? HitResult { get; set; }
        public IEnumerable<Position>? Positions { get; set; }
    }
}
