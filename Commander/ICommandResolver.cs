using System;
using System.Threading.Tasks;

namespace Commander
{
    public interface ICommandResolver
    {
        ICommand<TCommandParams, TResult> ResolveCommandImplementation<TCommandParams, TResult>()
            where TCommandParams : ICommandParams<TResult>;
    }
}