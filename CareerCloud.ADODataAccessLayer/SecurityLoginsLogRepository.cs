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
    public class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public SecurityLoginsLogRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params SecurityLoginsLogPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Security_Logins_Log]
                                                        (
                                                           [Id]
                                                          ,[Login]
                                                          ,[Source_IP]
                                                          ,[Logon_Date]
                                                          ,[Is_Succesful]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Login
                                                          ,@Source_IP
                                                          ,@Logon_Date
                                                          ,@Is_Succesful
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Source_IP", item.SourceIP);
                cmd.Parameters.AddWithValue("@Logon_Date", item.LogonDate);
                cmd.Parameters.AddWithValue("@Is_Succesful", item.IsSuccesful);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IList<SecurityLoginsLogPoco> securitylogin = new List<SecurityLoginsLogPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Login]
                                                ,[Source_IP]
                                                ,[Logon_Date]
                                                ,[Is_Succesful]
                                                FROM [JOB_PORTAL_DB].[dbo].[Security_Logins_Log]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SecurityLoginsLogPoco temp = new SecurityLoginsLogPoco();
                temp.Id = reader.GetGuid(0);
                temp.Login = reader.GetGuid(1);
                temp.SourceIP = reader.GetString(2);
                temp.LogonDate = reader.GetDateTime(3);
                temp.IsSuccesful = reader.GetBoolean(4);
                securitylogin.Add(temp);
            }
            _connection.Close();
            return securitylogin;
        }

        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsLogPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Security_Logins_Log] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            _connection.Open();

            foreach (SecurityLoginsLogPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                                 SET [Login] = @Login
                                                    ,[Source_IP] = @Source_IP
                                                    ,[Logon_Date] = @Logon_Date
                                                    ,[Is_Succesful] = @Is_Succesful
                                               WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Source_IP", item.SourceIP);
                cmd.Parameters.AddWithValue("@Logon_Date", item.LogonDate);
                cmd.Parameters.AddWithValue("@Is_Succesful", item.IsSuccesful);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}

