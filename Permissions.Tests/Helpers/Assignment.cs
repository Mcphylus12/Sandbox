using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    internal struct Assignment
    {
        public IObject Holder;
        public IRole Role;
        public IObject Target;

        public Assignment(IObject holder, IRole role, IObject target)
        {
            Holder = holder;
            Role = role;
            Target = target;
        }

        public override bool Equals(object obj)
        {
            return obj is Assignment other &&
                   EqualityComparer<IObject>.Default.Equals(Holder, other.Holder) &&
                   EqualityComparer<IRole>.Default.Equals(Role, other.Role) &&
                   EqualityComparer<IObject>.Default.Equals(Target, other.Target);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Holder, Role, Target);
        }

        public void Deconstruct(out IObject holder, out IRole role, out IObject target)
        {
            holder = Holder;
            role = Role;
            target = Target;
        }

        public static implicit operator (IObject, IRole, IObject)(Assignment value)
        {
            return (value.Holder, value.Role, value.Target);
        }

        public static implicit operator Assignment((IObject holder, IRole role, IObject target) value)
        {
            return new Assignment(value.holder, value.role, value.target);
        }
    }
}