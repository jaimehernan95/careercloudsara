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
    public class SecurityRoleRepository : IDataRepository<SecurityRolePoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public SecurityRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

       public void Add(params SecurityRolePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Security_Roles]
                                                        (
                                                           [Id]
                                                          ,[Role]
                                                          ,[Is_Inactive]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Role
                                                          ,@Is_Inactive
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Role", item.Role);
                cmd.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();

        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityRolePoco> GetAll(params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            IList<SecurityRolePoco> securityrole = new List<SecurityRolePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                            ,[Role]
                                            ,[Is_Inactive]
                                    FROM [JOB_PORTAL_DB].[dbo].[Security_Roles]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SecurityRolePoco temp = new SecurityRolePoco();
                temp.Id = reader.GetGuid(0);
                temp.Role = reader.GetString(1);
                temp.IsInactive = (bool)reader[2];
                securityrole.Add(temp);
            }
            _connection.Close();
            return securityrole;
        }

       public IList<SecurityRolePoco> GetList(Expression<Func<SecurityRolePoco, bool>> where, params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityRolePoco GetSingle(Expression<Func<SecurityRolePoco, bool>> where, params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityRolePoco> pocos = GetAll().AsQueryable();

            return pocos.Where(where).FirstOrDefault();
        }

       public void Remove(params SecurityRolePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Security_Roles] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params SecurityRolePoco[] items)
        {
            _connection.Open();

            foreach (SecurityRolePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Security_Roles]
                SET Role = @Role,
                                Is_Inactive = @Is_Inactive
                                WHERE ID = @Id";

                cmd.Parameters.AddWithValue("@Role", item.Role);
                cmd.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                cmd.Parameters.AddWithValue("@Id", item.Id);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}

