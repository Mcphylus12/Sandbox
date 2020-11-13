using System.Threading.Tasks;

namespace Commander
{
    public interface ICmder
    {
        Task<TResult> Execute<TResult>(ICommandParams<TResult> paramModel);
    }
}