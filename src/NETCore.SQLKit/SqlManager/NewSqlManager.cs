
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NETCore.SQLKit
{
    class NewSqlManager : BaseSqlManager<NewExpression>
    {
        //TODO 删除
        protected override SqlBuilder Update(NewExpression expression, SqlBuilder sqlBuilder)
        {
            for (int i = 0; i < expression.Members.Count; i++)
            {
                MemberInfo m = expression.Members[i];
                ConstantExpression c = expression.Arguments[i] as ConstantExpression;
                sqlBuilder.SqlUpdateStr += m.Name + " =";
                var dbParameName = sqlBuilder.AddDbParameter(c.Value);
                sqlBuilder.SqlUpdateStr += dbParameName;

                if (i < expression.Members.Count - 1)
                {
                    sqlBuilder.SqlUpdateStr += ",";
                }
            }
            return sqlBuilder;
        }

        protected override SqlBuilder Select(NewExpression expression, SqlBuilder sqlBuilder)
        {
            for (int i = 0; i < expression.Arguments.Count; i++)
            {
                var argument = expression.Arguments[i];

                var mermberExpression = ((MemberExpression)argument);
                PropertyInfo property = mermberExpression.Member as PropertyInfo;

                //排除非数据列
                if (property.IsNotColumn(mermberExpression.Expression.Type))
                {
                    continue;
                }

                SqlProvider.Select(argument, sqlBuilder);

                var fieldName = expression.Members[i].Name;

                sqlBuilder.SelectFieldsAlias.Add($"[{fieldName}]");
            }

            return sqlBuilder;
        }

        protected override SqlBuilder GroupBy(NewExpression expression, SqlBuilder sqlBuilder)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlProvider.GroupBy(item, sqlBuilder);
            }
            return sqlBuilder;
        }

        protected override SqlBuilder OrderBy(NewExpression expression, SqlBuilder sqlBuilder)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlProvider.OrderBy(item, sqlBuilder);
            }
            return sqlBuilder;
        }

        protected override SqlBuilder ThenBy(NewExpression expression, SqlBuilder sqlBuilder)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlProvider.ThenBy(item, sqlBuilder);
            }
            return sqlBuilder;
        }

        protected override SqlBuilder OrderByDescending(NewExpression expression, SqlBuilder sqlBuilder)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlProvider.OrderByDescending(item, sqlBuilder);
            }
            return sqlBuilder;
        }

        protected override SqlBuilder ThenByDescending(NewExpression expression, SqlBuilder sqlBuilder)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlProvider.ThenByDescending(item, sqlBuilder);
            }
            return sqlBuilder;
        }
    }
}
