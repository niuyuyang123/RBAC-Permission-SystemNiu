-- 切换到你的数据库
USE project_3_30
GO
-- 插入角色数据
INSERT INTO Roles (RoleName, Description)
VALUES 
    ('管理员', '拥有所有系统权限，可以管理用户、角色、权限'),
    ('普通用户', '基础权限，只能查看和修改自己的信息'),
    ('审核员', '审核相关权限，可以审核订单、内容等'),
    ('财务', '财务相关权限，可以查看财务报表'),
    ('运维', '系统运维权限，可以查看系统日志');
GO
-- 查看插入结果
SELECT * FROM Roles;

-- 插入权限数据
INSERT INTO Permissions (PermissionName, PermissionCode, ParentId, Type, Path)
VALUES 
    -- 一级菜单
    ('系统管理', 'system', 0, 1, '/system'),
    ('用户管理', 'user', 0, 1, '/user'),
    ('角色管理', 'role', 0, 1, '/role'),
    ('权限管理', 'permission', 0, 1, '/permission'),
    
    -- 用户管理子权限（按钮级）
    ('查看用户', 'user:view', 2, 2, NULL),
    ('添加用户', 'user:add', 2, 2, NULL),
    ('编辑用户', 'user:edit', 2, 2, NULL),
    ('删除用户', 'user:delete', 2, 2, NULL),
    
    -- 角色管理子权限
    ('查看角色', 'role:view', 3, 2, NULL),
    ('添加角色', 'role:add', 3, 2, NULL),
    ('编辑角色', 'role:edit', 3, 2, NULL),
    ('删除角色', 'role:delete', 3, 2, NULL),
    
    -- 权限管理子权限
    ('查看权限', 'permission:view', 4, 2, NULL),
    ('分配权限', 'permission:assign', 4, 2, NULL);
GO
-- 查看插入结果
SELECT * FROM Permissions;

-- 插入用户数据
INSERT INTO Users (UserName, PasswordHash, Email, Status, CreateTime)
VALUES 
    ('admin', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'admin@example.com', 1, GETDATE()),
    ('zhangsan', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'zhangsan@example.com', 1, GETDATE()),
    ('lisi', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'lisi@example.com', 1, GETDATE()),
    ('wangwu', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'wangwu@example.com', 0, GETDATE()),
    ('zhaoliu', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'zhaoliu@example.com', 1, GETDATE());
GO

-- 查看插入结果
SELECT * FROM Users;
GO

-- 给用户分配角色
-- admin (Id=1) 分配管理员角色 (RoleId=1)
INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1);

-- zhangsan (Id=2) 分配普通用户角色 (RoleId=2)
INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 2);

-- lisi (Id=3) 分配审核员角色 (RoleId=3)
INSERT INTO UserRoles (UserId, RoleId) VALUES (3, 3);

-- wangwu (Id=4) 分配财务角色 (RoleId=4)
INSERT INTO UserRoles (UserId, RoleId) VALUES (4, 4);

-- zhaoliu (Id=5) 分配运维角色 (RoleId=5)
INSERT INTO UserRoles (UserId, RoleId) VALUES (5, 5);

-- 也可以给一个用户分配多个角色（例如：zhangsan 同时是普通用户和审核员）
-- INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 3);

-- 查看插入结果
SELECT * FROM UserRoles;
GO

-- 给管理员（RoleId=1）分配所有权限
-- 先查看所有权限的 Id
SELECT Id, PermissionName, PermissionCode FROM Permissions;
GO

-- 给管理员分配所有权限（假设权限 Id 从 1 到 14）
INSERT INTO RolePermissions (RoleId, PermissionId)
VALUES 
    (1, 1), (1, 2), (1, 3), (1, 4),   -- 一级菜单
    (1, 5), (1, 6), (1, 7), (1, 8),   -- 用户管理权限
    (1, 9), (1, 10), (1, 11), (1, 12), -- 角色管理权限
    (1, 13), (1, 14);                  -- 权限管理权限
GO

-- 给普通用户（RoleId=2）分配查看权限
INSERT INTO RolePermissions (RoleId, PermissionId)
VALUES 
    (2, 1),  -- 系统管理菜单
    (2, 2),  -- 用户管理菜单
    (2, 5),  -- 查看用户
    (2, 3),  -- 角色管理菜单
    (2, 9);  -- 查看角色
GO

-- 给审核员（RoleId=3）分配审核相关权限
INSERT INTO RolePermissions (RoleId, PermissionId)
VALUES 
    (2, 1),  -- 系统管理菜单
    (2, 2),  -- 用户管理菜单
    (2, 5),  -- 查看用户
    (2, 3),  -- 角色管理菜单
    (2, 9);  -- 查看角色
GO

-- 查看插入结果
SELECT * FROM RolePermissions;
GO

-- 1. 查看所有用户
SELECT * FROM Users;
GO

-- 2. 查看所有角色
SELECT * FROM Roles;
GO

-- 3. 查看所有权限
SELECT * FROM Permissions;
GO

-- 4. 查看用户及其角色
SELECT u.UserName, r.RoleName 
FROM Users u
JOIN UserRoles ur ON u.Id = ur.UserId
JOIN Roles r ON ur.RoleId = r.Id;
GO

-- 5. 查看角色及其权限
SELECT r.RoleName, p.PermissionName, p.PermissionCode
FROM Roles r
JOIN RolePermissions rp ON r.Id = rp.RoleId
JOIN Permissions p ON rp.PermissionId = p.Id
ORDER BY r.Id, p.Id;
GO
