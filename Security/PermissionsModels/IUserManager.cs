using System.Threading.Tasks;

namespace PermissionsModels
{
    public interface IUserManager
    {
        Task<bool> CanDoOperation(IObject actor, Operation operation, IObject target);

        void Assign(IObject holder, Operation role, IObject target);
        void Unassign(IObject holder, Operation role, IObject target);

        Task Save();
        void AddChild(IObject parent, params IObject[] children);
        bool IsChild(IObject parent, IObject child);
        void RemoveChild(IObject parent, params IObject[] children);

        void AddChild(Operation parent, params Operation[] children);
        void RemoveChild(Operation parent, params Operation[] children);
    }
}
