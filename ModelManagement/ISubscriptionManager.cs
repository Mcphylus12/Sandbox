using System;

namespace ModelManagement
{
    public interface ISubscriptionManager
    {
        ISubscription Subscribe<T>(Action<T> onDataUpdated);
    }
}