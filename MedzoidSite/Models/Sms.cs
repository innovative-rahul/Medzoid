using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Sms
/// </summary>
public class Sms
{

    public string _mobileno { get; set; }
    public string _txt { get; set; }



    public string mobileno { set { _mobileno = value; } get { return _mobileno; } }
    public string txt { set { _txt = value; } get { return _txt; } }


    public Sms()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void SMS_SEND(string type)
    {

        //Your authentication key
        string authKey = "9b656361-fd4f-11e9-9fa5-0200cd936042";
        //Multiple mobiles numbers separated by comma
        string mobileNumber = mobileno;
        //Sender ID,While using route4 sender id should be 6 characters long.
        string senderId = "Medzod";
        //Your message to send, Add URL encoding here.
        string message = HttpUtility.UrlEncode(txt.ToString());
        //string message = HttpUtility.UrlEncode("Test");


        //Prepare you post parameters
        StringBuilder sbPostData = new StringBuilder();
        // sbPostData.AppendFormat("authkey={0}", authKey);
        sbPostData.AppendFormat("&To={0}", mobileNumber);
        sbPostData.AppendFormat("&Msg={0}", message);
        sbPostData.AppendFormat("&From={0}", senderId);
        //sbPostData.AppendFormat("&route={0}", "template");

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Call Send SMS API


            string sendSMSUri = "";
            if (type != "Promotional")
            {
                sendSMSUri = "http://2factor.in/API/V1/9b656361-fd4f-11e9-9fa5-0200cd936042/SMS/" + mobileNumber + "/" + message + "/medzoid1";
            }
            else
            {
                sendSMSUri = "http://2factor.in/API/V1/9b656361-fd4f-11e9-9fa5-0200cd936042/ADDON_SERVICES/SEND/PSMS";
            }

            //Create HTTPWebrequest
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
            //Prepare and Add URL Encoded data
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(sbPostData.ToString());
            //Specify post method
            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;
            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            //Get the response
            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseString = reader.ReadToEnd();

            //Close the response
            reader.Close();
            response.Close();
        }
        catch (SystemException ex)
        {
            //MessageBox.Show(ex.Message.ToString());


            // Response.Write(ex.ToString());
        }

    }
}