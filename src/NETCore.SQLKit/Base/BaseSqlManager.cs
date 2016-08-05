
using System;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;

namespace NETCore.SQLKit
{
    public abstract class BaseSqlManager<T> : ISqlManager where T : Expression
    {
        protected virtual SqlBuilder Insert(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Insert method");
        }
        protected virtual SqlBuilder Update(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Update method");
        }
        protected virtual SqlBuilder Select(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Select method");
        }
        protected virtual SqlBuilder Join(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Join method");
        }
        protected virtual SqlBuilder Where(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Where method");
        }
        protected virtual SqlBuilder In(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.In method");
        }
        protected virtual SqlBuilder GroupBy(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.GroupBy method");
        }
        protected virtual SqlBuilder OrderBy(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.OrderBy method");
        }
        protected virtual SqlBuilder ThenBy(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.ThenBy method");
        }
        protected virtual SqlBuilder OrderByDescending(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.OrderByDescending method");
        }
        protected virtual SqlBuilder ThenByDescending(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.ThenByDescending method");
        }
        protected virtual SqlBuilder Max(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Max method");
        }
        protected virtual SqlBuilder Min(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Min method");
        }
        protected virtual SqlBuilder Avg(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Avg method");
        }
        protected virtual SqlBuilder Count(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Count method");
        }
        protected virtual SqlBuilder Sum(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Sum method");
        }
        protected virtual SqlBuilder Delete(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "Sql.Delete method");
        }

        //新增
        public SqlBuilder Insert(Expression expression, SqlBuilder sqlBuilder)
        {
            return Insert((T)expression, sqlBuilder);
        }
        //更新
        public SqlBuilder Update(Expression expression, SqlBuilder sqlBuilder)
        {
            return Update((T)expression, sqlBuilder);
        }
        //查询
        public SqlBuilder Select(Expression expression, SqlBuilder sqlBuilder)
        {
            return Select((T)expression, sqlBuilder);
        }
        //关联
        public SqlBuilder Join(Expression expression, SqlBuilder sqlBuilder)
        {
            return Join((T)expression, sqlBuilder);
        }
        //条件
        public SqlBuilder Where(Expression expression, SqlBuilder sqlBuilder)
        {
            return Where((T)expression, sqlBuilder);
        }
        public SqlBuilder In(Expression expression, SqlBuilder sqlBuilder)
        {
            return In((T)expression, sqlBuilder);
        }
        //分组
        public SqlBuilder GroupBy(Expression expression, SqlBuilder sqlBuilder)
        {
            return GroupBy((T)expression, sqlBuilder);
        }
        //排序
        public SqlBuilder OrderBy(Expression expression, SqlBuilder sqlBuilder)
        {
            return OrderBy((T)expression, sqlBuilder);
        }
        public SqlBuilder ThenBy(Expression expression, SqlBuilder sqlBuilder)
        {
            return ThenBy((T)expression, sqlBuilder);
        }
        public SqlBuilder OrderByDescending(Expression expression, SqlBuilder sqlBuilder)
        {
            return OrderByDescending((T)expression, sqlBuilder);
        }
        public SqlBuilder ThenByDescending(Expression expression, SqlBuilder sqlBuilder)
        {
            return ThenByDescending((T)expression, sqlBuilder);
        }
        //计算
        public SqlBuilder Max(Expression expression, SqlBuilder sqlBuilder)
        {
            return Max((T)expression, sqlBuilder);
        }
        public SqlBuilder Min(Expression expression, SqlBuilder sqlBuilder)
        {
            return Min((T)expression, sqlBuilder);
        }
        public SqlBuilder Avg(Expression expression, SqlBuilder sqlBuilder)
        {
            return Avg((T)expression, sqlBuilder);
        }
        public SqlBuilder Count(Expression expression, SqlBuilder sqlBuilder)
        {
            return Count((T)expression, sqlBuilder);
        }
        public SqlBuilder Sum(Expression expression, SqlBuilder sqlBuilder)
        {
            return Sum((T)expression, sqlBuilder);
        }

        public SqlBuilder Delete(Expression expression, SqlBuilder sqlBuilder)
        {
            return Delete((T)expression, sqlBuilder);
        }
    }
}
