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
    public class MenuActivity : Activity
    {
        private Button btnCount;
        private Button btnCreatePallet;
        private Button btnMovePallet;
        private Button btnWrapPallet;
        private Button btnPickOrder;
        private Button btnPrintPallet;
        private Button btnLoadingPallet;
        private Button btnOtherFunctions;
        private TextView tvStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Menu);

            btnCount = FindViewById<Button>(Resource.Id.btnCount);
            btnCount.Click += OnCount_Click;

            btnCreatePallet = FindViewById<Button>(Resource.Id.btnCreatePallet);
            btnCreatePallet.Click += OnCreatePallet_Click; ;

            btnMovePallet = FindViewById<Button>(Resource.Id.btnMovePallet);
            btnMovePallet.Click += OnMovePallet_Click; ;

            btnWrapPallet = FindViewById<Button>(Resource.Id.btnWrapPallet);
            btnWrapPallet.Click += OnWrapPallet_Click; ;

            btnPickOrder = FindViewById<Button>(Resource.Id.btnPickOrder);
            btnPickOrder.Click += OnPickOrder_Click;

            btnPrintPallet = FindViewById<Button>(Resource.Id.btnPrintPallet);
            btnPrintPallet.Click += OnPrintPallet_Click;

            btnLoadingPallet = FindViewById<Button>(Resource.Id.btnLoadingPallet);
            btnLoadingPallet.Click += OnLoadingPallet_Click;

            btnOtherFunctions = FindViewById<Button>(Resource.Id.btnOtherFunctions);
            btnOtherFunctions.Click += OtherFunctions_Click;

            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            tvStatus.Click += OnStatusClick;
            tvStatus.Text = Helper.GetStatus();
        }

        private void OnCount_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(CountPalletActivity));
        }

        public void OnPickOrder_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(PickOrderActivity));
        }

        private void OnCreatePallet_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(CreatePalletActivity));
        }

        private void OnMovePallet_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MovePalletActivity));
        }

        private void OnWrapPallet_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(WrapPalletActivity));
        }

        private void OnPrintPallet_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(PrintPalletActivity));
        }

        private void OnLoadingPallet_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoadingPalletActivity));
        }

        private void OtherFunctions_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(OtherFunctionsActivity));
        }

        private void OnStatusClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SettingsActivity));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            tvStatus.Text = Helper.GetStatus();
        }
    }
}