
using System;
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
    internal class SqlProvider
    {
        private static ISqlManager GetSqlManager(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Cannot be null");
            }

            if (expression is BinaryExpression)
            {
                return new BinarySqlManager();
            }
            if (expression is BlockExpression)
            {
                throw new NotImplementedException("Unimplemented BlockExpressionSqlManager");
            }
            if (expression is ConditionalExpression)
            {
                throw new NotImplementedException("Unimplemented ConditionalExpressionSqlManager");
            }
            if (expression is ConstantExpression)
            {
                return new ConstantSqlManager();
            }
            if (expression is DebugInfoExpression)
            {
                throw new NotImplementedException("Unimplemented DebugInfoExpressionSqlManager");
            }
            if (expression is DefaultExpression)
            {
                throw new NotImplementedException("Unimplemented DefaultExpressionSqlManager");
            }
            //if (expression is DynamicExpression)
            //{
            //    throw new NotImplementedException("Unimplemented DynamicExpressionSqlManager");
            //}
            if (expression is GotoExpression)
            {
                throw new NotImplementedException("Unimplemented GotoExpressionSqlManager");
            }
            if (expression is IndexExpression)
            {
                throw new NotImplementedException("Unimplemented IndexExpressionSqlManager");
            }
            if (expression is InvocationExpression)
            {
                throw new NotImplementedException("Unimplemented InvocationExpressionSqlManager");
            }
            if (expression is LabelExpression)
            {
                throw new NotImplementedException("Unimplemented LabelExpressionSqlManager");
            }
            if (expression is LambdaExpression)
            {
                throw new NotImplementedException("Unimplemented LambdaExpressionSqlManager");
            }
            if (expression is ListInitExpression)
            {
                return new ListInitSqlManager();
            }
            if (expression is LoopExpression)
            {
                throw new NotImplementedException("Unimplemented LoopExpressionSqlManager");
            }
            if (expression is MemberExpression)
            {
                return new MemberSqlManager();
            }
            if (expression is MemberInitExpression)
            {
                return new MemberInitSqlManager();
            }
            if (expression is MethodCallExpression)
            {
                return new MethodCallSqlManager();
            }
            if (expression is NewArrayExpression)
            {
                return new NewArraySqlManager();
            }
            if (expression is NewExpression)
            {
                return new NewSqlManager();
            }
            if (expression is ParameterExpression)
            {
                return new ParameterSqlManager();
            }
            if (expression is RuntimeVariablesExpression)
            {
                throw new NotImplementedException("Unimplemented RuntimeVariablesExpressionSqlManager");
            }
            if (expression is SwitchExpression)
            {
                throw new NotImplementedException("Unimplemented SwitchExpressionSqlManager");
            }
            if (expression is TryExpression)
            {
                throw new NotImplementedException("Unimplemented TryExpressionSqlManager");
            }
            if (expression is TypeBinaryExpression)
            {
                throw new NotImplementedException("Unimplemented TypeBinaryExpressionSqlManager");
            }
            if (expression is UnaryExpression)
            {
                return new UnarySqlManager();
            }

            throw new NotImplementedException("Unimplemented ExpressionSqlManager");
        }

        public static void Insert(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Insert(expression, sqlBuilder);
        }

        public static void Update(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Update(expression, sqlBuilder);
        }

        public static void Select(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Select(expression, sqlBuilder);
        }

        public static void Join(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Join(expression, sqlBuilder);
        }

        public static void Where(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Where(expression, sqlBuilder);
        }

        public static void In(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).In(expression, sqlBuilder);
        }

        public static void GroupBy(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).GroupBy(expression, sqlBuilder);
        }

        public static void OrderBy(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).OrderBy(expression, sqlBuilder);
        }
        public static void ThenBy(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).ThenBy(expression, sqlBuilder);
        }
        public static void OrderByDescending(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).OrderByDescending(expression, sqlBuilder);
        }
        public static void ThenByDescending(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).ThenByDescending(expression, sqlBuilder);
        }

        public static void Max(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Max(expression, sqlBuilder);
        }

        public static void Min(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Min(expression, sqlBuilder);
        }

        public static void Avg(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Avg(expression, sqlBuilder);
        }

        public static void Count(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Count(expression, sqlBuilder);
        }

        public static void Sum(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Sum(expression, sqlBuilder);
        }

        public static void Delete(Expression expression, SqlBuilder sqlBuilder)
        {
            GetSqlManager(expression).Delete(expression, sqlBuilder);
        }
    }
}
