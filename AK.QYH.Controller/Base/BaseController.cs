using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using AK.QYH.Common;

/*!
* 标题：Controllers.cs
* 说明：Controllers基础类
* 功能：存放Control的公共方法
*/

namespace AK.QYH.Controller.Base
{
    /// <summary>
    /// Controllers基础类
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// Request对象
        /// </summary>
        protected HttpRequest Request
        {
            get{ return HttpContext.Current.Request;}
        }
        
        /// <summary>
        /// 返回正确信息
        /// <para>{ success = true}</para>
        /// </summary>
        protected string SuccessResult
        {
            get
            {
                var rsObj = new { success = true };
                return JsonHelper.GetJson(rsObj);
            }
        }

        /// <summary>
        /// 返回正确信息
        /// <para>{ success = true, msg = msgInfo }</para>
        /// </summary>
        /// <param name="msgInfo">信息，展示给前台的信息</param>
        /// <returns>前台可以根据action.result.msg获取提示信息</returns>
        protected string SuccessResultMsg(string msgInfo)
        {
            var rsObj = new { success = true, msg = msgInfo };
            return JsonHelper.GetJson(rsObj);
        }

        /// <summary>
        /// 返回正确信息，包含：消息、数据对象
        /// <para>{ success = true, rowCount = 0, msg = "", data = dataObj }</para>
        /// </summary>
        /// <param name="dataObj">数据对象，如List集合</param>
        /// <returns>前台可以根据action.result.msg获取提示信息</returns>
        protected string SuccessResultData(object dataObj)
        {
            var rsObj = new { success = true, rowCount = 0, msg = "", data = dataObj };
            return JsonHelper.GetJson(rsObj);
        }

        /// <summary>
        /// 返回正确信息，包含：消息、数据对象
        /// </summary>
        /// <param name="msgInfo">信息</param>
        /// <param name="dataObj">数据对象，如List集合</param>
        /// <returns>前台可以根据action.result.msg获取提示信息</returns>
        protected string SuccessResultData(string msgInfo,object dataObj)
        {
            var rsObj = new { success = true, rowCount=0,msg = msgInfo, data = dataObj };
            return JsonHelper.GetJson(rsObj);
        }

        /// <summary>
        /// 返回分页对象信息，包含：行数、分页数据对象
        /// <para>{ success = true, rowCount = rowCount, data = dataObj }</para>
        /// </summary>
        /// <param name="rowCount">查询行的总数</param>
        /// <param name="dataObj">分页数据对象，如List集合</param>
        /// <returns>前台可使用Grid的分页查询</returns>
        protected string SuccessResultPageData(int rowCount, object dataObj)
        {
            var rsObj = new { success = true, rowCount = rowCount, data = dataObj };
            return JsonHelper.GetJson(rsObj);
        }

        /// <summary>
        /// 返回错误错误信息
        /// </summary>
        /// <param name="msgInfo">错误信息</param>
        /// <returns>前台可以根据action.result.msg获取错误信息</returns>
        protected string FailureResultMsg(string msgInfo)
        {
            var rsObj = new { success = false, msg = msgInfo };
            return JsonHelper.GetJson(rsObj);
        }

    }
}
