using Android.Service.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;

namespace NotiicationRelay
{
    [Service(Label = "MyNotificationListenerService", Permission = "android.permission.BIND_NOTIFICATION_LISTENER_SERVICE")]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    class NLService : NotificationListenerService
    {
        public Queue<string> sbnList = new Queue<string>();

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {

            sbnList.Enqueue(sbn.PackageName);
            if (sbnList.Count > 5)
            {
                sbnList.Dequeue();
            }
            base.OnNotificationPosted(sbn);

        }
    }
}
