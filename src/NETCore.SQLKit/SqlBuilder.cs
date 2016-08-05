
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCore.SQLKit
{
    public class SqlBuilder
    {
        private static readonly List<string> S_listEnglishWords = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        private Dictionary<string, string> _dicTableName = new Dictionary<string, string>();
        private Queue<string> _queueEnglishWords = new Queue<string>(S_listEnglishWords);
        internal ISqlParser _dbSqlParser;

        private SqlCommandType _sqlCommandType;

        internal bool IsSingleTable { get; set; }

        //是否添加查询字段
        internal bool IsAddSelect { get; set; }

        //查询sql语句
        internal string SqlSelectStr { get; set; }

        //查询字段
        internal List<string> SelectFields { get; set; }
        //查询字段别名
        internal List<string> SelectFieldsAlias { get; set; }
        //查询字段（非分页，及分页内部表）
        internal string SelectFieldsStr
        {
            get
            {
                string selectFieldsStr = null;
                if (this.SelectFieldsAlias.Count > 0)
                {
                    for (int i = 0; i < this.SelectFields.Count; i++)
                    {
                        string field = this.SelectFields[i];
                        string fieldAlias = this.SelectFieldsAlias[i];
                        if (field.Split('.')[1] == fieldAlias)
                        {
                            selectFieldsStr += "," + field;
                        }
                        else
                        {
                            selectFieldsStr += "," + field + " " + fieldAlias;
                        }
                    }

                    if (selectFieldsStr.Length > 0 && selectFieldsStr[0] == ',')
                    {
                        selectFieldsStr = selectFieldsStr.Remove(0, 1);
                    }
                }
                else
                {
                    selectFieldsStr = string.Join(",", this.SelectFields);
                }

                if (string.IsNullOrEmpty(selectFieldsStr))
                {
                    selectFieldsStr = "*";
                }

                return selectFieldsStr;
            }
        }

        //分页时，查询字段
        internal string SelectPageFieldsStr
        {
            get
            {
                string selectFieldsStr = null;
                if (this.SelectFieldsAlias.Count > 0)
                {
                    selectFieldsStr = string.Join(",", this.SelectFieldsAlias);
                }
                else
                {
                    selectFieldsStr = string.Join(",", this.SelectFields);
                }

                if (string.IsNullOrEmpty(selectFieldsStr))
                {
                    selectFieldsStr = "*";
                }

                return selectFieldsStr;
            }
        }

        //筛选sql语句
        internal string SqlWhereStr { get; set; }
        //是否添加筛选条件
        internal bool IsAddWhere
        {
            get
            {
                if (string.IsNullOrEmpty(SqlWhereStr))
                {
                    return false;
                }
                return true;
            }
        }


        //关联sql语句
        internal string SqlJoinStr { get; set; }
        internal bool IsAddJoin
        {
            get
            {
                if (string.IsNullOrEmpty(SqlJoinStr))
                {
                    return false;
                }
                return true;
            }
        }

        //排序sql语句
        internal string SqlOrderStr { get; set; }
        //是否添加排序条件
        internal bool IsAddOrder
        {
            get
            {
                if (string.IsNullOrEmpty(SqlOrderStr))
                {
                    return false;
                }
                return true;
            }
        }

        //是否添加分页条件
        internal bool IsAddSkip { get; set; }
        internal int SkipNum { get; set; } = 0;

        //是否添加获取数量
        internal bool IsAddTake { get; set; }
        //默认获取分页数量
        internal int TakeNum { get; set; } = 20;


        //分组sql语句 
        internal string SqlGroupByStr { get; set; }
        //是否添加分组
        internal bool IsAddGroupBy
        {
            get
            {
                if (string.IsNullOrEmpty(SqlGroupByStr))
                {
                    return false;
                }
                return true;
            }
        }

        //Sql In 语句 如 x.UserName.In(new list<string>(){"name1","name2"})
        internal string SqlInStr { get; set; }

        //新增Sql
        internal string SqlInsertStr { get; set; }

        //更新sql
        internal string SqlUpdateStr { get; set; }

        //删除sql
        internal string SqlDeleteStr { get; set; }

        //计算sql
        internal string SqlCalculateStr { get; set; }

        internal string Sql
        {
            get { return this.ToString(); }
        }

        //参数值集合
        internal Dictionary<string, object> DbParams { get; private set; }

        internal SqlBuilder(ISqlParser dbSqlParser)
        {
            this.DbParams = new Dictionary<string, object>();

            this.SelectFields = new List<string>();
            this.SelectFieldsAlias = new List<string>();
            this._dbSqlParser = dbSqlParser;
        }

        internal void SetSqlCommandType(SqlCommandType type)
        {
            this._sqlCommandType = type;
        }

        //清除查询字段
        internal void ClearSelectFields()
        {
            this.SqlSelectStr = string.Empty;
            this.IsAddSelect = false;
            this.SelectFields.Clear();
            this.SelectFieldsAlias.Clear();
        }

        internal void Clear()
        {
            this.IsAddSelect = false;
            this.SelectFields.Clear();
            this.SelectFieldsAlias.Clear();
            this.DbParams.Clear();
            this._dicTableName.Clear();
            this._queueEnglishWords = new Queue<string>(S_listEnglishWords);
            this.SqlSelectStr = string.Empty;
            this.SqlOrderStr = string.Empty;
            this.SqlWhereStr = string.Empty;
            this.SqlDeleteStr = string.Empty;
            this.SqlGroupByStr = string.Empty;
            this.SqlInsertStr = string.Empty;
            this.SqlJoinStr = string.Empty;
            this.SqlUpdateStr = string.Empty;
            this.SqlInStr = string.Empty;
        }

        //添加参数
        internal string AddDbParameter(object dbParamValue)
        {
            string dbParamName = "";
            dbParamName = this._dbSqlParser.ParamPrefix + "param" + this.DbParams.Count;
            this.DbParams.Add(dbParamName, dbParamValue);

            return dbParamName;
        }

        //设置Table别名
        internal bool SetTableAlias(string tableName)
        {
            if (!this._dicTableName.Keys.Contains(tableName))
            {
                this._dicTableName.Add(tableName, this._queueEnglishWords.Dequeue());
                return true;
            }
            return false;
        }

        //获取Table别名
        internal string GetTableAlias(string tableName)
        {
            if (!this.IsSingleTable && this._dicTableName.Keys.Contains(tableName))
            {
                return this._dicTableName[tableName];
            }
            return "";
        }

        public override string ToString()
        {
            //新增
            if (_sqlCommandType == SqlCommandType.Insert)
            {
                return SqlInsertStr;
            }

            var sqlBuilder = new StringBuilder();

            //查询
            if (_sqlCommandType == SqlCommandType.Select)
            {
                if (!IsAddSkip)
                {
                    sqlBuilder.AppendFormat(SqlSelectStr, SelectFieldsStr);

                    if (IsAddJoin)
                    {
                        sqlBuilder.Append(SqlJoinStr);
                    }

                    if (IsAddWhere)
                    {
                        sqlBuilder.Append(SqlWhereStr);
                    }

                    if (IsAddOrder)
                    {
                        sqlBuilder.Append(SqlOrderStr);
                    }
                }
                else
                {
                    if (!IsAddOrder)
                    {
                        throw new Exception("分页查询必须要进行排序");
                    }

                    sqlBuilder.Append($"select {SelectPageFieldsStr} from (");

                    var pageSelectFieldsStr = $"{SelectFieldsStr},ROW_NUMBER() OVER ({SqlOrderStr}) AS RowNumber ";

                    sqlBuilder.AppendFormat(SqlSelectStr, pageSelectFieldsStr);

                    if (IsAddJoin)
                    {
                        sqlBuilder.Append(SqlJoinStr);
                    }

                    if (IsAddWhere)
                    {
                        sqlBuilder.Append(SqlWhereStr);
                    }

                    sqlBuilder.Append($") PageTable");

                    sqlBuilder.Append($" where PageTable.RowNumber>{SkipNum} and PageTable.RowNumber<={SkipNum + TakeNum}");
                }

                return sqlBuilder.ToString();
            }

            //更新
            if (_sqlCommandType == SqlCommandType.Update)
            {
                sqlBuilder.Append(SqlUpdateStr);

                if (IsAddWhere)
                {
                    sqlBuilder.Append(SqlWhereStr);
                }

                return sqlBuilder.ToString();
            }

            //删除
            if (_sqlCommandType == SqlCommandType.Delete)
            {
                sqlBuilder.Append(SqlDeleteStr);

                if (IsAddWhere)
                {
                    sqlBuilder.Append(SqlWhereStr);
                }

                return sqlBuilder.ToString();
            }

            //计算
            if (_sqlCommandType == SqlCommandType.Calculate)
            {
                sqlBuilder.Append(SqlCalculateStr);

                if (IsAddWhere)
                {
                    sqlBuilder.Append(SqlWhereStr);
                }

                if (IsAddGroupBy)
                {
                    sqlBuilder.Append(SqlGroupByStr);
                }

                return sqlBuilder.ToString();
            }

            return sqlBuilder.ToString();
        }
    }
}