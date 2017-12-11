using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ScannerNAV.Webservice;

namespace ScannerNAV
{
    [Activity(Label = "LogInActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LogInActivity : Activity
    {
        private Button btnLogin;
        private Spinner SpnResource;
        private TextView tvPIN;
        private resource[] resourceList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.LogIn);

            Helper.SetRescourceNo("");

            SpnResource = FindViewById<Spinner>(Resource.Id.spnResource);
            tvPIN = FindViewById<TextView>(Resource.Id.tvPIN);
            tvPIN.KeyPress += OnPin_KeyPress;

            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += OnLoginClick;

            try
            {
                ScannerInterface ws = Helper.GetInterface(this);

                resource_root resourceResponseRoot = new resource_root();

                Int32.TryParse(tvPIN.Text, out int PIN);
                ws.GetResources(ref resourceResponseRoot);
                resource_response resourceXmlResponse = resourceResponseRoot.resource_response[0];

                if (Helper.IsOK(resourceXmlResponse.status))
                {
                    int counter = 0;
                    List<string> items = new List<string>(resourceXmlResponse.resource.Length);
                    resourceList = resourceXmlResponse.resource;
                    foreach (resource item in resourceXmlResponse.resource)
                    {
                        items.Add(item.no + " - " + item.name);
                        counter++;
                    }
                    ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, items);
                    SpnResource.Adapter = adapter;
                }
                else
                {
                    Helper.ShowAlertDialog(this, resourceXmlResponse.status, resourceXmlResponse.status_text);
                }
            }
            catch (Exception ex)
            {
                Helper.ShowAlertDialog(this, "ERROR", ex.Message);
            }
        }

        private void OnPin_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                e.Handled = true;
                btnLogin.PerformClick();
            }
            else
                e.Handled = false;
        }

        private void OnLoginClick(object sender, System.EventArgs e)
        {
            try
            {
                ScannerInterface ws = Helper.GetInterface(this);

                default_root defaultResponseRoot = new default_root();

                Int32.TryParse(tvPIN.Text, out int PIN);
                ws.Login(ref defaultResponseRoot, resourceList[SpnResource.SelectedItemPosition].no,PIN);

                default_response xmlResponse = defaultResponseRoot.default_response[0];

                if (Helper.IsOK(xmlResponse.status))
                {
                    Helper.SetRescourceNo(resourceList[SpnResource.SelectedItemPosition].no);
                    StartActivity(typeof(MenuActivity));
                }
                else
                {
                    Helper.ShowAlertDialog(this, xmlResponse.status, xmlResponse.status_text);
                    Intent intent = new Intent(this, typeof(SettingsActivity));
                    StartActivityForResult(intent, 1);
                }
            }
            catch (Exception ex)
            {
                Helper.ShowAlertDialog(this, "ERROR", ex.Message);
            }

        }
    }
}