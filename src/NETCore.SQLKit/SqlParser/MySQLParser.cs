
using System;

namespace NETCore.SQLKit
{
    public class MySQLParser : ISqlParser
    {
        public string ElementLeftPrefix
        {
            get
            {
                return "`";
            }
        }

        public string ElementRightPrefix
        {
            get
            {
                return "`";
            }
        }

        public virtual string ParamPrefix
        {
            get
            {
                return "@";
            }
        }
    }
}
