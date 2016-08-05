
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
    class UnarySqlManager : BaseSqlManager<UnaryExpression>
    {
        protected override SqlBuilder Select(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Select(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Join(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            //解析x => !x.isDeletion这样的
            if (expression.NodeType == ExpressionType.Not && expression.Operand is MemberExpression && expression.Type == typeof(bool))
            {
                var dbParamName = sqlBuilder.AddDbParameter(0);

                var memberExpression = expression.Operand as MemberExpression;
                //添加到关联条件
                var tableName = memberExpression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
                sqlBuilder.SetTableAlias(tableName);
                string tableAlias = sqlBuilder.GetTableAlias(tableName);
                if (!string.IsNullOrWhiteSpace(tableAlias))
                {
                    tableAlias += ".";
                }
                sqlBuilder.SqlJoinStr += " " + tableAlias + memberExpression.Member.Name;
                sqlBuilder.SqlJoinStr += " = ";
                sqlBuilder.SqlJoinStr += $"{dbParamName}";
            }
            else
            {
                SqlProvider.Join(expression.Operand, sqlBuilder);
            }
            return sqlBuilder;
        }

        protected override SqlBuilder Where(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            //解析x => !x.isDeletion这样的
            if (expression.NodeType == ExpressionType.Not && expression.Operand is MemberExpression && expression.Type == typeof(bool))
            {
                var dbParamName = sqlBuilder.AddDbParameter(0);

                var memberExpression = expression.Operand as MemberExpression;
                //添加到查询条件
                var tableName = memberExpression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
                sqlBuilder.SetTableAlias(tableName);
                string tableAlias = sqlBuilder.GetTableAlias(tableName);
                if (!string.IsNullOrWhiteSpace(tableAlias))
                {
                    tableAlias += ".";
                }
                sqlBuilder.SqlWhereStr += " " + tableAlias + memberExpression.Member.Name;
                sqlBuilder.SqlWhereStr += " = ";
                sqlBuilder.SqlWhereStr += $"{dbParamName}";
            }
            else
            {
                SqlProvider.Where(expression.Operand, sqlBuilder);
            }

            return sqlBuilder;
        }

        protected override SqlBuilder GroupBy(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.GroupBy(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder OrderBy(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.OrderBy(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder ThenBy(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.ThenBy(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }
        protected override SqlBuilder OrderByDescending(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.OrderByDescending(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder ThenByDescending(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.ThenByDescending(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Max(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Max(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Min(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Min(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Avg(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Avg(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Count(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Count(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Sum(UnaryExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Sum(expression.Operand, sqlBuilder);
            return sqlBuilder;
        }
    }
}
