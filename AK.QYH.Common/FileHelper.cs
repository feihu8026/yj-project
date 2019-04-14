using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;

namespace AK.QYH.Common
{
    /// <summary>
    /// 系统文件处理
    /// </summary>
    public class FileHelper
    {
        /*                          Methods                         */



        /// <summary>
        /// 保存微信图片附件
        /// <para>保存目录：UpFiles/WXAttFiles</para>
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <param name="fileName">图片名称，eg:头像；注意不要加后缀</param>
        /// <returns>如果保存成功则返回文件的位置+保存的文件名;如果保存失败则返回空</returns>
        public static string SaveWeChatAttFileOfImage(Image img, string fileName)
        {
            try
            {
                fileName = fileName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff")+".jpg";
                var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UpFiles/WeChatFiles"), fileName);
                // 检测是否存在文件夹，若不存在就建立。
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                ImageCodecInfo icInfo = ImageCodecInfo.GetImageEncoders().Where(o => o.MimeType == "image/jpeg").FirstOrDefault();
                EncoderParameters epts = new EncoderParameters(1); // 压缩图片质量
                EncoderParameter ept = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                epts.Param[0] = ept;
                img.Save(filePath, icInfo, epts);
                return "UpFiles/WeChatFiles/" + fileName;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 删除附件
        /// <para>路径：UpFiles/attfiles</para>
        /// </summary>
        /// <param name="filePath">附件地址</param>
        /// <returns>true：删除成功；false：删除失败</returns>
        public static bool DelAttFile(string filePath)
        {
            try
            {
                #region 1.权限判断，只能删除 UpFiles\AttFiles文件夹内的文件

                if (!filePath.ToLower().StartsWith("upfiles/attfiles"))
                {
                    return false;
                }

                #endregion

                // 2.删除文件
                string locaFilePath = HttpContext.Current.Server.MapPath("~/" + filePath); // 本地地址
                if (File.Exists(locaFilePath))
                {
                    File.Delete(locaFilePath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>单位：KB</returns>
        public int GetFileSize(string filePath)
        {
            try
            {

                // 1.获取文件物理路径
                string locaFilePath = HttpContext.Current.Server.MapPath("~/" + filePath); // 本地地址
                if (File.Exists(locaFilePath))
                {
                    FileInfo fileInfo = new FileInfo(locaFilePath);
                    int kbSize = (int)fileInfo.Length / 1024 + 1; // 向上取整
                    fileInfo = null;
                    return kbSize;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文件短名称
        /// <param name="filePath">文件路径或文件名称</param>
        /// <para>eg:AttFiles/Js01-02-20160307154325204.jpg => Js01-02.jpg</para>
        /// </summary>
        public string GetFileShortName(string filePath)
        {
            try
            {
                // 1.获取文件物理路径
                string fileName = Path.GetFileName(filePath);

                // 2.清除额外的日期格式
                fileName = Regex.Replace(fileName, @"-\d{17}", "");
                return fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageCodecInfo Info { get; set; }
    }
}
