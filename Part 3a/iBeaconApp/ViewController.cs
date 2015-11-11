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
        private CLBeaconRegion _region;

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

            _region = new CLBeaconRegion(new NSUuid("18E1FDEA-15E6-425E-B88C-2642B8F3C378"), "MyRegion");
        }

        void LocationManagerAuthorizationChanged (object sender, CLAuthorizationChangedEventArgs e)
        {
            Debug.WriteLine("Authorisation state: {0}", e.Status);

            if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
            {
                _locationManager.StartRangingBeacons(_region);
            }
        }

        void LocationManagerDidRangeBeacons (object sender, CLRegionBeaconsRangedEventArgs e)
        {
            CLBeacon nearest = null;

            foreach (var beacon in e.Beacons.Where(b => b.Rssi != 0))
            {
                Debug.WriteLine("Ranged {0} {1}.{2} - distance {3}",
                    beacon.ProximityUuid.AsString(),
                    beacon.Major,
                    beacon.Minor,
                    beacon.Rssi);

                if (nearest == null || nearest.Rssi < beacon.Rssi)
                    nearest = beacon;
            }

            if (nearest == null)
                NearestBeacon.Text = "No beacons found";
            else
                NearestBeacon.Text = $"Nearest Beacon is {nearest.Major}.{nearest.Minor}";
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

