
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace NETCore.SQLKit
{
    class MemberSqlManager : BaseSqlManager<MemberExpression>
    {
        private static object GetValue(MemberExpression expr)
        {
            object value;
            var field = expr.Member as FieldInfo;
            if (field != null)
            {
                value = field.GetValue(((ConstantExpression)expr.Expression).Value);
            }
            else
            {
                value = ((PropertyInfo)expr.Member).GetValue(((ConstantExpression)expr.Expression).Value, null);
            }
            return value;
        }

        protected override SqlBuilder Insert(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            string columns = " ( ";
            string values = " values (";

            //获取新增对象
            object entity = GetValue(expression);
            //获取对象属性
            var properties = expression.Type.GetColumnPorperties();

            foreach (var propertyInfo in properties)
            {
                //排除自增列
                if (propertyInfo.IsIdentity(expression.Type))
                {
                    continue;
                }
                var name = propertyInfo.Name;
                var value = propertyInfo.GetValue(entity, null);

                columns += $"{name},";
                //如果value为bool值，则转成1或0
                if (value is bool)
                {
                    if ((bool)value)
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

        protected override SqlBuilder Select(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            //排除非数据列
            PropertyInfo propertyInfo = expression.Member as PropertyInfo;
            if (propertyInfo.IsNotColumn(expression.Member.DeclaringType))
            {
                return sqlBuilder;
            }

            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            string tableAlias = sqlBuilder.GetTableAlias(tableName);
            if (!string.IsNullOrWhiteSpace(tableAlias))
            {
                tableAlias += ".";
            }
            sqlBuilder.SelectFields.Add(tableAlias + expression.Member.Name);
            return sqlBuilder;
        }

        protected override SqlBuilder Join(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            string tableAlias = sqlBuilder.GetTableAlias(tableName);
            if (!string.IsNullOrWhiteSpace(tableAlias))
            {
                tableAlias += ".";
            }
            if (expression.NodeType == ExpressionType.MemberAccess && expression.Type == typeof(bool))
            {
                //添加值
                var dbParamName = sqlBuilder.AddDbParameter(1);

                //添加到查询条件
                sqlBuilder.SqlWhereStr += " " + tableAlias + expression.Member.Name;
                sqlBuilder.SqlWhereStr += " = ";
                sqlBuilder.SqlWhereStr += $"{dbParamName}";
            }
            else
            {
                sqlBuilder.SqlJoinStr += " " + tableAlias + expression.Member.Name;
            }
            return sqlBuilder;
        }

        protected override SqlBuilder Where(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            //获取table名称和别名
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            string tableAlias = sqlBuilder.GetTableAlias(tableName);
            if (!string.IsNullOrWhiteSpace(tableAlias))
            {
                tableAlias += ".";
            }

            if (expression.Expression.NodeType == ExpressionType.Constant)
            {
                object value = GetValue(expression);
                var dbParamName = sqlBuilder.AddDbParameter(value);
                sqlBuilder.SqlWhereStr += $" {dbParamName}";
            }
            //解析x => x.isDeletion这样的
            else if (expression.NodeType == ExpressionType.MemberAccess && expression.Type == typeof(bool))
            {
                //添加值
                var dbParamName = sqlBuilder.AddDbParameter(1);

                //添加到查询条件
                sqlBuilder.SqlWhereStr += " " + tableAlias + expression.Member.Name;
                sqlBuilder.SqlWhereStr += " = ";
                sqlBuilder.SqlWhereStr += $"{dbParamName}";
            }
            else if (expression.Expression.NodeType == ExpressionType.Parameter)
            {
                sqlBuilder.SqlWhereStr += " " + tableAlias + expression.Member.Name;
            }

            return sqlBuilder;
        }

        protected override SqlBuilder In(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var field = expression.Member as FieldInfo;
            if (field != null)
            {
                object val = field.GetValue(((ConstantExpression)expression.Expression).Value);

                if (val != null)
                {
                    string itemJoinStr = "";
                    IEnumerable array = val as IEnumerable;
                    foreach (var item in array)
                    {
                        if (!string.IsNullOrEmpty(itemJoinStr))
                        {
                            itemJoinStr += " ,";
                        }
                        if (field.FieldType.Name == "String[]" || field.FieldType == typeof(List<string>))
                        {
                            itemJoinStr += $" '{item}'";
                        }
                        else
                        {
                            itemJoinStr += $" {item}";
                        }
                    }

                    if (itemJoinStr.Length > 0)
                    {
                        itemJoinStr = $"({itemJoinStr} )";
                        sqlBuilder.SqlWhereStr += itemJoinStr;
                    }
                }
            }
            return sqlBuilder;
        }

        protected override SqlBuilder GroupBy(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            var tableAliaName = sqlBuilder.GetTableAlias(tableName);

            if (!sqlBuilder.IsAddGroupBy)
            {
                sqlBuilder.SqlGroupByStr += $" group by {tableAliaName + "." + expression.Member.Name}";
            }
            else
            {
                sqlBuilder.SqlGroupByStr += $" ,{tableAliaName + "." + expression.Member.Name}";
            }

            return sqlBuilder;
        }

        protected override SqlBuilder OrderBy(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            if (sqlBuilder.SqlOrderStr.Length > 10)
            {
                sqlBuilder.SqlOrderStr += ",";
            }
            sqlBuilder.SqlOrderStr += sqlBuilder.GetTableAlias(tableName) + "." + expression.Member.Name;
            return sqlBuilder;
        }

        protected override SqlBuilder ThenBy(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            sqlBuilder.SqlOrderStr += "," + sqlBuilder.GetTableAlias(tableName) + "." + expression.Member.Name;
            return sqlBuilder;
        }

        protected override SqlBuilder OrderByDescending(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            if (sqlBuilder.SqlOrderStr.Length > 10)
            {
                sqlBuilder.SqlOrderStr += ",";
            }
            sqlBuilder.SqlOrderStr += sqlBuilder.GetTableAlias(tableName) + "." + expression.Member.Name + " desc";
            return sqlBuilder;
        }

        protected override SqlBuilder ThenByDescending(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            var tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            sqlBuilder.SetTableAlias(tableName);
            sqlBuilder.SqlOrderStr += "," + sqlBuilder.GetTableAlias(tableName) + "." + expression.Member.Name + " desc";
            return sqlBuilder;
        }

        private SqlBuilder AggregateFunctionParser(MemberExpression expression, SqlBuilder sqlBuilder, string functionName)
        {
            string aggregateFunctionName = functionName;

            string tableName = expression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
            string columnName = expression.Member.Name;
            sqlBuilder.SetTableAlias(tableName);
            string tableAlias = sqlBuilder.GetTableAlias(tableName);

            if (!string.IsNullOrWhiteSpace(tableAlias))
            {
                tableName += " " + tableAlias;
                columnName = tableAlias + "." + columnName;
            }
            sqlBuilder.SqlCalculateStr = $"select {aggregateFunctionName}({columnName}) from {tableName}";
            return sqlBuilder;
        }

        protected override SqlBuilder Max(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            return AggregateFunctionParser(expression, sqlBuilder, "Max");
        }

        protected override SqlBuilder Min(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            return AggregateFunctionParser(expression, sqlBuilder, "Min");
        }

        protected override SqlBuilder Avg(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            return AggregateFunctionParser(expression, sqlBuilder, "Avg");
        }

        protected override SqlBuilder Count(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            return AggregateFunctionParser(expression, sqlBuilder, "Count");
        }

        protected override SqlBuilder Sum(MemberExpression expression, SqlBuilder sqlBuilder)
        {
            return AggregateFunctionParser(expression, sqlBuilder, "Sum");
        }
    }
}