namespace Guestline.Games.Battleships.Server.Abstractions
{
    public interface IMemoryCacheWrapper
    {
        void Add(Guid key, object value, TimeSpan? expiration = null);
        T Get<T>(Guid key) where T : class;
    }
}