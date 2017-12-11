using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ScannerNAV.Webservice;
using System.Collections.Generic;
using Android.Views;

namespace ScannerNAV
{
    [Activity(Label = "Settings", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SettingsActivity : Activity
    {
        private Button btnSave;
        private CheckBox cbShowAll;        
        private Spinner spnZone;
        private Spinner spnPrinterGroup;
        private TextView tvResourceNo;
        private TextView tvName;
        private TextView tvDistNo;
        private ScannerInterface ws;
        private zone[] zoneList;
        private printer_group[] printerList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                RequestWindowFeature(WindowFeatures.NoTitle);

                // Create your application here
                SetContentView(Resource.Layout.Settings);

                tvResourceNo = FindViewById<TextView>(Resource.Id.tvResourceNo);
                tvName = FindViewById<TextView>(Resource.Id.tvName);
                tvDistNo = FindViewById<TextView>(Resource.Id.tvDistNo);
                cbShowAll = FindViewById<CheckBox>(Resource.Id.cbShowAll);
                spnZone = FindViewById<Spinner>(Resource.Id.spnZone);
                spnPrinterGroup = FindViewById<Spinner>(Resource.Id.spnPrinterGroup);

                btnSave = FindViewById<Button>(Resource.Id.btnSave);
                btnSave.Click += OnSave;

                RefreshPage();
            }
            catch (Exception ex)
            {
                Helper.ShowAlertDialog(this, "ERROR", ex.Message);
            }
        }

        private void RefreshPage()
        {
            ws = Helper.GetInterface(this);
            resource_root resourceRoot = new resource_root();
 
            ws.GetResourcesInfo(ref resourceRoot, Helper.GetRescourceNo());

            resource_response resourceInfoXmlResponse = resourceRoot.resource_response[0];

            if (Helper.IsOK(resourceInfoXmlResponse.status))
            {
                resource resourceInfo = resourceInfoXmlResponse.resource[0];
                tvResourceNo.Text = resourceInfo.no;
                tvName.Text = resourceInfo.name;
                tvDistNo.Text = resourceInfo.distno;
                cbShowAll.Checked = resourceInfo.pickall != "0";

                // Zones
                zone_root zone_Root = new zone_root();
                ws.GetZone(ref zone_Root);
                zone_response zoneXmlResponse = zone_Root.zone_response[0];
                if (Helper.IsOK(zoneXmlResponse.status))
                {
                    int counter = 0;
                    int selectedItem = 0;
                    List<string> items = new List<string>(zoneXmlResponse.zone.Length);
                    zoneList = zoneXmlResponse.zone;
                    foreach (zone item in zoneXmlResponse.zone)
                    {
                        items.Add(item.zone_code + " - " + item.zone_desc);
                        if (item.zone_code == resourceInfo.zone)
                        {
                            selectedItem = counter;
                        }
                        counter++;
                    }
                    ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, items);
                    spnZone.Adapter = adapter;
                    spnZone.SetSelection(selectedItem);
                }
                else
                {
                    Helper.ShowAlertDialog(this, zoneXmlResponse.status, zoneXmlResponse.status_text);
                }

                // Printers
                printergroup_root printer_Root = new printergroup_root();
                ws.GetPrinterGroup(ref printer_Root);
                printergroup_response printerXmlResponse = printer_Root.printergroup_response[0];
                if (Helper.IsOK(printerXmlResponse.status))
                {
                    int counter = 0;
                    int selectedItem = 0;
                    List<string> items = new List<string>(printerXmlResponse.printer_group.Length);
                    printerList = printerXmlResponse.printer_group;
                    foreach (printer_group item in printerXmlResponse.printer_group)
                    {
                        items.Add(item.printer_no + " - " + item.printer_desc);
                        if (item.printer_no == resourceInfo.printergroup)
                        {
                            selectedItem = counter;
                        }
                        counter++;
                    }
                    ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, items);
                    spnPrinterGroup.Adapter = adapter;
                    spnPrinterGroup.SetSelection(selectedItem);
                }
                else
                {
                    Helper.ShowAlertDialog(this, zoneXmlResponse.status, zoneXmlResponse.status_text);
                }
            }
            else
            {
                Helper.ShowAlertDialog(this, resourceInfoXmlResponse.status, resourceInfoXmlResponse.status_text);
            }
        }

        private void OnSave(object sender, EventArgs e)
        {            
            default_root defaultResponseRoot = new default_root();
            ws.SetResourcesInfo(ref defaultResponseRoot,Helper.GetRescourceNo(),cbShowAll.Checked, zoneList[spnZone.SelectedItemPosition].zone_code, printerList[spnPrinterGroup.SelectedItemPosition].printer_no);
            default_response xmlResponse = defaultResponseRoot.default_response[0];
            if (Helper.IsOK(xmlResponse.status))
            {
                Helper.SetZone(zoneList[spnZone.SelectedItemPosition].zone_code);
                Finish();
            }
            else
            {
                Helper.ShowAlertDialog(this, xmlResponse.status, xmlResponse.status_text);
            }            
        }
    }
}