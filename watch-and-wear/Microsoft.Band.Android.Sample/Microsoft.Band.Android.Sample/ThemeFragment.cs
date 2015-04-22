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
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Microsoft.Band.Personalization;
using Microsoft.Band.Tiles;
using Fragment = Android.Support.V4.App.Fragment;

namespace Microsoft.Band.Sample
{
    public class ThemeFragment : Fragment, FragmentListener
    {
        protected internal const int SELECT_IMAGE = 0;

        private View mRootView;
        private BandThemeView mViewTheme;

        private Button mButtonSelectBackground;

        private Button mButtonChooseTheme;

        private Button mButtonGetTheme;
        private Button mButtonSetTheme;

        private ImageView mImageBackground;

        private Button mButtonGetBackground;
        private Button mButtonSetBackground;

        private Bitmap mSelectedImage;

        public virtual void OnFragmentSelected()
        {
            if (IsVisible)
            {
                RefreshControls();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mRootView = inflater.Inflate(Resource.Layout.fragment_theme, container, false);

            mViewTheme = mRootView.FindViewById<BandThemeView>(Resource.Id.viewTheme);
            mViewTheme.Theme = BandTheme.VioletTheme;

            mButtonGetTheme = mRootView.FindViewById<Button>(Resource.Id.buttonGetTheme);
            mButtonGetTheme.Click += OnGetThemeClick;

            mButtonChooseTheme = mRootView.FindViewById<Button>(Resource.Id.buttonChooseTheme);
            mButtonChooseTheme.Click += OnChooseThemeClick;

            mButtonSetTheme = mRootView.FindViewById<Button>(Resource.Id.buttonSetTheme);
            mButtonSetTheme.Click += OnSetThemeClick;

            mButtonSelectBackground = mRootView.FindViewById<Button>(Resource.Id.buttonSelectBackground);
            mButtonSelectBackground.Click += OnSelectBackgroundClick;

            mImageBackground = mRootView.FindViewById<ImageView>(Resource.Id.imageBackground);

            mButtonGetBackground = mRootView.FindViewById<Button>(Resource.Id.buttonGetBackground);
            mButtonGetBackground.Click += OnGetBackgroundClick;

            mButtonSetBackground = mRootView.FindViewById<Button>(Resource.Id.buttonSetBackground);
            mButtonSetBackground.Click += OnSetBackgroundClick;

            return mRootView;
        }

        private async void OnGetBackgroundClick(object sender, EventArgs e)
        {
            try
            {
                mSelectedImage = await Model.Instance.Client.PersonalizationManager.GetMeTileImageTaskAsync();

                mImageBackground.SetImageBitmap(mSelectedImage);
                RefreshControls();
            }
            catch (Exception ex)
            {
                Util.ShowExceptionAlert(Activity, "Get background image", ex);
            }
        }

        private async void OnSetBackgroundClick(object sender, EventArgs e)
        {
            try
            {
                await Model.Instance.Client.PersonalizationManager.SetMeTileImageTaskAsync(mSelectedImage);
            }
            catch (Exception ex)
            {
                Util.ShowExceptionAlert(Activity, "Set background image", ex);
            }
        }

        private void OnSelectBackgroundClick(object sender, EventArgs e)
        {
            StartActivityForResult(new Intent(Intent.ActionPick, MediaStore.Images.Media.InternalContentUri), SELECT_IMAGE);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == SELECT_IMAGE)
            {
                if (resultCode == (int)Result.Ok)
                {
                    Android.Net.Uri imageUri = data.Data;
                    string imagePath = GetPath(imageUri);
                    Bitmap bm = BitmapFactory.DecodeFile(imagePath);
                    if (bm != null)
                    {
                        mSelectedImage = bm;
                        mImageBackground.SetImageBitmap(mSelectedImage);
                        RefreshControls();
                    }
                }
            }
        }

        // Convert a gallery URI to a regular file path
        private string GetPath(Android.Net.Uri uri)
        {
            string[] projection = new string[] { MediaStore.MediaColumns.Data };
            CursorLoader loader = new CursorLoader(Activity, uri, projection, null, null, null);
            ICursor cursor = (ICursor)loader.LoadInBackground();
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.MediaColumns.Data);
            cursor.MoveToFirst();
            return cursor.GetString(column_index);
        }

        private async void OnChooseThemeClick(object sender, EventArgs e)
        {
            using (var builder = new AlertDialog.Builder(Activity))
            {
                PropertyInfo[] themes = typeof(BandTheme).GetProperties().Where(p => p.Name.EndsWith("Theme")).ToArray();

                builder.SetItems(themes.Select(x => x.Name).ToArray(), (dialog, args) =>
                {
                    mViewTheme.Theme = (BandTheme) themes[args.Which].GetValue(null);
                    ((Dialog) dialog).Dismiss();
                    RefreshControls();
                });

                builder.SetTitle("Select theme:");
                builder.Show();
            }
        }

        private async void OnGetThemeClick(object sender, EventArgs e)
        {
            try
            {
                BandTheme theme = await Model.Instance.Client.PersonalizationManager.GetThemeTaskAsync();

                mViewTheme.Theme = theme;
                RefreshControls();
            }
            catch (Exception ex)
            {
                Util.ShowExceptionAlert(Activity, "Get theme", ex);
            }
        }

        private async void OnSetThemeClick(object sender, EventArgs e)
        {
            try
            {
                await Model.Instance.Client.PersonalizationManager.SetThemeTaskAsync(mViewTheme.Theme);

                RefreshControls();
            }
            catch (Exception ex)
            {
                Util.ShowExceptionAlert(Activity, "Set theme", ex);
            }
        }

        private void RefreshControls()
        {
            bool connected = Model.Instance.Connected;

            mButtonGetTheme.Enabled = connected;
            mButtonSetTheme.Enabled = connected;

            mButtonGetBackground.Enabled = connected;
            mButtonSetBackground.Enabled = connected && mSelectedImage != null;
        }
    }

}