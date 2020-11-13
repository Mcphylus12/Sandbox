namespace PermissionsModels
{
    interface IOperationMap
    {
        public IRole Role { get; }
        public IOperation Operation { get; }
    }
}
