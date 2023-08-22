using Guestline.Games.Battleships.Server.Models;

namespace Guestline.Games.Battleships.Server.DTOs.Requests
{
    public record TryHitRequest
    {
        public Guid GameId { get; set; }
        public Position? Position { get; set; }
    }
}
