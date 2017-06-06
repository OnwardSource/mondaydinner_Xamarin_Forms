using Microsoft.WindowsAzure.MobileServices;
using Plugin.Geolocator;
using System;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace mondaydinner
{
    public partial class YodelList : ContentPage
    {
        YodelManager manager;
        Xamarin.Auth.OAuth2Authenticator authenticator = null;

        bool ShowYodels = false;

        // Track whether the user has authenticated.
        bool authenticated = false;

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (authenticated == true)
            {
                await RefreshItems(true, syncItems: false);
                ShowYodels = true;
                yodelList.IsVisible = true;
                LoginBar.IsVisible = false;
                ActionBar.IsVisible = true;
            }
        }

        async void authButton_Clicked(object sender, EventArgs e)
        {
            if (!Settings.IsLoggedIn)
            {
                //await azureService.Initialize();
                //var authenticator = await DependencyService.Get<IAuthentication>();
                //var user = authenticator.LoginAsync(azureService.MobileService, MobileServiceAuthenticationProvider.Facebook);
                //if (user == null)
                //    return;

                //pull latest data from server:
                //var coffees = await azureService.GetCoffees();
                //Coffees.ReplaceRange(coffees);
                //SortCoffees();
            }

            //OAuth2Authenticator auth = new OAuth2Authenticator
            authenticator = new OAuth2Authenticator
    (
        clientId: "424301387946620",
        scope: "",
        authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
        redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"),
        // switch for new Native UI API
        //      true = Android Custom Tabs and/or iOS Safari View Controller
        //      false = Embedded Browsers used (Android WebView, iOS UIWebView)
        //  default = false  (not using NEW native UI)
        isUsingNativeUI: true
    );

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            //authenticator
            //    = new Xamarin.Auth.OAuth2Authenticator
            //    (
            //        /*       
            //        clientId: "185391188679-9pa23l08ein4m4nmqccr9jm01udf3oup.apps.googleusercontent.com",
            //        scope: "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.login",
            //        authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
            //        redirectUrl: new Uri
            //                        (
            //                            "comauthenticationapp://localhost"
            //                        //"com.authentication.app://localhost"
            //                        //"com-authentication-app://localhost"
            //                        ),
            //        */
            //        clientId:
            //            new Func<string>
            //               (
            //                    () =>
            //                    {
            //                        string retval_client_id = "oops something is wrong!";

            //                        // some people are sending the same AppID for google and other providers
            //                        // not sure, but google (and others) might check AppID for Native/Installed apps
            //                        // Android and iOS against UserAgent in request from 
            //                        // CustomTabs and SFSafariViewContorller
            //                        // TODO: send deliberately wrong AppID and note behaviour for the future
            //                        // fitbit does not care - server side setup is quite liberal
            //                        switch (Xamarin.Forms.Device.RuntimePlatform)
            //                        {
            //                            case "iOS":
            //                                retval_client_id = "228CVW";
            //                                break;
            //                            case "Android":
            //                                retval_client_id = "228CVW";
            //                                break;
            //                        }
            //                        return retval_client_id;
            //                    }
            //              ).Invoke(),
            //        authorizeUrl: new Uri("https://www.fitbit.com/oauth2/authorize"),
            //        redirectUrl: new Uri("xamarin-auth://localhost"),
            //        scope: "profile",
            //        getUsernameAsync: null,
            //        isUsingNativeUI: false
            //    )
            //    {
            //        AllowCancel = true,
            //    };

            //NavigateLoginPage();

            return;
        }

        private void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            labelGPS.Text = "Auth worked.";
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            labelGPS.Text = "Auth ERROR.";
            return;
        }

        private void Button_Login_Google_New_NativeUI_Clicked(object sender, EventArgs e)
        {
            return;
        }

        private void Button_Login_Google_Old_WebApp_Clicked(object sender, EventArgs e)
        {
            return;
        }

        //Xamarin.Auth.XamarinForms.AuthenticatorPage login_page = null;

        //private void NavigateLoginPage()
        //{
        //    // / *
        //    //---------------------------------------------------------------------
        //    // ContentPage with CustomRenderers
        //    login_page = new Xamarin.Auth.XamarinForms.AuthenticatorPage()
        //    {
        //        Authenticator = authenticator,
        //    };
        //    Navigation.PushAsync(login_page);
        //    //---------------------------------------------------------------------
        //    // Xamarin.UNiversity Team Presenters Concept
        //    // Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
        //    // presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
        //    //presenter.Login (authenticator);
        //    //---------------------------------------------------------------------
        //    // * /

        //    return;
        //}

        public void Authentication_Completed(object sender, Xamarin.Auth.AuthenticatorCompletedEventArgs e)
        {
            return;
        }

        public void Authentication_Error(object sender, Xamarin.Auth.AuthenticatorErrorEventArgs e)
        {
            return;
        }

        public void Authentication_BrowsingCompleted(object sender, EventArgs e)
        {
            return;
        }

        public YodelList()
        {
            InitializeComponent();

            manager = YodelManager.DefaultManager;

            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
            //if (manager.IsOfflineEnabled &&
            //    (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone))
                if (manager.IsOfflineEnabled)
                {
                var syncButton = new Button
                {
                    Text = "Sync items",
                    HeightRequest = 30
                };
                syncButton.Clicked += OnSyncItems;

                buttonsPanel.Children.Add(syncButton);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                // Set syncItems to true in order to synchronize the data on startup when running in offline mode
                await RefreshItems(true, syncItems: true);

                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;

                ShowYodels = true;
            }
            else
            {
                await RefreshItems(true, syncItems: false);
            }
        }

        // Data methods
        async Task AddItem(Yodel item)
        {
            await manager.SaveYodelAsync(item);
            yodelList.ItemsSource = await manager.GetYodelsAsync();
        }

        async Task CompleteItem(Yodel item)
        {
            item.Deleted = true;
            //item.Done = true;
            await manager.SaveYodelAsync(item);
            yodelList.ItemsSource = await manager.GetYodelsAsync();
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            // Empty
            if (newItemName.Text.Length == 0)
            {
                YodelBar.IsVisible = false;
                ActionBar.IsVisible = true;
                return;
            }

            var locator = CrossGeolocator.Current;
            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
            {
                locator.DesiredAccuracy = 50;
                labelGPS.Text = "Acquiring gps";

                try
                {
                    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                    if (position == null)
                    {
                        await DisplayAlert("GPS null", "Null GPS", "OK");
                        //labelGPS.Text = "null gps :(";
                        return;
                    }
                    labelGPS.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \n Altitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \n Heading: {6} \n Speed: {7}",
                position.Timestamp, position.Latitude, position.Longitude,
        position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

                    var yodel = new Yodel {
                        CreatedAt = DateTime.Now,
                        Latitude = position.Latitude,
                        Longitude = position.Longitude,
                        Deleted = false,
                        Message = newItemName.Text };
                    await AddItem(yodel);

                    newItemName.Text = string.Empty;
                    newItemName.Unfocus();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Geolocator Error", "Couldn't access GPS, did not Yodel", "OK");
                }
            }
            else
            {
                await DisplayAlert("Location Error", "Couldn't access GPS, did not Yodel", "OK");
            }

            YodelBar.IsVisible = false;
            ActionBar.IsVisible = true;
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var yodel = e.SelectedItem as Yodel;

            string message = yodel.Message;
            double latitude = yodel.Latitude;
            double longitude = yodel.Longitude;

            YodelMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(latitude, longitude), Distance.FromMiles(6)));

            //if (Device.OS != TargetPlatform.iOS && yodel != null)
            //{
            //    // Not iOS - the swipe-to-delete is discoverable there
            //    if (Device.OS == TargetPlatform.Android)
            //    {
            //        await DisplayAlert(yodel.Message, "Press-and-hold to complete task " + yodel.Message, "Got it!");
            //    }
            //    else
            //    {
            //        // Windows, not all platforms support the Context Actions yet
            //        if (await DisplayAlert("Mark completed?", "Do you wish to complete " + yodel.Message + "?", "Complete", "Cancel"))
            //        {
            //            await CompleteItem(yodel);
            //        }
            //    }
            //}

            // prevents background getting highlighted
            //yodelList.SelectedItem = null;
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        public async void OnComplete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var yodel = mi.CommandParameter as Yodel;
            await CompleteItem(yodel);
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                yodelList.ItemsSource = await manager.GetYodelsAsync(syncItems);

                foreach(Yodel yodel in yodelList.ItemsSource)
                {
                    var pin = new Pin();
                    pin.Label = yodel.Message;
                    pin.Position = new Position(yodel.Latitude, yodel.Longitude);
                    pin.Type = PinType.SavedPin;
                    YodelMap.Pins.Add(pin);
                }                
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private async void gpsButton_Clicked(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
            {
                locator.DesiredAccuracy = 50;
                labelGPS.Text = "Getting gps";

                try
                {
                    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                    if (position == null)
                    {
                        labelGPS.Text = "null gps :(";
                        return;
                    }
                    labelGPS.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \n Altitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \n Heading: {6} \n Speed: {7}",
                position.Timestamp, position.Latitude, position.Longitude,
        position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
                }
                catch(Exception ex)
                {
                    //await DisplayAlert("Geolocator Error", "Couldn't access GPS", "OK");
                    labelGPS.Text = "Geolocator Error: " + ex.Message;
                }

            }
            else
            {
                //await DisplayAlert("Location Error", "Couldn't access GPS", "OK");
                labelGPS.Text = "Location Error: Couldn't access GPS.";
            }
        }

        private void yodelButton_Clicked(object sender, EventArgs e)
        {
            ActionBar.IsVisible = false;
            YodelBar.IsVisible = true;
        }

        private void quickButton1_Clicked(object sender, EventArgs e)
        {
            newItemName.Text = "867-5309 (Jenny), you got it?";
        }

        private void quickButton2_Clicked(object sender, EventArgs e)
        {
            newItemName.Text = "Yo-de-lay-hee-hooooo!";
        }

        private void quickButton3_Clicked(object sender, EventArgs e)
        {
            newItemName.Text = "Covfefe aka Coverage";
        }
    }
}

