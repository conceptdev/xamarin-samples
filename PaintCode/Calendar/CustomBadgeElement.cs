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
			var colorSpace = CGColorSpace.CreateDeviceRGB ();
			var context = UIGraphics.GetCurrentContext ();

//// Color Declarations
			UIColor dateRed = UIColor.FromRGBA (0.83f, 0.11f, 0.06f, 1.00f);

//// Gradient Declarations
			var greyGradientColors = new CGColor [] {
				UIColor.White.CGColor,
				UIColor.FromRGBA (0.57f, 0.57f, 0.57f, 1.00f).CGColor,
				UIColor.Black.CGColor
			};
			var greyGradientLocations = new float [] {0.63f, 0.71f, 0.71f};
			var greyGradient = new CGGradient (colorSpace, greyGradientColors, greyGradientLocations);

//// Shadow Declarations
			var dropShadow = UIColor.DarkGray.CGColor;
			var dropShadowOffset = new SizeF (2, 2);
			var dropShadowBlurRadius = 1;

//// Frames
			var frame = new RectangleF (0, 0, 42, 42);

//// Abstracted Graphic Attributes
			var textContent = "24";
			var text2Content = "MAR";


//// Rounded Rectangle Drawing
			var roundedRectangleRect = new RectangleF (
				frame.GetMinX() + 1.5f,
				frame.GetMinY() + 1.5f,
				frame.Width - 4,
				frame.Height - 4
			);
			var roundedRectanglePath = UIBezierPath.FromRoundedRect (roundedRectangleRect, 4);
			context.SaveState ();
			context.SetShadowWithColor (dropShadowOffset, dropShadowBlurRadius, dropShadow);
			context.BeginTransparencyLayer (null);
			roundedRectanglePath.AddClip ();
			context.DrawLinearGradient (greyGradient,
    new PointF (roundedRectangleRect.GetMidX (), roundedRectangleRect.GetMinY ()),
    new PointF (roundedRectangleRect.GetMidX (), roundedRectangleRect.GetMaxY ()),
    0);
			context.EndTransparencyLayer ();
			context.RestoreState ();

			UIColor.Black.SetStroke ();
			roundedRectanglePath.LineWidth = 1;
			roundedRectanglePath.Stroke ();


//// Rounded Rectangle 2 Drawing
			var roundedRectangle2Path = UIBezierPath.FromRoundedRect (
				new RectangleF(frame.GetMinX() + 2, frame.GetMinY() + frame.Height - 3 - (float)Math.Floor((frame.Height - 3) * 0.28f), frame.Width - 5, (float)Math.Floor((frame.Height - 3) * 0.28f)),
				UIRectCorner.BottomLeft | UIRectCorner.BottomRight,
				new SizeF(4, 4)
			);
			dateRed.SetFill ();
			roundedRectangle2Path.Fill ();



//// Text Drawing
			var textRect = new RectangleF (frame.GetMinX () + 2, frame.GetMinY () + 0, frame.Width - 5, frame.Height - 16);
			UIColor.Black.SetFill ();
			new NSString (textContent).DrawString (
				textRect,
				UIFont.FromName("Helvetica-Bold", 24),
				UILineBreakMode.WordWrap,
				UITextAlignment.Center
			);


//// Text 2 Drawing
			var text2Rect = new RectangleF (
				frame.GetMinX() + 2,
				frame.GetMinY() + frame.Height - 0 - (float)Math.Floor((frame.Height - 0) * 0.38f),
				frame.Width - 5,
				(float)Math.Floor((frame.Height - 0) * 0.38f)
			);
			UIColor.White.SetFill ();
			new NSString (text2Content).DrawString (
				text2Rect,
				UIFont.FromName("HelveticaNeue-Bold", 9),
				UILineBreakMode.WordWrap,
				UITextAlignment.Center
			);









			// ------------- END PAINTCODE ----------------

			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;

		}
	}
}