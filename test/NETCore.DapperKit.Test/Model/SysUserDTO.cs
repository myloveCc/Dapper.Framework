using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.DapperKit.Test
{
    public class SysUserDTO
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// 用户角色编号
        /// </summary>
        public string UserRoleNo { get; set; }

        /// <summary>
        /// 用户角色名称
        /// </summary>
        public string UserRoleName { get; set; }
    }
}
