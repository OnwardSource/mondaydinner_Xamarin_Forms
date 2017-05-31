using Plugin.Geolocator;
using System;

using Xamarin.Forms;

namespace mondaydinner
{
	public class App : Application
	{
        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        //Label timestampLabel;
        //Label latitudeLabel;
        //Label longitudeLabel;

        public App ()
		{
			// The root page of your application
			MainPage = new YodelList();

            //latitudeLabel = MainPage.FindByName<Label>("latitiudeLabel");
        }

		protected override void OnStart ()
		{
            // Handle when your app starts

            //var locator = CrossGeolocator.Current;
            //locator.DesiredAccuracy = 50;
            ////locator.DesiredAccuracy = 100;
            //var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            //locator.PositionChanged += (sender, e) => {
            //    var pos = e.Position;

            //    latitudeLabel.Text = pos.Latitude.ToString();
            //    longitudeLabel.Text = pos.Longitude.ToString();
            //};
            
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

