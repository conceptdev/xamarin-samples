using System;
using System.Drawing;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreImage;
using MonoTouch.CoreBluetooth;
using System.Collections.Generic;
using MonoTouch.HealthKit;

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

		UILabel statusLabel, heartRateLabel, heartRateUnitLabel, deviceNameLabel, permissionsLabel;
		UIButton connectButton, storeData;

		 

		HKHealthStore healthKitStore;

		public override void ViewDidLoad ()
		{	
			base.ViewDidLoad ();
			
			#region UI controls
			statusLabel = new UILabel (new RectangleF(10, 30, 300, 30));
			statusLabel.Text = "waiting...";

			heartRateLabel = new UILabel(new RectangleF(10, 70, 150, 30));
			heartRateLabel.Font = UIFont.BoldSystemFontOfSize (36);
			heartRateLabel.TextColor = UIColor.Red;

			heartRateUnitLabel = new UILabel(new RectangleF(160, 70, 150, 30));

			deviceNameLabel = new UILabel(new RectangleF(10, 120, 300, 30));

			connectButton = UIButton.FromType (UIButtonType.System);
			connectButton.SetTitle ("Connect", UIControlState.Normal);
			connectButton.SetTitle ("searching...", UIControlState.Disabled);
			connectButton.Enabled = false;
			connectButton.Frame = new RectangleF (10, 160, 300, 30);
			connectButton.TouchUpInside += ConnectToSelectedDevice;


			permissionsLabel = new UILabel (new RectangleF (10, 200, 300, 30));
			permissionsLabel.Text = "Permission was NOT granted to send to HealthKit :-(";
			permissionsLabel.Hidden = false;

			storeData = UIButton.FromType (UIButtonType.System);
			storeData.Frame = new RectangleF (10, 220, 300, 30);
			storeData.SetTitle ("Store in HealthKit", UIControlState.Normal);
			storeData.SetTitle ("requires permission", UIControlState.Disabled);
			storeData.Enabled = false;
			storeData.TouchUpInside += (sender, e) => {
				UpdateHealthKit(heartRateLabel.Text); // pretty hacky :)
			};

			Add (statusLabel);
			Add (heartRateLabel);
			Add (heartRateUnitLabel);
			Add (deviceNameLabel);
			Add (connectButton);
			Add (permissionsLabel);
			Add (storeData);
			#endregion

			InitializeCoreBluetooth ();

			#region HealthKit
			// https://gist.github.com/lobrien/1217d3cff7b29716c0d3
			// http://www.knowing.net/index.php/2014/07/11/exploring-healthkit-with-xamarin-provisioning-and-permissions-illustrated-walkthrough/

			healthKitStore = new HKHealthStore();
			//Permissions
			//Request HealthKit authorization
			var heartRateId = HKQuantityTypeIdentifierKey.HeartRate;
			var heartRateType = HKObjectType.GetQuantityType (heartRateId);
			//Request to write heart rate, read nothing...
			healthKitStore.RequestAuthorizationToShare (new NSSet (new [] { heartRateType }), new NSSet (), (success, error) =>
				InvokeOnMainThread (() => {
					if (success) {
						//Whatever...
						permissionsLabel.Hidden = true;
						storeData.Enabled = true;
					} else {
						//Whatever...
						permissionsLabel.Hidden = false;
						storeData.Enabled = false;
					}
					if (error != null) {
						Console.WriteLine ("HealthKit authorization error: " + error);
					}
				}));
			#endregion

		}
		#region HealthKit
		void UpdateHealthKit(string s) {
			//Creating a heartbeat sample
			int result = 0;
			if (Int32.TryParse (s, out result)) {

				var heartRateId = HKQuantityTypeIdentifierKey.HeartRate;
				var heartRateType = HKObjectType.GetQuantityType (heartRateId);
				var heartRateQuantityType = HKQuantityType.GetQuantityType (heartRateId);

				//Beats per minute = "Count/Minute" as a unit
				var heartRateUnitType = HKUnit.Count.UnitDividedBy (HKUnit.Minute);
				var quantity = HKQuantity.FromQuantity (heartRateUnitType, result);
				//If we know where the sensor is...
				var metadata = new HKMetadata ();
				metadata.HeartRateSensorLocation = HKHeartRateSensorLocation.Chest;
				//Create the sample
				var heartRateSample = HKQuantitySample.FromType (heartRateQuantityType, quantity, new NSDate (), new NSDate (), metadata);

				//Attempt to store it...
				healthKitStore.SaveObject (heartRateSample, (success, error) => {
					//Error will be non-null if permissions not granted
					Console.WriteLine ("Write succeeded: " + success);
					if (error != null) {
						Console.WriteLine (error);
					}
				});
			}
		}
		#endregion

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

		void ConnectToSelectedDevice (object sender, EventArgs e)
		{
			if (heartRateMonitors.Count > 0) {
				monitor = heartRateMonitors [0];
				Console.WriteLine ("monitor:" + monitor);
				if (monitor != null) {
					statusLabel.Text = "Connecting to " + monitor.Name + " ... ";
					monitor.Connect ();
				}
			} else {
				Console.WriteLine ("No heart rate monitors detected");
			}
		}

		void InitializeCoreBluetooth ()
		{
			manager.UpdatedState += OnCentralManagerUpdatedState;

			//HACK: Modified this to just quit after finding the first heart rate monitor
			EventHandler<CBDiscoveredPeripheralEventArgs> discovered = null;
			discovered += (sender, e) => {
				Console.WriteLine ("discovered!");
				if (monitor != null) {
					monitor.Dispose ();
				}

				monitor = new HeartRateMonitor (manager, e.Peripheral);
				monitor.HeartRateUpdated += OnHeartRateUpdated;
				monitor.HeartBeat += OnHeartBeat;

				heartRateMonitors.Add (monitor);
				connectButton.Enabled = true;

				//HACK: instead of adding to a list, just use this one
				statusLabel.Text = "Found " + monitor.Name + ".";
				manager.DiscoveredPeripheral -= discovered;
			};
			manager.DiscoveredPeripheral += discovered;

			manager.ConnectedPeripheral += (sender, e) => e.Peripheral.DiscoverServices ();

			manager.DisconnectedPeripheral += (sender, e) => DisconnectMonitor ();


		}

		void OnCentralManagerUpdatedState (object sender, EventArgs e)
		{
			string message = null;

			switch (manager.State) {
			case CBCentralManagerState.PoweredOn:
				HeartRateMonitor.ScanForHeartRateMonitors (manager);
				//connectButton.Enabled = true;
				message = "Scanning...";
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
				message = "Unhandled state: " + manager.State;
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