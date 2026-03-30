using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerModel
{
    /// <summary>
    /// 角色表 - 存储系统角色信息
    /// 例如：管理员、普通用户、审核员等
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 角色ID，主键，自增长
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称，如：管理员、普通用户、VIP会员
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// 角色描述，如：拥有所有系统权限
        /// </summary>
        public string? Description { get; set; }
    }
}
