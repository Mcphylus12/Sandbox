using System.Threading.Tasks;

namespace Commander
{
    public interface ICommand<TCommandParams, TResult> 
        where TCommandParams : ICommandParams<TResult>
    {
        Task<TResult> Execute(ICommandParams<TResult> parameterModel);
    }
}