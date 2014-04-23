using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Core;

namespace iPhoneUsingCore
{
	public partial class iPhoneUsingCoreViewController : UIViewController, NetworkCallbacks
	{
		private CoreNetworkController controller;
		private DBManager db;

		private int count = 1;

		public iPhoneUsingCoreViewController(IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			InitDB();

			controller = new CoreNetworkController();

			InitOutlets();

			// Couldn't get the gesture detector to work via the storyboard
			UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer(DismissKeyboard);
			tapRecognizer.NumberOfTapsRequired = 1;
			this.View.AddGestureRecognizer(tapRecognizer);
		}

		partial void DismissKeyboard(NSObject sender)
		{
			_DismissKeyboard();
		}

		// Want to dismiss the keyboard when pressing the buttons or tapping elsewhere
		private void _DismissKeyboard()
		{
			this.enterJson.ResignFirstResponder();
		}

		private void InitOutlets()
		{
			this.countButton.TouchUpInside += delegate {
				countButton.SetTitle(String.Format("{0} clicks!", count++), UIControlState.Normal);
				_DismissKeyboard();
			};

			this.networkButton.TouchUpInside += delegate {
				string enteredJson = this.enterJson.Text;
				controller.ValidateJSON(this, enteredJson);
				_DismissKeyboard();
			};

			this.pastResultsButton.TouchUpInside += delegate {
				// ViewController transition handled in the storyboard
			};
		}

		private async void InitDB()
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

			if (jsonObj.IsValid())
			{
				this.jsonResult.Text = "Valid JSON";
			}
			else
			{
				this.jsonResult.Text = jsonObj.error;
			}
		}

		void NetworkCallbacks.OnFail()
		{
			UIAlertView error = new UIAlertView("Error", "Network error", null, "OK", null);
			error.Show();
		}

		#endregion
	}
}