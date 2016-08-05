using NETCore.SQLKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NETCore.SqlKit.Test
{
    public class SqlQueryable_Update_Tests
    {
        SqlQueryable<SysUser> _Queryable;
        public SqlQueryable_Update_Tests()
        {
            _Queryable = QueryableFactory<SysUser>.SQLServerQueryable();
            _Queryable.Clear();
        }

        [Fact(DisplayName = "更新New对象")]
        public void Update_New()
        {
            var query = _Queryable.Update(() => new SysUser() { LoginName = "update test", UserName = "username test", TestNotColumn = "非数据列" });
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.Equal(sql, "update [SysUser] set LoginName = @param0 ,UserName = @param1 ;");

            Assert.Equal(paras.Count(), 2);
            Assert.Equal(paras["@param0"], "update test");
            Assert.Equal(paras["@param1"], "username test");
        }

        [Fact(DisplayName = "更新New对象(Where)")]
        public void Update_New_Where()
        {
            var query = _Queryable.Update(() => new SysUser() { Id = 1, LoginName = "update test", UserName = "username test" }).Where(m => m.Id == 11);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.Equal(sql, "update [SysUser] set LoginName = @param0 ,UserName = @param1  where Id = @param2;");

            Assert.Equal(paras.Count(), 3);
            Assert.Equal(paras["@param0"], "update test");
            Assert.Equal(paras["@param1"], "username test");
            Assert.Equal(paras["@param2"], 11);
        }
    }
}
