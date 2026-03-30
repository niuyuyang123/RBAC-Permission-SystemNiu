using ECommecerAPI.DTOs;
using ECommecerBll.DTOs;
using ECommecerBll;
using Microsoft.AspNetCore.Mvc;
using WebApplication1;

namespace ECommecerAPI.Controllers
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;

        public UserController(UserService userService,JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserRegisterDto dto)
        {
            // 参数验证
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(dto);
            if (result == null)
            {
                return BadRequest(new { message = "用户名已存在" });
            }

            return Ok(new { message = "注册成功", data = result });
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(UserLoginDto dto)
        {
            // 参数验证
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.LoginAsync(dto);
            if (user == null)
            {
                return BadRequest(new { message = "用户名或密码错误，或账号已禁用" });
            }

            // TODO: 生成 JWT Token（后续添加）
            var token = "temp_token_123456";

            var response = new LoginResponseDto
            {
                Token = token,
                User = user
            };

            return Ok(new { message = "登录成功", data = response });
        }

        /// <summary>
        /// JWT验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult> LoginToken(UserLoginDto dto)
        {
            var user = await _userService.ValidateUserAsync(dto.UserName, dto.Password);
            if (user == null)
            {
                return BadRequest(new { message = "用户名或密码错误" });
            }

            var token = _jwtHelper.GenerateToken(user);

            return Ok(new
            {
                message = "登录成功",
                token = token,
                user = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.Status
                }
            });
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new { message = "获取成功", data = users });
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "用户不存在" });
            }

            return Ok(new { message = "获取成功", data = user });
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        [HttpGet("username/{userName}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return NotFound(new { message = "用户不存在" });
            }

            return Ok(new { message = "获取成功", data = user });
        }

        /// <summary>
        /// 获取所有启用状态的用户
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<List<UserResponseDto>>> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(new { message = "获取成功", data = users });
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto dto)
        {
            // 参数验证
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result)
            {
                return NotFound(new { message = "用户不存在或用户名已被占用" });
            }

            return Ok(new { message = "更新成功" });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDto dto)
        {
            // 参数验证
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword);
            if (!result)
            {
                return BadRequest(new { message = "旧密码错误或用户不存在" });
            }

            return Ok(new { message = "密码修改成功" });
        }

        /// <summary>
        /// 修改用户状态（启用/禁用）
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeUserStatus(int id, [FromQuery] int status)
        {
            if (status != 0 && status != 1)
            {
                return BadRequest(new { message = "状态值必须是 0（禁用）或 1（启用）" });
            }

            var result = await _userService.ChangeUserStatusAsync(id, status);
            if (!result)
            {
                return NotFound(new { message = "用户不存在" });
            }

            var statusText = status == 1 ? "启用" : "禁用";
            return Ok(new { message = $"用户已{statusText}" });
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound(new { message = "用户不存在" });
            }

            return Ok(new { message = "删除成功" });
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        [HttpDelete("batch")]
        public async Task<IActionResult> BatchDeleteUsers([FromBody] BatchDeleteDto dto)
        {
            if (dto.Ids == null || dto.Ids.Count == 0)
            {
                return BadRequest(new { message = "请提供要删除的用户ID列表" });
            }

            var deletedCount = await _userService.BatchDeleteUsersAsync(dto.Ids);
            return Ok(new { message = $"成功删除 {deletedCount} 个用户" });
        }

        /// <summary>
        /// 获取用户总数
        /// </summary>
        [HttpGet("count/total")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var count = await _userService.GetUserCountAsync();
            return Ok(new { message = "获取成功", data = count });
        }

        /// <summary>
        /// 获取启用用户数量
        /// </summary>
        [HttpGet("count/active")]
        public async Task<ActionResult<int>> GetActiveUserCount()
        {
            var count = await _userService.GetActiveUserCountAsync();
            return Ok(new { message = "获取成功", data = count });
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        [HttpGet("check/{userName}")]
        public async Task<ActionResult<bool>> CheckUserNameExists(string userName)
        {
            var exists = await _userService.UserNameExistsAsync(userName);
            return Ok(new { message = "检查完成", data = exists });
        }
    }
}
