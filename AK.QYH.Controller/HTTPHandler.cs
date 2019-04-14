using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AK.QYH.Controller
{
    /// <summary>
    /// 前台请求处理类
    /// </summary>
    public class HTTPHandler : IHttpHandler, IRequiresSessionState
    {
        
        private string typeName;
        private string methodName;
        public HTTPHandler(string TypeName, string MethodName)
        {
            try
            {
                typeName = TypeName;
                methodName = MethodName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var ass = this.GetType().Assembly;
                var instance = ass.CreateInstance(typeName);
                if (instance != null)
                {
                    var tp = instance.GetType();
                    var method = tp.GetMethod(methodName);
                    var queryString = context.Request.QueryString;
                    var rs = method.Invoke(instance, null);
                    // 设置返回的头部
                    context.Response.AppendHeader("charset", "utf-8");
                    context.Response.AppendHeader("defaultCharset", "utf-8");
                    context.Response.AppendHeader("Content-Type", "text/html; charset=utf-8");
                    context.Response.Write(rs.ToString());
                }
            }
            catch(Exception ex)
            {
                context.Response.Write("Server HttpHandler Error："+ex);
            }
        }
    }
}
