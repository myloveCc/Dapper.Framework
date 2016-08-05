
using System;
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
    class ConstantSqlManager : BaseSqlManager<ConstantExpression>
    {
        protected override SqlBuilder Join(ConstantExpression expression, SqlBuilder sqlBuilder)
        {
            var dbParamName = sqlBuilder.AddDbParameter(expression.Value);
            sqlBuilder.SqlJoinStr += $" {dbParamName}";
            return sqlBuilder;
        }

        protected override SqlBuilder Where(ConstantExpression expression, SqlBuilder sqlBuilder)
        {
            var dbParamName = sqlBuilder.AddDbParameter(expression.Value);
            sqlBuilder.SqlWhereStr += $" {dbParamName}";
            return sqlBuilder;
        }

        protected override SqlBuilder In(ConstantExpression expression, SqlBuilder sqlBuilder)
        {
            if (expression.Type.Name == "String")
            {
                sqlBuilder.SqlInStr += $" '{expression.Value}' ,";
            }
            else
            {
                sqlBuilder.SqlInStr += $" {expression.Value} ,";
            }
            return sqlBuilder;
        }
    }
}