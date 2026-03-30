using System.ComponentModel.DataAnnotations;

namespace ECommecerAPI.DTOs
{
    /// <summary>
    /// 用户注册请求
    /// </summary>
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [MinLength(3, ErrorMessage = "用户名至少3个字符")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码至少6个字符")]
        public string Password { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string? Email { get; set; }
    }

    /// <summary>
    /// 用户登录请求
    /// </summary>
    public class UserLoginDto
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用户信息响应
    /// </summary>
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 登录成功响应
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDto User { get; set; } = new UserResponseDto();
    }

    /// <summary>
    /// 修改密码请求
    /// </summary>
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "新密码不能为空")]
        [MinLength(6, ErrorMessage = "新密码至少6个字符")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
