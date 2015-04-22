//Copyright (c) Microsoft Corporation All rights reserved.  
// 
//MIT License: 
// 
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//documentation files (the  "Software"), to deal in the Software without restriction, including without limitation
//the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
//to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
// 
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of
//the Software. 
// 
//THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Band.Sensors;
using Fragment = Android.Support.V4.App.Fragment;

namespace Microsoft.Band.Sample
{
    public class SensorsFragment : Fragment, FragmentListener
    {
        // Accelerometer controls
        private Switch mSwitchAccelerometer;
        private TableLayout mTableAccelerometer;
        private RadioGroup mRadioGroupAccelerometer;
        private TextView mTextAccX;
        private TextView mTextAccY;
        private TextView mTextAccZ;
        private RadioButton mRadioAcc16;
        private RadioButton mRadioAcc32;

        // Gyroscope controls
        private Switch mSwitchGyro;
        private TableLayout mTableGyro;
        private RadioGroup mRadioGroupGyro;
        private TextView mTextGyroAccX;
        private TextView mTextGyroAccY;
        private TextView mTextGyroAccZ;
        private TextView mTextGyroAngX;
        private TextView mTextGyroAngY;
        private TextView mTextGyroAngZ;
        private RadioButton mRadioGyro16;
        private RadioButton mRadioGyro32;

        // Distance sensor controls
        private Switch mSwitchDistance;
        private TableLayout mTableDistance;
        private TextView mTextTotalDistance;
        private TextView mTextSpeed;
        private TextView mTextPace;
        private TextView mTextPedometerMode;

        // HR sensor controls
        private Switch mSwitchHeartRate;
        private TableLayout mTableHeartRate;
        private TextView mTextHeartRate;
        private TextView mTextHeartRateQuality;

        // Contact sensor controls
        private Switch mSwitchContact;
        private TableLayout mTableContact;
        private TextView mTextContact;

        // Skin temperature sensor controls
        private Switch mSwitchSkinTemperature;
        private TableLayout mTableSkinTemperature;
        private TextView mTextSkinTemperature;

        // UV sensor controls
        private Switch mSwitchUltraviolet;
        private TableLayout mTableUltraviolet;
        private TextView mTextUltraviolet;

        // Pedometer sensor controls
        private Switch mSwitchPedometer;
        private TableLayout mTablePedometer;
        private TextView mTextTotalSteps;

        // Each sensor switch has an associated TableLayout containing it's display controls.
        // The TableLayout remains hidden until the corresponding sensor switch is turned on.
        private Dictionary<Switch, TableLayout> mSensorMap;

        // the sensors
        private AccelerometerSensor accelerometerSensor;
        private ContactSensor contactSensor;
        private DistanceSensor distanceSensor;
        private GyroscopeSensor gyroscopeSensor;
        private HeartRateSensor heartRateSensor;
        private PedometerSensor pedometerSensor;
        private SkinTemperatureSensor skinTemperatureSensor;
        private UVSensor uvSensor;

