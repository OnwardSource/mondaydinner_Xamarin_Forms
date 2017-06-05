using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace mondaydinner
{
    public interface IAuthentication
    {
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
    }
}
