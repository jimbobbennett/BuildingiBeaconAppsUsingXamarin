// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iBeaconApp
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NearestBeacon { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (NearestBeacon != null) {
				NearestBeacon.Dispose ();
				NearestBeacon = null;
			}
		}
	}
}
