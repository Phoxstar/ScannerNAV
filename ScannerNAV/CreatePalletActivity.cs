using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ScannerNAV.Webservice;
using System.Collections.Generic;
//using Android.Icu.Text;

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
        private TextView tvContentType;
        private Spinner spnContentType;
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

            etQuantity = FindViewById<EditText>(Resource.Id.etQuantity);
            etQuantity.SetRawInputType(Android.Text.InputTypes.NumberFlagDecimal | Android.Text.InputTypes.ClassNumber);

            spnContentType = FindViewById<Spinner>(Resource.Id.spnContentType);
            spnContentType.SetSelection(1);
            List<string> contentTypelist = new List<string>(new string[] { "Inner", "Outer"});
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, contentTypelist);
            spnContentType.Adapter = adapter;
        }




        private void OnCreateClick(object sender, EventArgs e)
        {
            try
            {
                ScannerInterface ws = Helper.GetInterface(this);

                default_root navResponse = new default_root();
                
                decimal decimal_qty = Decimal.Parse(etQuantity.Text);
                ws.CreatePallet(ref navResponse, etPalletNo.Text, etContentNo.Text, decimal_qty);

                default_response xmlResponse = navResponse.default_response[0];

                if (Helper.IsOK(xmlResponse.status))
                {
                    Helper.ShowToast(this, xmlResponse.status_text);
                    etPalletNo.Text = "";
                    etContentNo.Text = "";
                    etQuantity.Text = "";
                    etPalletNo.RequestFocus();
                 }
                else
                {
                    Helper.ShowAlertDialog(this, xmlResponse.status, xmlResponse.status_text);
                }
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