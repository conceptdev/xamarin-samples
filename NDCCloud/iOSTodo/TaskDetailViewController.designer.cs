// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace iOSTodo
{
	[Register ("TaskDetailViewController")]
	partial class TaskDetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton DeleteButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch DoneSwitch { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField NotesText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton SaveButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton SpeakButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField TitleText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleText != null) {
				TitleText.Dispose ();
				TitleText = null;
			}

			if (NotesText != null) {
				NotesText.Dispose ();
				NotesText = null;
			}

			if (DoneSwitch != null) {
				DoneSwitch.Dispose ();
				DoneSwitch = null;
			}

			if (DeleteButton != null) {
				DeleteButton.Dispose ();
				DeleteButton = null;
			}

			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}

			if (SpeakButton != null) {
				SpeakButton.Dispose ();
				SpeakButton = null;
			}
		}
	}
}
