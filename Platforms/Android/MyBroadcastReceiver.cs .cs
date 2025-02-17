using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using Microsoft.Extensions.Logging;
using Timeline;

namespace MyMauiApp.Platforms.Android
{
    [BroadcastReceiver]
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            string actionStr = intent.Action.Replace("com.companyname.timeline.","").Trim();
            string retStr = await UpdateDatabaseAsync(actionStr);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    "event_channel", // 渠道ID
                    "事件管理器",    // 渠道名称（用户可见）
                    NotificationImportance.Default // 优先级
                );
                var manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                manager.CreateNotificationChannel(channel);
            }

            var notificationBuilder = new NotificationCompat.Builder(context, "my_channel_id")
                .SetSmallIcon(Microsoft.Maui.Resource.Drawable.icon_notes) // 必须设置小图标
                .SetContentTitle("事件管理通知")          // 通知标题
                .SetContentText(retStr)                         // 通知内容
                .SetPriority(NotificationCompat.PriorityDefault) // 优先级
                .SetAutoCancel(true);                            // 点击后自动关闭

            // 显示通知
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(new Random().Next(1000), notificationBuilder.Build());

            //if (successOrNot)
            //{
                
            //    Toast.MakeText(context, "记录成功", ToastLength.Short).Show();
            //}
            //else
            //{
            //    Toast.MakeText(context, "记录失败", ToastLength.Short).Show();
            //}
        }

        private async Task<string> UpdateDatabaseAsync(string buttonTxt)
        {
            var timeEvents= await App.Database.GetTimeEventsAsync();

            var currentEvent = timeEvents.Where(data => data.Name == buttonTxt).FirstOrDefault();
            TimeEventData timeEventData = new TimeEventData();
            timeEventData.TimeEventId = currentEvent.Id;
            timeEventData.TimeCreated = DateTime.Now;
            if (currentEvent.DefaultValue != null)
            {
                timeEventData.ExtraData = currentEvent.DefaultValue?.ToString();
            }
            var tmpEventsData = await App.Database.SaveTimeEventDataAsync(timeEventData);
            if (tmpEventsData > 0)
            {
                return "记录成功："+ buttonTxt + " 当前时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
            }
            return "记录失败：" + buttonTxt + " 当前时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
        }
    }
}