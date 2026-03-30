using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerBll.DTOs
{
    /// <summary>
    /// 更新用户信息请求
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// 用户名（可选，如果要修改用户名）
        /// </summary>
        [MinLength(3, ErrorMessage = "用户名至少3个字符")]
        public string? UserName { get; set; }

        /// <summary>
        /// 邮箱（可选）
        /// </summary>
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string? Email { get; set; }
    }
}
