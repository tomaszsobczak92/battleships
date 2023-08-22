using Guestline.Games.Battleships.Server.Abstractions;

namespace Guestline.Games.Battleships.Server.Infrastructures
{
    public class RandomWrapper : IRandomWrapper
    {
        private readonly Random _random = new Random();

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
