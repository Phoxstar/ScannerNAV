using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ScannerNAV.Webservice;



namespace ScannerNAV
{
    [Activity(Label = "Create Pallet", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CreatePalletActivity : Activity
    {
        private Button btnCreate;
        private EditText etPalletNo;
        private TextView tvStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.CreatePallet);

            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            btnCreate.Click += OnCreateClick;

            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            tvStatus.Click += OnStatusClick;
            tvStatus.Text = Helper.GetStatus();
        }

        private void OnCreateClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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