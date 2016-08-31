using System;
using Android.App;
using Android.Appwidget;
using Android.Content;

namespace XWeather.Droid
{
	[BroadcastReceiver (Label = "XWeather")]
	[IntentFilter (new string [] { "android.appwidget.action.APPWIDGET_UPDATE" })]
	[MetaData ("android.appwidget.provider", Resource = "@xml/widget_provider")]
	public class AppWidget : AppWidgetProvider
	{

		public override void OnUpdate (Context context, AppWidgetManager appWidgetManager, int [] appWidgetIds)
		{
			base.OnUpdate (context, appWidgetManager, appWidgetIds);
		}


	}
}
