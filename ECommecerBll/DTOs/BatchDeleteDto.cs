using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommecerBll.DTOs
{
    /// <summary>
    /// 批量删除请求
    /// </summary>
    public class BatchDeleteDto
    {
        [Required(ErrorMessage = "用户ID列表不能为空")]
        public List<int> Ids { get; set; } = new List<int>();
    }
}
