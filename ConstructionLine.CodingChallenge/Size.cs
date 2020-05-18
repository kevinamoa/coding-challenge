using System;
using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public class Size : IEquatable<Size>
    {
        public Guid Id { get; }

        public string Name { get; }

        private Size(Guid id, string name)
        {
            Id = id;
            Name = name;
        }


        public static Size Small = new Size(Guid.NewGuid(), "Small");
        public static Size Medium = new Size(Guid.NewGuid(), "Medium");
        public static Size Large = new Size(Guid.NewGuid(), "Large");


        public static List<Size> All = 
            new List<Size>
            {
                Small,
                Medium,
                Large
            };

        #region Equality

        public override bool Equals(object obj)
        {
            return Equals(obj as Size);
        }

        public bool Equals(Size other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Size left, Size right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}