using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Core;

namespace iPhoneUsingCore
{
	public partial class iPhoneUsingCoreViewController : UIViewController, NetworkCallbacks
	{
		private DBManager db;
		private int count = 1;

		public iPhoneUsingCoreViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Stuff from Core
			CoreNetworkController controller = new CoreNetworkController();
			db = DBManager.GetInstance();
			db.init();

			this.countButton.TouchUpInside += delegate {
				countButton.SetTitle(String.Format("{0} clicks!", count++), UIControlState.Normal);
				_DismissKeyboard();
			};

			this.networkButton.TouchUpInside += delegate {
				string enteredJson = this.enterJson.Text;
				controller.MakeRequest(enteredJson, this);
				_DismissKeyboard();
			};

			this.pastResultsButton.TouchUpInside += delegate {
				// ViewController transition handled in the storyboard
			};

			// Couldn't get the gesture detector to work via the storyboard
			UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer(DismissKeyboard);
			tapRecognizer.NumberOfTapsRequired = 1;
			this.View.AddGestureRecognizer(tapRecognizer);
		}

		partial void DismissKeyboard (NSObject sender)
		{
			_DismissKeyboard();
		}

		// Want to be able to dismiss the keyboard when pressing the buttons
		private void _DismissKeyboard()
		{
			this.enterJson.ResignFirstResponder();
		}

		#region NetworkCallbacks

		void NetworkCallbacks.OnSuccess(Object data)
		{
			string json = (string) data;
			ValidatedJSON jsonObj = ValidatedJSON.CreateObject(json);

			if (jsonObj.IsValid())
			{
				this.jsonResult.Text = "Valid JSON";
			}
			else
			{
				this.jsonResult.Text = jsonObj.error;
			}

			db.Insert<ValidatedJSON>(jsonObj);
			Console.WriteLine("*********** {0} rows now in table", db.GetRowCountForTable<ValidatedJSON>());
		}

		void NetworkCallbacks.OnFail()
		{
			UIAlertView error = new UIAlertView("Error", "Network error", null, "OK", null);
			error.Show();
		}

		#endregion
	}
}