using Permissions.Tests.Tests;
using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    public class Service : TestObject
    {
        private HashSet<Operation> ops = new HashSet<Operation>()
        {
            "Start"
        };

        public override IEnumerable<Operation> SupportedOperations => ops;

        internal void ClearSupportedOperations()
        {
            ops.Clear();
        }
    }
}
