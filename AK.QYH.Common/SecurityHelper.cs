using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace AK.QYH.Common
{
    /// <summary>
    /// DES、MD5、sha1操作
    /// </summary>
    public static class SecurityHelper
    {

        #region "DES 加密解密"


        #region DES 加密算法
        /// <summary>
        /// DES 加密算法
        /// </summary>
        /// <param name="eKey">加密钥</param>
        /// <param name="data">需要加密的数据</param>
        /// <returns>解密后的数据字符串</returns>
        public static String EncryptDES(String eKey, String data)
        {
            //(1)建立对象
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //(2)把字符串放到byte数组中   
            byte[] bytArr = Encoding.Default.GetBytes(data);
            //(3)建立加密对象的密钥和偏移量  
            des.Key = ASCIIEncoding.ASCII.GetBytes(eKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(eKey);
            //(4)创建存储区为内存的流
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            //(5)以写的模式把数据流和加密的数据流建立连接
            CryptoStream cs = new CryptoStream(memory, des.CreateEncryptor(), CryptoStreamMode.Write);
            //(6)把数据加密写道转换流中 
            cs.Write(bytArr, 0, bytArr.Length);
            //(7)把加密过的数据流写道存储区为内存的数据流中, 并清空缓冲区
            cs.FlushFinalBlock();
            //(8)字节数组的值转换为等效的字符串形式 
            StringBuilder retStr = new StringBuilder();
            foreach (byte b in memory.ToArray())
            {
                retStr.AppendFormat("{0:X2}", b);
            }

            return retStr.ToString();
        }
       
        #endregion

        #region DES 解密算法
        /// <summary>
        /// DES 解密算法
        /// </summary>
        /// <param name="data">需要解密的数据</param>
        /// <param name="dKey">解密钥</param>
        /// <returns>解密后的数据字符串</returns>
        public static String DecryptDES(String dKey ,String data)
        {
            //(1)建立对象
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //(2)把把解密对象放到字符串数组中  
            byte[] bytArr = new byte[data.Length / 2];
            for (int x = 0; x < data.Length / 2; x++)
            {
                int i = (Convert.ToInt32(data.Substring(x * 2, 2), 16));
                bytArr[x] = (byte)i;
            }
            //(3)建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = ASCIIEncoding.ASCII.GetBytes(dKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(dKey);
            //(4)创建存储区为内存的流
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            //(5)以写的模式把数据流和加密的数据流建立连接
            CryptoStream cs = new CryptoStream(memory, des.CreateDecryptor(), CryptoStreamMode.Write);
            //(6)把数据加密写道转换流中  
            cs.Write(bytArr, 0, bytArr.Length);
            //(7)把加密过的数据流写道存储区为内存的数据流中, 并清空缓冲区
            cs.FlushFinalBlock();
            //(8)建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            StringBuilder ret = new StringBuilder();
            //(9)字节数组的值转换为等效的字符串形式

            return Encoding.Default.GetString(memory.ToArray());
        }
       
        #endregion

        #endregion

        #region"MD5加密"

        /// <summary>
        /// MD5 加密静态方法
        /// </summary>
        /// <param name="EncryptString">待加密的密文</param>
        /// <returns>returns</returns>
        public static string EncryptMD5(string strEncrypt)
        {
            try
            {

                if (string.IsNullOrEmpty(strEncrypt))
                {
                    throw (new Exception("密文不得为空"));
                }

                MD5 m_ClassMD5 = new MD5CryptoServiceProvider();
                string m_strEncrypt = "";

                m_strEncrypt = BitConverter.ToString(m_ClassMD5.ComputeHash(Encoding.Default.GetBytes(strEncrypt))).Replace("-", "");

                return m_strEncrypt;
            }

            catch
            {
                return strEncrypt;
            }


        }


        #endregion

        #region sha1加密

        /// <summary>
        /// sha1 加密静态方法
        /// </summary>
        /// <param name="value">源字符串</param>
        /// <returns>returns</returns>
        public static string EncryptSha1(string value)
        {
            try
            {
                byte[] StrRes = Encoding.Default.GetBytes(value);
                HashAlgorithm sha = new SHA1CryptoServiceProvider();
                StrRes = sha.ComputeHash(StrRes);
                StringBuilder EnText = new StringBuilder();
                foreach (byte iByte in StrRes)
                {
                    EnText.AppendFormat("{0:x2}", iByte);
                }
                return EnText.ToString();
            }

            catch
            {
                return "";
            }

        }

        #endregion
    }
}
