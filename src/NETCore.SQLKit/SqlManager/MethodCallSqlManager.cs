
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace NETCore.SQLKit
{
    class MethodCallSqlManager : BaseSqlManager<MethodCallExpression>
    {
        static Dictionary<string, Action<MethodCallExpression, SqlBuilder>> _Methods = new Dictionary<string, Action<MethodCallExpression, SqlBuilder>>
        {
            {"Contains",Like },
            {"Like",Like},
            {"LikeLeft",LikeLeft},
            {"LikeRight",LikeRight},
            {"In",InnerIn}
        };

        private static void InnerIn(MethodCallExpression expression, SqlBuilder sqlBuilder)
        {
            SqlProvider.Where(expression.Arguments[0], sqlBuilder);
            sqlBuilder.SqlWhereStr += " in";
            SqlProvider.In(expression.Arguments[1], sqlBuilder);
        }

        private static void Like(MethodCallExpression expression, SqlBuilder sqlBuilder)
        {
            if (expression.Object != null)
            {
                SqlProvider.Where(expression.Object, sqlBuilder);
                sqlBuilder.SqlWhereStr += " like '%' +";
                SqlProvider.Where(expression.Arguments[0], sqlBuilder);
                sqlBuilder.SqlWhereStr += " + '%'";
            }
            else
            {
                SqlProvider.Where(expression.Arguments[0], sqlBuilder);
                sqlBuilder.SqlWhereStr += " like '%' +";
                SqlProvider.Where(expression.Arguments[1], sqlBuilder);
                sqlBuilder.SqlWhereStr += " + '%'";
            }
        }

        private static void LikeLeft(MethodCallExpression expression, SqlBuilder sqlBuilder)
        {
            if (expression.Object != null)
            {
                SqlProvider.Where(expression.Object, sqlBuilder);
            }
            SqlProvider.Where(expression.Arguments[0], sqlBuilder);
            sqlBuilder.SqlWhereStr += " like '%' +";
            SqlProvider.Where(expression.Arguments[1], sqlBuilder);
        }

        private static void LikeRight(MethodCallExpression expression, SqlBuilder sqlBuilder)
        {
            if (expression.Object != null)
            {
                SqlProvider.Where(expression.Object, sqlBuilder);
            }
            SqlProvider.Where(expression.Arguments[0], sqlBuilder);
            sqlBuilder.SqlWhereStr += " like";
            SqlProvider.Where(expression.Arguments[1], sqlBuilder);
            sqlBuilder.SqlWhereStr += " + '%'";
        }

        protected override SqlBuilder Where(MethodCallExpression expression, SqlBuilder sqlBuilder)
        {
            var key = expression.Method;
            if (key.IsGenericMethod)
            {
                key = key.GetGenericMethodDefinition();
            }

            Action<MethodCallExpression, SqlBuilder> action;
            if (_Methods.TryGetValue(key.Name, out action))
            {
                action(expression, sqlBuilder);
                return sqlBuilder;
            }

            throw new NotImplementedException("Unimplemented method:" + expression.Method);
        }
    }
}