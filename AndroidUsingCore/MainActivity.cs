using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Core;

namespace AndroidUsingCore
{
	[Activity (Label = "AndroidUsingCore", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private int count = 1;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource and attach an event to it
			Button clickButton = FindViewById<Button>(Resource.Id.clickButton);
			clickButton.Click += delegate {
				clickButton.Text = String.Format("{0} clicks!", count++);
			};

			// Try accessing something from Core
			CoreNetworkController controller = new CoreNetworkController();
			controller.PrintSomething();

			// Network request
			Button networkButton = FindViewById<Button>(Resource.Id.networkButton);
			networkButton.Click += delegate {
				controller.MakeRequest();
			};
		}
	}
}