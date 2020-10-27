using System.Text;

namespace ModelManagement
{
    internal class ModelManager : IModelManager
    {
        private readonly DataManager _dataManager;

        public ModelManager()
        {
            this._dataManager = new DataManager();
        }

        public Subscription<T> GetSubscription<T>()
        {
            Subscription<T> newSubscription = new Subscription<T>(this._dataManager);
            this._dataManager.AddSubscription(newSubscription);
            return newSubscription;
        }

        public RequestLock<T> LockRequests<T>()
        {
            RequestLock<T> newLock = new RequestLock<T>(this._dataManager);
            this._dataManager.AddLock(newLock);
            return newLock;
        }
    }
}
