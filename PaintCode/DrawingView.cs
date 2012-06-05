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
			
			//get graphics context
			CGContext gctx = UIGraphics.GetCurrentContext ();
			
			//set up drawing attributes
			gctx.SetLineWidth(4);
			UIColor.Purple.SetFill ();
			UIColor.Black.SetStroke ();
		
			//create geometry
		    path = new CGPath ();
			
			path.AddLines(new PointF[]{
				new PointF(100,200),
				new PointF(160,100), 
				new PointF(220,200)});
			
			path.CloseSubpath();
			
			//add geometry to graphics context and draw it
			gctx.AddPath(path);		
			gctx.DrawPath(CGPathDrawingMode.FillStroke);	
		}

	}
}

