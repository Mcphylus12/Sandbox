using System.Collections.Generic;

namespace PermissionsModels
{
    public interface IObject : IHasId
    {
        IEnumerable<Operation> SupportedOperations { get; }
    }
}
