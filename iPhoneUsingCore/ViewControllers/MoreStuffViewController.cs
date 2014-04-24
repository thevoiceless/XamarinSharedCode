using System;
using System.Text;
using System.Collections.Generic;
using MonoTouch.UIKit;
using Xamarin.Geolocation;
using Core;

namespace iPhoneUsingCore
{
	public partial class MoreStuffViewController : UIViewController, NetworkCallbacks
	{
		CoreNetworkController controller;

		public MoreStuffViewController(IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			controller = CoreNetworkController.GetInstance();

			InitOutlets();
		}

		void InitOutlets()
		{
			locationButton.TouchUpInside += async delegate {
				locationInfo.Text = "Getting location info...";
				locationInfo.TextColor = UIColor.LightGray;
				locationInfo.Hidden = false;

				areaCodeInfo.Text = "Getting area codes...";
				areaCodeInfo.TextColor = UIColor.LightGray;
				areaCodeInfo.Hidden = true;

				var locator = new Geolocator { DesiredAccuracy = 30 };
				locationButton.Enabled = false;
				Position position = await locator.GetPositionAsync(10000);
				locationButton.Enabled = true;

				string info = string.Format("Latitude: {0}\nLongitude: {1}\nHeading: {2}\nSpeed: {3}\nAltitude: {4}\nAlt. Accuracy: {5}\nOverall Accuracy: {6}",
					position.Latitude, position.Longitude, position.Heading, position.Speed, position.Altitude, position.AltitudeAccuracy, position.Accuracy);
				locationInfo.TextColor = UIColor.DarkGray;
				locationInfo.Text = info;
				Console.WriteLine(info);

				areaCodeInfo.Hidden = false;
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

			areaCodeInfo.TextColor = UIColor.DarkGray;
			areaCodeInfo.Text = areaCodesText.ToString();
		}

		void NetworkCallbacks.OnFail()
		{

		}

		#endregion
	}
}

