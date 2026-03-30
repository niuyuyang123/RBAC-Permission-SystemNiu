using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerModel
{
    /// <summary>
    /// 用户-角色关联表
    /// 用于建立用户和角色之间的多对多关系
    /// 一个用户可以拥有多个角色，一个角色可以属于多个用户
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// 用户ID，外键，关联 User 表的 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 角色ID，外键，关联 Role 表的 Id
        /// </summary>
        public int RoleId { get; set; }
    }
}
