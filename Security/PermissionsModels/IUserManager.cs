using System.Threading.Tasks;

namespace PermissionsModels
{
    public interface IUserManager
    {
        Task<bool> CanDoOperation(IObject actor, Operation operation, IObject target);

        void AssignRole(IObject holder, IRole role, IObject target);
        void UnAssignRole(IObject holder, IRole role, IObject target);

        Task Save();
    }
}
