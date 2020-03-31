using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace WebApi.Models
{
    public class AndroidGCMPushNotification
    {
        public AndroidGCMPushNotification()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region In Use

        #region For User & Doctor
        //post = 1 for User,2 For Doctor, New Appointment creation
        public string SendNotificationJSON(string deviceId, string title, string message, string click_action, string post,string extra)
        {
            string SERVER_KEY_TOKEN = "AIzaSyBtrCaYEXmDCIzGaiYW7Owv3wGvR6POgh8";
            var SENDER_ID = "1058996348936";

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY_TOKEN));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            var a = new
            {
                data = new 
                {
                  title,
                  message,
                  image = "",
                  timestamp = (Convert.ToDateTime(DateTime.Now)).ToString("yyyy-MM-dd HH:mm:ss"),
                  post,
                  extra
                },

                to = deviceId
            };

            
            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(a));
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);
            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();

            return sResponseFromServer;
        }
        #endregion

        #region For Employee
        public string SendNotificationJSONForEmployee(string deviceId, string title, string message, string click_action, string post, string extra)
        {
            string SERVER_KEY_TOKEN = "AIzaSyC-5W4g_9GJQvdbGjzDakiPBGTLHV92hK8";
            var SENDER_ID = "476307135331";

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY_TOKEN));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            var a = new
            {
                data = new
                {
                    title,
                    message,
                    image = "",
                    timestamp = (Convert.ToDateTime(DateTime.Now)).ToString("yyyy-MM-dd HH:mm:ss"),
                    post,
                    extra
                },

                to = deviceId
            };


            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(a));
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);
            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();

            return sResponseFromServer;
        }
        #endregion
        #endregion


        #region Not in Use
        public string SendMessage()
        {
            string serverKey = "AIzaSyBtrCaYEXmDCIzGaiYW7Owv3wGvR6POgh8";
            var result = "-1";
            try
            {
               
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"c1Dyldn1fzU:APA91bEI8aQP-jpoxZl0mM_3IeuAa6ourJe0AFrGHFpkI8IQAtA0WnMdvbhM7v2c1rZih9VXFyCGgtkvXqSs4jBZny3ia441PJ6zpfFdmI70-jdglXvcoRiSFOmagz_0b_HTKs_I3HI_\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }


            }
            catch (Exception ex)
            {
                //  Response.Write(ex.Message);
            }
            return result;
        }



        public string PushNotification()
        {
            string str = "";
            try
            {
                var applicationID = "AIzaSyBtrCaYEXmDCIzGaiYW7Owv3wGvR6POgh8";

                var senderId = "1058996348936";

                string deviceId = "c1Dyldn1fzU:APA91bEI8aQP-jpoxZl0mM_3IeuAa6ourJe0AFrGHFpkI8IQAtA0WnMdvbhM7v2c1rZih9VXFyCGgtkvXqSs4jBZny3ia441PJ6zpfFdmI70-jdglXvcoRiSFOmagz_0b_HTKs_I3HI_";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                tRequest.Method = "post";

                tRequest.ContentType = "application/json";

                var data = new

                {

                    to = deviceId,

                    notification = new

                    {

                        body = "Test Messsage",

                        title = "Notification",

                        icon = "myicon"

                    }
                };

                var serializer = new JavaScriptSerializer();

                var json = serializer.Serialize(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                tRequest.ContentLength = byteArray.Length;


                using (Stream dataStream = tRequest.GetRequestStream())
                {

                    dataStream.Write(byteArray, 0, byteArray.Length);


                    using (WebResponse tResponse = tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {

                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {

                                String sResponseFromServer = tReader.ReadToEnd();

                                str = sResponseFromServer;

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                str = ex.Message;

            }
            return str;
        }
        #endregion



    }
}