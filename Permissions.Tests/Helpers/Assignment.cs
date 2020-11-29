using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests
{
    internal struct Assignment
    {
        public IObject Holder;
        public Operation Operation;
        public IObject Target;

        public Assignment(IObject holder, Operation operation, IObject target)
        {
            Holder = holder;
            Operation = operation;
            Target = target;
        }

        public override bool Equals(object obj)
        {
            return obj is Assignment other &&
                   EqualityComparer<IObject>.Default.Equals(Holder, other.Holder) &&
                   EqualityComparer<Operation>.Default.Equals(Operation, other.Operation) &&
                   EqualityComparer<IObject>.Default.Equals(Target, other.Target);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Holder, Operation, Target);
        }

        public void Deconstruct(out IObject holder, out Operation operation, out IObject target)
        {
            holder = Holder;
            operation = Operation;
            target = Target;
        }

        public static implicit operator (IObject, Operation, IObject)(Assignment value)
        {
            return (value.Holder, value.Operation, value.Target);
        }

        public static implicit operator Assignment((IObject holder, Operation operation, IObject target) value)
        {
            return new Assignment(value.holder, value.operation, value.target);
        }
    }
}