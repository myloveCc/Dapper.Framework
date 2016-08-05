using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;

namespace NETCore.SQLKit
{
    public class MemberInitSqlManager : BaseSqlManager<MemberInitExpression>
    {
        protected override SqlBuilder Insert(MemberInitExpression expression, SqlBuilder sqlBuilder)
        {
            string columns = " ( ";
            string values = " values (";

            foreach (MemberAssignment memberAss in expression.Bindings)
            {
                var property = memberAss.Member as PropertyInfo;
                //排除自增项
                if (property.IsIdentity(expression.Type))
                {
                    continue;
                }
                //排序非数据项
                if (property.IsNotColumn(expression.Type))
                {
                    continue;
                }
                var member = memberAss.Member;
                //添加列
                columns += $"{member.Name},";

                var memberExpression = memberAss.Expression;
                if (memberExpression is ConstantExpression)
                {
                    ConstantExpression c = memberExpression as ConstantExpression;
                    var value = c.Value;
                    if (c.Type == typeof(bool))
                    {
                        if (Convert.ToBoolean(value))
                        {
                            value = 1;
                        }
                        else
                        {
                            value = 0;
                        }
                    }
                    string dbParamName = sqlBuilder.AddDbParameter(value);
                    //添加值
                    values += $" {dbParamName} ,";
                }

                //解析DateTime.Now
                if (memberExpression is MemberExpression && memberExpression.Type == typeof(DateTime))
                {
                    LambdaExpression lambda = Expression.Lambda(memberExpression);
                    Delegate fn = lambda.Compile();
                    ConstantExpression value = Expression.Constant(fn.DynamicInvoke(null), memberExpression.Type);

                    string dbParamName = sqlBuilder.AddDbParameter(value.Value);
                    //添加值
                    values += $" {dbParamName} ,";
                }
            }

            if (columns[columns.Length - 1] == ',')
            {
                columns = columns.Remove(columns.Length - 1, 1);
            }
            columns += " )";

            if (values[values.Length - 1] == ',')
            {
                values = values.Remove(values.Length - 1, 1);
            }
            values += ")";

            sqlBuilder.SqlInsertStr += columns + values;

            return sqlBuilder;
        }

        protected override SqlBuilder Update(MemberInitExpression expression, SqlBuilder sqlBuilder)
        {
            foreach (MemberAssignment memberAss in expression.Bindings)
            {
                var property = memberAss.Member as PropertyInfo;
                //排除自增项
                if (property.IsIdentity(expression.Type))
                {
                    continue;
                }
                //排序非数据项
                if (property.IsNotColumn(expression.Type))
                {
                    continue;
                }

                MemberInfo member = memberAss.Member;
                sqlBuilder.SqlUpdateStr += $"{member.Name} =";

                var memberExpression = memberAss.Expression;
                if (memberExpression is ConstantExpression)
                {
                    ConstantExpression c = memberExpression as ConstantExpression;
                    var value = c.Value;
                    if (c.Type == typeof(bool))
                    {
                        if (Convert.ToBoolean(value))
                        {
                            value = 1;
                        }
                        else
                        {
                            value = 0;
                        }
                    }
                    string dbParamName = sqlBuilder.AddDbParameter(value);
                    //添加值
                    sqlBuilder.SqlUpdateStr += $" {dbParamName} ,";
                }

                //解析DateTime.Now
                if (memberExpression is MemberExpression && memberExpression.Type == typeof(DateTime))
                {
                    LambdaExpression lambda = Expression.Lambda(memberExpression);
                    Delegate fn = lambda.Compile();
                    ConstantExpression value = Expression.Constant(fn.DynamicInvoke(null), memberExpression.Type);

                    string dbParamName = sqlBuilder.AddDbParameter(value.Value);
                    //添加值
                    sqlBuilder.SqlUpdateStr += $" {dbParamName} ,";
                }
            }

            if (sqlBuilder.SqlUpdateStr.EndsWith(","))
            {
                sqlBuilder.SqlUpdateStr = sqlBuilder.SqlUpdateStr.Remove(sqlBuilder.SqlUpdateStr.Length - 1, 1);
            }
            return sqlBuilder;
        }

        protected override SqlBuilder Select(MemberInitExpression expression, SqlBuilder sqlBuilder)
        {
            var bindings = expression.Bindings;

            var names = expression.Bindings.Select(m => m.Member.Name).ToArray();

            foreach (MemberAssignment memberAss in expression.Bindings)
            {
                var property = memberAss.Member as PropertyInfo;

                //排序非数据项
                if (property.IsNotColumn(expression.Type))
                {
                    continue;
                }

                SqlProvider.Select(memberAss.Expression, sqlBuilder);
                //添加字段别名
                sqlBuilder.SelectFieldsAlias.Add($"{sqlBuilder._dbSqlParser.ElementLeftPrefix}{memberAss.Member.Name}{sqlBuilder._dbSqlParser.ElementRightPrefix}");
            }

            return sqlBuilder;
        }
    }
}
