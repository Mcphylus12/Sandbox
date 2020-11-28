using PermissionsModels;
using System.Collections.Generic;

namespace Permissions.Tests.Helpers
{
    public class Group : TestObject
    {
        public override IEnumerable<Operation> SupportedOperations => new Operation[0];
    }
}
