using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MedzoidSite.Models
{
    public class CommonFunctions
    {

        //public static string SearchAddress(string address)
        //{

        //    string URI = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + address + "&language=en&sensor=true&key=AIzaSyAwkve_xhk-YDVBegxHYQZjXS7Qr3pKzBI";

        //    using (WebClient wc = new WebClient())
        //    {
        //        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //        string HtmlResult = wc.UploadString(URI, address);
        //        return HtmlResult;
        //    }
        //}
        public static string SearchAddress(string address)
        {

            string URI = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + address + "&language=en&sensor=true&key=AIzaSyAwkve_xhk-YDVBegxHYQZjXS7Qr3pKzBI";

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, address);
                return HtmlResult;
            }
        }


        public static string GetLatLong(string placeId)
        {

            string URI = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + placeId + "&key=AIzaSyAwkve_xhk-YDVBegxHYQZjXS7Qr3pKzBI";

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, placeId);
                return HtmlResult;
            }
        }


        public string Data(string name)
        {
            string suggestion = "";
            using (var client = new WebClient())
            {
                suggestion = client.DownloadString("https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + name + "&language=en&sensor=true&key=AIzaSyAwkve_xhk-YDVBegxHYQZjXS7Qr3pKzBI");
            }

            return suggestion;
        }




    }
}