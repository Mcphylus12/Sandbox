using System;

namespace ModelManagement
{
    public interface ISubscription : IDisposable
    {
        void UpdateKey(object key);
    }
}