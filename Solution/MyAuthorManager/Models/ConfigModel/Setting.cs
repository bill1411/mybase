using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAuthorManager.Models.ConfigModel
{
    public class Setting
    {
        /// <summary>
        /// session过期时间（秒）
        /// </summary>
        public string session_timeout { get; set; }
    }
}
