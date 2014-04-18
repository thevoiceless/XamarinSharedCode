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
	[Activity (Label = "@string/app_name", MainLauncher = true)]
	public class MainActivity : Activity, NetworkCallbacks
	{
		private DBManager db;
		private int count = 1;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			// Stuff from Core
			CoreNetworkController controller = new CoreNetworkController();
			db = new DBManager();
			db.init();

			Button clickButton = FindViewById<Button>(Resource.Id.clickButton);
			clickButton.Click += delegate {
				clickButton.Text = String.Format("{0} clicks!", count++);
			};

			Button networkButton = FindViewById<Button>(Resource.Id.networkButton);
			networkButton.Click += delegate {
				string enteredJson = FindViewById<EditText>(Resource.Id.enterJson).Text;
				controller.MakeRequest(enteredJson, this);
			};

			Button pastResultsButton = FindViewById<Button>(Resource.Id.pastResultsButton);
			pastResultsButton.Click += delegate {
				StartActivity(typeof(PastResultsActivity));
			};

			Button moreStuffButton = FindViewById<Button>(Resource.Id.moreStuffButton);
			moreStuffButton.Click += delegate {
				StartActivity(typeof(MoreStuffActivity));
			};
		}

		void NetworkCallbacks.OnSuccess(Object data)
		{
			string json = (string) data;
			ValidatedJSON jsonObj = ValidatedJSON.CreateObject(json);

			EditText resultBox = FindViewById<EditText>(Resource.Id.jsonResult);
			if (jsonObj.IsValid())
			{
				resultBox.Text = GetString(Resource.String.valid);
				db.Insert<ValidatedJSON>(jsonObj);
				Console.WriteLine("*********** {0} rows now in table", db.GetRowsInTable<ValidatedJSON>());
			}
			else
			{
				resultBox.Text = jsonObj.error;
			}
		}

		void NetworkCallbacks.OnFail()
		{
			Toast.MakeText(this, Resource.String.error, ToastLength.Short).Show();
		}
	}
}