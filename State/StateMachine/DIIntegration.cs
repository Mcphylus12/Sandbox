using Microsoft.Extensions.DependencyInjection;
using System;

namespace StateMachine
{
    public static class DIIntegration
    {
        public static void AddStateMachines(this IServiceCollection services, Action<StateMachineRegistry> configure)
        {
            var stateMachineRegistry = new StateMachineRegistry();

            configure.Invoke(stateMachineRegistry);

            services.AddSingleton(stateMachineRegistry);
        }
    }

    public class StateMachineRegistry
    {
        public StateMachineRegistry()
        {
        }

        public void AddStateMachine<TState, TTransition>(StateMachineBuilder<TState, TTransition, ServiceContext> stateMachineBuilder)
        {
            throw new NotImplementedException();
        }
    }

    public class ServiceContext
    {
    }
}
