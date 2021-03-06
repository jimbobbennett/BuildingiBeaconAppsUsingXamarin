﻿using Foundation;
using UIKit;
using CoreLocation;
using System.Diagnostics;

namespace iBeaconApp
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        private CLLocationManager _locationManager;
        private CLBeaconRegion _region;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            var userNotificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert, new NSSet());
            UIApplication.SharedApplication.RegisterUserNotificationSettings(userNotificationSettings);

            _locationManager = new CLLocationManager();
            _locationManager.AuthorizationChanged += LocationManagerAuthorizationChanged;
            _locationManager.RegionEntered += LocationManagerRegionEntered;
            _locationManager.RegionLeft += LocationManagerRegionLeft;

            _locationManager.RequestAlwaysAuthorization();

            _region = new CLBeaconRegion(new NSUuid("18E1FDEA-15E6-425E-B88C-2642B8F3C378"), "MyRegion");

            return true;
        }

        void LocationManagerAuthorizationChanged (object sender, CLAuthorizationChangedEventArgs e)
        {
            Debug.WriteLine("Authorisation state: {0}", e.Status);

            if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
            {
                _locationManager.StartMonitoring(_region);
            }
        }

        void LocationManagerRegionLeft (object sender, CLRegionEventArgs e)
        {
            var notification = new UILocalNotification();
            notification.AlertBody = "Goodbye iBeacon!";
            UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
        }

        void LocationManagerRegionEntered (object sender, CLRegionEventArgs e)
        {
            var notification = new UILocalNotification();
            notification.AlertBody = "Hello iBeacon!";
            UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


