// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Corpy
{
	[Register ("EmployeeViewController")]
	partial class EmployeeViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel DepartmentLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton CallWorkButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton CallCellButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton EmailButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton TweetButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (DepartmentLabel != null) {
				DepartmentLabel.Dispose ();
				DepartmentLabel = null;
			}

			if (CallWorkButton != null) {
				CallWorkButton.Dispose ();
				CallWorkButton = null;
			}

			if (CallCellButton != null) {
				CallCellButton.Dispose ();
				CallCellButton = null;
			}

			if (EmailButton != null) {
				EmailButton.Dispose ();
				EmailButton = null;
			}

			if (TweetButton != null) {
				TweetButton.Dispose ();
				TweetButton = null;
			}
		}
	}
}
