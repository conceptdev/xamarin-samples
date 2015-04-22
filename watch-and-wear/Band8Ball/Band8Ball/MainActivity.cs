using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.Band.Notifications;
using Microsoft.Band;
using Android;
using Android.Graphics;
using Microsoft.Band.Tiles;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Band.Sensors;
using System.Linq;

[assembly: UsesPermission(Manifest.Permission.Bluetooth)]
[assembly: UsesPermission(Microsoft.Band.BandClientManager.BindBandService)]

namespace Band8Ball
{
	[Activity (Label = "Band8Ball", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		IBandDeviceInfo[] mPairedBands;
		int mSelectedBandIndex = 0;
		Button connectButton;
		Button chooseBandButton;
		Button vibrateButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			connectButton = FindViewById<Button> (Resource.Id.connectButton);
			connectButton.Click += OnConnectClick;

			chooseBandButton =  FindViewById<Button>(Resource.Id.chooseBandButton);
			chooseBandButton.Click += OnChooseBandClick;

			vibrateButton =  FindViewById<Button>(Resource.Id.vibrateButton);
			vibrateButton.Click += OnVibrateClick;

			// ---

			accelX = FindViewById<TextView>(Resource.Id.accelX);
			accelY = FindViewById<TextView>(Resource.Id.accelY);
			accelZ = FindViewById<TextView>(Resource.Id.accelZ);
			resultText = FindViewById<TextView>(Resource.Id.resultText);

		}



		// Handle connect/disconnect requests.
		private async void OnConnectClick(object sender, EventArgs args)
		{
			if (Model.Instance.Connected)
			{	// already connected
				try
				{
					await TurnOffSensor ();
					await Model.Instance.Client.DisconnectTaskAsync();
					RefreshControls();
					vibrateButton.Enabled = false;
				}
				catch (Exception ex)
				{
					Util.ShowExceptionAlert(this, "Disconnect", ex);
				}
			}
			else
			{	// connect!
				
				// Always recreate our BandClient since the selection might
				// have changed. This is safe since we aren't connected.
				IBandClient client = BandClientManager.Instance.Create(this, mPairedBands[mSelectedBandIndex]);
				Model.Instance.Client = client;

				connectButton.Enabled = false;

				// Connect must be called on a background thread.
				ConnectionResult result;
				try
				{
					result = await Model.Instance.Client.ConnectTaskAsync();
				}
				catch (Java.Lang.InterruptedException)
				{
					result = ConnectionResult.Timeout;
				}
				catch (BandException)
				{
					result = ConnectionResult.InternalError;
				}

				// callback that must be handled on the UI thread
				if (result != ConnectionResult.Ok) {
					Util.ShowExceptionAlert (this, "Connect", new Exception ("Connection failed: result=" + result));
					vibrateButton.Enabled = false;

				} else {
					EnsureSensorsCreated();
					vibrateButton.Enabled = true;
					await TurnOnSensor ();

					var tiles = (await Model.Instance.Client.TileManager.GetTilesTaskAsync()).ToList();
					// the the number of tiles we can add
					var capacity = await Model.Instance.Client.TileManager.GetRemainingTileCapacityTaskAsync ();
					if (tiles.Count == 0 && capacity > 0) {
						await AddTileAsync ();
					} else {
						uuid = tiles [0].TileId;
					}
				}
				RefreshControls();
			}
		}

		protected override void OnResume()
		{
			base.OnResume();

			mPairedBands = BandClientManager.Instance.GetPairedBands();

			// If one or more bands were removed, making our band selection invalid,
			// reset the selection to the first in the list.
			if (mSelectedBandIndex >= mPairedBands.Length)
			{
				mSelectedBandIndex = 0;
			}

			RefreshControls();
		}

		// If there are multiple bands, the "choose band" button is enabled and
		// launches a dialog where we can select the band to use.
		private void OnChooseBandClick(object sender, EventArgs e)
		{
			using (var builder = new AlertDialog.Builder(this))
			{
				string[] names = new string[mPairedBands.Length];
				for (int i = 0; i < names.Length; i++)
				{
					names[i] = mPairedBands[i].Name;
				}

				builder.SetItems(names, (dialog, args) =>
					{
						mSelectedBandIndex = args.Which;
						((Dialog) dialog).Dismiss();
						RefreshControls();
					});

				builder.SetTitle("Select band:");
				builder.Show();
			}
		}

		private async void OnVibrateClick(object sender, EventArgs e)
		{
			try
			{
				await Model.Instance.Client.NotificationManager.VibrateTaskAsync(VibrationType.NotificationAlarm);
			}
			catch (Exception ex)
			{
				Util.ShowExceptionAlert(this, "Vibrate band", ex);
			}
		}

		private void RefreshControls()
		{
			switch (mPairedBands.Length)
			{
			case 0:
				chooseBandButton.Text = "No paired bands";
				chooseBandButton.Enabled = false;
				connectButton.Enabled = false;
				break;

			case 1:
				chooseBandButton.Text = mPairedBands[mSelectedBandIndex].Name;
				chooseBandButton.Enabled = false;
				connectButton.Enabled = true;
				break;

			default:
				chooseBandButton.Text = mPairedBands[mSelectedBandIndex].Name;
				chooseBandButton.Enabled = true;
				connectButton.Enabled = true;
				break;
			}

			bool connected = Model.Instance.Connected;

			if (connected)
			{
				connectButton.SetText(Resource.String.disconnect_label);

				// must disconnect before changing the band selection
				chooseBandButton.Enabled = false;
			}
			else
			{
				connectButton.SetText(Resource.String.connect_label);
			}
		}


		protected override void OnDestroy()
		{
			try
			{
				if (Model.Instance.Connected)
				{
					Model.Instance.Client.DisconnectTaskAsync();
				}
			}
			catch (Exception ex)
			{
				// ignore failures here
				Console.WriteLine("Error disconnecting: " + ex);
			}
			finally
			{
				Model.Instance.Client = null;
			}

			base.OnPause();
		}

		//-------------

		private AccelerometerSensor accelerometerSensor;
		TextView accelX, accelY, accelZ;
		int xCount=0, yCount=0, zCount=0;

		TextView resultText;
		DateTime lastAnswered = DateTime.Now.Subtract(TimeSpan.FromSeconds(20));
		string lastAnswer = "";

		private void EnsureSensorsCreated()
		{
			IBandSensorManager sensorMgr = Model.Instance.Client.SensorManager;

			if (accelerometerSensor == null)
			{
				accelerometerSensor = sensorMgr.CreateAccelerometerSensor();
				accelerometerSensor.ReadingChanged += async (sender, e) =>
				{
					var accelerometerEvent = e.SensorReading;

					//if(Math.Abs(accelerometerEvent.AccelerationX) > 3) xCount++;
					if(Math.Abs(accelerometerEvent.AccelerationY) > 3) yCount++;
					if(Math.Abs(accelerometerEvent.AccelerationZ) > 3) zCount++;

					// rudimentary shake detection
					//if (xCount > 2
					if (yCount > 2
						&& zCount > 2) {

						// wait 20 seconds below allowing a new answer
						if (DateTime.Now.Subtract(lastAnswered).TotalSeconds > 20) {
							xCount = yCount = zCount = 0;
							lastAnswered = DateTime.Now;
							lastAnswer = Get8BallPrediction();

							await Model.Instance.Client.NotificationManager.SendMessageTaskAsync(
								uuid, 
								"8Ball says...", 
								lastAnswer, 
								DateTime.Now,
								MessageFlags.ShowDialog);
						}
					}
					this.RunOnUiThread(() =>
						{
//							accelX.Text = string.Format("{0:F3}", accelerometerEvent.AccelerationX);
//							accelY.Text = string.Format("{0:F3}", accelerometerEvent.AccelerationY);
//							accelZ.Text = string.Format("{0:F3}", accelerometerEvent.AccelerationZ);

							accelZ.Text = xCount + " " + yCount + " " + zCount + " (x,y,x) only (y,z) count";

							resultText.Text = lastAnswer;
						});
				};
			}
		}

		private async Task TurnOnSensor () {

			if (!Model.Instance.Connected)
			{
				return;
			}

			EnsureSensorsCreated();

			// Turn on the appropriate sensor
			try
			{
				SampleRate rate = SampleRate.Ms128;
				accelX.Text = "";
				accelY.Text = "";
				accelZ.Text = "";
				await accelerometerSensor.StartReadingsTaskAsync(rate);
			} catch (BandException ex)
			{
				Util.ShowExceptionAlert(this, "Register sensor listener", ex);
			}
		}

		private async Task TurnOffSensor() {
			try
			{
				await accelerometerSensor.StopReadingsTaskAsync();
			}
			catch (BandException ex)
			{
				Util.ShowExceptionAlert(this, "Unregister sensor listener", ex);
			}
		}

		//-------------

		private int mRemainingCapacity;
		private ICollection<BandTile> mTiles;
		Java.Util.UUID uuid;

		async Task AddTileAsync ()
		{
			string tileName = "8Ball";
			try
			{
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final android.graphics.BitmapFactory.Options options = new android.graphics.BitmapFactory.Options();
				BitmapFactory.Options options = new BitmapFactory.Options();
				options.InScaled = false;
				BandIcon tileIcon = BandIcon.ToBandIcon(BitmapFactory.DecodeResource(Resources, Resource.Raw.tile, options));

				//BandIcon badgeIcon = mCheckboxBadging.Checked ? BandIcon.ToBandIcon(BitmapFactory.DecodeResource(Resources, Resource.Raw.badge, options)) : null;
				BandIcon badgeIcon = BandIcon.ToBandIcon(BitmapFactory.DecodeResource(Resources, Resource.Raw.badge, options));

				uuid = Java.Util.UUID.RandomUUID();

				BandTile tile = 
					new BandTile.Builder(uuid, tileName, tileIcon)
						.SetTileSmallIcon(badgeIcon)
					//	.SetTheme(mCheckboxCustomTheme.Checked ? mThemeView.Theme : null)
						.Build();

				try
				{
					var result = await Model.Instance.Client.TileManager.AddTileTaskAsync(this, tile);
					if (result)
					{
						Toast.MakeText(this, "Tile added", ToastLength.Short).Show();
					}
					else
					{
						Toast.MakeText(this, "Unable to add tile", ToastLength.Short).Show();
					}
				}
				catch (Exception ex)
				{
					Util.ShowExceptionAlert(this, "Add tile", ex);
				}

				// Refresh our tile list and count
				await RefreshData();
				RefreshControls();
			}
			catch (Exception e)
			{
				Util.ShowExceptionAlert(this, "Add tile", e);
			}
		}

		List<BandTile> tiles;
		BandTile mSelectedTile;

		private async Task RefreshData()
		{
			if (Model.Instance.Connected)
			{
				try
				{
					var capacity = await Model.Instance.Client.TileManager.GetRemainingTileCapacityTaskAsync();
					mRemainingCapacity = capacity;
				}
				catch (Exception e)
				{
					mRemainingCapacity = -1;
					Util.ShowExceptionAlert(this, "Check capacity", e);
				}

				try
				{
					tiles = (await Model.Instance.Client.TileManager.GetTilesTaskAsync()).ToList();
//					mTileListAdapter.TileList = tiles.ToList();
				}
				catch (Exception e)
				{
					mTiles = null;
					mSelectedTile = null;
					Util.ShowExceptionAlert(this, "Get tiles", e);
				}
			}
			else
			{
				mRemainingCapacity = -1;
				mTiles = null;
			}
		}

		//-------

		List<string> answers = new List<string> {
			  "It is certain"
			, "It is decidedly so"
			, "Without a doubt"
			, "Yes definitely"
			, "You may rely on it"
			, "As I see it, yes"
			, "Most likely"
			, "Outlook good"
			, "Yes"
			, "Signs point to yes"

			, "Reply hazy try again"
			, "Ask again later"
			, "Better not tell you now"
			, "Cannot predict now"
			, "Concentrate and ask again"

			, "Don't count on it"
			, "My reply is no"
			, "My sources say no"
			, "Outlook not so good"
			, "Very doubtful"
		};

		Random randomAnswerSelector = new Random ();

		string Get8BallPrediction() {
			return answers[randomAnswerSelector.Next (answers.Count)];
		}
	}
}


