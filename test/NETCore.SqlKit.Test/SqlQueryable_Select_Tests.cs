using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NETCore.SQLKit;
using Xunit;

namespace NETCore.SqlKit.Test
{
    public class SqlQueryable_Select_Tests
    {
        SqlQueryable<SysUser> sqlQuery;

        public SqlQueryable_Select_Tests()
        {
            sqlQuery =  QueryableFactory<SysUser>.SQLServerQueryable();
            sqlQuery.Clear();
        }

        [Fact(DisplayName = "查询全部列")]
        public void Select_Simple()
        {
            var query = sqlQuery.Select();
            var sql = query.Sql;
            Assert.NotNull(sql);
            Assert.NotEmpty(sql);
            Assert.Equal(sql, "select * from [SysUser] a;");
        }

        [Fact(DisplayName = "查询单列")]
        public void Select_Simple_Column()
        {
            var query = sqlQuery.Select(m => m.Id);
            var sql = query.Sql;
            Assert.NotNull(sql);
            Assert.NotEmpty(sql);
            Assert.Equal(sql, "select a.Id from [SysUser] a;");
        }

        [Fact(DisplayName = "查询多列")]
        public void Select_Simple_Columns()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select(m => new { m.Id, m.UserName, m.TestNotColumn });
            var sql = query.Sql;
            Assert.NotNull(sql);
            Assert.NotEmpty(sql);
            Assert.Equal(sql, "select a.Id [Id],a.UserName [UserName] from [SysUser] a;");
        }

        [Fact(DisplayName = "单条件查询")]
        public void Select_Simple_Where()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select().Where(m => m.Id == 1);
            var sql = query.Sql;
            Assert.NotNull(sql);
            Assert.NotEmpty(sql);

