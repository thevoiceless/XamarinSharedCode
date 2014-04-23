﻿using System;
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
		private View layout;
		private Button countButton, networkButton, pastResultsButton, moreStuffButton;

		private CoreNetworkController controller;
		private DBManager db;
		private int count = 1;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			layout = FindViewById<LinearLayout>(Resource.Id.mainLayout);
			countButton = FindViewById<Button>(Resource.Id.countButton);
			networkButton = FindViewById<Button>(Resource.Id.networkButton);
			pastResultsButton = FindViewById<Button>(Resource.Id.pastResultsButton);
			moreStuffButton = FindViewById<Button>(Resource.Id.moreStuffButton);

			controller = new CoreNetworkController();

			InitListeners();
				
			InitDB();
		}

		private void InitListeners()
		{
			countButton.Click += delegate {
				countButton.Text = String.Format("{0} clicks!", count++);
			};
			networkButton.Click += delegate {
				string enteredJson = FindViewById<EditText>(Resource.Id.enterJson).Text;
				controller.MakeRequest(enteredJson, this);
			};
			pastResultsButton.Click += delegate {
				StartActivity(typeof(PastResultsActivity));
			};
			moreStuffButton.Click += delegate {
				StartActivity(typeof(MoreStuffActivity));
			};
		}

		private async void InitDB()
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

			EditText resultBox = FindViewById<EditText>(Resource.Id.jsonResult);
			if (jsonObj.IsValid())
			{
				resultBox.Text = GetString(Resource.String.valid);
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

		#endregion
	}
}