using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ScannerNAV
{
    [Activity(Label = "ScannerNAV", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoadingPalletActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            Helper.ShowAlertDialog(this, "Wait for it..", "Not Implemented");
        }
    }
}