using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NETCore.SQLKit;

namespace NETCore.SqlKit.Test
{
    public class SqlQueryable_Insert_Tests
    {

        SqlQueryable<SysUser> _Queryable;

        public SqlQueryable_Insert_Tests()
        {
            _Queryable = _Queryable = QueryableFactory<SysUser>.SQLServerQueryable();
            _Queryable.Clear();
        }

        [Fact(DisplayName = "新增（New）")]
        public void Insert_Simple_WithNew()
        {
            var query = _Queryable.Insert(() => new SysUser() { Id = 1, UserName = "测试", LoginName = "test", LoginPwd = "111", CreateTime = DateTime.Now, IsAdmin = true, Phone = "18701283105", UserRoleNo = "UR00001", TestNotColumn = "222" });
            var sql = query.Sql;
            var paras = query.DbParams;
            Assert.Equal(sql, "insert into [SysUser] ( UserName,LoginName,LoginPwd,CreateTime,IsAdmin,Phone,UserRoleNo ) values ( @param0 , @param1 , @param2 , @param3 , @param4 , @param5 , @param6 );");

            //Id和TestNotColumn均被排除
            Assert.Equal(paras.Count(), 7);
            Assert.Equal(paras["@param0"], "测试");
            Assert.Equal(paras["@param1"], "test");
            Assert.Equal(paras["@param2"], "111");
            Assert.IsType(typeof(DateTime), paras["@param3"]);
            Assert.Equal(paras["@param4"], 1);
            Assert.Equal(paras["@param5"], "18701283105");
            Assert.Equal(paras["@param6"], "UR00001");
        }

        [Fact(DisplayName = "新增（实例对象）")]
        public void Insert_Simple_WithEntity()
        {
            var userName = "测试1";
            var user = new SysUser() { Id = 1, UserName = userName, LoginName = "test", LoginPwd = "111", CreateTime = DateTime.Now, IsAdmin = true, Phone = "18701283105", UserRoleNo = "UR00001", TestNotColumn = "222" };
            var query = _Queryable.Insert(() => user);
            var sql = query.Sql;
            var paras = query.DbParams;
            Assert.Equal(sql, "insert into [SysUser] ( LoginName,LoginPwd,UserRoleNo,UserName,Phone,IsAdmin,CreateTime ) values ( @param0 , @param1 , @param2 , @param3 , @param4 , @param5 , @param6 );");

            //Id和TestNotColumn均被排除
            Assert.Equal(paras.Count(), 7);
            Assert.Equal(paras["@param0"], "test");
            Assert.Equal(paras["@param1"], "111");
            Assert.Equal(paras["@param2"], "UR00001");
            Assert.Equal(paras["@param3"], "测试1");
            Assert.Equal(paras["@param4"], "18701283105");
            Assert.Equal(paras["@param5"], 1);
            Assert.IsType(typeof(DateTime), paras["@param6"]);
        }
    }
}
