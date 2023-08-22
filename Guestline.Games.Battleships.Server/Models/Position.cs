namespace Guestline.Games.Battleships.Server.Models
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Position other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        public static bool operator ==(Position? p1, Position? p2)
        {
            if (ReferenceEquals(p1, p2))
            {
                return true;
            }

            if (p1 is null || p2 is null)
            {
                return false;
            }

            return p1.Equals(p2);
        }

        public static bool operator !=(Position? p1, Position? p2)
        {
            return !(p1 == p2);
        }
    }
}
