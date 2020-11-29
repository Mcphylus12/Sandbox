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
        private readonly HashSet<Membership<IObject>> _memberships;
        private readonly HashSet<Membership<Operation>> _operationMemberships;

        public InMemoryManager()
        {
            _assignments = new HashSet<Assignment>();
            _memberships = new HashSet<Membership<IObject>>();
            _operationMemberships = new HashSet<Membership<Operation>>();
        }

        public void Assign(IObject holder, Operation role, IObject target)
        {
            _assignments.Add((holder, role, target));
        }

        public void Unassign(IObject holder, Operation role, IObject target)
        {
            _assignments.Remove((holder, role, target));
        }

        public Task<bool> CanDoOperation(IObject actor, Operation operation, IObject target)
        {
            if (!target.SupportedOperations.Contains(operation)) return Task.FromResult(false);
            List<IObject> actorGroups = GetParents(actor, _memberships);
            List<IObject> targetGroups = GetParents(target, _memberships);
            List<Operation> validOperations = GetParents(operation, _operationMemberships);
            var result = _assignments.Where(a => targetGroups.Contains(a.Target)).Where(a => actorGroups.Contains(a.Holder)).Any(a => validOperations.Contains(a.Operation));
            return Task.FromResult(result);
        }

        private List<T> GetParents<T>(T actor, HashSet<Membership<T>> memberships)
        {
            var result = new List<T>();

            result.Add(actor);

            memberships.Where(m => m.Child.Equals(actor)).ForEach(m =>
            {
                result.AddRange(GetParents(m.Parent, memberships));
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
                    throw new CircularException<IObject>();
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

        public void AddChild(Operation parent, params Operation[] children)
        {
            children.ForEach(child =>
            {
                if (!IsChild(child, parent))
                    _operationMemberships.Add((parent, child));
                else
                    throw new CircularException<Operation>();
            });
        }

        private bool IsChild(Operation parent, Operation child)
        {
            bool immediate = _operationMemberships.Contains((parent, child));
            if (immediate) return true; // hashset optimisation

            return _operationMemberships.Where(m => m.Child.Equals(child)).Any(m => m.Parent.Equals(parent) || IsChild(parent, m.Parent));
        }

        public void RemoveChild(Operation parent, params Operation[] children)
        {
            children.ForEach(child => _operationMemberships.Remove((parent, child)));
        }
    }
}