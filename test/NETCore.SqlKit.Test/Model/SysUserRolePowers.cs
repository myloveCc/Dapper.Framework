using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.SqlKit.Test
{
    [Table("SysUserRolePowers")]
    public class SysUserRolePowers
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public string RoleNo { get; set; }

        /// <summary>
        /// 权限编号
        /// </summary>
        public string PowerNo { get; set; }
    }
}
