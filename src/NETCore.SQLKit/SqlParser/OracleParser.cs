
using System;

namespace NETCore.SQLKit
{
    public class OracleParser : ISqlParser
    {
        public string ElementLeftPrefix
        {
            get
            {
                return "";
            }
        }

        public string ElementRightPrefix
        {
            get
            {
                return "";
            }
        }

        public string ParamPrefix
        {
            get
            {
                return ":";
            }
        }
    }
}
