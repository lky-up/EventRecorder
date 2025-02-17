using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Views;
using Android.Widget;
using MyMauiApp.Platforms.Android;
using Timeline;
using Resource = Timeline.Resource;

[BroadcastReceiver(Label = "TimelineWidget")]
[IntentFilter(new[] { AppWidgetManager.ActionAppwidgetUpdate })]
[MetaData("android.appwidget.provider", Resource = "@xml/widget_info")]
public class MyAppWidgetProvider : AppWidgetProvider
{
    public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
    {
        foreach (var appWidgetId in appWidgetIds)
        {
           //创建 RemoteViews 实例
           var views = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);
            string buttonTexts = Preferences.Get("PreferenceString", "");
            string buttonIDs = Preferences.Get("PrefrenceIDString", "");
            string[] buttonTxts = buttonTexts.Split('|');

            for (int i = 0; i < buttonTxts.Length && i < 4; i++)
            {
                views.SetTextViewText(GetButtonId(i), buttonTxts[i]);
                views.SetOnClickPendingIntent(GetButtonId(i), CreatePendingIntent(context, i, buttonTxts[i]));
            }

            for (int i = buttonTxts.Length; i < 4; i++)
            {
                views.SetViewVisibility(GetButtonId(i), ViewStates.Gone);
            }

            // 更新小组件
            appWidgetManager.UpdateAppWidget(appWidgetId, views);
        }
    }

    private int GetButtonId(int index)
    {
        // 根据索引返回按钮的 ID
        return index switch
        {
            0 => Resource.Id.button_action_1,
            1 => Resource.Id.button_action_2,
            2 => Resource.Id.button_action_3,
            3 => Resource.Id.button_action_4,
            _ => Resource.Id.button_action_1, // 默认返回第一个按钮
        };
    }

    private PendingIntent CreatePendingIntent(Context context, int buttonIndex, string buttonTxt)
    {
        var intent = new Intent(context, typeof(MyBroadcastReceiver));
        intent.SetAction($"com.companyname.timeline.{buttonTxt}");
        return PendingIntent.GetBroadcast(context, buttonIndex, intent, PendingIntentFlags.UpdateCurrent);
    }
    //private void SetButtonClick(Context context, RemoteViews views, int buttonId, string action)
    //{
    //    var intent = new Intent(context, typeof(MyAppWidgetProvider));
    //    intent.SetAction(action);
    //    var pendingIntent = PendingIntent.GetBroadcast(context, buttonId, intent, PendingIntentFlags.UpdateCurrent);
    //    views.SetOnClickPendingIntent(buttonId, pendingIntent);
    //}

    //public override void OnReceive(Context context, Intent intent)
    //{
    //    base.OnReceive(context, intent);

    //    switch (intent.Action)
    //    {
    //        case "ACTION_BUTTON_1":
    //            // 执行与 APP 内对应按钮相同的功能
    //            Toast.MakeText(context, "Button 1 clicked!", ToastLength.Short).Show();
    //            break;
    //        case "ACTION_BUTTON_2":
    //            Toast.MakeText(context, "Button 2 clicked!", ToastLength.Short).Show();
    //            break;
    //        case "ACTION_BUTTON_3":
    //            Toast.MakeText(context, "Button 3 clicked!", ToastLength.Short).Show();
    //            break;
    //        case "ACTION_BUTTON_4":
    //            Toast.MakeText(context, "Button 4 clicked!", ToastLength.Short).Show();
    //            break;
    //    }
    //}
}