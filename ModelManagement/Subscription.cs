using System;

namespace ModelManagement
{
    internal class Subscription<T> : ISubscription
    {
        private Action<T> onDataUpdated;
        private readonly DataManager manager;
        internal object currentKey = null;

        public Subscription(Action<T> onDataUpdated, DataManager manager)
        {
            this.onDataUpdated = onDataUpdated;
            this.manager = manager;
        }

        public void UpdateData(T newData)
        {
            onDataUpdated(newData);
        }

        public void Dispose()
        {
            this.manager.RemoveSubscription(this);
        }

        public void UpdateKey(object newKey)
        {
            this.manager.InterestUpdated(this, newKey);
            this.currentKey = newKey;
        }
    }
}