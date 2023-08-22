namespace Guestline.Games.Battleships.Server.DTOs.Responses
{
    public record StartNewGameResponse
    {
        public Guid GameId { get; set; }
    }
}