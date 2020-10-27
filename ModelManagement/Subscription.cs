using System;

namespace ModelManagement
{
    public class Subscription<T> : IDisposable, ISubscription
    {
        private readonly DataManager _manager;
        private object _key;

        public event Action<T> OnUpdate;

        internal Subscription(DataManager manager)
        {
            this._manager = manager;
        }

        public void UpdateKey(object key)
        {
            this._key = key;
        }

        public void Dispose()
        {
            this._manager.RemoveSubscription(this);
        }

        public object GetKey()
        {
            return this._key;
        }
    }
}
