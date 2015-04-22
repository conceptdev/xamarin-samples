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
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Microsoft.Band.Sample.ColorPicker
{
    /// <summary>
    /// Created by Bunk3r on 10/25/2014.
    /// </summary>
    public class ColorPickerDialog : Dialog
    {
        private HueBarSlider mHuePicker;
        private ColorAreaPicker mColorAreaPicker;

        private View mCurrentColorPreview;
        private View mSelectedColorPreview;

        private Color mInitialColor;
        private Color mSelectedColor;

        public ColorPickerDialog(Context context) 
            : base(context)
        {
            RequestWindowFeature((int)WindowFeatures.NoTitle);
        }

        public virtual void SetInitialColor(Color value)
        {
            mInitialColor = value;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView (Resource.Layout.dialog_color_picker);

            mHuePicker =  FindViewById<HueBarSlider>(Resource.Id.hue_slider);
            mColorAreaPicker =  FindViewById<ColorAreaPicker>(Resource.Id.color_area_picker);
            Button okButton =  FindViewById<Button>(Resource.Id.ok_button);
            Button cancelButton =  FindViewById<Button>(Resource.Id.cancel_button);
            mCurrentColorPreview = FindViewById(Resource.Id.current_color);
            mSelectedColorPreview = FindViewById(Resource.Id.selected_color);

            mCurrentColorPreview.SetBackgroundColor(mInitialColor);
            mSelectedColorPreview.SetBackgroundColor(mInitialColor);

            mColorAreaPicker.ColorChanged += (sender, args) =>
            {
                mSelectedColor = args.Color;
                mSelectedColorPreview.SetBackgroundColor(mSelectedColor);
            };
            mColorAreaPicker.SetHuePicker(mHuePicker);
            mColorAreaPicker.SetColor(mInitialColor);

            okButton.Click += delegate
            {
                OnColorSelected(new ColorChangedEventArgs(mSelectedColor));
                Dismiss();
            };

            cancelButton.Click += delegate
            {
                Cancel();
            };
        }

        public event EventHandler<ColorChangedEventArgs> ColorSelected;

        protected virtual void OnColorSelected(ColorChangedEventArgs eventArgs)
        {
            var handler = ColorSelected;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }
    }
}
