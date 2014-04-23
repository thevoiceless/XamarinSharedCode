using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Geolocation;

namespace AndroidUsingCore
{
	[Activity (Label = "@string/more_stuff")]			
	public class MoreStuffActivity : Activity
	{
		Button locationButton;
		EditText locationInfo;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MoreStuff);
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			locationButton = FindViewById<Button>(Resource.Id.locationButton);
			locationInfo = FindViewById<EditText>(Resource.Id.locationInfo);

			InitListeners();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		void InitListeners()
		{
			locationButton.Click += async delegate {
				var locator = new Geolocator(this) { DesiredAccuracy = 30 };
				Position position = await locator.GetPositionAsync(10000);
				string info = string.Format("Latitude: {0}\n Longitude: {1}\n Heading: {2}\n Speed: {3}\n Altitude: {4}\n Alt. Accuracy: {5}\n Overall Accuracy: {6}",
					position.Latitude, position.Longitude, position.Heading, position.Speed, position.Altitude, position.AltitudeAccuracy, position.Accuracy);
				locationInfo.Text = info;
				Console.WriteLine(info);
			};
		}
	}
}

