//
// ElementBadge.cs: defines the Badge Element.
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
using System;
using System.Collections;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;

namespace PaintCode {
	/// <summary>
	/// Lifted this code from MT.D source, so it could be customized
	/// </summary>
	public class CustomBadgeElement {
		public CustomBadgeElement ()
		{
		}
		public static UIImage MakeCalendarBadge (UIImage template, string smallText, string bigText)
		{

			UIGraphics.BeginImageContext (new SizeF (42, 42));

			// ------------- START PAINTCODE ----------------


//// General Declarations
var colorSpace = CGColorSpace.CreateDeviceRGB();
var context = UIGraphics.GetCurrentContext();

//// Color Declarations
UIColor newGradientColor = UIColor.FromRGBA(0.00f, 0.01f, 0.01f, 1.00f);
UIColor newGradientColor2 = UIColor.FromRGBA(0.76f, 0.09f, 0.06f, 1.00f);

//// Gradient Declarations
var newGradientColors = new CGColor [] {newGradientColor.CGColor, UIColor.FromRGBA(0.43f, 0.04f, 0.02f, 1.00f).CGColor, newGradientColor2.CGColor};
var newGradientLocations = new float [] {0, 0, 0.32f};
var newGradient = new CGGradient(colorSpace, newGradientColors, newGradientLocations);
var calendarGradientColors = new CGColor [] {UIColor.DarkGray.CGColor, UIColor.FromRGBA(0.72f, 0.72f, 0.72f, 1.00f).CGColor, UIColor.White.CGColor};
var calendarGradientLocations = new float [] {0.23f, 0.25f, 0.57f};
var calendarGradient = new CGGradient(colorSpace, calendarGradientColors, calendarGradientLocations);

//// Shadow Declarations
var shadow = UIColor.DarkGray.CGColor;
var shadowOffset = new SizeF(2, 2);
var shadowBlurRadius = 2;

//// Abstracted Graphic Attributes
var dayContent = bigText;
var dayFont = UIFont.FromName("Helvetica-Bold", 24);
var monthContent = smallText;


//// Rounded Rectangle Drawing
var roundedRectanglePath = UIBezierPath.FromRoundedRect(new RectangleF(2.5f, 2.5f, 37, 36), 4);
context.SaveState();
context.SetShadowWithColor(shadowOffset, shadowBlurRadius, shadow);
context.BeginTransparencyLayer(null);
roundedRectanglePath.AddClip();
context.DrawLinearGradient(calendarGradient, new PointF(21, 38.5f), new PointF(21, 2.5f), 0);
context.EndTransparencyLayer();
context.RestoreState();

UIColor.DarkGray.SetStroke();
roundedRectanglePath.LineWidth = 1;
roundedRectanglePath.Stroke();


//// Day Drawing
var dayRect = new RectangleF(-6, 0, 54, 31);
UIColor.Black.SetFill();
new NSString(dayContent).DrawString(dayRect, dayFont, UILineBreakMode.WordWrap, UITextAlignment.Center);


//// Rounded Rectangle 3 Drawing
UIBezierPath roundedRectangle3Path = new UIBezierPath();
roundedRectangle3Path.MoveTo(new PointF(3, 33.8f));
roundedRectangle3Path.AddCurveToPoint(new PointF(6.56f, 38), new PointF(3, 36.12f), new PointF(4.32f, 38));
roundedRectangle3Path.AddLineTo(new PointF(34.94f, 38));
roundedRectangle3Path.AddCurveToPoint(new PointF(39, 33.8f), new PointF(37.18f, 38), new PointF(39, 36.12f));
roundedRectangle3Path.AddLineTo(new PointF(39, 29.16f));
roundedRectangle3Path.AddCurveToPoint(new PointF(37.42f, 27.51f), new PointF(39, 26.85f), new PointF(39.66f, 27.51f));
roundedRectangle3Path.AddLineTo(new PointF(4.94f, 27.51f));
roundedRectangle3Path.AddCurveToPoint(new PointF(3, 29.16f), new PointF(2.7f, 27.51f), new PointF(3, 26.85f));
roundedRectangle3Path.AddLineTo(new PointF(3, 33.8f));
roundedRectangle3Path.ClosePath();
context.SaveState();
roundedRectangle3Path.AddClip();
context.DrawLinearGradient(newGradient, new PointF(21.04f, 27.42f), new PointF(21.04f, 38), 0);
context.RestoreState();



//// Month Drawing
var monthRect = new RectangleF(3, 27, 34, 15);
UIColor.White.SetFill();
new NSString(monthContent).DrawString(monthRect, UIFont.FromName("Helvetica", 9), UILineBreakMode.WordWrap, UITextAlignment.Center);





			// ------------- END PAINTCODE ----------------

			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;

		}
	}
}