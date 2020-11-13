using Commander;
using System;
using System.Collections.Generic;

namespace Application
{
    internal class TestStartUp
    {
        private readonly ICommandResolverStore commanderResolverStore;

        public TestStartUp(TestApplicationContainer container)
        {
            this.commanderResolverStore = container.CommandResolverStore;
        }

        internal void Start()
        {
            commanderResolverStore.AddCommandResolver(new TestCommandResolver(), new List<Type>
            {
                typeof(TestParam)
            });
        }
    }
}