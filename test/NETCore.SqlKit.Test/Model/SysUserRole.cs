using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace NETCore.SqlKit.Test
{
    [Table("SysUserRole")]
    public class SysUserRole
    {
        /// <summary>
        /// 角色Id，自增Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色编号，已SUR开头
        /// </summary>
        [Key]
        public string No { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
