using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerModel
{
    /// <summary>
    /// 用户表 - 存储系统用户信息
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户ID，主键，自增长
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名，登录账号，唯一
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码（加密后存储），例如用 MD5、SHA256 或 BCrypt 加密
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱，用于找回密码或接收通知
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 状态：1=启用（可以登录），0=禁用（禁止登录）
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 创建时间，默认当前时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
