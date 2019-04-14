using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using AK.QYH.Common;
using AK.QYH.Controller.WeChat;

namespace AK.QYH.Controller
{
    /// <summary>
    /// 前台请求处理类
    /// </summary>
    public class HTTPModule : IHttpModule, IRequiresSessionState
    {
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 开始请求
        /// </summary>
        /// <param name="httpApp"></param>
        public void Init(HttpApplication httpApp)
        {
            //页面开始请求时，绑定事件
            httpApp.BeginRequest += new EventHandler(context_PreRequestHandlerExecute);
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {

            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            context.Server.ScriptTimeout = 10;

            // 获取请求的路径。eg：~/PreProcess/PreProcessC/GetAllList 去掉前面2位就为PreProcess/PreProcessC/GetAllList。不包含'?'后面的参数
            string requestPath = context.Request.AppRelativeCurrentExecutionFilePath.Remove(0, 2);

            try
            {
                // 判断当前的请求为资源还是服务：若请求的地址包含'.'点就表示资源(图片、页面等)，否则为服务
                if (requestPath.IndexOf('.') == -1)
                {
                    int index = requestPath.LastIndexOf('/');
                    if (index > 0)
                    {
                        string className = requestPath.Substring(0, index).Replace("/", ".");
                        className = string.Format("AK.QYH.Controller.{0}", className);
                        string methodName = requestPath.Substring(index + 1);
                        HTTPHandler handler = new HTTPHandler(className, methodName);
                        HttpContext.Current.RemapHandler(handler); // 指定管道的handler
                    }
                }
                else if (requestPath.ToUpper().EndsWith(".HTML") && requestPath.ToUpper().StartsWith("WECHAT"))
                {
                    // 进行微信身份校验
                   // WeChatBase.Auth(context);
                }
            }
            catch (Exception ex)
            {
                context.Response.AppendHeader("Content-Type", "text/html; charset=utf-8");
                context.Response.Write(string.Format(
                    "<!DOCTYPE htm>" +
                    "<html>" +
                    "<head>" +
                    "    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1,user-scalable=no\" />" +
                    "    <title>发生错误</title>" +
                    "</head>" +
                    "<body>" +
                    "    <span>{0}</span>" +
                    "</body>" +
                    "</html>",ex.Message));
                context.Response.End();
            }
        }
    }
}
