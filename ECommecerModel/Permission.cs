using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerModel
{
    /// <summary>
    /// 权限表 - 存储系统中的所有权限点
    /// 权限可以是菜单、按钮或API接口
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 权限ID，主键，自增长
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 权限名称，显示用，如：用户管理、添加用户
        /// </summary>
        public string PermissionName { get; set; } = string.Empty;

        /// <summary>
        /// 权限代码，程序判断用，如：user:add、user:delete、user:list
        /// 格式建议：模块:操作
        /// </summary>
        public string PermissionCode { get; set; } = string.Empty;

        /// <summary>
        /// 父级权限ID，用于构建树形菜单
        /// 0 表示顶级菜单/权限
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// 权限类型：1=菜单（显示在左侧导航栏），2=按钮/操作（页面内的功能按钮）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 前端路由路径，仅当 Type=1（菜单）时有效
        /// 例如：/user/list、/role/index
        /// </summary>
        public string? Path { get; set; }
    }
}
