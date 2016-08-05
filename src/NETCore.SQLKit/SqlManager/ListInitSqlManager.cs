using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NETCore.SQLKit
{
    public class ListInitSqlManager : BaseSqlManager<ListInitExpression>
    {
        protected override SqlBuilder In(ListInitExpression expression, SqlBuilder sqlBuilder)
        {
            sqlBuilder.SqlInStr += "(";

            foreach (var expressionInit in expression.Initializers)
            {
                foreach (var expressionItem in expressionInit.Arguments)
                {
                    SqlProvider.In(expressionItem, sqlBuilder);
                }
            }

            if (sqlBuilder.SqlInStr.EndsWith(","))
            {
                sqlBuilder.SqlInStr = sqlBuilder.SqlInStr.Substring(0, sqlBuilder.SqlInStr.Length - 1);
            }

            sqlBuilder.SqlInStr += ")";

            //将SqlInStr追加到SqlWhereStr
            sqlBuilder.SqlWhereStr += sqlBuilder.SqlInStr;
            //清空SqlInStr
            sqlBuilder.SqlInStr = string.Empty;

            return sqlBuilder;
        }
    }
}
