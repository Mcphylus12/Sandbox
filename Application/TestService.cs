﻿using ModelManagement;

namespace Application
{
    class TestService : IDataUpdater
    {
        private IDataManager manager;
        private object[] newKeys;
        private int modelVersion = 0;

        public TestService(TestApplicationContainer container)
        {
            this.manager = container.DataManager;
        }

        internal void Init()
        {
            this.manager.RegisterDataUpdater(this);
        }

        public void OnInterestsChanged<T>(object[] newKeys)
        {
            this.newKeys = newKeys;
        }

        internal void UpdateData()
        {
            this.manager.NotifyDataUpdated("testKey", new TestModel()
            {
                Value = modelVersion++.ToString()
            });
        }
    }
}
