using Android.App;
using Android.Content;
using ScannerNAV.Webservice;
using Android.Widget;
using Android.Graphics;

namespace ScannerNAV
{
    public class Helper
    {
        internal static void ShowAlertDialog(Context Context,string Title,string Message)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(Context);
            AlertDialog alert = dialog.Create();
            alert.SetTitle(Title);
            alert.SetMessage(Message);
            alert.SetButton("OK", (c, ev) => { });
            alert.Show();
        }

        internal static void ShowToast(Context Context, string status_text)
        {
            Toast toast = Toast.MakeText(Context, status_text, ToastLength.Long);
            toast.View.SetBackgroundColor(Color.Green);
            TextView view = (TextView)toast.View.FindViewById(Android.Resource.Id.Message);            
            view.SetTextColor(Color.Black);
            toast.Show();
        }

        internal static ScannerInterface GetInterface(Context Context)
        {
            ScannerInterface ws = new ScannerInterface
            {
                Credentials = new System.Net.NetworkCredential(GetWSUser(), GetWSPassword()),
                Url = GetWSUrl()
            };
            return ws;
        }

        internal static string GetStatus()
        {
            return GetRescourceNo() + " | " + GetZone();
        }

        internal static bool IsOK(string Value)
        {
            if (Value == "ok")
                return true;
            else
                return false;
        }

        internal static bool IsNeddInfo(string Value)
        {
            if (Value == "needInfo")
                return true;
            else
                return false;
        }

        // WSUser
        internal static string GetWSUser()
        {
            return GetKey("WSUser");
        }
        internal static void SetWSUser(string KeyValue)
        {
            SetKey("WSUser",KeyValue);
        }
        // WSPassword
        internal static string GetWSPassword()
        {
            return GetKey("WSPassword");
        }
        internal static void SetWSPasword(string KeyValue)
        {
            SetKey("WSPassword", KeyValue);
        }
        // WSUrl
        internal static string GetWSUrl()
        {
            return GetKey("WSUrl");
        }
        internal static void SetWSUrl(string KeyValue)
        {
            SetKey("WSUrl", KeyValue);
        }
        // RescourceNo
        internal static string GetRescourceNo()
        {
            return GetKey("RescourceNo");
        }
        internal static void SetRescourceNo(string KeyValue)
        {
            SetKey("RescourceNo", KeyValue);
        }
        // Zone
        internal static string GetZone()
        {
            return GetKey("Zone");
        }
        internal static void SetZone(string KeyValue)
        {
            SetKey("Zone", KeyValue);
        }

        private static void SetKey(string KeyName, string KeyValue)
        {
            var prefs = Application.Context.GetSharedPreferences("ScannerNAV", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(KeyName, KeyValue);
            prefEditor.Commit();
        }

        private static string GetKey(string KeyName)
        {
            //retreive 
            var prefs = Application.Context.GetSharedPreferences("ScannerNAV", FileCreationMode.Private);
            return prefs.GetString(KeyName, null);
        }
    }
}