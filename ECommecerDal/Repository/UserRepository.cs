using ECommecerModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerDal.Repository
{
    /// <summary>
    /// 用户仓储 - 用户相关的数据访问操作
    /// </summary>
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(AppDbContext db) : base(db)
        {
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName">用户名</param>
        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        public async Task<bool> UserNameExistsAsync(string userName)
        {
            return await _dbSet.AnyAsync(u => u.UserName == userName);
        }

        /// <summary>
        /// 获取启用状态的所有用户
        /// </summary>
        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.Status == 1).ToListAsync();
        }
    }
}
