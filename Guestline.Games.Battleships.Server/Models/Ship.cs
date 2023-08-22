namespace Guestline.Games.Battleships.Server.Models
{
    public class Ship
    {
        public HashSet<Position> Positions { get; private set; }
        public HashSet<Position> HitList { get; private set; }

        public bool IsDestroyed => Positions.Count() == HitList.Count();


        public Ship(HashSet<Position> localization)
        {
            Positions = localization;
            HitList = new HashSet<Position>();
        }
    }
}
