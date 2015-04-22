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
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Microsoft.Band.Sample.ColorPicker
{
    /// <summary>
    /// Created by Bunk3r on 10/25/2014.
    /// </summary>
    public class HueBarSlider : View
    {
        /// <summary>
        /// Default values for the control
        /// </summary>
        private const int NUMBER_OF_HUES = 360;
        private const int NUMBER_OF_HUE_SETS = 6;
        private const int DEFAULT_SELECTED_HUE_WIDTH = 3;
        private const int DEFAULT_WIDTH = 256;
        private const int DEFAULT_HEIGHT = 30;

        // Used for when the hue is set before the first layout
        private bool mIsHuePending = false;

        // Holds the width of the Slider's bitmap
        private int mSliderWidth;

        // True if inflated trough XML, false if created programmatically
        private bool mWasInflated;

        // The width of the line that shows what color is currently selected
        private int mSelectedWidth;

        // The ratio between the number of hues and the size of the view
        private float mDensityMultiplier;

        // Paint object used throughout the view
        private Paint mPaint;

        // The Bitmap that holds the original 360 different hues
        private Bitmap mHuesBitmap;

        // The Bitmap that caches the scaled version of the hues
        private Bitmap mHueBarBitmap;

        private float mCurrentHue;

        public HueBarSlider(Context context) 
            : base(context)
        {

            if (context != null)
            {
                mDensityMultiplier = (int) Math.Ceiling(context.Resources.DisplayMetrics.Density);
            }

            Init(false);
        }

        public HueBarSlider(Context context, IAttributeSet attrs) 
            : this(context, attrs, 0)
        {
        }

        public HueBarSlider(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Init(true);
        }

        /// <summary>
        /// Sets all the initial configuration and pre-rendering for the view
        /// </summary>
        /// <param name="wasInflated"> if it was or not inflated via XML </param>
        private void Init(bool wasInflated)
        {
            mWasInflated = wasInflated;
            mSelectedWidth = DEFAULT_SELECTED_HUE_WIDTH;
            mPaint = new Paint();
            PreRenderHueBar();
        }

        /// <summary>
        /// Sets the width of the line that shows the current selected hue
        /// </summary>
        /// <param name="width"> a positive value greater than 0 (1, 2, ....) </param>
        public virtual void SetCurrentHueWidth(int value)
        {
                mSelectedWidth = value > 0 ? value : 1;
        }

        /// <summary>
        /// Calculates the different hues and caches them in a Bitmap
        /// </summary>
        private void PreRenderHueBar()
        {
            int index = 0;
            int[] hueBarColors = new int[NUMBER_OF_HUES];
            float hueIncrement = 255f / (NUMBER_OF_HUES / NUMBER_OF_HUE_SETS);

            for (float i = hueIncrement; i < 256; i += hueIncrement) // red (#f00) - yellow (#ff0)
            {
                hueBarColors[index] = Color.Rgb(255, (int) i, 0);
                index++;
            }

            for (float i = hueIncrement; i < 256; i += hueIncrement) // yellow (#ff0) - green (#0f0)
            {
                hueBarColors[index] = Color.Rgb(255 - (int) i, 255, 0);
                index++;
            }

            for (float i = hueIncrement; i < 256; i += hueIncrement) // green (#0f0) - cyan (#0ff)
            {
                hueBarColors[index] = Color.Rgb(0, 255, (int) i);
                index++;
            }

            for (float i = hueIncrement; i < 256; i += hueIncrement) // cyan (#0ff) - blue (#00f)
            {
                hueBarColors[index] = Color.Rgb(0, 255 - (int) i, 255);
                index++;
            }

            for (float i = hueIncrement; i < 256; i += hueIncrement) // blue (#00f) - Pink (#f0f)
            {
                hueBarColors[index] = Color.Rgb((int) i, 0, 255);
                index++;
            }

            for (float i = hueIncrement; i < 256; i += hueIncrement) // pink (#f0f) - Red (#f00)
            {
                hueBarColors[index] = Color.Rgb(255, 0, 255 - (int) i);
                index++;
            }

            mHuesBitmap = Bitmap.CreateBitmap(NUMBER_OF_HUES, 1, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(mHuesBitmap);
            PreDrawHueSlider(canvas, hueBarColors);
        }

        /// <summary>
        /// Draws all the hues into the canvas
        /// </summary>
        /// <param name="canvas"> where the hues will be drawn </param>
        /// <param name="hues"> list of colors that will be drawn </param>
        private void PreDrawHueSlider(Canvas canvas, int[] hues)
        {
            int height = canvas.Height;

            // Display all the colors of the hue bar with lines
            // The current selected color will be drawn with a BLACK line
            mPaint.StrokeWidth = 0;
            for (int x = 0; x < hues.Length; x++)
            {
                mPaint.Color = new Color(hues[x]);
                canvas.DrawLine(x, 0, x, height, mPaint);
            }
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            // We only modify the configuration if something changes
            if (changed)
            {
                int width = right - left;
                int height = bottom - top;

                mDensityMultiplier = (float)width / NUMBER_OF_HUES;
                mHueBarBitmap = Bitmap.CreateScaledBitmap(mHuesBitmap, width, height, false);
                mSliderWidth = width;

                if (mIsHuePending)
                {
                    mCurrentHue *= mDensityMultiplier;
                }
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            // we use default size if it's inflated through code
            if (!mWasInflated)
            {
                widthMeasureSpec = (int)(DEFAULT_WIDTH * mDensityMultiplier);
                heightMeasureSpec = (int)(DEFAULT_HEIGHT * mDensityMultiplier);
            }

            SetMeasuredDimension(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnDraw(Canvas canvas)
        {
            // Draws the scaled version of the hues
            canvas.DrawBitmap(mHueBarBitmap, 0, 0, mPaint);

            // Draws the line of the current selected hue
            int translatedHue = (int)(mCurrentHue);
            mPaint.Color = Color.Black;
            mPaint.StrokeWidth = (int)(mSelectedWidth * mDensityMultiplier);
            canvas.DrawLine(translatedHue, 0, translatedHue, canvas.Height, mPaint);
        }

        /// <summary>
        /// Returns the hue value for an specific color
        /// </summary>
        /// <param name="color"> in form of ARGB (the alpha part is optional) </param>
        /// <returns> the hue value [0 - 360) </returns>
        private float GetHueFromColor(Color color)
        {
            float[] hsv = new float[3];
            Color.ColorToHSV(new Color(color), hsv);
            return hsv[0] * mDensityMultiplier;
        }

        public virtual void SetHue(float value)
        {
            if (value < 0f || value >= NUMBER_OF_HUES)
            {
                throw new ArgumentException("The hue has to be between 0 (inclusive) and 360 (exclusive)");
            }
    
            // If it was inflated and hasn't being layout as far now,
            // we set it as a pending transaction
            if (mDensityMultiplier == 0)
            {
                mDensityMultiplier = 1;
                mIsHuePending = true;
            }
    
            mCurrentHue = value * mDensityMultiplier;
            Invalidate();
        }

        public virtual void SetColor(Color value)
        {
            // If it was inflated and hasn't being layout as far now,
            // we set it as a pending transaction
            if (mDensityMultiplier == 0)
            {
                mDensityMultiplier = 1;
                mIsHuePending = true;
            }
    
            mCurrentHue = GetHueFromColor(value);
            Invalidate();
        }

        public event EventHandler<ColorChangedEventArgs> HueChanged;

        protected virtual void OnHueChanged(ColorChangedEventArgs eventArgs)
        {
            var handler = HueChanged;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        public Color CurrentHue
        {
            get { return new Color(mHuesBitmap.GetPixel((int) (mCurrentHue/mDensityMultiplier), 0)); }
        }

        public override bool OnTouchEvent(MotionEvent @event)
        {
            // If the action is anything different that DOWN or MOVE we ignore the rest of the gesture
            if (@event.Action != MotionEventActions.Down && @event.Action != MotionEventActions.Move)
            {
                return false;
            }

            // Transform the coordinates to a position inside the view
            float x = @event.GetX();
            if (x < 0)
            {
                x = 0;
            }
            else if (x >= mSliderWidth)
            {
                x = mSliderWidth - 1;
            }

            // Update the main field colors
            mCurrentHue = x;
            OnHueChanged(new ColorChangedEventArgs(CurrentHue));

            // Re-draw the view
            Invalidate();

            return true;
        }
    }
}