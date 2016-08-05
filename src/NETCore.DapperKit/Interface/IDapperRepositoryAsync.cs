using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.DapperKit
{
    interface IDapperRepositoryAsync<T> where T : class
    {
        /// <summary>
        /// 新增信息(异步)
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>返回自增Id</returns>
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// 新增信息集合(异步)
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        Task<int> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// 更新信息(异步)
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// 更新信息(异步)
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数集合<see cref="DynamicParameters"/></param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> UpdateAsync(string sql, DynamicParameters param);

        /// <summary>
        /// 更新信息集合(异步)
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// 删除信息(异步)
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// 删除信息(异步)
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> DeleteAsync(object id);

        /// <summary>
        /// 删除信息集合(异步)
        /// </summary>
        /// <param name="entity">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// 获取信息(异步)
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>信息</returns>
        Task<T> GetInfoAsync(object id);

        /// <summary>
        /// 获取信息(异步)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象<see cref="DynamicParameters"/></param>
        /// <returns></returns>
        Task<T> GetInfoAsync(string sql, DynamicParameters param);

        /// <summary>
        /// 获取全部信息(异步)
        /// </summary>
        /// <returns>信息集合</returns>
        Task<IEnumerable<T>> GetAllInfosAsync();


        /// <summary>
        /// 事务处理(异步)
        /// </summary>
        /// <param name="action">执行事务方法</param>
        /// <returns></returns>
        Task<bool> TransationAsync(Action<IDbConnection, IDbTransaction> action);
    }
}
