
using System;
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
	public static class SQLServerQueryable
    {
        private static SqlQueryable<T> CreateQueryable<T>()
        {
            return new SqlQueryable<T>(new SQLServerParser());
        }

        public static SqlQueryable<T> Insert<T>(Expression<Func<T>> expression)
        {
            return CreateQueryable<T>().Insert(expression);
        }

        public static SqlQueryable<T> Delete<T>()
        {
            return CreateQueryable<T>().Delete();
        }

        public static SqlQueryable<T> Update<T>(Expression<Func<T>> expression)
        {
            return CreateQueryable<T>().Update(expression);
        }
        public static SqlQueryable<T> SelectTest<T>(Expression<Func<T>> expression = null)
        {
            return null;
        }
        public static SqlQueryable<T> Select<T>(Expression<Func<T, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2>(Expression<Func<T, T2, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3>(Expression<Func<T, T2, T3, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }
        public static SqlQueryable<T> Select<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> expression = null)
        {
            return CreateQueryable<T>().Select(expression);
        }

        public static SqlQueryable<T> Max<T>(Expression<Func<T, object>> expression)
        {
            return CreateQueryable<T>().Max(expression);
        }

        public static SqlQueryable<T> Min<T>(Expression<Func<T, object>> expression)
        {
            return CreateQueryable<T>().Min(expression);
        }

        public static SqlQueryable<T> Avg<T>(Expression<Func<T, object>> expression)
        {
            return CreateQueryable<T>().Avg(expression);
        }

        public static SqlQueryable<T> Count<T>(Expression<Func<T, object>> expression = null)
        {
            return CreateQueryable<T>().Count(expression);
        }

        public static SqlQueryable<T> Sum<T>(Expression<Func<T, object>> expression)
        {
            return CreateQueryable<T>().Sum(expression);
        }
    }
}
