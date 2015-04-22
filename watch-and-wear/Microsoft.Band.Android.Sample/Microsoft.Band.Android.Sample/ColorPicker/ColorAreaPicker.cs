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
    public class ColorAreaPicker : View
    {
        /// <summary>
        /// Default values for the control
        /// </summary>
        private const int NUMBER_OF_GRADIENTS = 256;
        private const int DEFAULT_SELECTED_COLOR_RADIUS = 4;
        private const int DEFAULT_WIDTH = 256;
        private const int DEFAULT_HEIGHT = 256;

        // True if inflated trough XML, false if created programmatically
        private bool mWasInflated;

        // The ratio between the number of hues and the size of the view
        private float mWidthDensityMultiplier;
        private float mHeightDensityMultiplier;

        // Paint objects used throughout the view
        private Paint mGradientsPaint;
        private Paint mInnerCirclePaint;
        private Paint mOutterCirclePaint;
        private Paint mBitmapPaint;

        // Holds the width of the Slider's bitmap
        private int mInnerCircleWidth;

        // This is the color currently selected
        private Color mCurrentColor;

        // The color used as the base for all calculations
        private Color mBaseColor;

        // Location of the last selected color
        private int mCurrentX = 0, mCurrentY = 0;
        private bool mHasMoved = false;

        // The size of the picker area
        private int mPickerWidth = 0, mPickerHeight = 0;

        // Objects needed for caching the colors
        private Matrix mColorBitmapMatrix;
        private Bitmap mColorsBitmap;
        private Canvas mPreRenderingCanvas;

        // Hue picker that will notify if the current hue has changed
        private HueBarSlider mHuePicker;

        /// <summary>
        /// Use this constructor to generate a DEFAULT_SIZE color picker area
        /// </summary>
        /// <param name="context"> to be used for inflating and to search for resources </param>
        public ColorAreaPicker(Context context)
            : base(context)
        {

            if (context != null)
            {
                int screenDensity = (int) Math.Ceiling(context.Resources.DisplayMetrics.Density);
                mWidthDensityMultiplier = screenDensity;
                mHeightDensityMultiplier = screenDensity;
            }

            Init(false);
        }

        public ColorAreaPicker(Context context, IAttributeSet attrs) 
            : this(context, attrs, 0)
        {
        }

        public ColorAreaPicker(Context context, IAttributeSet attrs, int defStyleAttr)
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

            mInnerCirclePaint = new Paint();
            mOutterCirclePaint = new Paint();
            mGradientsPaint = new Paint();
            mBitmapPaint = new Paint();

            mInnerCircleWidth = DEFAULT_SELECTED_COLOR_RADIUS;
            mInnerCirclePaint.SetStyle(Paint.Style.Stroke);
            mInnerCirclePaint.Color = Color.Black;

            mOutterCirclePaint.SetStyle(Paint.Style.Stroke);
            mOutterCirclePaint.Color = Color.White;

            mColorBitmapMatrix = new Matrix();
            mColorsBitmap = Bitmap.CreateBitmap(256, 256, Bitmap.Config.Argb8888);
            mPreRenderingCanvas = new Canvas(mColorsBitmap);
        }

        // Update the main field colors depending on the current selected hue
        private void UpdateMainColors(Color color)
        {
            mBaseColor = color;

            int baseRed = mBaseColor.R;
            int baseGreen = mBaseColor.G;
            int baseBlue = mBaseColor.B;

            // draws the NUMBER_OF_GRADIENTS into a bitmap for later use
            int[] colors = new int[2];
            colors[1] = Color.Black;
            for (int x = 0; x < 256; ++x)
            {
                colors[0] = Color.Rgb(255 - (255 - baseRed) * x / 255, 255 - (255 - baseGreen) * x / 255, 255 - (255 - baseBlue) * x / 255);
                Shader gradientShader = new LinearGradient(0, 0, 0, 256, colors, null, Shader.TileMode.Clamp);
                mGradientsPaint.SetShader(gradientShader);
                mPreRenderingCanvas.DrawLine(x, 0, x, 256, mGradientsPaint);
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

                mPickerWidth = width;
                mPickerHeight = height;

                mWidthDensityMultiplier = (float) width / NUMBER_OF_GRADIENTS;
                mHeightDensityMultiplier = (float) height / NUMBER_OF_GRADIENTS;
                mColorBitmapMatrix.SetScale(mWidthDensityMultiplier, mHeightDensityMultiplier);
                mInnerCirclePaint.StrokeWidth = mWidthDensityMultiplier;
                mOutterCirclePaint.StrokeWidth = mWidthDensityMultiplier + 1;
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            // we use default size if it's inflated through code
            if (!mWasInflated)
            {
                widthMeasureSpec = (int)(DEFAULT_WIDTH * mWidthDensityMultiplier);
                heightMeasureSpec = (int)(DEFAULT_HEIGHT * mHeightDensityMultiplier);
            }

            SetMeasuredDimension(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnDraw(Canvas canvas)
        {
            // Draws the scaled version of the hues
            canvas.DrawBitmap(mColorsBitmap, mColorBitmapMatrix, mBitmapPaint);

            // Display the circle around the currently selected color in the main field
            if (mHasMoved)
            {
                canvas.DrawCircle(mCurrentX, mCurrentY, mInnerCircleWidth * mWidthDensityMultiplier, mOutterCirclePaint);
                canvas.DrawCircle(mCurrentX, mCurrentY, mInnerCircleWidth * mWidthDensityMultiplier, mInnerCirclePaint);
            }
        }

        public virtual void SetColor(Color value)
        {
                NotifyHuePicker(value);
    
                UpdateMainColors(value);
    
                UpdateCurrentColor();
    
                Invalidate();
        }

        public void SetHuePicker(HueBarSlider value)
        {
            mHuePicker = value;
            mHuePicker.HueChanged += (sender, e) =>
            {
                SetColor(e.Color);
            };
            SetColor(mHuePicker.CurrentHue);
        }

        /*
        Notifies the hue picker when the base color has been change on the color picker
         */
        private void NotifyHuePicker(Color color)
        {
            if (mHuePicker != null)
            {
                mHuePicker.SetColor(color);
            }
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
            float y = @event.GetY();
            mHasMoved = true;

            // Adjust X coordinate
            if (x < 0)
            {
                x = 0;
            }
            else if (x >= mPickerWidth)
            {
                x = mPickerWidth - 1;
            }

            // Adjust Y coordinate
            if (y < 0)
            {
                y = 0;
            }
            else if (y >= mPickerHeight)
            {
                y = mPickerHeight - 1;
            }

            mCurrentX = (int) x;
            mCurrentY = (int) y;

            UpdateCurrentColor();

            // Re-draw the view
            Invalidate();

            return true;
        }

        private void UpdateCurrentColor()
        {
            if (mHasMoved)
            {
                int transX = (int)(mCurrentX / mWidthDensityMultiplier);
                int transY = (int)(mCurrentY / mHeightDensityMultiplier);
                mCurrentColor = new Color(mColorsBitmap.GetPixel(transX, transY));
            }
            else
            {
                mCurrentColor = mBaseColor;
            }

            OnColorChanged(new ColorChangedEventArgs(mCurrentColor));
        }

        public int CurrentColor
        {
            get { return mCurrentColor; }
        }

        public event EventHandler<ColorChangedEventArgs> ColorChanged;

        protected virtual void OnColorChanged(ColorChangedEventArgs eventArgs)
        {
            var handler = ColorChanged;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }
    }

}