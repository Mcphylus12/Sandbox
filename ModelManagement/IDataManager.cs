namespace ModelManagement
{
    public interface IDataManager
    {
        void NotifyDataUpdated<T>(object key, T model);
        void RegisterDataUpdater(IDataUpdater updater);
    }
}