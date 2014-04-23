using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Core;

namespace AndroidUsingCore
{
	[Activity (Label = "@string/past_results")]			
	public class PastResultsActivity : ListActivity
	{
		View layout;
		ProgressBar progressBar;
		Button clearButton;

		DBManager db;
		ValidatedJSONAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.PastResults);
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			layout = FindViewById<LinearLayout>(Resource.Id.resultsLayout);
			progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
			clearButton = FindViewById<Button>(Resource.Id.clearButton);

			db = DBManager.GetInstance();

			InitListeners();

			LoadFromDB();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		void InitListeners()
		{
			clearButton.Click += delegate {
				AlertDialog.Builder confirm = new AlertDialog.Builder(this);

				confirm.SetTitle(Resource.String.confirm_delete);
				confirm.SetMessage(Resource.String.confirm_delete_msg);

				confirm.SetPositiveButton(Android.Resource.String.Ok,
					delegate(object sender, DialogClickEventArgs e) {
						db.ClearTable<ValidatedJSON>();

						clearButton.Enabled = false;

						adapter.Entries = new List<ValidatedJSON>();
						adapter.NotifyDataSetChanged();

						((AlertDialog) sender).Dismiss();
					});

				confirm.SetNegativeButton(Android.Resource.String.Cancel,
					delegate(object sender, DialogClickEventArgs e) {
						((AlertDialog) sender).Dismiss();
					});

				confirm.Show();
			};
		}

		async void LoadFromDB()
		{
			List<ValidatedJSON> entries = await db.GetAll<ValidatedJSON>();
			clearButton.Enabled = (entries.Count > 0) ? true : false;
			adapter = new ValidatedJSONAdapter(this, entries);
			ListAdapter = adapter;

			progressBar.Visibility = ViewStates.Gone;
			layout.Visibility = ViewStates.Visible;
		}
	}

	class ValidatedJSONAdapter : BaseAdapter
	{
		Context context;
		public List<ValidatedJSON> Entries { get; set; }

		public ValidatedJSONAdapter(Context ctx, List<ValidatedJSON> vals)
		{
			context = ctx;
			Entries = vals;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			if (convertView == null)
			{
				LayoutInflater inflater = LayoutInflater.FromContext(context);
				convertView = inflater.Inflate(Resource.Layout.JsonListItem, parent, false);
			}

			convertView.FindViewById<TextView>(Resource.Id.jsonText).Text = Entries[position].AsJSON();

			return convertView;
		}

		public override int Count {
			get { return (Entries == null) ? 0 : Entries.Count; }
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

