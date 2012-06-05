using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;

namespace PaintCode
{
	// http://mikebluestein.wordpress.com/2010/02/21/drawing-with-coregraphics-in-monotouch-2/
	public class DrawingView : UIView
	{
		public DrawingView ()
		{
			BackgroundColor = UIColor.White;
		}

		CGPath path;
		
		public DrawingView (IntPtr p) : base(p)
		{
			BackgroundColor = UIColor.White;
		}
			
		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);


//// General Declarations
var colorSpace = CGColorSpace.CreateDeviceRGB();
var context = UIGraphics.GetCurrentContext();

//// Color Declarations
UIColor gold = UIColor.FromRGBA(1.00f, 0.95f, 0.57f, 1.00f);
UIColor brown = UIColor.FromRGBA(0.79f, 0.75f, 0.18f, 1.00f);
UIColor lightBrown = UIColor.FromRGBA(0.69f, 0.57f, 0.23f, 1.00f);

//// Gradient Declarations
var newGradientColors = new CGColor [] {UIColor.Black.CGColor, UIColor.White.CGColor};
var newGradientLocations = new float [] {0, 1};
var newGradient = new CGGradient(colorSpace, newGradientColors, newGradientLocations);
var calendarGradientColors = new CGColor [] {UIColor.DarkGray.CGColor, UIColor.FromRGBA(0.72f, 0.72f, 0.72f, 1.00f).CGColor, UIColor.White.CGColor};
var calendarGradientLocations = new float [] {0, 0.01f, 0.14f};
var calendarGradient = new CGGradient(colorSpace, calendarGradientColors, calendarGradientLocations);

//// Shadow Declarations
var shadow = UIColor.DarkGray.CGColor;
var shadowOffset = new SizeF(2, 2);
var shadowBlurRadius = 2;

//// Abstracted Graphic Attributes
var monthContent = "MAR";
var dayContent = "24";
var dayFont = UIFont.FromName("Helvetica-Bold", 24);
var textContent = "News Headline";


//// Oval 11 Drawing
var oval11Path = UIBezierPath.FromOval(new RectangleF(250.5f, 46.5f, 13, 14));
lightBrown.SetFill();
oval11Path.Fill();

UIColor.Black.SetStroke();
oval11Path.LineWidth = 1;
oval11Path.Stroke();


//// Oval 12 Drawing
var oval12Path = UIBezierPath.FromOval(new RectangleF(274.5f, 46.5f, 13, 14));
lightBrown.SetFill();
oval12Path.Fill();

UIColor.Black.SetStroke();
oval12Path.LineWidth = 1;
oval12Path.Stroke();


//// Rounded Rectangle Drawing
var roundedRectanglePath = UIBezierPath.FromRoundedRect(new RectangleF(2.5f, 60.5f, 37, 36), 4);
context.SaveState();
context.SetShadowWithColor(shadowOffset, shadowBlurRadius, shadow);
context.BeginTransparencyLayer(null);
roundedRectanglePath.AddClip();
context.DrawLinearGradient(calendarGradient, new PointF(21, 96.5f), new PointF(21, 60.5f), 0);
context.EndTransparencyLayer();
context.RestoreState();

UIColor.DarkGray.SetStroke();
roundedRectanglePath.LineWidth = 1;
roundedRectanglePath.Stroke();


//// Rounded Rectangle 3 Drawing
UIBezierPath roundedRectangle3Path = new UIBezierPath();
roundedRectangle3Path.MoveTo(new PointF(3, 91.2f));
roundedRectangle3Path.AddCurveToPoint(new PointF(6.56f, 95), new PointF(3, 93.3f), new PointF(4.32f, 95));
roundedRectangle3Path.AddLineTo(new PointF(34.94f, 95));
roundedRectangle3Path.AddCurveToPoint(new PointF(39, 91.2f), new PointF(37.18f, 95), new PointF(39, 93.3f));
roundedRectangle3Path.AddLineTo(new PointF(39, 87));
roundedRectangle3Path.AddCurveToPoint(new PointF(37.42f, 85.5f), new PointF(39, 84.9f), new PointF(39.66f, 85.5f));
roundedRectangle3Path.AddLineTo(new PointF(4.94f, 85.5f));
roundedRectangle3Path.AddCurveToPoint(new PointF(3, 87), new PointF(2.7f, 85.5f), new PointF(3, 84.9f));
roundedRectangle3Path.AddLineTo(new PointF(3, 91.2f));
roundedRectangle3Path.ClosePath();
UIColor.Red.SetFill();
roundedRectangle3Path.Fill();



//// Month Drawing
var monthRect = new RectangleF(4, 84, 34, 15);
UIColor.White.SetFill();
new NSString(monthContent).DrawString(monthRect, UIFont.FromName("Helvetica-Bold", 9), UILineBreakMode.WordWrap, UITextAlignment.Center);


//// Day Drawing
var dayRect = new RectangleF(-6, 58, 54, 31);
UIColor.Black.SetFill();
new NSString(dayContent).DrawString(dayRect, dayFont, UILineBreakMode.WordWrap, UITextAlignment.Center);


//// Text Drawing
var textRect = new RectangleF(48, 60, 75, 38);
UIColor.Black.SetFill();
new NSString(textContent).DrawString(textRect, UIFont.FromName("Helvetica", 16), UILineBreakMode.WordWrap, UITextAlignment.Left);


//// Star Drawing
UIBezierPath starPath = new UIBezierPath();
starPath.MoveTo(new PointF(25, 14.5f));
starPath.AddLineTo(new PointF(20.24f, 21.45f));
starPath.AddLineTo(new PointF(12.16f, 23.83f));
starPath.AddLineTo(new PointF(17.3f, 30.5f));
starPath.AddLineTo(new PointF(17.06f, 38.92f));
starPath.AddLineTo(new PointF(25, 36.1f));
starPath.AddLineTo(new PointF(32.94f, 38.92f));
starPath.AddLineTo(new PointF(32.7f, 30.5f));
starPath.AddLineTo(new PointF(37.84f, 23.83f));
starPath.AddLineTo(new PointF(29.76f, 21.45f));
starPath.ClosePath();
gold.SetFill();
starPath.Fill();

brown.SetStroke();
starPath.LineWidth = 1;
starPath.Stroke();


//// Bezier Drawing
UIBezierPath bezierPath = new UIBezierPath();
bezierPath.MoveTo(new PointF(250.5f, 16.5f));
bezierPath.AddCurveToPoint(new PointF(234.5f, 41.5f), new PointF(229.5f, 37.5f), new PointF(211.53f, 41.55f));
bezierPath.AddCurveToPoint(new PointF(259.5f, 30.5f), new PointF(257.47f, 41.45f), new PointF(259.5f, 30.5f));
bezierPath.AddCurveToPoint(new PointF(250.5f, 16.5f), new PointF(259.5f, 30.5f), new PointF(271.5f, -4.5f));
bezierPath.ClosePath();
UIColor.Cyan.SetFill();
bezierPath.Fill();

UIColor.Blue.SetStroke();
bezierPath.LineWidth = 1;
bezierPath.Stroke();


//// Rounded Rectangle 2 Drawing
var roundedRectangle2Path = UIBezierPath.FromRoundedRect(new RectangleF(48.5f, 10.5f, 163, 31), 4);
context.SaveState();
roundedRectangle2Path.AddClip();
context.DrawRadialGradient(newGradient,
    new PointF(94.39f, 55.13f), 7.84f,
    new PointF(130, 26), 86.67f,
    CGGradientDrawingOptions.DrawsBeforeStartLocation | CGGradientDrawingOptions.DrawsAfterEndLocation);
context.RestoreState();

UIColor.Black.SetStroke();
roundedRectangle2Path.LineWidth = 1;
roundedRectangle2Path.Stroke();


//// Oval Drawing
var ovalPath = UIBezierPath.FromOval(new RectangleF(153.5f, 49.5f, 47, 47));
gold.SetFill();
ovalPath.Fill();

UIColor.Black.SetStroke();
ovalPath.LineWidth = 1;
ovalPath.Stroke();


//// Oval 2 Drawing
var oval2Path = UIBezierPath.FromOval(new RectangleF(163.5f, 64.5f, 8, 8));
UIColor.Black.SetFill();
oval2Path.Fill();

UIColor.Black.SetStroke();
oval2Path.LineWidth = 1;
oval2Path.Stroke();


//// Oval 3 Drawing
var oval3Path = UIBezierPath.FromOval(new RectangleF(182.5f, 64.5f, 8, 8));
UIColor.Black.SetFill();
oval3Path.Fill();

UIColor.Black.SetStroke();
oval3Path.LineWidth = 1;
oval3Path.Stroke();


//// Bezier 2 Drawing
UIBezierPath bezier2Path = new UIBezierPath();
bezier2Path.MoveTo(new PointF(166.5f, 80.5f));
bezier2Path.AddCurveToPoint(new PointF(179.5f, 85.5f), new PointF(171.75f, 85), new PointF(176.04f, 86.03f));
bezier2Path.AddCurveToPoint(new PointF(188.5f, 79.5f), new PointF(185.27f, 84.62f), new PointF(188.5f, 79.5f));
UIColor.Black.SetStroke();
bezier2Path.LineWidth = 2;
bezier2Path.Stroke();


//// Oval 5 Drawing
var oval5Path = UIBezierPath.FromOval(new RectangleF(250.5f, 52.5f, 36, 33));
lightBrown.SetFill();
oval5Path.Fill();

UIColor.Black.SetStroke();
oval5Path.LineWidth = 1;
oval5Path.Stroke();


//// Oval 6 Drawing
var oval6Path = UIBezierPath.FromOval(new RectangleF(256.5f, 59.5f, 10, 19));
UIColor.White.SetFill();
oval6Path.Fill();

UIColor.Black.SetStroke();
oval6Path.LineWidth = 1;
oval6Path.Stroke();


//// Oval 7 Drawing
var oval7Path = UIBezierPath.FromOval(new RectangleF(269.5f, 59.5f, 10, 19));
UIColor.White.SetFill();
oval7Path.Fill();

UIColor.Black.SetStroke();
oval7Path.LineWidth = 1;
oval7Path.Stroke();


//// Oval 9 Drawing
var oval9Path = UIBezierPath.FromOval(new RectangleF(258.5f, 68.5f, 6, 5));
UIColor.Black.SetFill();
oval9Path.Fill();

UIColor.Black.SetStroke();
oval9Path.LineWidth = 1;
oval9Path.Stroke();


//// Oval 10 Drawing
var oval10Path = UIBezierPath.FromOval(new RectangleF(271.5f, 68.5f, 6, 5));
UIColor.Black.SetFill();
oval10Path.Fill();

UIColor.Black.SetStroke();
oval10Path.LineWidth = 1;
oval10Path.Stroke();


//// Oval 4 Drawing
var oval4Path = UIBezierPath.FromOval(new RectangleF(244.5f, 70.5f, 47, 24));
lightBrown.SetFill();
oval4Path.Fill();

UIColor.Black.SetStroke();
oval4Path.LineWidth = 1;
oval4Path.Stroke();


//// Oval 8 Drawing
var oval8Path = UIBezierPath.FromOval(new RectangleF(261.5f, 77.5f, 9, 4));
UIColor.Black.SetFill();
oval8Path.Fill();

UIColor.Black.SetStroke();
oval8Path.LineWidth = 1;
oval8Path.Stroke();


//// Bezier 5 Drawing
UIBezierPath bezier5Path = new UIBezierPath();
bezier5Path.MoveTo(new PointF(264.5f, 81.5f));
bezier5Path.AddCurveToPoint(new PointF(261.5f, 88.5f), new PointF(263.5f, 85.5f), new PointF(261.5f, 88.5f));
UIColor.Black.SetStroke();
bezier5Path.LineWidth = 1;
bezier5Path.Stroke();


//// Bezier 6 Drawing
UIBezierPath bezier6Path = new UIBezierPath();
bezier6Path.MoveTo(new PointF(266.5f, 81.5f));
bezier6Path.AddLineTo(new PointF(268.5f, 87.5f));
UIColor.Black.SetStroke();
bezier6Path.LineWidth = 1;
bezier6Path.Stroke();






		}

	}
}

