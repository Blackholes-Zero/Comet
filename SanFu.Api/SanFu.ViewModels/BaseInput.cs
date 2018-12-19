using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanFu.ViewModels
{
    public class BaseInput
    {
        /// <summary>
        /// 访问渠道
        /// </summary>
        [Required]
        public virtual string Channel { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        [Required]
        public virtual string RequestTime { get; set; }

        /// <summary>
        /// 随机
        /// </summary>
        [Required]
        public virtual string Nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [Required]
        public virtual string Sign { get; set; }

    }
}
