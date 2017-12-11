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
using ScannerNAV.Webservice;

namespace ScannerNAV
{
    [Activity(Label = "ScannerNAV", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class WrapPalletActivity : Activity
    {
        private Button btnWrap;
        private CheckBox cbPrint;
        private EditText etPalletNo;
        private EditText etBin;
        private Spinner spnWrapType;
        private TextView tvStatus;
        private TextView tvWrapType;
        private wraptype[] wraptypeList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.WrapPallet);

            etPalletNo = FindViewById<EditText>(Resource.Id.etPalletNo);
            etPalletNo.RequestFocus();

            etBin = FindViewById<EditText>(Resource.Id.etBin);
            etBin.KeyPress += OnBin_KeyPress;
            etBin.ShowSoftInputOnFocus = true;

            btnWrap = FindViewById<Button>(Resource.Id.btnWrap);
            btnWrap.Click += OnWrapClick;
            
            cbPrint = FindViewById<CheckBox>(Resource.Id.cbPrint);
            cbPrint.Checked = true;

            tvWrapType = FindViewById<TextView>(Resource.Id.tvWrapType);
            tvWrapType.Visibility = ViewStates.Invisible;
            spnWrapType = FindViewById<Spinner>(Resource.Id.spnWrapType);
            spnWrapType.Visibility = ViewStates.Invisible;

            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            tvStatus.Click += OnStatusClick;
            tvStatus.Text = Helper.GetStatus();
        }

        private void OnBin_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                e.Handled = true;
                btnWrap.CallOnClick();
                etPalletNo.RequestFocus();
            }
            else
                e.Handled = false;
        }

        private void OnWrapClick(object sender, EventArgs e)
        {
            try
            {
                ScannerInterface ws = Helper.GetInterface(this);

                wraptype_root navResponse = new wraptype_root();

                if(tvWrapType.Visibility == ViewStates.Invisible)
                {
                    ws.WrapPallet(ref navResponse, Helper.GetRescourceNo(), etPalletNo.Text, etBin.Text, cbPrint.Checked, "0");
                }
                else
                {
                    ws.WrapPallet(ref navResponse, Helper.GetRescourceNo(), etPalletNo.Text, etBin.Text, cbPrint.Checked, wraptypeList[spnWrapType.SelectedItemPosition].wraptype_entryno.ToString());
                }           

                wraptype_response wraptypexmlResponse = navResponse.wraptype_response[0];

                if (Helper.IsOK(wraptypexmlResponse.status))
                {
                    Helper.ShowToast(this, wraptypexmlResponse.status_text);
                    etPalletNo.Text = "";
                    etBin.Text = "";
                    tvWrapType.Visibility = ViewStates.Invisible;
                    spnWrapType.Visibility = ViewStates.Invisible;
                    etPalletNo.RequestFocus();
                }
                else if (Helper.IsNeddInfo(wraptypexmlResponse.status))
                {
                    int counter = 0;
                    int selectedItem = 0;
                    List<string> items = new List<string>(wraptypexmlResponse.wraptype.Length);
                    wraptypeList = wraptypexmlResponse.wraptype;
                    foreach (wraptype item in wraptypexmlResponse.wraptype)
                    {
                        items.Add(item.wraptype_entryno + " - " + item.wraptype_desc);
                        if (item.wraptype_entryno.ToString() == wraptypexmlResponse.default_wraptype)
                        {
                            selectedItem = counter;
                        }
                        counter++;
                    }
                    ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, items);
                    spnWrapType.Adapter = adapter;
                    spnWrapType.SetSelection(selectedItem);
                    tvWrapType.Visibility = ViewStates.Visible;
                    spnWrapType.Visibility = ViewStates.Visible;
                    spnWrapType.RequestFocus();
                }
                else
                {
                    Helper.ShowAlertDialog(this, wraptypexmlResponse.status, wraptypexmlResponse.status_text);
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