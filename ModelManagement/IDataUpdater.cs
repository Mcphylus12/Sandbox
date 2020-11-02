namespace ModelManagement
{
    public interface IDataUpdater
    {
        void OnInterestsChanged<T>(object[] newKeys);
    }
}
