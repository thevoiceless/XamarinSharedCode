using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Core;

namespace iPhoneUsingCore
{
	public partial class iPhoneUsingCoreViewController : UIViewController, NetworkCallbacks
	{
		CoreNetworkController controller;
		DBManager db;

		int count = 1;

		public iPhoneUsingCoreViewController(IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, null);

			InitDB();

			controller = CoreNetworkController.GetInstance();

			InitOutlets();

			// Couldn't get the gesture detector to work via the storyboard
			UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer(DismissKeyboard);
			tapRecognizer.NumberOfTapsRequired = 1;
			View.AddGestureRecognizer(tapRecognizer);
		}

		partial void DismissKeyboard(NSObject sender)
		{
			_DismissKeyboard();
		}

		// Want to dismiss the keyboard when pressing the buttons or tapping elsewhere
		void _DismissKeyboard()
		{
			enterJson.ResignFirstResponder();
		}

		void InitOutlets()
		{
			countButton.TouchUpInside += delegate {
				countButton.SetTitle(String.Format("{0} clicks!", count++), UIControlState.Normal);
				_DismissKeyboard();
			};

			networkButton.TouchUpInside += delegate {
				jsonResult.Text = "Validating...";
				string enteredJson = this.enterJson.Text;
				controller.ValidateJSON(this, enteredJson);
				_DismissKeyboard();
			};

			pastResultsButton.TouchUpInside += delegate {
				// ViewController transition handled in the storyboard
			};
		}

		async void InitDB()
		{
			db = DBManager.GetInstance();
			// There's nothing preventing the user from interacting with the UI before this finishes
			await db.Init();
		}

		#region NetworkCallbacks

		async void NetworkCallbacks.OnSuccess(Object data)
		{
			string json = (string) data;
			ValidatedJSON jsonObj = ValidatedJSON.CreateObject(json);

			await db.Insert<ValidatedJSON>(jsonObj);
			Console.WriteLine("*********** {0} rows now in table", await db.GetRowCountForTable<ValidatedJSON>());

			jsonResult.TextColor = UIColor.DarkGray;
			jsonResult.Text = jsonObj.IsValid() ? "Valid JSON" : jsonObj.error;
		}

		void NetworkCallbacks.OnFail()
		{
			UIAlertView error = new UIAlertView("Error", "Network error", null, "OK", null);
			error.Show();
		}

		#endregion
	}
}