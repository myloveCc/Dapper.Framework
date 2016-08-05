
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NETCore.SQLKit
{
    class BinarySqlManager : BaseSqlManager<BinaryExpression>
    {
        private string OperatorParser(ExpressionType expressionNodeType, bool useIs = false)
        {
            switch (expressionNodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " and";
                case ExpressionType.Equal:
                    if (useIs)
                    {
                        return " is";
                    }
                    else
                    {
                        return " =";
                    }
                case ExpressionType.GreaterThan:
                    return " >";
                case ExpressionType.GreaterThanOrEqual:
                    return " >=";
                case ExpressionType.NotEqual:
                    if (useIs)
                    {
                        return " is not";
                    }
                    else
                    {
                        return " <>";
                    }
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " or";
                case ExpressionType.LessThan:
                    return " <";
                case ExpressionType.LessThanOrEqual:
                    return " <=";
                default:
                    throw new NotImplementedException("Unimplemented expressionType:" + expressionNodeType);
            }
        }

        private static int GetOperatorPrecedence(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return 10;
                case ExpressionType.And:
                    return 6;
                case ExpressionType.AndAlso:
                    return 3;
                case ExpressionType.Coalesce:
                case ExpressionType.Assign:
                case ExpressionType.AddAssign:
                case ExpressionType.AndAssign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.SubtractAssignChecked:
                    return 1;
                case ExpressionType.Constant:
                case ExpressionType.Parameter:
                    return 15;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.Throw:
                case ExpressionType.Unbox:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.OnesComplement:
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:
                    return 12;
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return 11;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    return 7;
                case ExpressionType.ExclusiveOr:
                    return 5;
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.TypeAs:
                case ExpressionType.TypeIs:
                case ExpressionType.TypeEqual:
                    return 8;
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    return 9;
                case ExpressionType.Or:
                    return 4;
                case ExpressionType.OrElse:
                    return 2;
                case ExpressionType.Power:
                    return 13;
            }
            return 14;
        }

        private static bool NeedsParenthesesLast(Expression parent, Expression child)
        {
            BinaryExpression binaryExpression = parent as BinaryExpression;
            return binaryExpression != null && child == binaryExpression.Right;
        }

        private static bool NeedsParenthesesPrecedence(Expression parent, Expression child)
        {
            int operatorPrecedenceChild = GetOperatorPrecedence(child);
            int operatorPrecedenceParent = GetOperatorPrecedence(parent);
            if (operatorPrecedenceChild == operatorPrecedenceParent)
            {
                var nodeType = parent.NodeType;
                if (nodeType <= ExpressionType.MultiplyChecked)
                {
                    if (nodeType <= ExpressionType.Divide)
                    {
                        switch (nodeType)
                        {
                            case ExpressionType.Add:
                            case ExpressionType.AddChecked:
                                break;
                            case ExpressionType.And:
                            case ExpressionType.AndAlso:
                                return false;
                            default:
                                if (nodeType != ExpressionType.Divide)
                                {
                                    return true;
                                }
                                return NeedsParenthesesLast(parent, child);
                        }
                    }
                    else
                    {
                        if (nodeType == ExpressionType.ExclusiveOr)
                        {
                            return false;
                        }
                        switch (nodeType)
                        {
                            case ExpressionType.Modulo:
                                return NeedsParenthesesLast(parent, child);
                            case ExpressionType.Multiply:
                            case ExpressionType.MultiplyChecked:
                                break;
                            default:
                                return true;
                        }
                    }
                    return false;
                }
                if (nodeType <= ExpressionType.OrElse)
                {
                    if (nodeType != ExpressionType.Or && nodeType != ExpressionType.OrElse)
                    {
                        return true;
                    }
                }
                else
                {
                    if (nodeType != ExpressionType.Subtract && nodeType != ExpressionType.SubtractChecked)
                    {
                        return true;
                    }
                    return NeedsParenthesesLast(parent, child);
                }
                return false;
            }
            return (child.NodeType == ExpressionType.Constant && (parent.NodeType == ExpressionType.Negate || parent.NodeType == ExpressionType.NegateChecked)) || operatorPrecedenceChild < operatorPrecedenceParent;
        }

        private static bool IsNeedsParentheses(Expression parent, Expression child)
        {
            if (child == null)
            {
                return false;
            }
            ExpressionType nodeType = parent.NodeType;
            if (nodeType <= ExpressionType.Increment)
            {
                if (nodeType != ExpressionType.Decrement && nodeType != ExpressionType.Increment)
                {
                    return NeedsParenthesesPrecedence(parent, child);
                }
            }
            else if (nodeType != ExpressionType.Unbox && nodeType != ExpressionType.IsTrue && nodeType != ExpressionType.IsFalse)
            {
                return NeedsParenthesesPrecedence(parent, child);
            }
            return true;
        }

        //检查二院表达式左侧的Expression是否为非数据列属性
        private static bool CheckMemberIsNotColumn(BinaryExpression expression, SqlBuilder sqlBuilder)
        {
            if (expression.Left is MemberExpression)
            {
                var memberExpression = expression.Left as MemberExpression;
                PropertyInfo property = memberExpression.Member as PropertyInfo;
                var type = memberExpression.Expression.Type;

                //检查是否为非数据列
                if (property.IsNotColumn(type))
                {
                    if (sqlBuilder.SqlWhereStr.EndsWith("and"))
                    {
                        sqlBuilder.SqlWhereStr = sqlBuilder.SqlWhereStr.Remove(sqlBuilder.SqlWhereStr.Length - 3, 3);
                    }
                    return true;
                }
            }

            return false;
        }

        protected override SqlBuilder Join(BinaryExpression expression, SqlBuilder sqlBuilder)
        {
            bool hasHandle = false;

            if (CheckMemberIsNotColumn(expression, sqlBuilder))
            {
                return sqlBuilder;
            }

            //添加二元表达式左侧sql
            if (expression.Left is MemberExpression && expression.Left.Type == typeof(bool) && expression.Left.NodeType == ExpressionType.MemberAccess && (expression.NodeType == ExpressionType.AndAlso || expression.Right is ConstantExpression))
            {
                //解析（m=>m.IsAdmin) 
                var memberExpression = expression.Left as MemberExpression;

                var tableName = memberExpression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
                sqlBuilder.SetTableAlias(tableName);
                string tableAlias = sqlBuilder.GetTableAlias(tableName);
                if (!string.IsNullOrWhiteSpace(tableAlias))
                {
                    tableAlias += ".";
                }
                var dbParamName = string.Empty;
                if (expression.Right is ConstantExpression)
                {
                    hasHandle = true;
                    var value = Convert.ToBoolean(((ConstantExpression)expression.Right).Value) ? 1 : 0;
                    dbParamName = sqlBuilder.AddDbParameter(value);
                }
                else
                {
                    dbParamName = sqlBuilder.AddDbParameter(1);
                }
                //添加关联条件
                sqlBuilder.SqlJoinStr += " " + tableAlias + memberExpression.Member.Name;
                sqlBuilder.SqlJoinStr += " = ";
                sqlBuilder.SqlJoinStr += $"{dbParamName}";
            }
            else
            {
                SqlProvider.Join(expression.Left, sqlBuilder);
            }

            if (!hasHandle)
            {
                //添加二元表达式操作符
                var joinOperator = " =";

                if ((expression.Right is ConstantExpression) && ((ConstantExpression)expression.Right).Value == null)
                {
                    joinOperator = OperatorParser(expression.NodeType, true);
                }
                else
                {
                    joinOperator = OperatorParser(expression.NodeType);
                }

                sqlBuilder.SqlJoinStr += $"{joinOperator}";

                //添加二元表达式右侧sql
                if (expression.Right is MemberExpression && expression.Right.Type == typeof(bool) && expression.Right.NodeType == ExpressionType.MemberAccess && expression.NodeType == ExpressionType.AndAlso)
                {
                    //解析（m=>m.IsAdmin) 
                    var memberExpression = expression.Right as MemberExpression;

                    var tableName = memberExpression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
                    sqlBuilder.SetTableAlias(tableName);
                    string tableAlias = sqlBuilder.GetTableAlias(tableName);
                    if (!string.IsNullOrWhiteSpace(tableAlias))
                    {
                        tableAlias += ".";
                    }
                    //添加参数
                    var dbParamName = string.Empty;
                    if (expression.Right is ConstantExpression)
                    {
                        var value = Convert.ToBoolean(((ConstantExpression)expression.Right).Value) ? 1 : 0;
                        dbParamName = sqlBuilder.AddDbParameter(1);
                    }
                    else
                    {
                        dbParamName = sqlBuilder.AddDbParameter(1);
                    }
                    //添加关联条件
                    sqlBuilder.SqlJoinStr += " " + tableAlias + memberExpression.Member.Name;
                    sqlBuilder.SqlJoinStr += " = ";
                    sqlBuilder.SqlJoinStr += $"{dbParamName}";
                }
                else
                {
                    SqlProvider.Join(expression.Right, sqlBuilder);
                }
            }
            return sqlBuilder;
        }

        protected override SqlBuilder Where(BinaryExpression expression, SqlBuilder sqlBuilder)
        {
            bool hasHandle = false;

            if (CheckMemberIsNotColumn(expression, sqlBuilder))
            {
                return sqlBuilder;
            }

            //添加Where表达式左侧sql
            if (IsNeedsParentheses(expression, expression.Left))
            {
                sqlBuilder.SqlWhereStr += "(";
            }

            if (expression.Left is MemberExpression && expression.Left.Type == typeof(bool) && expression.Left.NodeType == ExpressionType.MemberAccess && (expression.NodeType == ExpressionType.AndAlso || expression.Right is ConstantExpression))
            {
                //解析（m=>m.IsAdmin) 
                var memberExpression = expression.Left as MemberExpression;

                var tableName = memberExpression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
                sqlBuilder.SetTableAlias(tableName);
                string tableAlias = sqlBuilder.GetTableAlias(tableName);
                if (!string.IsNullOrWhiteSpace(tableAlias))
                {
                    tableAlias += ".";
                }
                //添加参数
                var dbParamName = string.Empty;
                if (expression.Right is ConstantExpression)
                {
                    hasHandle = true;
                    var value = Convert.ToBoolean(((ConstantExpression)expression.Right).Value) ? 1 : 0;
                    dbParamName = sqlBuilder.AddDbParameter(value);
                }
                else
                {
                    dbParamName = sqlBuilder.AddDbParameter(1);
                }
                //添加关联条件
                sqlBuilder.SqlWhereStr += " " + tableAlias + memberExpression.Member.Name;
                sqlBuilder.SqlWhereStr += " = ";
                sqlBuilder.SqlWhereStr += $"{dbParamName}";
            }
            else
            {
                SqlProvider.Where(expression.Left, sqlBuilder);
            }

            if (IsNeedsParentheses(expression, expression.Left))
            {
                sqlBuilder.SqlWhereStr += ")";
            }

            if (!hasHandle)
            {
                //添加Where操作符
                var whereOperator = " =";

                if ((expression.Right is ConstantExpression) && ((ConstantExpression)expression.Right).Value == null)
                {
                    whereOperator = OperatorParser(expression.NodeType, true);
                }
                else
                {
                    whereOperator = OperatorParser(expression.NodeType);
                }

                sqlBuilder.SqlWhereStr += $"{whereOperator}";

                //添加Where表达式右侧sql
                if (IsNeedsParentheses(expression, expression.Right))
                {
                    sqlBuilder.SqlWhereStr += "(";
                }

                if (expression.Right is MemberExpression && expression.Right.Type == typeof(bool) && expression.Right.NodeType == ExpressionType.MemberAccess && expression.NodeType == ExpressionType.AndAlso)
                {
                    //解析（m=>m.IsAdmin) 
                    var memberExpression = expression.Right as MemberExpression;

                    var tableName = memberExpression.Member.DeclaringType.GetTableName(sqlBuilder._dbSqlParser);
                    sqlBuilder.SetTableAlias(tableName);
                    string tableAlias = sqlBuilder.GetTableAlias(tableName);
                    if (!string.IsNullOrWhiteSpace(tableAlias))
                    {
                        tableAlias += ".";
                    }
                    //添加参数
                    var dbParamName = sqlBuilder.AddDbParameter(1);
                    //添加关联条件
                    sqlBuilder.SqlWhereStr += " " + tableAlias + memberExpression.Member.Name;
                    sqlBuilder.SqlWhereStr += " = ";
                    sqlBuilder.SqlWhereStr += $"{dbParamName}";
                }
                else
                {
                    SqlProvider.Where(expression.Right, sqlBuilder);
                }

                if (IsNeedsParentheses(expression, expression.Right))
                {
                    sqlBuilder.SqlWhereStr += ")";
                }
            }
            return sqlBuilder;
        }
    }
}