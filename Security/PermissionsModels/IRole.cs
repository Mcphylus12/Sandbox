using System.Collections.Generic;

namespace PermissionsModels
{
    public interface IRole : IHasId
    {
        IEnumerable<Operation> Operations { get; }

        void AssignOperation(params Operation[] operations);

        void RemoveOperation(params Operation[] operations);
    }
}
