using System.Collections.Generic;

namespace PermissionsModels
{
    public class Operation : IOperation
    {
        private string Value;

        public Operation(string value)
        {
            this.Value = value;
        }

        public static implicit operator Operation(string s) => new Operation(s);
        public static implicit operator string(Operation o) => o.Value;


        public override bool Equals(object obj)
        {
            return obj is Operation other &&
                   EqualityComparer<string>.Default.Equals(Value, other.Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}