            Assert.Equal(sql, "select * from [SysUser] a where a.Id = @param0;");
        }

        [Fact(DisplayName = "全部列多条件查询")]
        public void Select_Simple_WhereMany()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select().Where(m => m.Id == 1 && m.UserName == null);
            var sql = query.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.Where(m => m.Id == 1 && m.UserName == null).Select();
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.NotNull(sql);
            Assert.NotEmpty(sql);

            Assert.Equal(sql, "select * from [SysUser] a where a.Id = @param0 and a.UserName is @param1;");
        }

        [Fact(DisplayName = "单列多条件查询")]
        public void Select_Simple_Column_WhereMany()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select(m => m.Id).Where(m => m.Id == 1 && m.UserName == null);
            var sql = query.Sql;
            sqlQuery.Clear();
            var query2 = sqlQuery.Where(m => m.Id == 1 && m.UserName == null).Select(m => m.Id);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);
            Assert.NotNull(sql);
            Assert.NotEmpty(sql);

            Assert.Equal(sql, "select a.Id from [SysUser] a where a.Id = @param0 and a.UserName is @param1;");
        }

        [Fact(DisplayName = "多列多条件查询（True）")]
        public void Select_Simple_Columns_WhereMany_True()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select(m => new { UserId = m.Id, m.UserName, m.TestNotColumn }).Where(m => m.Id == 1 && m.UserName == null).Where(m => m.IsAdmin);
            var sql = query1.Sql;
            sqlQuery.Clear();
            var query2 = sqlQuery.Where(m => m.Id == 1 && m.UserName == null).Select(m => new { UserId = m.Id, m.UserName }).Where(m => m.IsAdmin);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.NotNull(sql);
            Assert.NotEmpty(sql);

            Assert.Equal(sql, "select a.Id [UserId],a.UserName [UserName] from [SysUser] a where a.Id = @param0 and a.UserName is @param1 and a.IsAdmin = @param2;");

            var selParams = query1.DbParams;
            Assert.Equal(selParams.Count(), 3);
            Assert.Equal(selParams["@param2"], 1);
        }

        [Fact(DisplayName = "多列多条件（False）")]
        public void Select_Simple_Comlumns_WhereMany_False()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select(m => new { UserId = m.Id, m.UserName, m.TestNotColumn }).Where(m => m.Id == 1 && m.UserName == null).Where(m => !m.IsAdmin);
            var sql = query1.Sql;
            sqlQuery.Clear();
            var query2 = sqlQuery.Where(m => m.Id == 1 && m.UserName == null).Select(m => new { UserId = m.Id, m.UserName }).Where(m => !m.IsAdmin);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.NotNull(sql);
            Assert.NotEmpty(sql);

            Assert.Equal(sql, "select a.Id [UserId],a.UserName [UserName] from [SysUser] a where a.Id = @param0 and a.UserName is @param1 and a.IsAdmin = @param2;");

            var selParams = query1.DbParams;
            Assert.Equal(selParams.Count(), 3);
            Assert.Equal(selParams["@param2"], 0);
        }

        [Fact(DisplayName = "全列单排序查询")]
        public void Selec_OrderBy()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select().OrderBy(m => m.Id);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderBy(m => m.Id).Select();
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select * from [SysUser] a order by a.Id;");
        }

        [Fact(DisplayName = "单列单排序查询")]
        public void Selct_Column_OrderBy()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select(m => m.Id).OrderBy(m => m.Id);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderBy(m => m.Id).Select(m => m.Id);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select a.Id from [SysUser] a order by a.Id;");
        }


        [Fact(DisplayName = "全列单排序查询(倒序)")]
        public void Selec_OrderBy_Desc()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select().OrderByDescending(m => m.Id);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderByDescending(m => m.Id).Select();
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select * from [SysUser] a order by a.Id desc;");
        }

        [Fact(DisplayName = "单列单排序查询（倒序）")]
        public void Selct_Column_OrderBy_Desc()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select(m => m.Id).OrderByDescending(m => m.Id);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderByDescending(m => m.Id).Select(m => m.Id);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select a.Id from [SysUser] a order by a.Id desc;");
        }

        [Fact(DisplayName = "全列多排序查询")]
        public void Select_OrderBy_Many()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select().OrderBy(m => m.Id).ThenBy(m => m.CreateTime);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Select();
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select * from [SysUser] a order by a.Id,a.CreateTime;");
        }

        [Fact(DisplayName = "单列多排序查询")]
        public void Select_Column_OrderBy_Many()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select(m => m.Id).OrderBy(m => m.Id).ThenBy(m => m.CreateTime);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Select(m => m.Id);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select a.Id from [SysUser] a order by a.Id,a.CreateTime;");
        }


        [Fact(DisplayName = "多列多排序查询")]
        public void Select_Columns_OrderBy_Many()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select(m => new { UserId = m.Id, UserName = m.UserName }).OrderBy(m => m.Id).ThenBy(m => m.CreateTime);
            var sql = query1.Sql;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Select(m => new { UserId = m.Id, UserName = m.UserName });
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);

            Assert.Equal(sql, "select a.Id [UserId],a.UserName [UserName] from [SysUser] a order by a.Id,a.CreateTime;");
        }

        [Fact(DisplayName = "测试两表连接查询")]
        public void Select_Join()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole>((u, r) => new { UserId = u.Id, UaerName = u.UserName, TestNotColumn = u.TestNotColumn, RoleNo = r.No, RoleName = r.Name }).OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Join<SysUserRole>((u, r) => u.UserRoleNo == r.No).Where(u => u.Id == 13 && u.IsAdmin);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Select<SysUserRole>((u, r) => new { UserId = u.Id, UaerName = u.UserName, RoleNo = r.No, RoleName = r.Name }).Join<SysUserRole>((u, r) => u.UserRoleNo == r.No).Where(u => u.Id == 13 && u.IsAdmin);
            var sql2 = query2.Sql;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras.Count(), 2);
            Assert.Equal(paras["@param0"], 13);
            Assert.Equal(paras["@param1"], 1);

            sqlQuery.Clear();
            var query3 = sqlQuery.Select<SysUserRole>((u, r) => new { UserId = u.Id, UaerName = u.UserName, RoleNo = r.No, RoleName = r.Name }).OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Join<SysUserRole>((u, r) => u.UserRoleNo == r.No).Where(u => u.Id == 8 && !u.IsAdmin);
            var sql3 = query3.Sql;
            var paras2 = query3.DbParams;

            sqlQuery.Clear();
            var query4 = sqlQuery.OrderBy(m => m.Id).ThenBy(m => m.CreateTime).Select<SysUserRole>((u, r) => new { UserId = u.Id, UaerName = u.UserName, RoleNo = r.No, RoleName = r.Name }).Join<SysUserRole>((u, r) => u.UserRoleNo == r.No).Where(u => u.Id == 8 && !u.IsAdmin);
            var sql4 = query4.Sql;

            Assert.Equal(sql3, sql4);
            Assert.Equal(paras.Count(), 2);
            Assert.Equal(paras["@param0"], 8);
            Assert.Equal(paras["@param1"], 0);
        }

        [Fact(DisplayName = "测试多表连接查询（True）")]
        public void Select_JoinMany()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo, TestNotColumn = u.TestNotColumn, }).Join<SysUserRole>((u, r) => u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).Join<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo, TestNotColumn = u.TestNotColumn, }).Join<SysUserRole>((u, r) => u.IsAdmin == true && u.UserRoleNo == r.No && u.Id == 13).Join<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);
        }

        [Fact(DisplayName = "测试多表连接查询（Fale）")]
        public void Select_JoinMany_False()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).Join<SysUserRole>((u, r) => !u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).Join<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).Join<SysUserRole>((u, r) => u.IsAdmin == false && u.UserRoleNo == r.No && u.Id == 13).Join<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);
        }

        [Fact(DisplayName = "测试两表左连接查询")]
        public void Select_LeftJoin()
        {

        }

        [Fact(DisplayName = "测试多表左连接查询（True）")]
        public void Select_LeftJoinMany()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).LeftJoin<SysUserRole>((u, r) => u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13 && u.TestNotColumn == "111").Join<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).LeftJoin<SysUserRole>((u, r) => u.IsAdmin == true && u.UserRoleNo == r.No && u.Id == 13 && u.TestNotColumn == "111").Join<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);

        }

        [Fact(DisplayName = "测试多表左连接查询（False）")]
        public void Select_LeftJoinMany_False()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).LeftJoin<SysUserRole>((u, r) => !u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).LeftJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).LeftJoin<SysUserRole>((u, r) => u.IsAdmin == false && u.UserRoleNo == r.No && u.Id == 13).LeftJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);

        }

        [Fact(DisplayName = "测试两表右连接查询")]
        public void Select_RightJoin()
        {

        }

        [Fact(DisplayName = "测试多表右连接查询(True)")]
        public void Select_RightJoinMany()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).RightJoin<SysUserRole>((u, r) => u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).RightJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).RightJoin<SysUserRole>((u, r) => u.IsAdmin == true && u.UserRoleNo == r.No && u.Id == 13).RightJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);
        }

        [Fact(DisplayName = "测试多表右连接查询(False)")]
        public void Select_RightJoinMany_False()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).RightJoin<SysUserRole>((u, r) => !u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).RightJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).RightJoin<SysUserRole>((u, r) => u.IsAdmin == false && u.UserRoleNo == r.No && u.Id == 13).RightJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);
        }

        [Fact(DisplayName = "测试多表右连接查询(True)")]
        public void Select_InnerJoinMany()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).InnerJoin<SysUserRole>((u, r) => u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).InnerJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).InnerJoin<SysUserRole>((u, r) => u.IsAdmin == true && u.UserRoleNo == r.No && u.Id == 13).InnerJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);
        }

        [Fact(DisplayName = "测试多表右连接查询(False)")]
        public void Select_InnerJoinMany_False()
        {
            sqlQuery.Clear();
            var query1 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).InnerJoin<SysUserRole>((u, r) => !u.IsAdmin && u.UserRoleNo == r.No && u.Id == 13).InnerJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql = query1.Sql;
            var paras = query1.DbParams;

            sqlQuery.Clear();
            var query2 = sqlQuery.Select<SysUserRole, SysUserRolePowers>((u, r, p) => new { UserId = u.Id, RoleName = r.Name, PowerNo = p.PowerNo }).InnerJoin<SysUserRole>((u, r) => u.IsAdmin == false && u.UserRoleNo == r.No && u.Id == 13).InnerJoin<SysUserRole, SysUserRolePowers>((r, p) => r.No == p.RoleNo).Where(m => m.Id == 8);
            var sql2 = query2.Sql;
            var paras2 = query1.DbParams;

            Assert.Equal(sql, sql2);
            Assert.Equal(paras, paras2);
        }

        [Fact(DisplayName = "测试单条件Where查询")]
        public void Select_Where()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select(m => m.TestNotColumn).Where(m => m.Id == 8);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.NotNull(sql);
            Assert.Equal(sql, "select * from [SysUser] a where a.Id = @param0;");

            Assert.Equal(paras["@param0"], 8);
        }

        [Fact(DisplayName = "测试单条件Where查询(True)")]
        public void Select_Where_True()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select().Where(m => m.IsAdmin);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.NotNull(sql);
            Assert.Equal(sql, "select * from [SysUser] a where a.IsAdmin = @param0;");

            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "测试单条件Where查询(False)")]
        public void Select_Where_False()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select().Where(m => m.IsAdmin == false);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.NotNull(sql);
            Assert.Equal(sql, "select * from [SysUser] a where a.IsAdmin = @param0;");

            Assert.Equal(paras["@param0"], 0);
        }

        [Fact(DisplayName = "测试多条件Where查询")]
        public void Select_Where_Many()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select().Where(m => m.Id == 8 && m.UserName.Like("111") && m.TestNotColumn == "test");
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.NotNull(sql);
            Assert.Equal(sql, "select * from [SysUser] a where a.Id = @param0 and a.UserName like '%' + @param1 + '%' ;");

            Assert.Equal(paras["@param0"], 8);
            Assert.Equal(paras["@param1"], "111");
        }

        [Fact(DisplayName = "测试多条件Where查询(True)")]
        public void Select_Where_Many_True()
        {
            sqlQuery.Clear();
            var query = sqlQuery.Select().Where(m => m.Id == 8 && m.IsAdmin);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.NotNull(sql);
            Assert.Equal(sql, "select * from [SysUser] a where a.Id = @param0 and a.IsAdmin = @param1;");

            Assert.Equal(paras["@param0"], 8);
            Assert.Equal(paras["@param1"], 1);
        }

        [Fact(DisplayName = "测试多条件Where查询(False)")]
        public void Select_Where_Many_False()
        {
            var query = sqlQuery.Select().Where(m => m.Id == 8 && !m.IsAdmin);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.NotNull(sql);
            Assert.Equal(sql, "select * from [SysUser] a where a.Id = @param0 and a.IsAdmin = @param1;");

            Assert.Equal(paras["@param0"], 8);
            Assert.Equal(paras["@param1"], 0);
        }

        [Fact(DisplayName = "查询单表Where In Int数组 2")]
        public void Select_Where_In_IntArray2()
        {
            var idArray = new int[] { 1, 2, 3 };
            var query = sqlQuery.Select().Where(m => m.Id.In(idArray));
            var sql = query.Sql;

            Assert.Equal(sql, "select * from [SysUser] a where a.Id in( 1 , 2 , 3 );");
        }

        [Fact(DisplayName = "查询单表Where In Int数组 3")]
        public void Select_Where_In_IntArray3()
        {
            var query = sqlQuery.Select().Where(m => m.Id.In(new int[] { 1, 2, 3 }));
            var sql = query.Sql;

            Assert.Equal(sql, "select * from [SysUser] a where a.Id in( 1 , 2 , 3 );");
        }

        [Fact(DisplayName = "查询单表Where In String 数组 1")]
        public void Select_Where_In_StringArray1()
        {
            var query = sqlQuery.Select().Where(m => m.UserName.In(new string[] { "test1", "test2" }));
            var sql = query.Sql;

            Assert.Equal(sql, "select * from [SysUser] a where a.UserName in( 'test1' , 'test2' );");
        }

        [Fact(DisplayName = "查询单表Where In String 数组 2")]
        public void Select_Where_In_StringArray2()
        {
            var stringArray = new string[] { "test1", "test2" };
            var query = sqlQuery.Select().Where(m => m.UserName.In(stringArray));
            var sql = query.Sql;

            Assert.Equal(sql, "select * from [SysUser] a where a.UserName in( 'test1' , 'test2' );");
        }

        [Fact(DisplayName = "查询单表Where In Int列表 1")]
        public void Select_Where_In_IntList()
        {
            var query = sqlQuery.Select().Where(m => m.Id.In(new List<int>() { 1, 2, 3 }));
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a where a.Id in( 1 , 2 , 3 );");
        }

        [Fact(DisplayName = "查询单表Where In Int列表 2")]
        public void Select_Where_In_IntList2()
        {
            var idList = new List<int>() { 1, 2, 3 };
            var query = sqlQuery.Select().Where(m => m.Id.In(idList));
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a where a.Id in( 1 , 2 , 3 );");
        }

        [Fact(DisplayName = "查询单表Where In String列表 1")]
        public void Select_Where_In_StringList1()
        {
            var query = sqlQuery.Select().Where(m => m.Id.In(new List<string>() { "test1", "test2" }));
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a where a.Id in( 'test1' , 'test2' );");
        }

        [Fact(DisplayName = "查询单表Where In String列表 2")]
        public void Select_Where_In_StringList2()
        {
            var nameList = new List<string>() { "test1", "test2" };
            var query = sqlQuery.Select().Where(m => m.Id.In(nameList));
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a where a.Id in( 'test1' , 'test2' );");
        }

        [Fact(DisplayName = "查询单表Where Like 1")]
        public void Select_Where_Like1()
        {
            var likeWord = "沈";
            var query = sqlQuery.Select().Where(m => m.UserName.Like(likeWord));
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like '%' + @param0 + '%';");
            Assert.Equal(dbParas["@param0"], likeWord);
        }

        [Fact(DisplayName = "查询单表Where Like 2")]
        public void Select_Where_Like2()
        {
            var query = sqlQuery.Select().Where(m => m.UserName.Like("test1"));
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like '%' + @param0 + '%';");
            Assert.Equal(dbParas["@param0"], "test1");
        }

        [Fact(DisplayName = "查询单表Where Like And ")]
        public void Select_Where_Like3()
        {
            var query = sqlQuery.Select().Where(m => m.UserName.Like("test1") && m.Id == 8);
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like '%' + @param0 + '%' and a.Id = @param1;");
            Assert.Equal(dbParas["@param0"], "test1");
            Assert.Equal(dbParas["@param1"], 8);
        }

        [Fact(DisplayName = "查询单表Where LikeLeft 1")]
        public void Select_Where_LikeLeft1()
        {
            var likeWord = "沈";
            var query = sqlQuery.Select().Where(m => m.UserName.LikeLeft(likeWord));
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like '%' + @param0;");
            Assert.Equal(dbParas["@param0"], likeWord);
        }

        [Fact(DisplayName = "查询单表Where LikeLeft 2")]
        public void Select_Where_LikeLeft2()
        {
            var query = sqlQuery.Select().Where(m => m.UserName.LikeLeft("test1"));
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like '%' + @param0;");
            Assert.Equal(dbParas["@param0"], "test1");
        }

        [Fact(DisplayName = "查询单表Where LikeRight 1")]
        public void Select_Where_LikeRight1()
        {
            var likeWord = "沈";
            var query = sqlQuery.Select().Where(m => m.UserName.LikeRight(likeWord));
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like @param0 + '%';");
            Assert.Equal(dbParas["@param0"], likeWord);
        }

        [Fact(DisplayName = "查询单表Where LikeRight 2")]
        public void Select_Where_LikeRight2()
        {
            var query = sqlQuery.Select().Where(m => m.UserName.LikeRight("test1"));
            var sql = query.Sql;
            var dbParas = query.DbParams;
            Assert.Equal(sql, "select * from [SysUser] a where a.UserName like @param0 + '%';");
            Assert.Equal(dbParas["@param0"], "test1");
        }


        [Fact(DisplayName = "测试单排序单属性查询(升序)")]
        public void Select_OrderBy()
        {
            var query = sqlQuery.Select().OrderBy(m => m.Id);
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id;");
        }

        [Fact(DisplayName = "测试单排序多属性查询(升序)")]
        public void Select_OrderBy_Columns()
        {
            var query = sqlQuery.Select().OrderBy(m => new { m.Id, m.CreateTime });
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id,a.CreateTime;");
        }

        [Fact(DisplayName = "测试多排序单属性查询(升序)")]
        public void Select_OrderByMany()
        {
            var query = sqlQuery.Select().OrderBy(m => m.Id).ThenBy(m => m.CreateTime);
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id,a.CreateTime;");
        }

        [Fact(DisplayName = "测试多排序多属性查询(升序)")]
        public void Select_OrderByMany_Columns()
        {
            var query = sqlQuery.Select().OrderBy(m => new { m.Id, m.UserName }).ThenBy(m => new { m.CreateTime, m.IsAdmin });
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id,a.UserName,a.CreateTime,a.IsAdmin;");
        }

        [Fact(DisplayName = "测试单排序单属性查询(降序)")]
        public void Select_OrderBy_Desc()
        {
            var query = sqlQuery.Select().OrderByDescending(m => m.Id);
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id desc;");
        }

        [Fact(DisplayName = "测试单排序多属性查询(降序)")]
        public void Select_OrderBy_Columns_Desc()
        {
            var query = sqlQuery.Select().OrderByDescending(m => new { m.Id, m.CreateTime });
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id desc,a.CreateTime desc;");
        }

        [Fact(DisplayName = "测试多排序单属性查询(降序)")]
        public void Select_OrderByMany_Desc()
        {
            var query = sqlQuery.Select().OrderByDescending(m => m.Id).ThenByDescending(m => m.CreateTime);
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id desc,a.CreateTime desc;");
        }

        [Fact(DisplayName = "测试多排序多属性查询(降序)")]
        public void Select_OrderByMany_Columns_Desc()
        {
            var query = sqlQuery.Select().OrderByDescending(m => new { m.Id, m.UserName }).ThenByDescending(m => new { m.CreateTime, m.IsAdmin });
            var sql = query.Sql;
            Assert.Equal(sql, "select * from [SysUser] a order by a.Id desc,a.UserName desc,a.CreateTime desc,a.IsAdmin desc;");
        }

        [Fact(DisplayName = "获取总数量")]
        public void Select_Count()
        {
            var query = sqlQuery.Count();
            var sql = sqlQuery.Sql;

            Assert.Equal(sql, "select Count(*) from [SysUser] a;");
        }

        [Fact(DisplayName = "获取总数量(指定列)")]
        public void Select_Count_WithColumn()
        {
            var query = sqlQuery.Count(m => m.Id);
            var sql = sqlQuery.Sql;

            Assert.Equal(sql, "select Count(a.Id) from [SysUser] a;");
        }

        [Fact(DisplayName = "获取总数量(Where条件)")]
        public void Select_Count_With_Where()
        {
            var query = sqlQuery.Count(m => m.Id).Where(m => m.IsAdmin);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Count(a.Id) from [SysUser] a where a.IsAdmin = @param0;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取总数量（GroupBy条件")]
        public void Select_Count_With_GroupBy()
        {
            var query = sqlQuery.Count(m => m.Id).Where(m => m.IsAdmin).GroupBy(m => m.UserRoleNo);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Count(a.Id) from [SysUser] a where a.IsAdmin = @param0 group by a.UserRoleNo;");
            Assert.Equal(paras["@param0"], 1);
        }


        [Fact(DisplayName = "获取总和(指定列)")]
        public void Select_Sum_WithColumn()
        {
            var query = sqlQuery.Sum(m => m.Id);
            var sql = sqlQuery.Sql;

            Assert.Equal(sql, "select Sum(a.Id) from [SysUser] a;");
        }

        [Fact(DisplayName = "获取总和(Where条件)")]
        public void Select_Sum_With_Where()
        {
            var query = sqlQuery.Sum(m => m.Id).Where(m => m.IsAdmin);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Sum(a.Id) from [SysUser] a where a.IsAdmin = @param0;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取总和（GroupBy条件")]
        public void Select_Sum_With_GroupBy()
        {
            var query = sqlQuery.Sum(m => m.Id).Where(m => m.IsAdmin).GroupBy(m => m.UserRoleNo);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Sum(a.Id) from [SysUser] a where a.IsAdmin = @param0 group by a.UserRoleNo;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取平均值(指定列)")]
        public void Select_Avg_WithColumn()
        {
            var query = sqlQuery.Avg(m => m.Id);
            var sql = sqlQuery.Sql;

            Assert.Equal(sql, "select Avg(a.Id) from [SysUser] a;");
        }

        [Fact(DisplayName = "获取平均值(Where条件)")]
        public void Select_Avg_With_Where()
        {
            var query = sqlQuery.Avg(m => m.Id).Where(m => m.IsAdmin);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Avg(a.Id) from [SysUser] a where a.IsAdmin = @param0;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取平均值（GroupBy条件")]
        public void Select_Avg_With_GroupBy()
        {
            var query = sqlQuery.Avg(m => m.Id).Where(m => m.IsAdmin).GroupBy(m => m.UserRoleNo);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Avg(a.Id) from [SysUser] a where a.IsAdmin = @param0 group by a.UserRoleNo;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取最小值(指定列)")]
        public void Select_Min_WithColumn()
        {
            var query = sqlQuery.Min(m => m.Id);
            var sql = sqlQuery.Sql;

            Assert.Equal(sql, "select Min(a.Id) from [SysUser] a;");
        }

        [Fact(DisplayName = "获取最小值(Where条件)")]
        public void Select_Min_With_Where()
        {
            var query = sqlQuery.Min(m => m.Id).Where(m => m.IsAdmin);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Min(a.Id) from [SysUser] a where a.IsAdmin = @param0;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取最小值（GroupBy条件")]
        public void Select_Min_With_GroupBy()
        {
            var query = sqlQuery.Min(m => m.Id).Where(m => m.IsAdmin).GroupBy(m => m.UserRoleNo);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Min(a.Id) from [SysUser] a where a.IsAdmin = @param0 group by a.UserRoleNo;");
            Assert.Equal(paras["@param0"], 1);
        }


        [Fact(DisplayName = "获取最大值(指定列)")]
        public void Select_Max_WithColumn()
        {
            var query = sqlQuery.Max(m => m.Id);
            var sql = sqlQuery.Sql;

            Assert.Equal(sql, "select Max(a.Id) from [SysUser] a;");
        }

        [Fact(DisplayName = "获取最大值(Where条件)")]
        public void Select_Max_With_Where()
        {
            var query = sqlQuery.Max(m => m.Id).Where(m => m.IsAdmin);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Max(a.Id) from [SysUser] a where a.IsAdmin = @param0;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取最大值（GroupBy条件")]
        public void Select_Max_With_GroupBy()
        {
            var query = sqlQuery.Max(m => m.Id).Where(m => m.IsAdmin).GroupBy(m => m.UserRoleNo);
            var sql = sqlQuery.Sql;
            var paras = sqlQuery.DbParams;
            Assert.Equal(sql, "select Max(a.Id) from [SysUser] a where a.IsAdmin = @param0 group by a.UserRoleNo;");
            Assert.Equal(paras["@param0"], 1);
        }

        [Fact(DisplayName = "获取全部列(分页)")]
        public void Select_AllColumn_Page()
        {
            var query = sqlQuery.Select().OrderBy(m => m.CreateTime).Skip(1).Take(1);
            var sql = query.Sql;

            Assert.Equal(sql, "select * from (select *,ROW_NUMBER() OVER ( order by a.CreateTime) AS RowNumber  from [SysUser] a) PageTable where PageTable.RowNumber>1 and PageTable.RowNumber<=2;");
        }

        [Fact(DisplayName = "获取部分列（分页）")]
        public void Select_SomeColumn_Page()
        {
            //自动排除非数据列
            var query = sqlQuery.Select(m => new SysUser() { Id = m.Id, UserName = m.UserName, CreateTime = m.CreateTime, TestNotColumn = m.TestNotColumn }).OrderBy(m => m.CreateTime).Skip(1).Take(1);
            var sql = query.Sql;

            Assert.Equal(sql, "select [Id],[UserName],[CreateTime] from (select a.Id [Id],a.UserName [UserName],a.CreateTime [CreateTime],ROW_NUMBER() OVER ( order by a.CreateTime) AS RowNumber  from [SysUser] a) PageTable where PageTable.RowNumber>1 and PageTable.RowNumber<=2;");
        }

        [Fact(DisplayName = "获取部分列（分页）")]
        public void Select_SomeColumn_Page1()
        {
            var query = sqlQuery.Select(m => new { Id = m.Id, UserName = m.UserName, CreateTime = m.CreateTime }).OrderBy(m => m.CreateTime).Skip(1).Take(1);
            var sql = query.Sql;

            Assert.Equal(sql, "select [Id],[UserName],[CreateTime] from (select a.Id [Id],a.UserName [UserName],a.CreateTime [CreateTime],ROW_NUMBER() OVER ( order by a.CreateTime) AS RowNumber  from [SysUser] a) PageTable where PageTable.RowNumber>1 and PageTable.RowNumber<=2;");
        }

        [Fact(DisplayName = "获取部分列，关联查询（分页）")]
        public void Select_Join_Page()
        {
            var query = sqlQuery.Select<SysUserRole>((u, r) => new { UserId = u.Id, UserName = u.UserName, UserRoleNo = u.UserRoleNo, UserRoleName = r.Name })
                                .LeftJoin<SysUserRole>((u, r) => u.UserRoleNo == r.No)
                                .OrderBy(m => m.CreateTime)
                                .Skip(0)
                                .Take(3);
            var sql = query.Sql;
            Assert.Equal(sql, "select a.Id [UserId],a.UserName [UserName],a.UserRoleNo [UserRoleNo],b.Name [UserRoleName] from [SysUser] a left join [SysUserRole] b on a.UserRoleNo = b.No order by a.CreateTime;");
        }

        [Fact(DisplayName = "终极测试（查询，关联,多条件查询，多条件排序，分页")]
        public void SeleteTest1()
        {
            var query = sqlQuery.Select<SysUserRole>((u, r) => new { Id = u.Id, Account = u.LoginName, Password = u.LoginPwd, RoleName = r.Name, CreateTime = u.CreateTime })
                                .LeftJoin<SysUserRole>((u, r) => u.UserRoleNo == r.No)
                                .Where(m => m.Id == 8)
                                .Where(m => m.UserName.Like("沈"))
                                .OrderByDescending(m => m.CreateTime)
                                .ThenBy(m => m.Id)
                                .Skip(1)
                                .Take(2);
            var sql = query.Sql;
            var paras = query.DbParams;

            Assert.Equal(sql, "select [Id],[Account],[Password],[RoleName],[CreateTime] from (select a.Id [Id],a.LoginName [Account],a.LoginPwd [Password],b.Name [RoleName],a.CreateTime [CreateTime],ROW_NUMBER() OVER ( order by a.CreateTime desc,a.Id) AS RowNumber  from [SysUser] a left join [SysUserRole] b on a.UserRoleNo = b.No where a.Id = @param0 and a.UserName like '%' + @param1 + '%') PageTable where PageTable.RowNumber>1 and PageTable.RowNumber<=3;");
        }
    }
}
