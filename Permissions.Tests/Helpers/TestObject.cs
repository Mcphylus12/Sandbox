using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    public abstract class TestObject : IObject
    {
        public Guid Id { get; } = Guid.NewGuid();

       public abstract IEnumerable<Operation> SupportedOperations { get; }

        public override bool Equals(object obj)
        {
            return obj is TestObject other &&
                   EqualityComparer<Guid>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
