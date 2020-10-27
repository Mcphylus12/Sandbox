namespace ModelManagement
{
    public interface IModelManager
    {
        Subscription<T> GetSubscription<T>();
    }
}
