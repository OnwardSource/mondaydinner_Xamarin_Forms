using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace mondaydinner
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
        //Task<MobileServiceUser> AuthenticateAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
    }
}
