﻿using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Core;

namespace iPhoneUsingCore
{
	public partial class PastResultsViewController : UIViewController
	{
		public PastResultsViewController (IntPtr handle) : base (handle)
		{
			Console.WriteLine("********************** PastResultsViewController");
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			DBManager db = DBManager.GetInstance();
			this.pastResultsList.Source = new MyTableSource(db.GetAll<ValidatedJSON>());
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

		private List<ValidatedJSON> entries;

		public MyTableSource(List<ValidatedJSON> vals)
		{
			entries = vals;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return entries.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(identifier);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, identifier);
			}
			cell.TextLabel.Text = entries[indexPath.Row].AsJSON();
			cell.TextLabel.Font = UIFont.FromName(fontName, fontSize);
			cell.TextLabel.Lines = 100;
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			string json = entries[indexPath.Row].AsJSON();
			int newlines = json.Split('\n').Length + 1;
			return ((NSString) json).StringSize(UIFont.FromName(fontName, fontSize)).Height * newlines;
		}
	}
}
