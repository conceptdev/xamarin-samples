using System.Net;
using MonoTouch.Foundation;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using MonoTouch.UIKit;
using System.Drawing;
using System;


namespace HttpClient
{
    public class Renderer : HttpPortable.IRenderer
    {
        AppDelegate ad;
        public Renderer(AppDelegate ad)
        {
            this.ad = ad;
        }

        public void RenderStream(System.IO.Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

            ad.InvokeOnMainThread(delegate
            {
                var view = new UIViewController();
                var label = new UILabel(new RectangleF(20, 20, 300, 80))
                {
                    Text = "The HTML returned by the server:"
                };
                var tv = new UITextView(new RectangleF(20, 100, 300, 400))
                {
                    Text = reader.ReadToEnd()
                };
                view.Add(label);
                view.Add(tv);

                Console.WriteLine(tv.Text);

                if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
                {
                    view.EdgesForExtendedLayout = UIRectEdge.None;
                }

                ad.NavigationController.PushViewController(view, true);
            });
        }
    }
}