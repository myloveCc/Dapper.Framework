
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
    class ParameterSqlManager : BaseSqlManager<ParameterExpression>
    {
        protected override SqlBuilder Select(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Select(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Where(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Where(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder GroupBy(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.GroupBy(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder OrderBy(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.OrderBy(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Max(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Max(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Min(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Min(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Avg(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Avg(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Count(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Count(expression, sqlBuilder);
            return sqlBuilder;
        }

        protected override SqlBuilder Sum(ParameterExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Sum(expression, sqlBuilder);
            return sqlBuilder;
        }
    }
}
