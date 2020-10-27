namespace ModelManagement
{
    internal class RequestLock<T> : IRequestLock
    {
        private DataManager _dataManager;

        internal RequestLock(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void Release()
        {
            this._dataManager.ReleaseLock<T>(this);
        }
    }
}
