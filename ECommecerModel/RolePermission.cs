using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerModel
{
    /// <summary>
    /// 角色-权限关联表
    /// 用于建立角色和权限之间的多对多关系
    /// 一个角色可以拥有多个权限，一个权限可以分配给多个角色
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// 角色ID，外键，关联 Role 表的 Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限ID，外键，关联 Permission 表的 Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}
