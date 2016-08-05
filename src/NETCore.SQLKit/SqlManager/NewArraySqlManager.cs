
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
    class NewArraySqlManager : BaseSqlManager<NewArrayExpression>
    {
        protected override SqlBuilder In(NewArrayExpression expression, SqlBuilder sqlBuilder)
        {
            sqlBuilder.SqlInStr += "(";

            foreach (Expression expressionItem in expression.Expressions)
            {
                SqlProvider.In(expressionItem, sqlBuilder);
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
