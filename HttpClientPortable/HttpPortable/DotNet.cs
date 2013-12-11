//
// This file contains the sample code to use System.Net.WebRequest
// on the iPhone to communicate with HTTP and HTTPS servers
//
// Author:
//   Miguel de Icaza
//

using System;
using System.Net;
//using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace HttpPortable
{
  public class DotNet {
		IRenderer ad;

        public DotNet(IRenderer ad)
		{
			this.ad = ad;
		}
		
		//
		// Asynchronous HTTP request
		//
        public void HttpSample(string url)
		{
			//Application.Busy ();
			var request = WebRequest.Create (url);
			request.BeginGetResponse (FeedDownloaded, request);
		}
		
		//
		// Invoked when we get the stream back from the twitter feed
		// We parse the RSS feed and push the data into a 
		// table.
		//
		void FeedDownloaded (IAsyncResult result)
		{
		//	Application.Done ();
			var request = result.AsyncState as HttpWebRequest;
			
			try {
				var response = request.EndGetResponse (result);
				ad.RenderStream (response.GetResponseStream ());
			} catch (Exception e) {
				Debug.WriteLine (e);
			}
		}
		
		//
		// Asynchornous HTTPS request
		//
        public void HttpSecureSample(string url)
		{
            throw new NotImplementedException("Some issues with ServicePointManager in PCL (for now)");
            //var https = (HttpWebRequest) WebRequest.Create (url);
            //// 
            //// To not depend on the root certficates, we will
            //// accept any certificates:
            ////
            //ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, ssl) =>  true;

            //https.BeginGetResponse (GmailDownloaded, https);
		}
		
		//
		// This sample just gets the result from calling
		// https://gmail.com, an HTTPS secure connection,
		// we do not attempt to parse the output, but merely
		// dump it as text
		//
		void GmailDownloaded (IAsyncResult result)
		{
		//	Application.Done ();
			var request = result.AsyncState as HttpWebRequest;
			
			try {
            		var response = request.EndGetResponse (result);
                //ad.RenderStream (response.GetResponseStream ());
			} catch {
				// Error
			}
		}
		
		//
		// For an explanation of this AcceptingPolicy class, see
		// http://mono-project.com/UsingTrustedRootsRespectfully
		//
		// This will not be needed in the future, when MonoTouch 
		// pulls the certificates from the iPhone directly
		//
        //class AcceptingPolicy : ICertificatePolicy {
        //    public bool CheckValidationResult (ServicePoint sp, X509Certificate cert, WebRequest req, int error)
        //    {
        //        // Trust everything
        //        return true;
        //    }
        //}	
	}
}
