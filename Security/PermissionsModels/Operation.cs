namespace PermissionsModels
{
    public struct Operation : IOperation
    {
        private string Value;

        public Operation(string value)
        {
            this.Value = value;
        }

        public static implicit operator Operation(string s) => new Operation(s);
        public static implicit operator string(Operation o) => o.Value;

        public override string ToString()
        {
            return Value;
        }
    }
}