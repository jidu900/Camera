using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.WebApi.Models
{
    public class ResultMsg
    { 
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// 消息信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据集
        /// </summary>
        public object Data { get; set; }
        
    }
}