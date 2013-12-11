using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Drawing;
using System;
using Android.Widget;
using Android.App;


namespace HttpClient
{
    public class Renderer : HttpPortable.IRenderer
    {
		Activity activity;
		EditText text;
		public Renderer(Activity context, EditText text)
        {
			this.activity = context;
			this.text = text;
        }

        public void RenderStream(System.IO.Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

			activity.RunOnUiThread(delegate()
            {
				text.Text = "The HTML returned by the server:";
				text.Text += reader.ReadToEnd();
            });
        }
    }
}