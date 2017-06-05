using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace mondaydinner
{
    public static class Settings
    {
        // TODO: Implement Encryption for auth storage
        // These settings are stored directly in each operating system’s preference systems such as NSUserDefaults or SharedPreferences. These settings are stored in clear text, so you may want to think about additional security if you want to store secret data.

        private static ISettings AppSettings => CrossSettings.Current;

        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        public static string AuthToken
        {
            get { return AppSettings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault); }
            set { AppSettings.AddOrUpdateValue<string>(AuthTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserIdKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
    }
}
