using Android.Service.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Java.Util;
using Plugin.BluetoothLE;

namespace NotiicationRelay
{
    [Service(Label = "MyNotificationListenerService", Permission = "android.permission.BIND_NOTIFICATION_LISTENER_SERVICE")]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    class NLService : NotificationListenerService
    {
        public Queue<string> sbnList = new Queue<string>();
        IDevice d;
        bool flashing = false;

        private void DeviceDiscovered(IDevice device)
        {
            if (device.Name == "Bluno")
            {
                CrossBleAdapter.Current.StopScan();
                d = device;

                d.WhenConnected().Subscribe(result => Console.WriteLine("Connected"));
                d.WhenConnectionFailed().Subscribe(result => Console.WriteLine("Failed"));

                d.WhenAnyCharacteristicDiscovered().Subscribe(characteristic => {
                    // read, write, or subscribe to notifications here
                    Console.WriteLine(characteristic.Uuid.ToString());
                });

                d.Connect();
            }
        }

        public override void OnCreate()
        {
            CrossBleAdapter.Current.ScanForUniqueDevices().Subscribe(device => DeviceDiscovered(device));
            base.OnCreate();
        }

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {
            sbnList.Enqueue(sbn.PackageName);

            while (sbnList.Count > 0)
            {
                string name = sbnList.Dequeue();
                string cmd = "255,255,255,100\n";

                if (name.Contains("textra"))
                {
                    cmd = "0,0,255,200\n";
                }
                else if (name.Contains("outlook"))
                {
                    cmd = "255,165,0,200\n";
                }
                else if (name.Contains("signal"))
                {
                    cmd = "255,0,0,200\n";
                }
                else if (name.Contains("slack"))
                {
                    cmd = "0,255,0,200\n";
                }

                UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] buffer = encoding.GetBytes(cmd);

                d.WriteCharacteristic(new Guid("0000dfb0-0000-1000-8000-00805f9b34fb"), new Guid("0000dfb1-0000-1000-8000-00805f9b34fb"), buffer).Subscribe(
                    result => Console.WriteLine(result)
                );
                
            }

            base.OnNotificationPosted(sbn);
        }
    }
}
