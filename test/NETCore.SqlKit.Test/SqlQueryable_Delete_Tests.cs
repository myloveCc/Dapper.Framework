using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NETCore.SQLKit;
using System.Reflection;
using Xunit;

namespace NETCore.SqlKit.Test
{
    public class SqlQueryable_Delete_Tests
    {
        SqlQueryable<SysUser> _Queryable;
        public SqlQueryable_Delete_Tests()
        {
            _Queryable = QueryableFactory<SysUser>.SQLServerQueryable();
            _Queryable.Clear();
        }

        [Fact(DisplayName = "删除全部")]
        public void Delete_Test_New()
        {

            var query = _Queryable.Delete();
            var sql = query.Sql;

            Assert.Equal(sql, "delete [SysUser];");

        }

        [Fact(DisplayName = "根据条件删除")]
        public void Delete_Test_Where()
        {
            var query = _Queryable.Delete().Where(m => m.Id == 1);
            var sql = query.Sql;

            Assert.Equal(sql, "delete [SysUser] where Id = @param0;");
        }

    }
}
