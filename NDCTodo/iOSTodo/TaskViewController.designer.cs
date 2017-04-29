// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace NDCTodo
{
	[Register ("TaskViewController")]
	partial class TaskViewController
	{
		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UISwitch DoneSwitch { get; set; }

		[Outlet]
		UIKit.UIButton SaveButton { get; set; }

		[Outlet]
		UIKit.UITextField TitleText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (DoneSwitch != null) {
				DoneSwitch.Dispose ();
				DoneSwitch = null;
			}

			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}

			if (TitleText != null) {
				TitleText.Dispose ();
				TitleText = null;
			}
		}
	}
}
