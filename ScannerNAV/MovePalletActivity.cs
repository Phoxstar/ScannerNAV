using System;
using Android.App;
using Android.OS;
using ScannerNAV.Webservice;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace ScannerNAV
{
    [Activity(Label = "Move Pallet", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MovePalletActivity : Activity
    {
        private Button btnMove;
        private EditText etPalletNo;
        private EditText etBin;
        private TextView tvStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.MovePallet);

            etPalletNo = FindViewById<EditText>(Resource.Id.etPalletNo);
            etPalletNo.RequestFocus();

            etBin = FindViewById<EditText>(Resource.Id.etBin);
            etBin.KeyPress += OnBin_KeyPress;
            
            btnMove = FindViewById<Button>(Resource.Id.btnMove);
            btnMove.Click += OnMoveClick;

            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            tvStatus.Click += OnStatusClick;
            tvStatus.Text = Helper.GetStatus();
        }

        private void OnBin_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                e.Handled = true;
                btnMove.PerformClick();
            }
            else
                e.Handled = false;
        }

        private void OnMoveClick(object sender, EventArgs e)
        {
            try
            {
                ScannerInterface ws = Helper.GetInterface(this);

                default_root navResponse = new default_root();
           
                ws.MovePallet(ref navResponse, Helper.GetRescourceNo(), etPalletNo.Text, etBin.Text);

                default_response xmlResponse = navResponse.default_response[0];

                if (Helper.IsOK(xmlResponse.status))
                {
                    Helper.ShowToast(this, xmlResponse.status_text);
                    etPalletNo.Text = "";
                    etBin.Text = "";
                }
                else
                {
                    Helper.ShowAlertDialog(this, xmlResponse.status, xmlResponse.status_text);
                }
            }
            catch (Exception ex)
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