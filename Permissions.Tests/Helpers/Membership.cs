using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    internal struct Membership
    {
        public IObject Parent;
        public IObject Child;

        public Membership(IObject parent, IObject child)
        {
            Parent = parent;
            Child = child;
        }

        public override bool Equals(object obj)
        {
            return obj is Membership other &&
                   EqualityComparer<IObject>.Default.Equals(Parent, other.Parent) &&
                   EqualityComparer<IObject>.Default.Equals(Child, other.Child);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Parent, Child);
        }

        public void Deconstruct(out IObject parent, out IObject child)
        {
            parent = Parent;
            child = Child;
        }

        public static implicit operator (IObject Parent, IObject Child)(Membership value)
        {
            return (value.Parent, value.Child);
        }

        public static implicit operator Membership((IObject Parent, IObject Child) value)
        {
            return new Membership(value.Parent, value.Child);
        }
    }
}