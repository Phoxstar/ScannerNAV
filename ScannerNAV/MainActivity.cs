using Android.App;
using Android.Widget;
using Android.OS;
using ScannerNAV.Webservice;
using System;
using Android.Content;

namespace ScannerNAV
{
    [Activity(Label = "ScannerNAV", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private Button btnLogin;
        private TextView tvStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // TEST
            if (Helper.GetWSUser() == null)
            {
                Helper.SetWSUser("PSH-NAV2015\\Administrator");
                Helper.SetWSPasword("TanHus4d");
                Helper.SetWSUrl("http://10.100.3.63:7147/OPC/WS/OPC/Codeunit/ScannerInterface");
            }

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += OnLoginClick;

            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            tvStatus.Click += OnStatusClick;
        }

        private void OnLoginClick(object sender, System.EventArgs e)
        {
            try
            {
                if (Helper.GetRescourceNo() == null)
                {
                    Intent intent = new Intent(this, typeof(LogInActivity));
                    StartActivityForResult(intent, 1);
                }
                else
                {
                    StartActivity(typeof(MenuActivity));
                }
            }
            catch (Exception ex)
            {
                Helper.ShowAlertDialog(this, "ERROR", ex.Message);                
            }

        }
        
        private void OnStatusClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SettingsConnectActivity));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 1)
            {
                btnLogin.PerformClick();
            }
        }
    }
}

