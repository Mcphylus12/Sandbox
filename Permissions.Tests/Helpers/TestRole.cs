using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    internal class TestRole : IRole
    {
        public Guid Id { get; }
        private HashSet<Operation> internalOperations;
        public IEnumerable<Operation> Operations => internalOperations;

        public TestRole(Guid id)
        {
            this.Id = id;
            internalOperations = new HashSet<Operation>();
        }

        public void AssignOperation(params Operation[] newOperations)
        {
            foreach (var op in newOperations)
            {
                this.internalOperations.Add(op);
            }
        }

        public void RemoveOperation(params Operation[] operations)
        {
            foreach (var op in operations)
            {
                this.internalOperations.Remove(op);
            }
        }
    }
}