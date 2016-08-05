
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NETCore.DapperKit
{
    /// <summary>
    /// Dapper配置
    /// </summary>
    public class DapperConfig
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DapperConfig()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "DapperConfig.json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("DapperConfig.json不存在", path);
            }

            var builder = new ConfigurationBuilder().AddJsonFile(path);

            IConfigurationRoot Configuration = builder.Build();

            //获取数据库类型
            if (!string.IsNullOrEmpty(Configuration["DataBaseType"]))
            {
                DbType = (DataBaseType)Enum.Parse(typeof(DataBaseType), Configuration["DataBaseType"]);
            }

            //获取连接地址
            if (!string.IsNullOrEmpty(Configuration["ConnectionString"]))
            {
                ConnectionString = Configuration["ConnectionString"];
            }
            //执行超时时间
            if (!string.IsNullOrEmpty(Configuration["CommandTimeOut"]))
            {
                CommandTimeOut = int.Parse(Configuration["CommandTimeOut"]);
            }

        }

        /// <summary>
        /// 数据库类型，默认为SQLSERVER
        /// </summary>
        public static DataBaseType DbType { get; private set; } = DataBaseType.SQLServer;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// 执行命令超时时间,可为NULL
        /// </summary>
        public static int? CommandTimeOut { get; private set; } = null;

    }
}
