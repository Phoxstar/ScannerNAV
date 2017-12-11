using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace ScannerNAV
{
    [Activity(Label = "SettingsConnect", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SettingsConnectActivity : Activity
    {
        private Button btnSave;
        private EditText etUrl;
        private EditText etUsername;
        private EditText etPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.SettingsConnect);            
            etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            etUrl = FindViewById<EditText>(Resource.Id.etUrl);
            
            etUsername.Text = Helper.GetWSUser();
            etPassword.Text = Helper.GetWSPassword();
            etUrl.Text = Helper.GetWSUrl();

            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Click += OnSave;            
        }

        private void OnSave(object sender, EventArgs e)
        {            
            Helper.SetWSUser(etUsername.Text);
            Helper.SetWSPasword(etPassword.Text);
            Helper.SetWSUrl(etUrl.Text);
            SetResult(Result.Ok);
            Finish();
        }
    }
}