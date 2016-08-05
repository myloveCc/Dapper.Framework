using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NETCore.DapperKit
{
    public interface IDapperRepository<T> where T : class
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        IDbConnection GetDbConnection();

        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>返回自增Id</returns>
        long Insert(T entity);

        /// <summary>
        /// 新增信息集合
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        bool Insert(IEnumerable<T> entities);

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>true 成功 false 失败</returns>
        bool Update(T entity);

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数集合<see cref="DynamicParameters"/></param>
        /// <returns>true 成功 false 失败</returns>
        bool Update(string sql, DynamicParameters param);

        /// <summary>
        /// 更新信息集合
        /// </summary>
        /// <param name="entities">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        bool Update(IEnumerable<T> entities);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns>true 成功 false 失败</returns>
        bool Delete(T entity);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>true 成功 false 失败</returns>
        bool Delete(object id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="entity">信息实体集合</param>
        /// <returns>true 成功 false 失败</returns>
        bool Delete(IEnumerable<T> entities);

        /// <summary>
        /// 删除全部信息
        /// </summary>
        /// <returns></returns>
        bool DeleteAll();

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>信息</returns>
        T GetInfo(object id);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象<see cref="DynamicParameters"/></param>
        /// <returns></returns>
        T GetInfo(string sql, DynamicParameters param);

        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <returns>信息集合</returns>
        IEnumerable<T> GetAllInfos();

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="action">执行事务方法</param>
        /// <returns></returns>
        bool Transation(Action<IDbConnection, IDbTransaction> action);

    }
}
