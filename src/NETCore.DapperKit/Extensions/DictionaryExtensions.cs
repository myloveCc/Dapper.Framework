using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.DapperKit
{
    public static class DictionaryExtensions
    {
        public static DynamicParameters ToDynamicParameters(this Dictionary<string, object> dbParas)
        {
            var dapperParams = new DynamicParameters();
            foreach (var paraItem in dbParas)
            {
                dapperParams.Add(paraItem.Key, paraItem.Value);
            }

            return dapperParams;
        }
    }
}
