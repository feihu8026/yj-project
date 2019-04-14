using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using AK.QYH.Common;
using AK.QYH.Model.WeChat;

namespace AK.QYH.Controller.WeChat
{
    /// <summary>
    /// 微信操作
    /// </summary>
    public class WeChatBase
    {
        private WeChatBase() { }

        static string CORPID;
        static string SECRET;

        /// <summary>
        /// .Ctor
        /// </summary>
        static WeChatBase()
        {
            CORPID = ConfigurationManager.AppSettings["CorpID"];
            SECRET = ConfigurationManager.AppSettings["Secret"];
        }

        #region 微信企业号配置信息

        public static string GetCorpId()
        {
            return CORPID;
        }



        #endregion

        #region GetAccessToken：获取微信访问凭证

        /// <summary>
        /// ACCESS_TOKEN最后一次更新时间
        /// </summary>
        static DateTime _lastGetTimeOfAccessToken = DateTime.Now.AddSeconds(-7201);

        /// <summary>
        /// 微信访问凭证
        /// </summary>
        static string _AccessToken;

        /// <summary>
        /// 微信访问凭证
        /// </summary>
        public static string GetAccessToken()
        {
            try
            {
                if (_lastGetTimeOfAccessToken < DateTime.Now)
                {

                    // 每隔7200秒刷新一次 ACCESS_TOKEN
                    string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", CORPID, SECRET);
                    string responseText = HttpHelper.Instance.get(url);
                    /*
                        API：http://qydev.weixin.qq.com/wiki/index.php?title=%E4%B8%BB%E5%8A%A8%E8%B0%83%E7%94%A8#.E8.8E.B7.E5.8F.96AccessToken
                        正确的Json返回示例:
                        {
                           "access_token": "accesstoken000001",
                           "expires_in": 7200
                        }
                        错误的Json返回示例:
                        {
                           "errcode": 43003,
                           "errmsg": "require https"
                        }
                    */
                    var rsEntity = new { access_token = "", expires_in = 0, errcode = 0, errmsg = "" };
                    dynamic en = JsonHelper.GetEntity<object>(responseText, rsEntity);
                    _lastGetTimeOfAccessToken = DateTime.Now.AddSeconds((double)en.expires_in - 1);
                    _AccessToken = en.access_token;
                }
                return _AccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetJSapiTicket：获取JSSDK的访问凭证

        /// <summary>
        /// JSAPI_TICKET最后一次更新时间
        /// </summary>
        static DateTime LastGetTimeOfJSapiTicket = DateTime.Now.AddSeconds(-7201);

        /// <summary>
        /// JSAPI的访问凭证
        /// </summary>
        static string _jsapiTicket;

        /// <summary>
        /// 微信访问凭证
        /// </summary>
        public static string GetJSapiTicket()
        {
            try
            {
                if (LastGetTimeOfJSapiTicket < DateTime.Now)
                {
                    // 每隔7200秒刷新一次 JSAPI_TICKET
                    string url = "https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token=" + GetAccessToken();
                    string responseText = HttpHelper.Instance.get(url);
                    /*
                        API：http://qydev.weixin.qq.com/wiki/index.php?title=%E5%BE%AE%E4%BF%A1JS-SDK%E6%8E%A5%E5%8F%A3#.E9.99.84.E5.BD.951-JS-SDK.E4.BD.BF.E7.94.A8.E6.9D.83.E9.99.90.E7.AD.BE.E5.90.8D.E7.AE.97.E6.B3.95
                        正确的Json返回示例:
                        {
                            "errcode":0,
                            "errmsg":"ok",
                            "ticket":"bxLdikRXVbTPdHSM05e5u5sUoXNKd8-41ZO3MhKoyN5OfkWITDGgnr2fwJ0m9E8NYzWKVZvdVtaUgWvsdshFKA",
                            "expires_in":7200
                        }
                        错误的Json返回示例:
                        {
                           "errcode": 43003,
                           "errmsg": "require https"
                        }
                    */
                    var rsEntity = new { ticket = "", expires_in = 0, errcode = 0, errmsg = "" };
                    dynamic en = JsonHelper.GetEntity<object>(responseText, rsEntity);
                    _lastGetTimeOfAccessToken = DateTime.Now.AddSeconds((double)en.expires_in - 1);
                    _jsapiTicket = en.ticket;
                }
                return _jsapiTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 验证微信访问
        /// </summary>
        public static void Auth(HttpContext webContext)
        {

            string requestURL = webContext.Request.Url.AbsoluteUri;

            try
            {
                // 用户访问微信页面有3种情况：
                // 1.第一次访问，没code
                // 2.有code，没cookie；
                // 3.有code，有cookie

                // 1.第一次访问，没code，没cookie：跳转到Oauth2.0认证
                if (string.IsNullOrEmpty(webContext.Request["code"]))
                {
                    string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect", CORPID, webContext.Server.UrlEncode(requestURL));
                    webContext.Response.Redirect(url, false);
                }
                else if (!string.IsNullOrEmpty(webContext.Request["code"]) && string.IsNullOrEmpty(CookieHelper.GetCookie("WXToken")))
                {
                    // 2.有code，没cookie：根据code获取userID
                    string code = webContext.Request["code"];
                    string userId = "";
                    string userInfo = "";

                    #region 1)根据code获取userId

                    string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", GetAccessToken(), code);
                    string responseText = HttpHelper.Instance.get(url);
                    /*
                        API：http://qydev.weixin.qq.com/wiki/index.php?title=OAuth%E9%AA%8C%E8%AF%81%E6%8E%A5%E5%8F%A3#.E6.A0.B9.E6.8D.AEcode.E8.8E.B7.E5.8F.96.E6.88.90.E5.91.98.E4.BF.A1.E6.81.AF
                        正确的Json返回示例:
                        {
                           "UserId":"USERID",
                           "DeviceId":"DEVICEID"
                        }
                        未关注企业号时返回：
                        {
                           "OpenId":"OPENID",
                           "DeviceId":"DEVICEID"
                        }
                        错误的Json返回示例:
                        {
                           "errcode": "40029",
                           "errmsg": "invalid code"
                        }
                    */
                    WeChatUserCodeEntity codeEn = JsonHelper.GetEntity<WeChatUserCodeEntity>(responseText);
                    if (codeEn.errcode > 0)
                    {
                        throw new Exception(codeEn.errmsg);
                    }
                    else if (string.IsNullOrEmpty(codeEn.UserId))
                    {
                        throw new Exception("请先关注企业号!");
                    }
                    userId = codeEn.UserId;


                    #endregion

                    #region 2)根据userId获取用户信息

                    url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}", GetAccessToken(), userId);
                    responseText = HttpHelper.Instance.get(url);
                    /*
                        API：http://qydev.weixin.qq.com/wiki/index.php?title=%E7%AE%A1%E7%90%86%E6%88%90%E5%91%98#.E8.8E.B7.E5.8F.96.E6.88.90.E5.91.98
                        正确的Json返回示例:
                        {
                           "errcode": 0,
                           "errmsg": "ok",
                           "userid": "zhangsan",
                           "name": "李四",
                           "department": [1, 2],
                           "position": "后台工程师",
                           "mobile": "15913215421",
                           "gender": "1",
                           "email": "zhangsan@gzdev.com",
                           "weixinid": "lisifordev",  
                           "avatar": "http://wx.qlogo.cn/mmopen/ajNVdqHZLLA3WJ6DSZUfiakYe37PKnQhBIeOQBO4czqrnZDS79FH5Wm5m4X69TBicnHFlhiafvDwklOpZeXYQQ2icg/0",
                           "status": 1,
                           "extattr": {"attrs":[{"name":"爱好","value":"旅游"},{"name":"卡号","value":"1234567234"}]}
                        }
                        错误的Json返回示例:
                        {
                           "errcode": "40029",
                           "errmsg": "invalid code"
                        }
                    */
                    WeChatUserInfoEntity userInfoEn = JsonHelper.GetEntity<WeChatUserInfoEntity>(responseText);
                    if (userInfoEn.errcode > 0)
                    {
                        throw new Exception(userInfoEn.errmsg);
                    }
                    userInfo = responseText;

                    #endregion

                    // 3.把userInfo传入到cookie里
                    CookieHelper.SetCookie("WXToken", userInfo, -1);
                }
                else if (!string.IsNullOrEmpty(webContext.Request["code"]) && !string.IsNullOrEmpty(CookieHelper.GetCookie("WXToken")))
                {
                    #region 3.有code，有cookie：校验cookie
                    // TODO：在上面进行存入cookie时可采用AES加密，在这部进行解密校验
                    // CookieHelper.SetCookie("WXToken", "", -1);
                    #endregion
                }
                else
                {
                    throw new Exception("非授权访问！");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从微信服务器获取图片
        /// </summary>
        /// <param name="imgServerId">图片服务器存储id</param>
        /// <returns>图片</returns>
        public static Image GetImage(string imgServerId)
        {
            string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", GetAccessToken(), imgServerId);
            try
            {
                // 1.创建httpWebRequest对象
                WebRequest webRequest = WebRequest.Create(url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;

                // 2.填充httpWebRequest的基本信息
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)"; ;
                httpRequest.ContentType = "application/x-www-form-urlencoded"; ;
                httpRequest.Method = "get";

                Stream responseStream = httpRequest.GetResponse().GetResponseStream();
                Image img = Image.FromStream(responseStream);
                responseStream.Close();
                return img;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从Cookie里获取userInfoObj的字符串
        /// </summary>
        /// <returns>微信的用户个人字符串</returns>
        public static string GetUserInfo()
        {
            try
            {
                string value = CookieHelper.GetCookie("WXToken");
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