        public virtual void OnFragmentSelected()
        {
            if (IsVisible)
            {
                RefreshControls();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_sensors, container, false);

            mSensorMap = new Dictionary<Switch, TableLayout>();

            //
            // Accelerometer setup
            //
            mSwitchAccelerometer = rootView.FindViewById<Switch>(Resource.Id.switchAccelerometer);
            mTableAccelerometer = rootView.FindViewById<TableLayout>(Resource.Id.tableAccelerometer);
            mRadioGroupAccelerometer = rootView.FindViewById<RadioGroup>(Resource.Id.rgAccelerometer);
            mSensorMap[mSwitchAccelerometer] = mTableAccelerometer;
            mTableAccelerometer.Visibility = ViewStates.Gone;
            mSwitchAccelerometer.CheckedChange += OnToggleSensorSection;

            mTextAccX = rootView.FindViewById<TextView>(Resource.Id.textAccX);
            mTextAccY = rootView.FindViewById<TextView>(Resource.Id.textAccY);
            mTextAccZ = rootView.FindViewById<TextView>(Resource.Id.textAccZ);
            mRadioAcc16 = rootView.FindViewById<RadioButton>(Resource.Id.rbAccelerometerRate16ms);
            mRadioAcc32 = rootView.FindViewById<RadioButton>(Resource.Id.rbAccelerometerRate32ms);

            //
            // Gyro setup
            //
            mSwitchGyro = rootView.FindViewById<Switch>(Resource.Id.switchGyro);
            mTableGyro = rootView.FindViewById<TableLayout>(Resource.Id.tableGyro);
            mRadioGroupGyro = rootView.FindViewById<RadioGroup>(Resource.Id.rgGyro);
            mSensorMap[mSwitchGyro] = mTableGyro;
            mTableGyro.Visibility = ViewStates.Gone;
            mSwitchGyro.CheckedChange += OnToggleSensorSection;

            mTextGyroAccX = rootView.FindViewById<TextView>(Resource.Id.textGyroAccX);
            mTextGyroAccY = rootView.FindViewById<TextView>(Resource.Id.textGyroAccY);
            mTextGyroAccZ = rootView.FindViewById<TextView>(Resource.Id.textGyroAccZ);
            mTextGyroAngX = rootView.FindViewById<TextView>(Resource.Id.textAngX);
            mTextGyroAngY = rootView.FindViewById<TextView>(Resource.Id.textAngY);
            mTextGyroAngZ = rootView.FindViewById<TextView>(Resource.Id.textAngZ);
            mRadioGyro16 =  rootView.FindViewById<RadioButton>(Resource.Id.rbGyroRate16ms);
            mRadioGyro32 = rootView.FindViewById<RadioButton>(Resource.Id.rbGyroRate32ms);

            //
            // Distance setup
            //
            mSwitchDistance = rootView.FindViewById<Switch>(Resource.Id.switchDistance);
            mTableDistance = rootView.FindViewById<TableLayout>(Resource.Id.tableDistance);
            mSensorMap[mSwitchDistance] = mTableDistance;
            mTableDistance.Visibility = ViewStates.Gone;
            mSwitchDistance.CheckedChange += OnToggleSensorSection;

            mTextTotalDistance = rootView.FindViewById<TextView>(Resource.Id.textTotalDistance);
            mTextSpeed = rootView.FindViewById<TextView>(Resource.Id.textSpeed);
            mTextPace = rootView.FindViewById<TextView>(Resource.Id.textPace);
            mTextPedometerMode = rootView.FindViewById<TextView>(Resource.Id.textPedometerMode);

            //
            // Heart rate setup
            //
            mSwitchHeartRate = rootView.FindViewById<Switch>(Resource.Id.switchHeartRate);
            mTableHeartRate = rootView.FindViewById<TableLayout>(Resource.Id.tableHeartRate);
            mSensorMap[mSwitchHeartRate] = mTableHeartRate;
            mTableHeartRate.Visibility = ViewStates.Gone;
            mSwitchHeartRate.CheckedChange += OnToggleSensorSection;

            mTextHeartRate = rootView.FindViewById<TextView>(Resource.Id.textHeartRate);
            mTextHeartRateQuality = rootView.FindViewById<TextView>(Resource.Id.textHeartRateQuality);

            //
            // Contact setup
            //
            mSwitchContact = rootView.FindViewById<Switch>(Resource.Id.switchContact);
            mTableContact = rootView.FindViewById<TableLayout>(Resource.Id.tableContact);
            mSensorMap[mSwitchContact] = mTableContact;
            mTableContact.Visibility = ViewStates.Gone;
            mSwitchContact.CheckedChange += OnToggleSensorSection;

            mTextContact = rootView.FindViewById<TextView>(Resource.Id.textContact);

            //
            // Skin temperature setup
            //
            mSwitchSkinTemperature = rootView.FindViewById<Switch>(Resource.Id.switchSkinTemperature);
            mTableSkinTemperature = rootView.FindViewById<TableLayout>(Resource.Id.tableSkinTemperature);
            mSensorMap[mSwitchSkinTemperature] = mTableSkinTemperature;
            mTableSkinTemperature.Visibility = ViewStates.Gone;
            mSwitchSkinTemperature.CheckedChange += OnToggleSensorSection;

            mTextSkinTemperature = rootView.FindViewById<TextView>(Resource.Id.textSkinTemperature);

            //
            // Ultraviolet setup
            //
            mSwitchUltraviolet = rootView.FindViewById<Switch>(Resource.Id.switchUltraviolet);
            mTableUltraviolet = rootView.FindViewById<TableLayout>(Resource.Id.tableUltraviolet);
            mSensorMap[mSwitchUltraviolet] = mTableUltraviolet;
            mTableUltraviolet.Visibility = ViewStates.Gone;
            mSwitchUltraviolet.CheckedChange += OnToggleSensorSection;

            mTextUltraviolet = rootView.FindViewById<TextView>(Resource.Id.textUltraviolet);

            //
            // Pedometer setup
            //
            mSwitchPedometer = rootView.FindViewById<Switch>(Resource.Id.switchPedometer);
            mTablePedometer = rootView.FindViewById<TableLayout>(Resource.Id.tablePedometer);
            mSensorMap[mSwitchPedometer] = mTablePedometer;
            mTablePedometer.Visibility = ViewStates.Gone;
            mSwitchPedometer.CheckedChange += OnToggleSensorSection;

            mTextTotalSteps = rootView.FindViewById<TextView>(Resource.Id.textTotalSteps);

            return rootView;
        }


