using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace Commander
{
    public class Cmder : ICmder, ICommandResolverStore
    {
        private Dictionary<Type, object> _cache;
        private Dictionary<Type, ICommandResolver> _resolvers;

        public Cmder()
        {
            this._cache = new Dictionary<Type, object>();
            this._resolvers = new Dictionary<Type, ICommandResolver>();
        }

        public async Task<TResult> Execute<TResult>(ICommandParams<TResult> paramModel)
        {
            ICommandResolver resolver = GetCommandResolver(paramModel.GetType());
            var wrapper = GetWrapper<TResult>(typeof(TResult), paramModel.GetType(), resolver);
            return await wrapper.Execute(paramModel);
        }

        private ICommandResolver GetCommandResolver(Type type)
        {
            ICommandResolver result;

            if (!this._resolvers.TryGetValue(type, out result))
            {
                throw new Exception("No Command Implementation found for type: " + type.Name);
            }

            return result;
        }

        public void AddCommandResolver(ICommandResolver resolver, List<Type> commandTypes)
        {
            foreach (Type t in commandTypes)
            {
                this._resolvers.Add(t, resolver);
            }
        }

        private CommandWrapperAbstraction<TResult> GetWrapper<TResult>(Type resultType, Type requestType, ICommandResolver resolver)
        {
            object wrapper;
            if (!_cache.TryGetValue(requestType, out wrapper))
            {
                wrapper = Activator.CreateInstance(typeof(CommandWrapperImplementation<,>).MakeGenericType(requestType, resultType), resolver);
                _cache.Add(requestType, wrapper);
            }

            return (CommandWrapperAbstraction<TResult>)wrapper;
        }

        private class CommandWrapperImplementation<TCommandParams, TResult> : CommandWrapperAbstraction<TResult>
            where TCommandParams : ICommandParams<TResult>
        {
            private readonly ICommandResolver _resolver;

            public CommandWrapperImplementation(ICommandResolver resolver)
            {
                this._resolver = resolver;
            }

            internal override async Task<TResult> Execute(ICommandParams<TResult> parameterModel)
            {
                ICommand<TCommandParams, TResult> command = _resolver.ResolveCommandImplementation<TCommandParams, TResult>();

                return await command.Execute(parameterModel);
            }
        }

        private abstract class CommandWrapperAbstraction<TResult>
        {
            internal abstract Task<TResult> Execute(ICommandParams<TResult> request);
        }
    }


}
