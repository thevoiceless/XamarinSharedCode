using System;
using System.Text;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Geolocation;
using Core;

namespace AndroidUsingCore
{
	[Activity (Label = "@string/more_stuff")]			
	public class MoreStuffActivity : Activity, NetworkCallbacks
	{
		CoreNetworkController controller;

		Button locationButton;
		EditText locationInfo, areaCodeInfo;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MoreStuff);
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			locationButton = FindViewById<Button>(Resource.Id.locationButton);
			locationInfo = FindViewById<EditText>(Resource.Id.locationInfo);
			areaCodeInfo = FindViewById<EditText>(Resource.Id.areaCodeInfo);

			controller = CoreNetworkController.GetInstance();

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
				locationInfo.Text = "";
				locationInfo.Visibility = ViewStates.Visible;

				areaCodeInfo.Text = "";
				areaCodeInfo.Visibility = ViewStates.Gone;

				var locator = new Geolocator(this) { DesiredAccuracy = 30 };
				locationButton.Enabled = false;
				Position position = await locator.GetPositionAsync(10000);
				locationButton.Enabled = true;

				string info = string.Format("Position:\n\nLatitude: {0}\nLongitude: {1}\nHeading: {2}\nSpeed: {3}\nAltitude: {4}\nAlt. Accuracy: {5}\nOverall Accuracy: {6}",
					position.Latitude, position.Longitude, position.Heading, position.Speed, position.Altitude, position.AltitudeAccuracy, position.Accuracy);
				locationInfo.Text = info;
				Console.WriteLine(info);

				areaCodeInfo.Visibility = ViewStates.Visible;
				controller.GetAreaCode(this, position.Latitude, position.Longitude);
			};
		}

		#region NetworkCallbacks

		void NetworkCallbacks.OnSuccess(Object data)
		{
			string jsonArray = (string) data;
			List<AreaCode> areaCodes = AreaCode.CreateList(jsonArray);
			StringBuilder areaCodesText = new StringBuilder("Nearby area codes:\n");
			foreach (AreaCode code in areaCodes)
			{
				areaCodesText.Append(String.Format("\n{0} in {1}, {2}", code.postalCode, code.placeName, code.adminCode1));
			}

			areaCodeInfo.Text = areaCodesText.ToString();
		}

		void NetworkCallbacks.OnFail()
		{
			Toast.MakeText(this, Resource.String.error, ToastLength.Short).Show();
		}

		#endregion
	}
}

