using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Core;

namespace iPhoneUsingCore
{
	public partial class PastResultsViewController : UIViewController
	{
		private DBManager db;

		public PastResultsViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			db = DBManager.GetInstance();

			InitOutlets();

			LoadFromDB();
		}

		private void InitOutlets()
		{

			this.clearButton.TouchUpInside += delegate {
				UIAlertView confirm = new UIAlertView("Confirm delete", "Are you sure you want to delete all saved results?", null, "OK", new string[] { "Cancel" });

				confirm.Clicked += (object sender, UIButtonEventArgs e) => {
					if (e.ButtonIndex == 0)
					{
						db.ClearTable<ValidatedJSON>();

						UpdateUI(new List<ValidatedJSON>());
					}
				};

				confirm.Show();
			};
		}

		private async void LoadFromDB()
		{
			this.loadingSpinner.Center = this.View.Center;
			this.loadingSpinner.Layer.CornerRadius = 3F;
			this.loadingSpinner.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5F);

			List<ValidatedJSON> entries = await db.GetAll<ValidatedJSON>();
			UpdateUI(entries);
		}

		private void UpdateUI(List<ValidatedJSON> entries)
		{
			this.loadingSpinner.StopAnimating();

			if (entries.Count > 0)
			{
				this.clearButton.Enabled = true;

				this.pastResultsList.Source = new MyTableSource(entries);
				this.pastResultsList.ReloadData();

				this.loadingSpinner.Hidden = true;
				this.pastResultsList.Hidden = false;
			}
			else
			{
				this.clearButton.Enabled = false;
				this.pastResultsList.Hidden = true;
			}
		}
	}

	/*
	 * If you are looking at Objective-C code or documentation, be aware that Xamarin.iOS aggregates
	 * the UITableViewDelegate protocol and UITableViewDataSource class into this one class. There is
	 * no comparable UITableViewSource class or protocol in Objective-C.
	 */
	public class MyTableSource : UITableViewSource
	{
		private static NSString identifier = new NSString("cell");
		private static string fontName = "Courier";
		private static int fontSize = 12;

		public List<ValidatedJSON> Entries { private get; set; }

		public MyTableSource(List<ValidatedJSON> vals)
		{
			Entries = vals;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return Entries.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(identifier);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, identifier);
			}
			cell.TextLabel.Text = Entries[indexPath.Row].AsJSON();
			cell.TextLabel.Font = UIFont.FromName(fontName, fontSize);
			cell.TextLabel.Lines = 100;
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			string json = Entries[indexPath.Row].AsJSON();
			int newlines = json.Split('\n').Length + 1;
			return ((NSString) json).StringSize(UIFont.FromName(fontName, fontSize)).Height * newlines;
		}
	}
}

