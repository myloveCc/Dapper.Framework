using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.SQLKit
{
    public class QueryableFactory<T> where T : class
    {

        public static SqlQueryable<T> SQLServerQueryable()
        {
            return new SqlQueryable<T>(new SQLServerParser());
        }


        public static SqlQueryable<T> SQLiteQueryable()
        {
            return new SqlQueryable<T>(new SQLiteParser());
        }


        public static SqlQueryable<T> MySqlQueryable()
        {
            return new SqlQueryable<T>(new MySQLParser());
        }


        public static SqlQueryable<T> OracleQueryable()
        {
            return new SqlQueryable<T>(new OracleParser());
        }
    }
}
