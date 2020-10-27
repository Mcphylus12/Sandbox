using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ModelManagement
{
    internal class DataManager
    {
        private Dictionary<Type, List<IRequestLock>> _locks;
        private Dictionary<Type, List<ISubscription>> _subscriptions;
        private HashSet<Type> _typeCache;

        public DataManager()
        {
            this._locks = new Dictionary<Type, List<IRequestLock>>();
            this._subscriptions = new Dictionary<Type, List<ISubscription>>();
            this._typeCache = new HashSet<Type>();
        }

        internal void AddLock<T>(RequestLock<T> newLock)
        {
            if (!this._locks.ContainsKey(typeof(T)))
            {
                this._locks.Add(typeof(T), new List<IRequestLock>());
            }

            this._locks[typeof(T)].Add(newLock);
        }

        internal void ReleaseLock<T>(RequestLock<T> requestLock)
        {
            this._locks[typeof(T)].Remove(requestLock); // O(n)
        }

        internal void AddSubscription<T>(Subscription<T> newSubscription)
        {
            Type type = typeof(T);

            if (!this._subscriptions.ContainsKey(type))
            {
                this._subscriptions.Add(type, new List<ISubscription>());
            }

            this._subscriptions[type].Add(newSubscription);
            this._typeCache.Add(type);
        }

        internal void RemoveSubscription<T>(Subscription<T> subscription)
        {
            Type type = typeof(T);
            this._subscriptions[type].Remove(subscription); // O(n)
            this._typeCache.Add(type);
        }

        public IEnumerable<object> GetSubscriptionKeys<T>()
        {
            this._typeCache.Remove(typeof(T));
            return this._subscriptions[typeof(T)].Select(sub => sub.GetKey());
        }
        
        public IEnumerable<KeyValuePair<Type, object>> EnumerateSubscriptions()
        {
            this._typeCache.Clear();
            return this._subscriptions.SelectMany(kv => kv.Value, (kv, sub) => new KeyValuePair<Type, object>(kv.Key, sub.GetKey()));
        }

        public bool HaveInterestsChanged<T>()
        {
            return this._typeCache.Contains(typeof(T));
        }

        public bool IsLocked<T>()
        {
            return this._locks.ContainsKey(typeof(T));
        }
    }
}
