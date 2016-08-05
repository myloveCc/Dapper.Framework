using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.SQLKit
{
    /// <summary>
    /// sql命令类型
    /// </summary>
    public enum SqlCommandType
    {
        //新增
        Insert=1,
        //删除
        Delete,
        //更新
        Update,
        //查询
        Select,
        //计算
        Calculate
    }
}
