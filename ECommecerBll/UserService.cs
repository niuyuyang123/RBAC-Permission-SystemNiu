using ECommecerAPI.DTOs;
using ECommecerBll.DTOs;
using ECommecerDal.Repository;
using ECommecerModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace ECommecerBll
{
    /// <summary>
    /// 用户服务 - 处理用户相关的业务逻辑
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region 私有方法

        /// <summary>
        /// 密码加密
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// 用户登录验证（返回完整 User 实体，用于生成 Token）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>验证通过返回 User 实体，否则返回 null</returns>
        public async Task<User?> ValidateUserAsync(string userName, string password)
        {
            // 1. 根据用户名查找用户
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return null;

            // 2. 验证密码
            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword) return null;

            // 3. 检查账号状态（1=启用，0=禁用）
            if (user.Status != 1) return null;

            // 4. 返回完整的用户实体
            return user;
        }

        /// <summary>
        /// 将 User 实体转换为 UserResponseDto
        /// </summary>
        private UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Status = user.Status,
                CreateTime = user.CreateTime
            };
        }

        /// <summary>
        /// 将 User 实体列表转换为 UserResponseDto 列表
        /// </summary>
        private List<UserResponseDto> MapToResponseDtoList(List<User> users)
        {
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Status = u.Status,
                CreateTime = u.CreateTime
            }).ToList();
        }

        #endregion

        #region 用户注册

        /// <summary>
        /// 用户注册
        /// </summary>
        public async Task<UserResponseDto?> RegisterAsync(UserRegisterDto dto)
        {
            // 检查用户名是否已存在
            var exists = await _userRepository.UserNameExistsAsync(dto.UserName);
            if (exists) return null;

            // 创建用户实体
            var user = new User
            {
                UserName = dto.UserName,
                PasswordHash = HashPassword(dto.Password),
                Email = dto.Email,
                Status = 1,
                CreateTime = DateTime.Now
            };

            var newUser = await _userRepository.AddAsync(user);
            return MapToResponseDto(newUser);
        }

        #endregion

        #region 用户登录

        /// <summary>
        /// 用户登录（返回 DTO）
        /// </summary>
        public async Task<UserResponseDto?> LoginAsync(UserLoginDto dto)
        {
            // 根据用户名查找用户
            var user = await _userRepository.GetByUserNameAsync(dto.UserName);
            if (user == null) return null;

            // 验证密码
            var hashedPassword = HashPassword(dto.Password);
            if (user.PasswordHash != hashedPassword) return null;

            // 检查账号状态
            if (user.Status != 1) return null;

            return MapToResponseDto(user);
        }

        #endregion

        #region 查询用户

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAll().ToListAsync();
            return MapToResponseDtoList(users);
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return MapToResponseDto(user);
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        public async Task<UserResponseDto?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return null;
            return MapToResponseDto(user);
        }

        /// <summary>
        /// 获取所有启用状态的用户
        /// </summary>
        public async Task<List<UserResponseDto>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            return MapToResponseDtoList(users);
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        public async Task<bool> UserNameExistsAsync(string userName)
        {
            return await _userRepository.UserNameExistsAsync(userName);
        }

        #endregion

        #region 更新用户

        /// <summary>
        /// 更新用户信息
        /// </summary>
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            // 查找用户
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            // 检查用户名是否被其他用户占用
            if (!string.IsNullOrEmpty(dto.UserName))
            {
                var exists = await _userRepository.UserNameExistsAsync(dto.UserName);
                if (exists && user.UserName != dto.UserName)
                {
                    return false; // 用户名已被其他用户使用
                }
                user.UserName = dto.UserName;
            }

            // 更新邮箱
            if (dto.Email != null)
            {
                user.Email = dto.Email;
            }

            // 保存更改
            await _userRepository.UpdateAsync(user);
            return true;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public async Task<bool> ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            // 查找用户
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            // 验证旧密码
            var hashedOldPassword = HashPassword(oldPassword);
            if (user.PasswordHash != hashedOldPassword) return false;

            // 更新密码
            user.PasswordHash = HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        /// <summary>
        /// 修改用户状态（启用/禁用）
        /// </summary>
        public async Task<bool> ChangeUserStatusAsync(int id, int status)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.Status = status;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        #endregion

        #region 删除用户

        /// <summary>
        /// 删除用户
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        public async Task<int> BatchDeleteUsersAsync(List<int> ids)
        {
            var deletedCount = 0;
            foreach (var id in ids)
            {
                var success = await _userRepository.DeleteByIdAsync(id);
                if (success) deletedCount++;
            }
            return deletedCount;
        }

        #endregion

        #region 统计

        /// <summary>
        /// 获取用户总数
        /// </summary>
        public async Task<int> GetUserCountAsync()
        {
            return await _userRepository.GetAll().CountAsync();
        }

        /// <summary>
        /// 获取启用用户数量
        /// </summary>
        public async Task<int> GetActiveUserCountAsync()
        {
            return await _userRepository.GetAll().CountAsync(u => u.Status == 1);
        }

        #endregion
    }
}

