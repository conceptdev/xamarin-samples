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
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using ActionBar = Android.Support.V7.App.ActionBar;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

[assembly: UsesPermission(Manifest.Permission.Bluetooth)]
[assembly: UsesPermission(Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Microsoft.Band.BandClientManager.BindBandService)]

namespace Microsoft.Band.Sample
{
    [Activity(Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, Theme = "@style/Theme.AppCompat")]
    public class MainActivity : ActionBarActivity, ActionBar.ITabListener
    {
        /// <summary>
        /// The <seealso cref="android.support.v4.view.PagerAdapter"/> that will provide
        /// fragments for each of the sections. We use a <seealso cref="FragmentPagerAdapter"/>
        /// derivative, which will keep every loaded fragment in memory. If this
        /// becomes too memory intensive, it may be best to switch to a
        /// <seealso cref="android.support.v13.app.FragmentStatePagerAdapter"/>.
        /// </summary>
        internal SectionsPagerAdapter mSectionsPagerAdapter;

        /// <summary>
        /// The <seealso cref="ViewPager"/> that will host the section contents.
        /// </summary>
        internal ViewPager mViewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            // Set up the action bar.
            ActionBar actionBar = SupportActionBar;
            actionBar.NavigationMode = (int)ActionBarNavigationMode.Tabs;

            // Create the adapter that will return a fragment for each of the three
            // primary sections of the activity.
            mSectionsPagerAdapter = new SectionsPagerAdapter(this, SupportFragmentManager);

            // Set up the ViewPager with the sections adapter.
            mViewPager = FindViewById(Resource.Id.pager).JavaCast<ViewPager>();
            mViewPager.Adapter = mSectionsPagerAdapter;

            // When swiping between different sections, select the corresponding
            // tab. We can also use ActionBar.Tab#select() to do this if we have
            // a reference to the Tab.
            mViewPager.PageSelected += (sender, args) =>
            {
                actionBar.SetSelectedNavigationItem(args.Position);
            };

            // For each of the sections in the app, add a tab to the action bar.
            for (int i = 0; i < mSectionsPagerAdapter.Count; i++)
            {
                // Create a tab with text corresponding to the page title defined by
                // the adapter. Also specify this Activity object, which implements
                // the TabListener interface, as the callback (listener) for when
                // this tab is selected.
                var tab = actionBar
                    .NewTab()
                    .SetText(mSectionsPagerAdapter.GetPageTitle(i))
                    .SetTabListener(this);
                actionBar.AddTab(tab);
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the menu; this adds items to the action bar if it is present.
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle action bar item clicks here. The action bar will
            // automatically handle clicks on the Home/Up button, so long
            // as you specify a parent activity in AndroidManifest.xml.
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
            // When the given tab is selected, switch to the corresponding page in
            // the ViewPager.
            int pos = tab.Position;
            mViewPager.CurrentItem = pos;

            Fragment fragment = ((FragmentPagerAdapter) mViewPager.Adapter).GetItem(pos);
            if (fragment is FragmentListener)
            {
                ((FragmentListener) fragment).OnFragmentSelected();
            }
        }

        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
        }

        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction)
        {
        }

        /// <summary>
        /// A <seealso cref="FragmentPagerAdapter"/> that returns a fragment corresponding to
        /// one of the sections/tabs/pages.
        /// </summary>
        public class SectionsPagerAdapter : FragmentPagerAdapter
        {
            internal IList<Tuple<Fragment, string>> mFragments;

            public SectionsPagerAdapter(Context context, FragmentManager fragmentManager)
                : base(fragmentManager)
            {
                mFragments = new List<Tuple<Fragment, string>>();
                mFragments.Add(new Tuple<Fragment, string>(
                    new BasicsFragment(),
                    context.GetString(Resource.String.title_basics_section).ToUpper(CultureInfo.CurrentCulture)));
                mFragments.Add(new Tuple<Fragment, string>(
                    new ThemeFragment(),
                    context.GetString(Resource.String.title_theme_section).ToUpper(CultureInfo.CurrentCulture)));
                mFragments.Add(new Tuple<Fragment, string>(
                    new TilesFragment(),
                    context.GetString(Resource.String.title_tiles_section).ToUpper(CultureInfo.CurrentCulture)));
                mFragments.Add(new Tuple<Fragment, string>(
                    new SensorsFragment(),
                    context.GetString(Resource.String.title_sensors_section).ToUpper(CultureInfo.CurrentCulture)));
            }

            public override Fragment GetItem(int position)
            {
                return mFragments[position].Item1;
            }

            public override int Count
            {
                get { return mFragments.Count; }
            }

            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(mFragments[position].Item2);
            }
        }
    }
}