using Microsoft.WindowsAzure.MobileServices;
using mondaydinner.Droid;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Authentication))]
namespace mondaydinner.Droid
{
    public class Authentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                //login and save user status
                var user = await client.LoginAsync(Forms.Context, provider);
                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId ?? string.Empty;
                return user;
            }
            catch (Exception e)
            {
                e.Data["method"] = "LoginAsync";
                //Xamarin.Insights.Report(e);
            }

            return null;
        }
    }
}