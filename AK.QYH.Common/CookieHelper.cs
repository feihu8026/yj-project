using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AK.QYH.Common
{
    public static class CookieHelper
    {
        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="key">cookie键</param>
        public static void ClearCookie(string key)
        {
            SetCookie(key, string.Empty, 0.0);
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="key">cookie键</param>
        /// <param name="encrypt">密钥</param>
        public static void ClearCookie(string key, string encrypt)
        {
            SetCookie(key,encrypt,string.Empty, 0.0);
        }

        /// <summary>
        /// 写入加密Cookie
        /// </summary>
        /// <param name="domain">加密密钥</param>
        /// <param name="key">cookie键</param>
        /// <param name="encrypt">密钥</param>
        /// <param name="value">需要写入的值</param>
        /// <param name="expired">保存时间（小时）</param>
        public static void SetCookie(string domain, string key, string encrypt, string value, double expired = 0)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Domain = domain;
            if (expired > 0.0)
            {
                cookie.Expires = DateTime.Now.AddHours(expired);
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }
            cookie.Path = "/";
            cookie.Value = SecurityHelper.EncryptDES(value, encrypt);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="key">DES加密的Key</param>
        /// <param name="encrypt">DES加密的Data</param>
        /// <param name="value">所要加密的Cookie数据</param>
        /// <param name="expired">Cookie过期时间，小时</param>
        public static void SetCookie(string key, string encrypt, string value, double expired = 0)
        {
            HttpCookie cookie = new HttpCookie(SecurityHelper.EncryptDES(key, encrypt));
            cookie.Domain = "";
            if (expired > 0.0)
            {
                cookie.Expires = DateTime.Now.AddHours(expired);
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }
            cookie.Path = "/";
            cookie.Value = SecurityHelper.EncryptDES(value, encrypt);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expired">多少个小时后过期</param>
        public static void SetCookie(string key, string value, double expired = 0)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Domain = "";
            if (expired > 0.0)
            {
                cookie.Expires = DateTime.Now.AddHours(expired);
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }
            cookie.Path = "/";
            cookie.Value = value;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 移除cookie
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveCookie(HttpContext context)
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = context.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = context.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                context.Response.Cookies.Add(aCookie);
            }

        }

        /// <summary>
        /// 移除cookie
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveCookie()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = HttpContext.Current.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }

        /// <summary>
        /// 获取用户cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回值</returns>
        public static string GetCookie(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies[key] != null)
            {
                return HttpContext.Current.Request.Cookies[key].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取解密后的cookie数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="decrypt">解密密钥</param>
        /// <returns>解密后数据</returns>
        public static string GetCookie(string key, string decrypt)
        {
            key = SecurityHelper.EncryptDES(key, decrypt);
            if (HttpContext.Current.Request.Cookies.Get(key) != null)
            {
                string decryPTData = HttpContext.Current.Request.Cookies.Get(key).Value;
                if (!string.IsNullOrEmpty(decryPTData))
                    return SecurityHelper.DecryptDES(decryPTData, decrypt);
            }
            return string.Empty;
        }
    }
}
