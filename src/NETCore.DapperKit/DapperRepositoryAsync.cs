
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using NETCore.DapperKit.Extensions;
using MySql.Data.MySqlClient;

namespace NETCore.DapperKit
{
    public partial class DapperRepository<T> : IDapperRepositoryAsync<T>
    {
        #region Insert
        /// <summary>
        /// 新增信息（异步）
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>返回自增主键Id</returns>
        public Task<int> InsertAsync(T entity)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.InsertAsync(entity);
            }
        }

        /// <summary>
        /// 新增信息集合（异步）
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns>返回新增总数量</returns>
        public Task<int> InsertAsync(IEnumerable<T> entities)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.InsertAsync(entities);
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// 更新实体（异步）
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> UpdateAsync(T entity)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// 更新信息(异步)
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数集合<see cref="DynamicParameters"/></param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> UpdateAsync(string sql, DynamicParameters param)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                using (IDbConnection conn = GetDbConnection())
                {
                    return conn.Execute(sql, param) > 0;
                }
            });
        }

        /// <summary>
        /// 更新实体集合(异步)
        /// </summary>
        /// <param name="entities">实体信息集合</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.UpdateAsync(entities);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 根据主键Id删除(异步）
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> DeleteAsync(object id)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                using (IDbConnection conn = GetDbConnection())
                {
                    return conn.Delete<T>(id);
                }
            });
        }

        /// <summary>
        /// 删除实体信息(异步)
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> DeleteAsync(T entity)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.DeleteAsync(entity);
            }
        }

        /// <summary>
        /// 删除实体信息集合（异步)
        /// </summary>
        /// <param name="entities">实体信息集合</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> DeleteAsync(IEnumerable<T> entities)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.DeleteAsync(entities);
            }
        }

        /// <summary>
        /// 删除全部信息
        /// </summary>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> DeleteAllAsync()
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.DeleteAllAsync<T>();
            }
        }

        #endregion

        #region Select

        /// <summary>
        /// 获取单个信息(异步)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>实体信息</returns>
        public Task<T> GetInfoAsync(object id)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.GetAsync<T>(id);
            }
        }

        /// <summary>
        /// 获取单个信息(异步)
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数集合<see cref="DynamicParameters"/></param>
        /// <returns></returns>
        public Task<T> GetInfoAsync(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.QueryFirstOrDefaultAsync<T>(sql, param);
            }
        }

        /// <summary>
        /// 获取全部信息(异步)
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllInfosAsync()
        {
            using (IDbConnection conn = GetDbConnection())
            {
                return conn.GetAllAsync<T>();
            }
        }

        #endregion

        #region Transation

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <returns></returns>
        public Task<bool> TransationAsync(Action<IDbConnection, IDbTransaction> action)
        {
            return Task<bool>.Factory.StartNew(() =>
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
            });
        }

        #endregion
    }
}
