using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPP.Models
{
    public class User_Info
    {/// <summary>
     /// 用户名
     /// </summary>
     /// 

        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWrod { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public string IsAdmin { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public string IsAction { get; set; }
        /// <summary>
        /// 用户QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 用户名拼写
        /// </summary>
        public string Spell { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        public string Address { get; set; }
        public string job { get; set; }
        public DateTime Bartime { get; set; }
        public string Sex { get; set; }
        public string Input { get; set; }

    }
}