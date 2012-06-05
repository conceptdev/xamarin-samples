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
var context = UIGraphics.GetCurrentContext ();




//// Gradient Declarations
var calendarGradientColors = new CGColor [] {UIColor.DarkGray.CGColor, UIColor.FromRGBA(0.72f, 0.72f, 0.72f, 1.00f).CGColor, UIColor.White.CGColor};
var calendarGradientLocations = new float [] {0, 0.04f, 0.86f};
var calendarGradient = new CGGradient(colorSpace, calendarGradientColors, calendarGradientLocations);

//// Shadow Declarations
var shadow = UIColor.DarkGray.CGColor;
var shadowOffset = new SizeF(2, 2);
var shadowBlurRadius = 2;

//// Abstracted Graphic Attributes
var monthContent = "MAR";
var dayContent = "24";
var dayFont = UIFont.FromName("Helvetica-Bold", 24);


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


//// Month Drawing
var monthRect = new RectangleF(4, 25, 34, 15);
UIColor.Black.SetFill();
new NSString(monthContent).DrawString(monthRect, UIFont.FromName("Helvetica-Bold", 9), UILineBreakMode.WordWrap, UITextAlignment.Center);


//// Day Drawing
var dayRect = new RectangleF(-6, 1, 54, 31);
UIColor.Black.SetFill();
new NSString(dayContent).DrawString(dayRect, dayFont, UILineBreakMode.WordWrap, UITextAlignment.Center);







			// ------------- END PAINTCODE ----------------

			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;

		}
	}
}