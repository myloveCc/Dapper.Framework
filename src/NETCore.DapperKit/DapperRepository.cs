using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using NETCore.DapperKit.Extensions;
using MySql.Data.MySqlClient;

namespace NETCore.DapperKit
{
    public partial class DapperRepository<T> : IDapperRepository<T> where T : class
    {
        #region Conn
        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            if (string.IsNullOrEmpty(DapperConfig.ConnectionString))
            {
                throw new Exception("请在DapperConfig.json中配置ConnectionString地址");
            }

            IDbConnection conn = null;

            if (DapperConfig.DbType == DataBaseType.SQLServer)
            {
                conn = new SqlConnection(DapperConfig.ConnectionString);
                conn.Open();
            }

            if (DapperConfig.DbType == DataBaseType.MySQL)
            {
                conn = new MySqlConnection(DapperConfig.ConnectionString);
                conn.Open();
            }

            if (conn == null)
            {
                throw new Exception("请检查DapperConfig.json中的配置是否正确");
            }

            return conn;
        }

        #endregion

        #region Insert

        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns></returns>
        public long Insert(T entity)
        {
            using (var conn = GetDbConnection())
            {
                var result = conn.Insert(entity);
                return result;
            }
        }

        /// <summary>
        /// 新增信息集合
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns></returns>
        public bool Insert(IEnumerable<T> entities)
        {
            using (var conn = GetDbConnection())
            {
                var result = conn.Insert(entities) > 0;
                return result;
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>true 成功 false 失败</returns>
        public bool Update(T entity)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Update(entity);
            }
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数集合<see cref="DynamicParameters"/></param>
        /// <returns>true 成功 false 失败</returns>
        public bool Update(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.Execute(sql, param) > 0;
            }
        }

        /// <summary>
        /// 批量更新信息
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        public bool Update(IEnumerable<T> entities)
        {
            using (var conn = GetDbConnection())
            {
                var result = conn.Update(entities);
                return result;
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>true 成功 false 失败</returns>
        public bool Delete(T entity)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Delete(entity);
            }
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public bool Delete(object id)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Delete<T>(id);
            }
        }

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="entity">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        public bool Delete(IEnumerable<T> entities)
        {
            using (var conn = GetDbConnection())
            {
                var result = conn.Delete(entities);
                return result;
            }
        }

        /// <summary>
        /// 删除全部信息
        /// </summary>
        /// <returns>true 成功 false 失败</returns>
        public bool DeleteAll()
        {
            using (var conn = GetDbConnection())
            {
                var result = conn.DeleteAll<T>();
                return result;
            }
        }

        #endregion

        #region Select

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>信息</returns>
        public T GetInfo(object id)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Get<T>(id);
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <param name="param">参数对象 <see cref="DynamicParameters"/></param>
        /// <returns></returns>
        public T GetInfo(string sql, DynamicParameters param)
        {
            using (var conn = GetDbConnection())
            {
                return conn.QueryFirstOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <returns>信息集合</returns>
        public IEnumerable<T> GetAllInfos()
        {
            using (var conn = GetDbConnection())
            {
                return conn.GetAll<T>();
            }
        }
        #endregion

        #region Transation

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <returns></returns>
        public bool Transation(Action<IDbConnection, IDbTransaction> action)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        action(conn, tran);
                        //事务提交
                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //事务回滚
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        #endregion
    }
}
