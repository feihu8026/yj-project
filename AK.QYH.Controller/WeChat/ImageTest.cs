using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AK.QYH.Common;
using AK.QYH.Controller.Base;
using System.Drawing;

namespace AK.QYH.Controller.WeChat
{
    /// <summary>
    /// 微信图片相关操作
    /// </summary>
    public class ImageTest : BaseController
    {
        /// <summary>
        /// 获取图片操作权限
        /// </summary>
        public string GetImageJsSdk()
        {
            try
            {
                /*
                 参照文档：http://qydev.weixin.qq.com/wiki/index.php?title=%E5%BE%AE%E4%BF%A1JS-SDK%E6%8E%A5%E5%8F%A3
                 wx.config({
                    debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: '', // 必填，企业号的唯一标识，此处填写企业号corpid
                    timestamp: , // 必填，生成签名的时间戳
                    nonceStr: '', // 必填，生成签名的随机串
                    signature: '',// 必填，签名，见附录1
                    jsApiList: [] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });
                */
                // 1.设置所需的值
                string jsapi_ticket = WeChatBase.GetJSapiTicket(); // jsapi凭证
                string appId = WeChatBase.GetCorpId(); // 企业ID
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                long timestamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 时间戳
                string nonceStr = startTime.ToShortTimeString(); // 随机字符串
                string signature = SecurityHelper.EncryptSha1(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, nonceStr, timestamp, Request["url"]));

                // 2.返回wxconfig对象
                var en = new
                {
                    debug = false,
                    appId = appId,
                    timestamp = timestamp,
                    nonceStr = nonceStr,
                    signature = signature,
                    jsApiList = new List<string>() { "chooseImage", "previewImage", "uploadImage", "downloadImage" },
                };
                return SuccessResultData(en);
            }
            catch (Exception ex)
            {
                return FailureResultMsg(ex.Message);
            }
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <returns></returns>
        public string AddImage()
        {
            List<string> rsFilePathList = new List<string>();
            try
            {
                string imgServerIds = Request["imgServerIds"]; // 微信服务器图片Id
                List<string> imgServerIdList = imgServerIds.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (string imgServerId in imgServerIdList)
                {
                    // 1)获取图片
                    Image img = WeChatBase.GetImage(imgServerId);
                    // 2)存放本地
                    string imageFilePath = FileHelper.SaveWeChatAttFileOfImage(img, "WeChat");
                    rsFilePathList.Add(imageFilePath);
                }
                return SuccessResultData(rsFilePathList);
            }
            catch (Exception ex)
            {
                return FailureResultMsg(ex.Message) ;
            }
        }
    }
}
