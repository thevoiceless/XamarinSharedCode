using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Core;

namespace AndroidUsingCore
{
	[Activity (Label = "@string/past_results")]			
	public class PastResultsActivity : ListActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.PastResults);

			DBManager db = DBManager.GetInstance();
			ListAdapter = new ValidatedJSONAdapter(this, db.GetAll<ValidatedJSON>());
		}
	}

	class ValidatedJSONAdapter : BaseAdapter
	{
		private Context context;
		private List<ValidatedJSON> entries;

		public ValidatedJSONAdapter(Context ctx, List<ValidatedJSON> vals)
		{
			context = ctx;
			entries = vals;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			if (convertView == null)
			{
				LayoutInflater inflater = LayoutInflater.FromContext(context);
				convertView = inflater.Inflate(Resource.Layout.JsonListItem, parent, false);
			}

			convertView.FindViewById<TextView>(Resource.Id.jsonText).Text = entries[position].AsJSON();

			return convertView;
		}

		public override int Count {
			get { return (entries == null) ? 0 : entries.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			// https://github.com/xamarin/prebuilt-apps/blob/master/EmployeeDirectory/EmployeeDirectory.Android/PersonAdapter.cs#L102-L115
			// See above for wrapping C# objects in Java ones, going to assume it's not needed here
			return null;
		}

		public override long GetItemId(int position)
		{
			return position;
		}
	}
}

