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
        private TextView tvContentNo;
        private EditText etContentNo;
        private TextView tvContentDescription;
        private EditText etContentDescription;
        private TextView tvQuantity;
        private EditText etQuantity;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.CreatePallet);

            etPalletNo = FindViewById<EditText>(Resource.Id.etPalletNo);
            etPalletNo.RequestFocus();

            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            btnCreate.Click += OnCreateClick;

            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            tvStatus.Click += OnStatusClick;
            tvStatus.Text = Helper.GetStatus();
        }

        private void OnCreateClick(object sender, EventArgs e)
        {
            try
            {
                ScannerInterface ws = Helper.GetInterface(this);
            }
            catch(Exception ex)
            {
                Helper.ShowAlertDialog(this, "ERROR", ex.Message);
            }
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