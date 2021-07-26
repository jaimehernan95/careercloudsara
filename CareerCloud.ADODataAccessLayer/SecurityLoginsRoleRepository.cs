using System;
using System.IO;
using System.Data;
using System.Linq;
using CareerCloud.Pocos;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Collections.Generic;
using CareerCloud.DataAccessLayer;
using Microsoft.Extensions.Configuration;
namespace CareerCloud.ADODataAccessLayer
{
    public class SecurityLoginsRoleRepository:IDataRepository<SecurityLoginsRolePoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public SecurityLoginsRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params SecurityLoginsRolePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Security_Logins_Roles]
                                                        (
                                                           [Id]
                                                          ,[Login]
                                                          ,[Role]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Login
                                                          ,@Role
                                                        )";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Role", item.Role);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsRolePoco> GetAll(params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IList<SecurityLoginsRolePoco> securitylogin = new List<SecurityLoginsRolePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                            ,[Login]
                                            ,[Role]
                                            ,[Time_Stamp]
                                    FROM [JOB_PORTAL_DB].[dbo].[Security_Logins_Roles]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SecurityLoginsRolePoco temp = new SecurityLoginsRolePoco();

                temp.Id = reader.GetGuid(0);
                temp.Login = reader.GetGuid(1);
                temp.Role = reader.GetGuid(2);
                securitylogin.Add(temp);
            }
            _connection.Close();
            return securitylogin;

    }

        public IList<SecurityLoginsRolePoco>  GetList(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsRolePoco GetSingle(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsRolePoco> pocos = GetAll().AsQueryable();

            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsRolePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Security_Logins_Roles] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params SecurityLoginsRolePoco[] items)
        {
            _connection.Open();

            foreach (SecurityLoginsRolePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Security_Logins_Roles]
                                                 SET [Login] = @Login
                                                    ,[Role] = @Role
                                               WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Role", item.Role);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
