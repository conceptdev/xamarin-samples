using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace PaintCode
{
	public class NewsElement : OwnerDrawnElement
	{
	    public NewsElement (string text) : base(UITableViewCellStyle.Default, "NewsElement")
	    {
	        this.Text = text;
	    }
	    public string Text
	    {
	        get;set;   
	    }
	    public override void Draw (RectangleF bounds, CGContext context, UIView view)
	    {
			


//// General Declarations
var colorSpace = CGColorSpace.CreateDeviceRGB();


//// Gradient Declarations
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


//// Rounded Rectangle Drawing
var roundedRectanglePath = UIBezierPath.FromRoundedRect(new RectangleF(2.5f, 3.5f, 37, 36), 4);
context.SaveState();
context.SetShadowWithColor(shadowOffset, shadowBlurRadius, shadow);
context.BeginTransparencyLayer(null);
roundedRectanglePath.AddClip();
context.DrawLinearGradient(calendarGradient, new PointF(21, 3.5f), new PointF(21, 39.5f), 0);
context.EndTransparencyLayer();
context.RestoreState();

UIColor.DarkGray.SetStroke();
roundedRectanglePath.LineWidth = 1;
roundedRectanglePath.Stroke();


//// Month Drawing
var monthRect = new RectangleF(4, 2, 34, 15);
UIColor.Black.SetFill();
new NSString(monthContent).DrawString(monthRect, UIFont.FromName("Helvetica-Bold", 9), UILineBreakMode.WordWrap, UITextAlignment.Center);


//// Day Drawing
var dayRect = new RectangleF(-6, 11, 54, 31);
UIColor.Black.SetFill();
new NSString(dayContent).DrawString(dayRect, dayFont, UILineBreakMode.WordWrap, UITextAlignment.Center);


//// Text Drawing
var textRect = new RectangleF(48, 3, 272, 29);
UIColor.Black.SetFill();
new NSString(textContent).DrawString(textRect, UIFont.FromName("Helvetica", 16), UILineBreakMode.WordWrap, UITextAlignment.Left);




	    }
	    public override float Height (RectangleF bounds)
	    {
	        return 44.0f;
	    }
	 }
}

