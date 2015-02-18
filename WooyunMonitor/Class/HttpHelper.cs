using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;

namespace Class.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        private static string DefaultCharset = "utf-8";

        /// <summary>
        /// WebClient.DownloadData下载数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="charset"></param>
        /// <param name="credent"></param>
        /// <returns></returns>
        public  string getHtmlByDownloadData(string url, bool credent, string charset = "utf-8")
        {
            string Charset = charset.Length > 0 ? charset : DefaultCharset;
            try
            {
                WebClient myWebClient = new WebClient();
                byte[] myDataBuffer = myWebClient.DownloadData(url);
                if (credent)
                {
                    myWebClient.Credentials = CredentialCache.DefaultCredentials;
                }
                return Encoding.GetEncoding(Charset).GetString(myDataBuffer);
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        ///// <summary>
        ///// MSXML2.XMLHTTP(unsafe)
        ///// </summary>
        ///// <param name="HttpUrl">要get的url</param>
        ///// <param name="RefererUrl">跳转的referer</param>
        ///// <param name="Charset">字符集,默认utf-8</param>
        ///// <returns></returns>
        //public static string getHtmlByXMLHTTP(string HttpUrl, string RefererUrl, string Chartset = "utf-8")
        //{
        //    string html = string.Empty;
        //    string charset = Chartset.Length > 0 ? Chartset : DefaultCharset;
        //    try
        //    {
        //        MSXML2.XMLHTTP Http = new MSXML2.XMLHTTP();
        //        Http.open("GET", HttpUrl, false, null, null);
        //        Http.setRequestHeader("Referer", RefererUrl);
        //        Http.setRequestHeader("Content-Type", "text/html;charset=" + charset);
        //        Http.send("");
        //        html = Encoding.GetEncoding(charset).GetString((byte[])Http.responseBody);
        //        Http = null;
        //        return html;
        //    }
        //    catch (HttpListenerException ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        /// <summary>
        /// HttpWebRequest(POST)
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="chartset"></param>
        /// <returns></returns>
        public  string getHtmlByHttpGET(string strURL, string charset = "utf-8")
        {
            string Charset = charset.Length > 0 ? charset : DefaultCharset;

            try
            {
                HttpWebRequest req;
                if (strURL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    req = WebRequest.Create(strURL) as HttpWebRequest;
                    //req = HttpHelper.CreateGetHttpResponse(strURL, null);
                    req.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
                }
                else
                {
                    req = WebRequest.Create(strURL) as HttpWebRequest;
                }

                req.ContentType = "text/html;charset=" + Charset;

                //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(Charset)))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (WebException x)
            {
                return x.Message.ToString();
            }
            catch (IOException x)
            {
                return x.Message.ToString();
            }
        }

        /// <summary>
        /// getHtmlByHttpPOST
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="para"></param>
        /// <param name="chartset"></param>
        /// <returns></returns>
        public  string getHtmlByHttpPOST(string strURL, string para, string charset = "utf-8")
        {
            string Charset = charset.Length > 0 ? charset : DefaultCharset;

            try
            {
                HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(strURL);
                //req.Proxy = new System.Net.WebProxy(ProxyString, true);//true means no proxy

                //Add these, as we're doing a POST
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";

                //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(para);
                req.ContentLength = bytes.Length;
                using (System.IO.Stream stream = req.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length); //Push it out there
                    using (System.Net.WebResponse resp = req.GetResponse())
                    {
                        //if (resp == null) return null;
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(Charset)))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException x)
            {
                return x.Message.ToString();
            }
            catch (IOException x)
            {
                return x.Message.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="para"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public  string getHtmlByHttpPOST(string strURL, IDictionary<string, string> para, string charset = "utf-8")
        {
            if (!(para == null || para.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in para.Keys)
                {
                    if (i == 0)
                    {
                        buffer.AppendFormat("{0}={1}", key, para[key]);
                        i++;
                    }
                    else
                    {
                        buffer.AppendFormat("&{0}={1}", key, para[key]);
                    }
                }
                return getHtmlByHttpPOST(strURL, buffer.ToString(), charset);
            }
            else
            {
                return getHtmlByHttpPOST(strURL, string.Empty, charset);
            }
        }


        /// <summary>
        /// 验证证书
        /// </summary>
        private  bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}