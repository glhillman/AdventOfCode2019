using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Coordinates
    {
        public Coordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int AbsSum
        {
            get {return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z); }
        }

        public override string ToString()
        {
            return string.Format("X,Y,Z: {0},{1},{2}", X, Y, Z);
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    public class Moon
    {
        public Moon (string name, Coordinates position)
        {
            Name = name;
            Position = position;
            Velocity = new Coordinates(0, 0, 0);
            AnchorPosition = new Coordinates(position.X, position.Y, position.Z);
        }

        public void ApplyGravity(Moon other)
        {
            ApplyGravityInternal(this, other);
            ApplyGravityInternal(other, this);
        }

        private void ApplyGravityInternal(Moon a, Moon b)
        {
            int diff = a.Position.X - b.Position.X;
            a.Velocity.X += (diff < 0) ? 1 : (diff > 0 ? -1 : 0);

            diff = a.Position.Y - b.Position.Y;
            a.Velocity.Y += (diff < 0) ? 1 : (diff > 0 ? -1 : 0);

            diff = a.Position.Z - b.Position.Z;
            a.Velocity.Z += (diff < 0) ? 1 : (diff > 0 ? -1 : 0);
        }

        public void ApplyVelocity()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            Position.Z += Velocity.Z;
        }

        public int TotalEnergy
        {
            get { return Position.AbsSum * Velocity.AbsSum; }
        }

        public string Name { get; private set; }
        public Coordinates Position { get; private set; }
        public Coordinates Velocity { get; private set; }

        public Coordinates AnchorPosition { get; set; }

        public override string ToString()
        {
            return string.Format("{0} Pos X,Y,Z: {1},{2},{3}, Vel X,Y,Z: {4},{5},{6}", Name, Position.X, Position.Y, Position.Z, Velocity.X, Velocity.Y, Velocity.Z);
        }
    }
}
