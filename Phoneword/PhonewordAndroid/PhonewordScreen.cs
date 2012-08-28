using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword
{
    [Activity(Label = "Phoneword", MainLauncher = true)]
    public class PhonewordScreen : Activity
    {
		string translatedNumber;
		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
			Button CallButton = FindViewById<Button>(Resource.Id.CallButton);
            EditText PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);

			TranslateButton.Click += delegate
            {
				translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text);
				if (translatedNumber == "") {
					CallButton.Text = "Call";
					CallButton.Enabled = false;
				} else {
					CallButton.Text = "Call " + translatedNumber;
					CallButton.Enabled = true;
					// new AlertDialog.Builder(this).SetMessage("No number entered").SetNeutralButton("Ok", delegate { });
				}
            };

			CallButton.Click += delegate {
				var callIntent = new Intent(Intent.ActionCall);
				callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
				StartActivity(callIntent);
			};
        }
    }
}