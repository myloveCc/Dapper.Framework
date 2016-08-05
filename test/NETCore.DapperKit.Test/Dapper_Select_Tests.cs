using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NETCore.DapperKit;
using NETCore.SQLKit;
using Dapper;

namespace NETCore.DapperKit.Test
{
    public class Dapper_Select_Tests
    {
        DapperRepository<SysUser> _Repository;
        SqlQueryable<SysUser> _Queryable;

        public Dapper_Select_Tests()
        {
            _Repository = new DapperRepository<SysUser>();
            _Queryable = QueryableFactory<SysUser>.SQLServerQueryable();
        }

        [Fact(DisplayName = "根据Id获取SysUser")]
        public void Select_By_Id()
        {
            var user = _Repository.GetInfo(1);
            Assert.NotNull(user);
            Assert.Equal(user.Id, 1);
            Assert.Equal(user.UserName, "张三");

            user = _Repository.GetInfo(6);
            Assert.Null(user);
        }

        [Fact(DisplayName = "根据sql语句和参数获取SysUser")]
        public void Select_By_Sql()
        {
            var sql = "select *　from SysUser where id=@Id";
            var param = new { Id = 1 };
            using (var conn = _Repository.GetDbConnection())
            {
                var user = conn.QueryFirstOrDefault<SysUser>(sql, param);
                Assert.NotNull(user);
                Assert.Equal(user.Id, 1);
                Assert.Equal(user.UserName, "张三");
            }
        }

        [Fact(DisplayName = "根据SqlQueryabl获取sql语句和参数，再获取SysUser")]
        public void Select_By_Queryable()
        {
            var query = _Queryable.Select().Where(m => m.Id == 1);
            var sql = query.Sql;
            var param = query.DbParams.ToDynamicParameters();
            var user = _Repository.GetInfo(sql, param);
            Assert.NotNull(user);
            Assert.Equal(user.Id, 1);
            Assert.Equal(user.UserName, "张三");
        }

        [Fact(DisplayName = "获取全部SysUsers")]
        public void Select_All()
        {
            var users = _Repository.GetAllInfos();
            Assert.NotNull(users);
            Assert.Equal(users.Count(), 5);
        }


        [Fact(DisplayName = "根据sql获取全部SysUsers")]
        public void Select_All_Sql()
        {
            var sql = "select * from SysUser";
            using (var conn = _Repository.GetDbConnection())
            {
                var users = conn.Query<SysUser>(sql);
                Assert.NotNull(users);
                Assert.Equal(users.Count(), 5);
            }
        }

        [Fact(DisplayName = "根据SqlQueryable获取全部SysUsers")]
        public void Select_All_SqlQueryable()
        {
            var query = _Queryable.Select();
            var sql = query.Sql;
            using (var conn = _Repository.GetDbConnection())
            {
                var users = conn.Query<SysUser>(sql);
                Assert.NotNull(users);
                Assert.Equal(users.Count(), 5);
            }
        }

        [Fact(DisplayName = "左连接获取单条数据")]
        public void Select_LeftJoin()
        {
            var query = _Queryable.Select<SysUserRole>((u, r) => new SysUserDTO() { Id = u.Id, UserName = u.UserName, UserRoleNo = u.UserRoleNo, UserRoleName = r.Name })
                                  .LeftJoin<SysUser, SysUserRole>((u, r) => u.UserRoleNo == r.No)
                                  .Where(u => u.Id == 1);
            var sql = query.Sql;
            var param = query.DbParams.ToDynamicParameters();
            using (var conn = _Repository.GetDbConnection())
            {
                var userDTO = conn.QueryFirstOrDefault<SysUserDTO>(sql, param);
                Assert.NotNull(userDTO);
                Assert.Equal(userDTO.Id, 1);
                Assert.Equal(userDTO.UserRoleNo, "UR00001");
                Assert.Equal(userDTO.UserRoleName, "管理员");
            }
        }

        [Fact(DisplayName = "左连接获取多条数据")]
        public void Select_LeftJoin_List()
        {
            var query = _Queryable.Select<SysUserRole>((u, r) => new SysUserDTO() { Id = u.Id, UserName = u.UserName, UserRoleNo = u.UserRoleNo, UserRoleName = r.Name }).LeftJoin<SysUser, SysUserRole>((u, r) => u.UserRoleNo == r.No);
            var sql = query.Sql;
            using (var conn = _Repository.GetDbConnection())
            {
                var userDTOs = conn.Query<SysUserDTO>(sql);
                Assert.NotNull(userDTOs);
                Assert.Equal(userDTOs.Count(), 5);
            }
        }
    }
}
