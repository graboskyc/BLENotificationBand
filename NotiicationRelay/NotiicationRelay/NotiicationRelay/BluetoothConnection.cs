using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NotiicationRelay
{
    public class BluetoothConnection
    {

        public void getAdapter() { this.thisAdapter = BluetoothAdapter.DefaultAdapter; }
        public void getDevice() { this.thisDevice = (from bd in this.thisAdapter.BondedDevices where bd.Name == "HC-05" select bd).FirstOrDefault(); }

        public BluetoothAdapter thisAdapter { get; set; }
        public BluetoothDevice thisDevice { get; set; }

        public BluetoothSocket thisSocket { get; set; }



    }
}
