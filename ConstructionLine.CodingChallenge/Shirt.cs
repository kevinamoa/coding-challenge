using System;

namespace ConstructionLine.CodingChallenge
{
    public class Shirt : IEquatable<Shirt>
    {
        public Shirt(Guid id, string name, Size size, Color color)
        {
            Id = id;
            Name = name;
            Size = size;
            Color = color;
        }

        public Guid Id { get; }

        public string Name { get; }

        public Size Size { get; set; }

        public Color Color { get; set; }
        
        #region Equality

        public override bool Equals(object obj)
        {
            return Equals(obj as Shirt);
        }

        public bool Equals(Shirt other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Shirt left, Shirt right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Shirt left, Shirt right)
        {
            return !Equals(left, right);
        }

        #endregion

    }
}