
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NETCore.SQLKit
{
    public class SqlQueryable<T> : IDisposable
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private SqlBuilder _sqlBuilder;
        private string _mainTableName;

        public string Sql
        {
            get
            {
                return this._sqlBuilder.Sql + ";";
            }
        }
        public Dictionary<string, object> DbParams
        {
            get
            {
                return this._sqlBuilder.DbParams;
            }
        }

        internal SqlQueryable(ISqlParser dbSqlParser)
        {
            _mainTableName = typeof(T).GetTableName(dbSqlParser);
            this._sqlBuilder = new SqlBuilder(dbSqlParser);
        }

        //清除查询
        public void ClearSelect()
        {
            this._sqlBuilder.ClearSelectFields();
        }

        public void Clear()
        {
            this._sqlBuilder.Clear();
        }


        private SqlQueryable<T> SelectParser(Expression expression, Expression expressionBody, params Type[] ary)
        {
            this.ClearSelect();
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Select);
            this._sqlBuilder.IsAddSelect = true;
            this._sqlBuilder.IsSingleTable = false;

            foreach (var item in ary)
            {
                string tableName = item.GetTableName(_sqlBuilder._dbSqlParser);
                this._sqlBuilder.SetTableAlias(tableName);
            }

            this._sqlBuilder.SqlSelectStr = "select {0} from " + this._mainTableName + " " + this._sqlBuilder.GetTableAlias(this._mainTableName);

            if (expression != null)
            {
                SqlProvider.Select(expressionBody, this._sqlBuilder);
            }

            return this;
        }

        public SqlQueryable<T> Select(Expression<Func<T, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2>(Expression<Func<T, T2, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3>(Expression<Func<T, T2, T3, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }
        public SqlQueryable<T> Select<T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> expression = null)
        {
            return SelectParser(expression, expression == null ? null : expression.Body, typeof(T));
        }


        private SqlQueryable<T> JoinParser<T2>(Expression<Func<T, T2, bool>> expression, string leftOrRightJoin = "")
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            string joinTableName = typeof(T2).GetTableName(_sqlBuilder._dbSqlParser);
            this._sqlBuilder.SetTableAlias(joinTableName);

            this._sqlBuilder.SqlJoinStr += $"{leftOrRightJoin} join {joinTableName + " " + this._sqlBuilder.GetTableAlias(joinTableName)} on";
            SqlProvider.Join(expression.Body, this._sqlBuilder);
            return this;
        }
        private SqlQueryable<T> JoinParser2<T2, T3>(Expression<Func<T2, T3, bool>> expression, string leftOrRightJoin = "")
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            string joinTableName = typeof(T3).GetTableName(_sqlBuilder._dbSqlParser);
            this._sqlBuilder.SetTableAlias(joinTableName);

            this._sqlBuilder.SqlJoinStr += $"{leftOrRightJoin} join {joinTableName + " " + this._sqlBuilder.GetTableAlias(joinTableName)} on";
            SqlProvider.Join(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Join<T2>(Expression<Func<T, T2, bool>> expression)
        {
            return JoinParser(expression);
        }
        public SqlQueryable<T> Join<T2, T3>(Expression<Func<T2, T3, bool>> expression)
        {
            return JoinParser2(expression);
        }

        public SqlQueryable<T> InnerJoin<T2>(Expression<Func<T, T2, bool>> expression)
        {
            return JoinParser(expression, " inner");
        }
        public SqlQueryable<T> InnerJoin<T2, T3>(Expression<Func<T2, T3, bool>> expression)
        {
            return JoinParser2(expression, "inner");
        }

        public SqlQueryable<T> LeftJoin<T2>(Expression<Func<T, T2, bool>> expression)
        {
            return JoinParser(expression, " left");
        }
        public SqlQueryable<T> LeftJoin<T2, T3>(Expression<Func<T2, T3, bool>> expression)
        {
            return JoinParser2(expression, " left");
        }

        public SqlQueryable<T> RightJoin<T2>(Expression<Func<T, T2, bool>> expression)
        {
            return JoinParser(expression, " right");
        }
        public SqlQueryable<T> RightJoin<T2, T3>(Expression<Func<T2, T3, bool>> expression)
        {
            return JoinParser2(expression, " right");
        }

        public SqlQueryable<T> FullJoin<T2>(Expression<Func<T, T2, bool>> expression)
        {
            return JoinParser(expression, " full");
        }
        public SqlQueryable<T> FullJoin<T2, T3>(Expression<Func<T2, T3, bool>> expression)
        {
            return JoinParser2(expression, " full");
        }

        public SqlQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            if (expression.Body != null && expression.Body.NodeType == ExpressionType.Constant)
            {
                throw new ArgumentException("Cannot be parse expression", "expression");
            }

            if (!this._sqlBuilder.IsAddWhere)
            {
                this._sqlBuilder.SqlWhereStr += " where";
            }
            else
            {
                this._sqlBuilder.SqlWhereStr += " and";
            }
            SqlProvider.Where(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> GroupBy(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            SqlProvider.GroupBy(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> OrderBy(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            this._sqlBuilder.SqlOrderStr += " order by ";
            SqlProvider.OrderBy(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> ThenBy(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            SqlProvider.ThenBy(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            this._sqlBuilder.SqlOrderStr += " order by ";
            SqlProvider.OrderByDescending(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> ThenByDescending(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }
            SqlProvider.ThenByDescending(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Max(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(" max expression", "Value cannot be null");
            }
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Calculate);
            this.Clear();
            SqlProvider.Max(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Min(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("min expression", "Value cannot be null");
            }
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Calculate);
            this.Clear();
            SqlProvider.Min(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Avg(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("avg expression", "Value cannot be null");
            }
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Calculate);
            this.Clear();
            SqlProvider.Avg(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Count(Expression<Func<T, object>> expression = null)
        {
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Calculate);
            this.Clear();
            if (expression == null)
            {
                string tableName = typeof(T).GetTableName(_sqlBuilder._dbSqlParser);

                this._sqlBuilder.SetTableAlias(tableName);
                string tableAlias = this._sqlBuilder.GetTableAlias(tableName);

                if (!string.IsNullOrWhiteSpace(tableAlias))
                {
                    tableName += " " + tableAlias;
                }
                this._sqlBuilder.SqlCalculateStr = $"select Count(*) from {tableName}";
            }
            else
            {
                SqlProvider.Count(expression.Body, this._sqlBuilder);
            }

            return this;
        }

        public SqlQueryable<T> Sum(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Calculate);
            this.Clear();
            SqlProvider.Sum(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Insert(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            this.Clear();
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Insert);
            this._sqlBuilder.IsSingleTable = true;
            this._sqlBuilder.SqlInsertStr = $"insert into {this._mainTableName}";
            SqlProvider.Insert(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Delete()
        {
            this.Clear();
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Delete);
            this._sqlBuilder.IsSingleTable = true;

            this._sqlBuilder.SqlDeleteStr = $"delete {this._mainTableName}";
            return this;
        }

        public SqlQueryable<T> Update(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Value cannot be null");
            }

            this.Clear();
            this._sqlBuilder.SetSqlCommandType(SqlCommandType.Update);
            this._sqlBuilder.IsSingleTable = true;
            this._sqlBuilder.SqlUpdateStr = $"update {this._mainTableName} set ";

            SqlProvider.Update(expression.Body, this._sqlBuilder);
            return this;
        }

        public SqlQueryable<T> Skip(int skipNum)
        {
            if (skipNum > 0)
            {
                this._sqlBuilder.IsAddSkip = true;
            }

            this._sqlBuilder.SkipNum = skipNum;

            return this;
        }

        public SqlQueryable<T> Take(int takeNum)
        {
            if (takeNum > 0)
            {
                this._sqlBuilder.IsAddTake = true;
            }
            this._sqlBuilder.TakeNum = takeNum;
            return this;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Clear();
            _sqlBuilder = null;
            GC.SuppressFinalize(this);
        }
    }
}
