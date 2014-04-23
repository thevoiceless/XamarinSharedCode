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

namespace AndroidUsingCore
{
	[Activity (Label = "@string/more_stuff")]			
	public class MoreStuffActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.MoreStuff);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
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
	}
}

