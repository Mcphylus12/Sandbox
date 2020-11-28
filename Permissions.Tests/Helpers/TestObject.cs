using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    public abstract class TestObject : IObject
    {
        public Guid Id { get; } = Guid.NewGuid();

       public abstract IEnumerable<Operation> SupportedOperations { get; }
    }
}
