using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    internal struct Membership<T>
    {
        public T Parent;
        public T Child;

        public Membership(T parent, T child)
        {
            Parent = parent;
            Child = child;
        }

        public override bool Equals(object obj)
        {
            return obj is Membership<T> other &&
                   EqualityComparer<T>.Default.Equals(Parent, other.Parent) &&
                   EqualityComparer<T>.Default.Equals(Child, other.Child);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Parent, Child);
        }

        public void Deconstruct(out T parent, out T child)
        {
            parent = Parent;
            child = Child;
        }

        public static implicit operator (T Parent, T Child)(Membership<T> value)
        {
            return (value.Parent, value.Child);
        }

        public static implicit operator Membership<T>((T Parent, T Child) value)
        {
            return new Membership<T>(value.Parent, value.Child);
        }
    }
}