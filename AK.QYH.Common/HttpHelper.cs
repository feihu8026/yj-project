using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace AK.QYH.Common
{
    /// <summary>
    /// HTTPWeb操作帮助类
    /// </summary>
    public class HttpHelper
    {
        const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        const string sContentType = "application/x-www-form-urlencoded";
        const string sResponseEncoding = "utf-8";

        private HttpHelper() { }

        private static HttpHelper _instance;

        /// <summary>
        /// 单一实例
        /// </summary>
        public static HttpHelper Instance
        {
            get{
                if (_instance == null) {
                    _instance = new HttpHelper();
                }
                return _instance;
            }
        }


        /*                          .Ctor                           */

        /// <summary>
        /// 发起请求
        /// </summary>
        /// <param name="httpRequest">HttpWebRequest对象</param>
        /// <returns>返回本次请求的结果</returns>
        public string SendHttpWebRequest(HttpWebRequest httpRequest)
        {
            Stream responseStream;
            string rs = "";
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
                using (StreamReader responseReader = new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
                {
                    rs = responseReader.ReadToEnd();
                }
                responseStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return rs;
        }

        /// <summary>
        /// 发起请求(get)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>返回本次请求的结果</returns>
        public string get(string url)
        {
            string rs = "";
            try
            {
                // 1.创建httpWebRequest对象
                WebRequest webRequest = WebRequest.Create(url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;

                // 2.填充httpWebRequest的基本信息
                httpRequest.UserAgent = sUserAgent;
                httpRequest.ContentType = sContentType;
                httpRequest.Method = "get";
                //httpRequest.CookieContainer = cc; //把接收到的包一起发送

                Stream responseStream = httpRequest.GetResponse().GetResponseStream();
                using (StreamReader responseReader = new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
                {
                    rs = responseReader.ReadToEnd();
                }
                responseStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return rs;
        }

        /// <summary>
        /// 发起请求(post)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数</param>
        /// <returns>返回本次请求的结果</returns>
        public string post(string url, string param)
        {
            string rs = "";
            try
            {
                // 1.创建httpWebRequest对象
                WebRequest webRequest = WebRequest.Create(url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;

                // 2.填充httpWebRequest的基本信息
                httpRequest.UserAgent = sUserAgent;
                httpRequest.ContentType = sContentType;
                httpRequest.Method = "post";
                //httpRequest.CookieContainer = cc; //把接收到的包一起发送

                // 3.Post参数
                Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] bytesToPost = encoding.GetBytes(param);

                // 4.填充post内容
                httpRequest.ContentLength = bytesToPost.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytesToPost, 0, bytesToPost.Length);
                requestStream.Close();


                Stream responseStream = httpRequest.GetResponse().GetResponseStream();
                using (StreamReader responseReader = new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
                {
                    rs = responseReader.ReadToEnd();
                }
                responseStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return rs;
        }
    }
}
