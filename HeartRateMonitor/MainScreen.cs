using System;
using System.Drawing;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreImage;
using MonoTouch.CoreBluetooth;
using System.Collections.Generic;

namespace Xamarin.HeartMonitor
{
	/// <summary>
	/// View containing Buttons, TextView and ImageViews to show off the samples
	/// </summary>
	/// <remarks>
	/// See the 'SampleCode.cs' file for the actual sample code
	/// </remarks> 
	public class MainScreen : UIViewController
	{
		CBCentralManager manager = new CBCentralManager ();
		HeartRateMonitor monitor;

		List<HeartRateMonitor> heartRateMonitors = new List<HeartRateMonitor> ();

		UILabel statusLabel, heartRateLabel, heartRateUnitLabel, deviceNameLabel;
		UIButton connectButton;

		public override void ViewDidLoad ()
		{	
			base.ViewDidLoad ();
			

			statusLabel = new UILabel (new RectangleF(10, 30, 300, 30));

			heartRateLabel = new UILabel(new RectangleF(10, 70, 150, 30));
			heartRateLabel.Font = UIFont.BoldSystemFontOfSize (36);
			heartRateLabel.TextColor = UIColor.Red;

			heartRateUnitLabel = new UILabel(new RectangleF(160, 70, 150, 30));

			deviceNameLabel = new UILabel(new RectangleF(10, 120, 300, 30));

			connectButton = UIButton.FromType (UIButtonType.System);
			connectButton.SetTitle ("Connect", UIControlState.Normal);
			connectButton.Frame = new RectangleF (10, 160, 300, 30);
			connectButton.TouchUpInside += ConnectToSelectedDevice;

			Add (statusLabel);
			Add (heartRateLabel);
			Add (heartRateUnitLabel);
			Add (deviceNameLabel);
			Add (connectButton);

			InitializeCoreBluetooth ();
		}


		void ConnectToSelectedDevice (object sender, EventArgs e)
		{
			monitor = heartRateMonitors [0];
			Console.WriteLine ("monitor:" + monitor);
			if (monitor != null) {
				statusLabel.Text = "Connecting to " + monitor.Name + " ... ";
				monitor.Connect ();
			}
		}

		#region Heart Pulse UI

		void DisconnectMonitor ()
		{
			statusLabel.Text = "Not connected";
			heartRateLabel.Text = "0";
			heartRateLabel.Hidden = true;
			heartRateUnitLabel.Hidden = true;
			deviceNameLabel.Text = String.Empty;
			deviceNameLabel.Hidden = true;

			if (monitor != null) {
				monitor.Dispose ();
				monitor = null;
			}
		}

		void OnHeartRateUpdated (object sender, HeartBeatEventArgs e)
		{
			heartRateUnitLabel.Hidden = false;
			heartRateLabel.Hidden = false;
			heartRateLabel.Text = e.CurrentHeartBeat.Rate.ToString();

			var monitor = (HeartRateMonitor)sender;
			if (monitor.Location == HeartRateMonitorLocation.Unknown) {
				statusLabel.Text = "Connected";
			} else {
				statusLabel.Text = String.Format ("Connected on {0}", monitor.Location);
			}

			deviceNameLabel.Hidden = false;
			deviceNameLabel.Text = monitor.Name;
		}

		void OnHeartBeat (object sender, EventArgs e)
		{
			// TODO: removed the animation stuff from Mac
			Console.WriteLine ("OnHeartBeat");
		}

		#endregion

		#region Bluetooth

		void InitializeCoreBluetooth ()
		{
			manager.UpdatedState += OnCentralManagerUpdatedState;

			//HACK: Modified this to just quit after finding the first heart rate monitor
			EventHandler<CBDiscoveredPeripheralEventArgs> discovered = null;
			discovered += (sender, e) => {
				if (monitor != null) {
					monitor.Dispose ();
				}

				monitor = new HeartRateMonitor (manager, e.Peripheral);
				monitor.HeartRateUpdated += OnHeartRateUpdated;
				monitor.HeartBeat += OnHeartBeat;

				heartRateMonitors.Add (monitor);

				//HACK: instead of adding to a list, just use this one
				statusLabel.Text = "Found " + monitor.Name + ".";
				manager.DiscoveredPeripheral -= discovered;
			};
			manager.DiscoveredPeripheral += discovered;

			manager.ConnectedPeripheral += (sender, e) => e.Peripheral.DiscoverServices ();

			manager.DisconnectedPeripheral += (sender, e) => DisconnectMonitor ();

			HeartRateMonitor.ScanForHeartRateMonitors (manager);
		}

		void OnCentralManagerUpdatedState (object sender, EventArgs e)
		{
			string message = null;

			switch (manager.State) {
			case CBCentralManagerState.PoweredOn:
				connectButton.Enabled = true;
				return;
			case CBCentralManagerState.Unsupported:
				message = "The platform or hardware does not support Bluetooth Low Energy.";
				break;
			case CBCentralManagerState.Unauthorized:
				message = "The application is not authorized to use Bluetooth Low Energy.";
				break;
			case CBCentralManagerState.PoweredOff:
				message = "Bluetooth is currently powered off.";
				break;
			default:
				break;
			}

			if (message != null) {
				var alert = new UIAlertView ("Alert", message, null, "OK", null);
				alert.Show ();
			}
		}

		#endregion
	}
}