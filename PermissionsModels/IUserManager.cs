using System.Threading.Tasks;

namespace PermissionsModels
{
    public interface IFacade
    {
        Task<bool> IsInGroup(IGroup group, IObject member);
        Task<bool> CanDoOperation(IObject actor, IObject target, IOperation operation);

        Task AddToGroup(IGroup group, params IObject[] members);
        Task RemoveFromGroup(IGroup group, params IObject[] members);

        Task AddRole(IRole role, params IOperation[] operations);
        Task AssignOperation(IRole role, params IOperation[] operations);
        Task RemoveOperation(IRole role, params IOperation[] operations);
        Task RemoveRole(IRole role);

        Task AssignRole(IObject holder, IObject target, IRole role);
        Task UnAssignRole(IObject holder, IObject target, IRole role);
    }
}
