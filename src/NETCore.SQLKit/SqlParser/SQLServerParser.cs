
using System;

namespace NETCore.SQLKit
{
    public class SQLServerParser : ISqlParser
    {
        public string ElementLeftPrefix
        {
            get
            {
                return "[";
            }
        }

        public string ElementRightPrefix
        {
            get
            {
                return "]";
            }
        }

        public string ParamPrefix
        {
            get
            {
                return "@";
            }
        }
    }
}
