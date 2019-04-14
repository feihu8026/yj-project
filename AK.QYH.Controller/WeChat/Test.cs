using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AK.QYH.Common;

namespace AK.QYH.Controller.WeChat
{
    public class Test
    {
        /// <summary>
        /// 添加操作
        /// </summary>
        /// <returns>json</returns>
        public string GetUserInfo()
        {
            try
            {
                return WeChatBase.GetUserInfo();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
