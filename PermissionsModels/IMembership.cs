namespace PermissionsModels
{
    public interface IMembership
    {
        public IGroup Group { get; }
        public IObject Member { get; }
    }
}
