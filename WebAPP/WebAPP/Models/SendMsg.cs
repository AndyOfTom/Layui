using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPP.Models
{
    public class SendMsg
    {
        public class Result
        {
            /// <summary>
            /// 
            /// </summary>
            public string sid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int fee { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int count { get; set; }
        }

      
            /// <summary>
            /// 操作成功
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Result result { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int error_code { get; set; }
        
    }
}