using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AK.QYH.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 对象转换为Json,
        /// </summary>
        /// <param name="en">对象</param>
        /// <returns>Json</returns>
        public static string GetJson(object en)
        {
            try
            {
                string rs = JsonConvert.SerializeObject(en);
                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对象转换为Json并忽略null值
        /// </summary>
        /// <param name="en">对象</param>
        /// <returns>Json</returns>
        public static string GetJsonOfIgnoreNull(object en)
        {
            try
            {
                string rs = JsonConvert.SerializeObject(
                    en,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                ); // 忽略null值
                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// json转换为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json</param>
        /// <returns>实体类</returns>
        public static T GetEntity<T>(string json)
        {
            try 
	        {	        
		        T en =  JsonConvert.DeserializeObject<T>(json);
                return en;
	        }
	        catch (Exception ex)
	        {
		
		        throw ex;
	        }
        }

        /// <summary>
        /// json转换为匿名对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json</param>
        /// <returns>实体类</returns>
        public static T GetEntity<T>(string json,T type)
        {
            try
            {
                T en = JsonConvert.DeserializeAnonymousType<T>(json, type);
                return en;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
