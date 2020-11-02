namespace ModelManagement.Tests
{
    internal class MockDataUpdater : IDataUpdater
    {
        public object[] LatestKeys;

        public void OnInterestsChanged<T>(object[] newKeys)
        {
            this.LatestKeys = newKeys;
        }
    }
}