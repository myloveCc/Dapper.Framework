using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.SQLKit
{
    /// <summary>
    /// 自增长特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute: Attribute
    {

    }

}
