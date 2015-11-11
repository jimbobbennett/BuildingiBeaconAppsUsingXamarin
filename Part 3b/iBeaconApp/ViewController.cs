using System;

using UIKit;
using CoreLocation;
using System.Diagnostics;
using Foundation;
using System.Linq;

namespace iBeaconApp
{
    public partial class ViewController : UIViewController
    {
        private CLLocationManager _locationManager;
        private CLBeaconRegion _region1;
        private CLBeaconRegion _region2;

        private double _beacon1Rssi;
        private double _beacon2Rssi;

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            _locationManager = new CLLocationManager();
            _locationManager.AuthorizationChanged += LocationManagerAuthorizationChanged;
            _locationManager.DidRangeBeacons += LocationManagerDidRangeBeacons;

            _locationManager.RequestAlwaysAuthorization();

            _region1 = new CLBeaconRegion(new NSUuid("18E1FDEA-15E6-425E-B88C-2642B8F3C378"), 9358, 5544, "Beacon1");
            _region2 = new CLBeaconRegion(new NSUuid("18E1FDEA-15E6-425E-B88C-2642B8F3C378"), 15274, 22477, "Beacon2");
        }

        void LocationManagerAuthorizationChanged (object sender, CLAuthorizationChangedEventArgs e)
        {
            Debug.WriteLine("Authorisation state: {0}", e.Status);

            if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
            {
                _locationManager.StartRangingBeacons(_region1);
                _locationManager.StartRangingBeacons(_region2);
            }
        }

        void LocationManagerDidRangeBeacons (object sender, CLRegionBeaconsRangedEventArgs e)
        {
            if (e.Region.Identifier == _region1.Identifier)
            {
                var beacon = e.Beacons.FirstOrDefault();
                if (beacon != null)
                    _beacon1Rssi = Math.Min(beacon.Rssi, _beacon1Rssi);
                else
                    _beacon1Rssi = 0;
            }

            if (e.Region.Identifier == _region2.Identifier)
            {
                var beacon = e.Beacons.FirstOrDefault();
                if (beacon != null)
                    _beacon2Rssi = Math.Min(beacon.Rssi, _beacon2Rssi);
                else
                    _beacon2Rssi = 0;
            }

            var message = "No beacons found";

            if (_beacon1Rssi != 0)
            {
                if (_beacon2Rssi == 0)
                    message = "Nearest beacon is Beacon 1";
                else
                    message = "Nearest beacon is Beacon " + (_beacon1Rssi > _beacon2Rssi ? "1" : "2");
            }
            else
            {
                if (_beacon2Rssi != 0)
                    message = "Nearest beacon is Beacon 2";
            }

            NearestBeacon.Text = message;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

