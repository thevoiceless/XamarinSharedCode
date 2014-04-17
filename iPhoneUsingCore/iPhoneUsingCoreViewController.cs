using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Core;

namespace iPhoneUsingCore
{
	public partial class iPhoneUsingCoreViewController : UIViewController
	{
		private int count = 1;

		public iPhoneUsingCoreViewController (IntPtr handle) : base (handle)
		{
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Attach an event to our button outlet
			this.countButton.TouchUpInside += delegate {
				countButton.SetTitle(String.Format("{0} clicks!", count++), UIControlState.Normal);
			};

			// Try accessing something from Core
			CoreNetworkController controller = new CoreNetworkController();
			controller.PrintSomething();

			// Network request
			this.networkButton.TouchUpInside += delegate {
				controller.MakeRequest();
			};
		}

		#endregion
	}
}