        //
        // Sensor event handlers
        //
        private void EnsureSensorsCreated()
        {
            IBandSensorManager sensorMgr = Model.Instance.Client.SensorManager;

            if (accelerometerSensor == null)
            {
                accelerometerSensor = sensorMgr.CreateAccelerometerSensor();
                accelerometerSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var accelerometerEvent = e.SensorReading;
                        mTextAccX.Text = string.Format("{0:F3}", accelerometerEvent.AccelerationX);
                        mTextAccY.Text = string.Format("{0:F3}", accelerometerEvent.AccelerationY);
                        mTextAccZ.Text = string.Format("{0:F3}", accelerometerEvent.AccelerationZ);
                    });
                };
            }

            if (contactSensor == null)
            {
                contactSensor = sensorMgr.CreateContactSensor();
                contactSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var contactEvent = e.SensorReading;
                        mTextContact.Text = contactEvent.ContactStatus.ToString();
                    });
                };
            }

            if (distanceSensor == null)
            {
                distanceSensor = sensorMgr.CreateDistanceSensor();
                distanceSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var distanceEvent = e.SensorReading;
                        mTextTotalDistance.Text = string.Format("{0:D} cm", distanceEvent.TotalDistance);
                        mTextSpeed.Text = string.Format("{0:F2} cm/s", distanceEvent.Speed);
                        mTextPace.Text = string.Format("{0:F2} ms/m", distanceEvent.Pace);
                        mTextPedometerMode.Text = distanceEvent.PedometerMode.ToString();
                    });
                };
            }

            if (gyroscopeSensor == null)
            {
                gyroscopeSensor = sensorMgr.CreateGyroscopeSensor();
                gyroscopeSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var gyroscopeEvent = e.SensorReading;
                        mTextGyroAccX.Text = string.Format("{0:F3}", gyroscopeEvent.AccelerationX);
                        mTextGyroAccY.Text = string.Format("{0:F3}", gyroscopeEvent.AccelerationY);
                        mTextGyroAccZ.Text = string.Format("{0:F3}", gyroscopeEvent.AccelerationZ);
                        mTextGyroAngX.Text = string.Format("{0:F2}", gyroscopeEvent.AngularVelocityX);
                        mTextGyroAngY.Text = string.Format("{0:F2}", gyroscopeEvent.AngularVelocityY);
                        mTextGyroAngZ.Text = string.Format("{0:F2}", gyroscopeEvent.AngularVelocityZ);
                    });
                };
            }

            if (heartRateSensor == null)
            {
                heartRateSensor = sensorMgr.CreateHeartRateSensor();
                heartRateSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var heartRateEvent = e.SensorReading;
                        mTextHeartRate.Text = Convert.ToString(heartRateEvent.HeartRate);
                        mTextHeartRateQuality.Text = heartRateEvent.Quality.ToString();
                    });
                };
            }

            if (pedometerSensor == null)
            {
                pedometerSensor = sensorMgr.CreatePedometerSensor();
                pedometerSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var pedometerEvent = e.SensorReading;
                        mTextTotalSteps.Text = string.Format("{0:D}", pedometerEvent.TotalSteps);
                    });
                };
            }

            if (skinTemperatureSensor == null)
            {
                skinTemperatureSensor = sensorMgr.CreateSkinTemperatureSensor();
                skinTemperatureSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var skinTemperatureEvent = e.SensorReading;
                        mTextSkinTemperature.Text = string.Format("{0:F1}", skinTemperatureEvent.Temperature);
                    });
                };
            }

            if (uvSensor == null)
            {
                uvSensor = sensorMgr.CreateUVSensor();
                uvSensor.ReadingChanged += (sender, e) =>
                {
                    Activity.RunOnUiThread(() =>
                    {
                        var uvEvent = e.SensorReading;
                        mTextUltraviolet.Text = uvEvent.UVIndexLevel.ToString();
                    });
                };
            }
        }


        //
        // When pausing, turn off any active sensors.
        //
        public override void OnPause()
        {
            foreach (Switch sw in mSensorMap.Keys)
            {
                if (sw.Checked)
                {
                    sw.Checked = false;
                    OnToggleSensorSection(sw, new CompoundButton.CheckedChangeEventArgs(false));
                }
            }

            base.OnPause();
        }

        private async void OnToggleSensorSection(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (!Model.Instance.Connected)
            {
                return;
            }

            EnsureSensorsCreated();

            Switch sw = (Switch)sender;
            TableLayout table = mSensorMap[sw];

            if (e.IsChecked)
            {
                table.Visibility = ViewStates.Visible;

                if (table == mTableAccelerometer)
                {
                    mRadioGroupAccelerometer.Enabled = false;
                    SetChildrenEnabled(mRadioGroupAccelerometer, false);
                }
                else if (table == mTableGyro)
                {
                    mRadioGroupGyro.Enabled = false;
                    SetChildrenEnabled(mRadioGroupGyro, false);
                }

                // Turn on the appropriate sensor

                try
                {
                    if (sw == mSwitchAccelerometer)
                    {
                        SampleRate rate;
                        if (mRadioAcc16.Checked)
                        {
                            rate = SampleRate.Ms16;
                        }
                        else if (mRadioAcc32.Checked)
                        {
                            rate = SampleRate.Ms32;
                        }
                        else
                        {
                            rate = SampleRate.Ms128;
                        }

                        mTextAccX.Text = "";
                        mTextAccY.Text = "";
                        mTextAccZ.Text = "";
                        await accelerometerSensor.StartReadingsTaskAsync(rate);
                    }
                    else if (sw == mSwitchGyro)
                    {
                        SampleRate rate;
                        if (mRadioGyro16.Checked)
                        {
                            rate = SampleRate.Ms16;
                        }
                        else if (mRadioGyro32.Checked)
                        {
                            rate = SampleRate.Ms32;
                        }
                        else
                        {
                            rate = SampleRate.Ms128;
                        }

                        mTextGyroAccX.Text = "";
                        mTextGyroAccY.Text = "";
                        mTextGyroAccZ.Text = "";
                        mTextGyroAngX.Text = "";
                        mTextGyroAngY.Text = "";
                        mTextGyroAngZ.Text = "";
						await gyroscopeSensor.StartReadingsTaskAsync(rate);
                    }
                    else if (sw == mSwitchDistance)
                    {
                        mTextTotalDistance.Text = "";
                        mTextSpeed.Text = "";
                        mTextPace.Text = "";
                        mTextPedometerMode.Text = "";
						await distanceSensor.StartReadingsTaskAsync();
                    }
                    else if (sw == mSwitchHeartRate)
                    {
                        mTextHeartRate.Text = "";
                        mTextHeartRateQuality.Text = "";
						await heartRateSensor.StartReadingsTaskAsync();
                    }
                    else if (sw == mSwitchContact)
                    {
                        mTextContact.Text = "";
						await contactSensor.StartReadingsTaskAsync();
                    }
                    else if (sw == mSwitchSkinTemperature)
                    {
                        mTextSkinTemperature.Text = "";
						await skinTemperatureSensor.StartReadingsTaskAsync();
                    }
                    else if (sw == mSwitchUltraviolet)
                    {
                        mTextUltraviolet.Text = "";
						await uvSensor.StartReadingsTaskAsync();
                    }
                    else if (sw == mSwitchPedometer)
                    {
                        mTextTotalSteps.Text = "";
						await pedometerSensor.StartReadingsTaskAsync();
                    }
                }
                catch (BandException ex)
                {
                    Util.ShowExceptionAlert(Activity, "Register sensor listener", ex);
                }
            }
            else
            {
                table.Visibility = ViewStates.Gone;

                if (table == mTableAccelerometer)
                {
                    mRadioGroupAccelerometer.Enabled = true;
                    SetChildrenEnabled(mRadioGroupAccelerometer, true);
                }
                else if (table == mTableGyro)
                {
                    mRadioGroupGyro.Enabled = true;
                    SetChildrenEnabled(mRadioGroupGyro, true);
                }

                // Turn off the appropriate sensor

                try
                {
                    if (sw == mSwitchAccelerometer)
                    {
                        await accelerometerSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchGyro)
                    {
						await gyroscopeSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchDistance)
                    {
						await distanceSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchHeartRate)
                    {
						await heartRateSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchContact)
                    {
						await contactSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchSkinTemperature)
                    {
						await skinTemperatureSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchUltraviolet)
                    {
						await uvSensor.StopReadingsTaskAsync();
                    }
                    else if (sw == mSwitchPedometer)
                    {
						await pedometerSensor.StopReadingsTaskAsync();
                    }
                }
                catch (BandException ex)
                {
                    Util.ShowExceptionAlert(Activity, "Unregister sensor listener", ex);
                }
            }
        }

        //
        // Other helpers
        //

        private static void SetChildrenEnabled(RadioGroup radioGroup, bool enabled)
        {
            for (int i = radioGroup.ChildCount - 1; i >= 0; i--)
            {
                radioGroup.GetChildAt(i).Enabled = enabled;
            }
        }

        private void RefreshControls()
        {
            bool connected = Model.Instance.Connected;

            foreach (Switch sw in mSensorMap.Keys)
            {
                sw.Enabled = connected;
                if (!connected)
                {
                    sw.Checked = false;
                    OnToggleSensorSection(sw, new CompoundButton.CheckedChangeEventArgs(false));
                }
            }
        }
    }

}