namespace AK.QYH.Model.WeChat
{
    /// <summary>
    /// 微信用户信息实体类
    /// </summary>
    public class WeChatUserInfoEntity
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
        /// 通讯录中的账号
        /// </summary>
        public string userid{get;set;}

        /// <summary>
        /// 通讯录中的名称
        /// </summary>
        public string name{get;set;}

        /// <summary>
        /// 所属部门id列表
        /// </summary>
        public int[] department{get;set;}

        /// <summary>
        /// 职位信息
        /// </summary>
        public string position{get;set;}

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile{get;set;}

        /// <summary>
        /// 性别。
        /// <para>0表示未定义，1表示男性，2表示女性</para>
        /// </summary>
        public string gender{get;set;}

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email{get;set;}

        /// <summary>
        /// 微信号
        /// </summary>
        public string weixinid{get;set;}

        /// <summary>
        /// 头像url
        /// <para>如果要获取小图将url最后的"/0"改成"/64"即可</para>
        /// </summary>
        public string avatar{get;set;}

        /// <summary>
        /// 关注状态
        /// <para>: 1=已关注，2=已禁用，4=未关注</para>
        /// </summary>
         public int status{get;set;}

        // 扩展属性
        // extattr{
    }
}