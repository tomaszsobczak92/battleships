namespace Guestline.Games.Battleships.Server.Abstractions
{
    public interface IRandomWrapper
    {
        int Next(int minValue, int maxValue);
    }
}