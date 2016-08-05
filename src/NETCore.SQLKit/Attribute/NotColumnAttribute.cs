using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.SQLKit
{
    /// <summary>
    /// 非数据库列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotColumnAttribute : Attribute
    {
    }
}
