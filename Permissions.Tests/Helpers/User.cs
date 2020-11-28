using Permissions.Tests.Tests;
using PermissionsModels;
using System.Collections.Generic;

namespace Permissions.Tests
{
    public class User : TestObject
    {
        public override IEnumerable<Operation> SupportedOperations => new HashSet<Operation>();
    }
}
