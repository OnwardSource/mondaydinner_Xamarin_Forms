using System.Threading.Tasks;

namespace mondaydinner
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
}
