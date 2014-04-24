using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Core;

namespace AndroidUsingCore
{
	[Activity (Label = "@string/app_name", MainLauncher = true)]
	public class MainActivity : Activity, NetworkCallbacks
	{
		View layout;
		EditText enterJson, jsonResult;
		Button countButton, networkButton, pastResultsButton, moreStuffButton;

		CoreNetworkController controller;
		DBManager db;
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			layout = FindViewById<LinearLayout>(Resource.Id.mainLayout);
			countButton = FindViewById<Button>(Resource.Id.countButton);
			enterJson = FindViewById<EditText>(Resource.Id.enterJson);
			networkButton = FindViewById<Button>(Resource.Id.networkButton);
			jsonResult = FindViewById<EditText>(Resource.Id.jsonResult);
			pastResultsButton = FindViewById<Button>(Resource.Id.pastResultsButton);
			moreStuffButton = FindViewById<Button>(Resource.Id.moreStuffButton);

			controller = CoreNetworkController.GetInstance();

			InitListeners();
				
			InitDB();
		}

		void InitListeners()
		{
			countButton.Click += delegate {
				countButton.Text = String.Format("{0} clicks!", count++);
			};
			networkButton.Click += delegate {
				string enteredJson = enterJson.Text;
				jsonResult.Text = GetString(Resource.String.validating);
				controller.ValidateJSON(this, enteredJson);
			};
			pastResultsButton.Click += delegate {
				StartActivity(typeof(PastResultsActivity));
			};
			moreStuffButton.Click += delegate {
				StartActivity(typeof(MoreStuffActivity));
			};
		}

		async void InitDB()
		{
			db = DBManager.GetInstance();
			await db.Init();

			layout.Visibility = ViewStates.Visible;
		}

		#region NetworkCallbacks

		async void NetworkCallbacks.OnSuccess(Object data)
		{
			string json = (string) data;
			ValidatedJSON jsonObj = ValidatedJSON.CreateObject(json);

			await db.Insert<ValidatedJSON>(jsonObj);
			Console.WriteLine("*********** {0} rows now in table", await db.GetRowCountForTable<ValidatedJSON>());

			jsonResult.Text = jsonObj.IsValid() ? GetString(Resource.String.valid) : jsonObj.error;
		}

		void NetworkCallbacks.OnFail()
		{
			Toast.MakeText(this, Resource.String.error, ToastLength.Short).Show();
		}

		#endregion
	}
}