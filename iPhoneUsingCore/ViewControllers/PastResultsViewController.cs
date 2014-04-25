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
		DBManager db;

		public PastResultsViewController(IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			db = DBManager.GetInstance();

			InitOutlets();

			LoadFromDB();
		}

		void InitOutlets()
		{

			clearButton.TouchUpInside += delegate {
				UIAlertView confirm = new UIAlertView("Confirm delete", "Are you sure you want to delete all saved results?", null, "OK", new string[] { "Cancel" });

				confirm.Clicked += (sender, e) => {
					if (e.ButtonIndex == 0)
					{
						db.ClearTable<ValidatedJSON>();

						UpdateUI(new List<ValidatedJSON>());
					}
				};

				confirm.Show();
			};
		}

		async void LoadFromDB()
		{
			loadingSpinner.Center = View.Center;
			loadingSpinner.Layer.CornerRadius = 3F;
			loadingSpinner.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5F);

			List<ValidatedJSON> entries = await db.GetAll<ValidatedJSON>();
			UpdateUI(entries);
		}

		void UpdateUI(List<ValidatedJSON> entries)
		{
			loadingSpinner.StopAnimating();

			if (entries.Count > 0)
			{
				clearButton.Enabled = true;

				pastResultsList.Source = new MyTableSource(entries);
				pastResultsList.ReloadData();

				loadingSpinner.Hidden = true;
				pastResultsList.Hidden = false;
			}
			else
			{
				clearButton.Enabled = false;
				clearButton.BackgroundColor = UIColor.LightGray;
				clearButton.SetTitleColor(UIColor.DarkGray, UIControlState.Disabled);
				pastResultsList.Hidden = true;
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
		static NSString identifier = new NSString("cell");
		const string fontName = "Courier";
		const int fontSize = 12;
		const int magic_height_fudge = 30;

		public List<ValidatedJSON> Entries { private get; set; }

		public MyTableSource(List<ValidatedJSON> vals)
		{
			Entries = vals;
		}

		public override int RowsInSection(UITableView tableview, int section)
		{
			return Entries.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(identifier);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, identifier);
			}
			cell.TextLabel.Text = Entries[indexPath.Row].AsJSON();
			cell.TextLabel.Font = UIFont.FromName(fontName, fontSize);
			cell.TextLabel.Lines = 100;

			cell.TextLabel.BackgroundColor = UIColor.Clear;
			cell.ContentView.BackgroundColor = UIColor.Clear;

			return cell;
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			string json = Entries[indexPath.Row].AsJSON();
			int newlines = json.Split('\n').Length + 1;
			return ((NSString) json).StringSize(UIFont.FromName(fontName, fontSize), new SizeF(tableView.Bounds.Width, 1000), UILineBreakMode.WordWrap).Height + magic_height_fudge;
//			return ((NSString) json).StringSize(UIFont.FromName(fontName, fontSize)).Height * newlines;
		}
	}
}

