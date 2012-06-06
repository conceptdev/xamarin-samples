using System;
using MonoTouch.UIKit;

namespace PaintCode
{
	public class DrawingViewController : UIViewController
	{
		public DrawingViewController ()
		{
		}

		UIView drawing;
		UIButton glassButton;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			drawing = new DrawingView ();
			drawing.Frame = new System.Drawing.RectangleF (0, 0, 320, 640);
			
			View.AddSubview (drawing);
		}
	}
}

