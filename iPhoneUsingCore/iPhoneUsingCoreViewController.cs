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

			// Attach delegates to our button outlets
			this.countButton.TouchUpInside += delegate {
				countButton.SetTitle(String.Format("{0} clicks!", count++), UIControlState.Normal);
			};

			this.networkButton.TouchUpInside += delegate {
				string enteredJson = this.enterJson.Text;
				controller.MakeRequest(enteredJson, this);
			};

			this.pastResultsButton.TouchUpInside += delegate {
				// TODO
			};
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
			// TODO
		}

		#endregion
	}
}