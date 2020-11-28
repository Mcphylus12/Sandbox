using Permissions.Tests.Helpers;
using PermissionsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permissions.Tests
{
    public class InMemoryManager : IUserManager
    {
        private readonly HashSet<Assignment> _assignments;
        private readonly HashSet<Membership> _memberships;

        public InMemoryManager()
        {
            _assignments = new HashSet<Assignment>();
            _memberships = new HashSet<Membership>();
        }

        public void AssignRole(IObject holder, IRole role, IObject target)
        {
            _assignments.Add((holder, role, target));
        }

        public void UnAssignRole(IObject holder, IRole role, IObject target)
        {
            _assignments.Remove((holder, role, target));
        }

        public Task<bool> CanDoOperation(IObject actor, Operation operation, IObject target)
        {
            if (!target.SupportedOperations.Contains(operation)) return Task.FromResult(false);
            List<IObject> actorGroups = GetGroups(actor);
            List<IObject> targetGroups = GetGroups(target);
            IEnumerable<Operation> operations = _assignments.Where(a => targetGroups.Contains(a.Target) && actorGroups.Contains(a.Holder)).Select(a => a.Role).SelectMany(r => r.Operations);
            var result = operations.Any(op => op.Equals(operation));
            return Task.FromResult(result);
        }

        private List<IObject> GetGroups(IObject actor)
        {
            var result = new List<IObject>();

            result.Add(actor);

            _memberships.Where(m => m.Child.IdMatches(actor)).ForEach(m =>
            {
                result.AddRange(GetGroups(m.Parent));
            });

            return result;
        }

        public Task Save()
        {
            return Task.CompletedTask;
        }

        public void AddChild(IObject parent, params IObject[] children)
        {
            children.ForEach(child =>
            {
                if (!IsChild(child, parent))
                    _memberships.Add((parent, child));
                else
                    throw new CircularMembershipExceptions();
            });
        }

        public bool IsChild(IObject parent, IObject child)
        {
            bool immediate = _memberships.Contains((parent, child));
            if (immediate) return true; // hashset optimisation

            return _memberships.Where(m => m.Child.IdMatches(child)).Any(m => m.Parent.IdMatches(parent) || IsChild(parent, m.Parent));
        }

        public void RemoveChild(IObject parent, params IObject[] children)
        {
            children.ForEach(child => _memberships.Remove((parent, child)));
        }
    }
}