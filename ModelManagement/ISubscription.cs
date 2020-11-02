using System;

namespace ModelManagement
{
    public interface ISubscription
    {
        void UpdateKey(object key);

        void Dispose();
    }
}