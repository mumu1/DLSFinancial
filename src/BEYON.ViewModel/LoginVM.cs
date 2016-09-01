﻿/************************************
 * 描述：尚未添加描述
 * 作者：林永恒
 * 日期：2016/5/15 15:16:59  
*************************************/

using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel
{
    public class LoginVM
    {
        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        [Required]
        [Display(Name = "登录账号")]
        public string LoginName { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 是否记住登录
        /// </summary>
        [Display(Name = "记住登录")]
        public bool IsRememberLogin { get; set; }
      }
}
