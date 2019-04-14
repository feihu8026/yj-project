namespace AK.QYH.Model.WeChat
{
    /// <summary>
    /// 微信用户code实体类
    /// </summary>
    public class WeChatUserCodeEntity
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode{get;set;}

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg{get;set;}

        /// <summary>
        /// 企业号成员的标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 非企业成员的标识(不存在企业通讯录里)
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 手机设备号
        /// </summary>
        public string DeviceId { get; set; }
    }
}