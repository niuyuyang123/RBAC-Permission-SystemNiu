using ECommecerModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerDal
{
    /// <summary>
    /// 数据库上下文 - 负责连接数据库、管理实体映射
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// 构造函数，接收数据库连接配置
        /// </summary>
        /// <param name="options">数据库配置选项</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// 权限表
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// 用户-角色关联表
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// 角色-权限关联表
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// 配置数据库映射关系
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== 配置 UserRole 表（用户-角色）==========
            // 设置联合主键：UserId + RoleId
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // ========== 配置 RolePermission 表（角色-权限）==========
            // 设置联合主键：RoleId + PermissionId
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            // ========== 可选：设置默认数据（方便测试）==========
            // 添加一个默认管理员角色
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "管理员", Description = "拥有所有权限" }
            );

            // 添加一个默认普通用户角色
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 2, RoleName = "普通用户", Description = "基础权限" }
            );
        }
    }
}
