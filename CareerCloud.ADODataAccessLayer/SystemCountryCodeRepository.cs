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
    public class SystemCountryCodeRepository : IDataRepository<SystemCountryCodePoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public SystemCountryCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params SystemCountryCodePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[System_Country_Codes]
                                                        (
                                                           [Code]
                                                          ,[Name]
                                                        )
                                                        VALUES
                                                        (
                                                           @Code
                                                          ,@Name
                                                        )";

                cmd.Parameters.AddWithValue("@Code", item.Code);
                cmd.Parameters.AddWithValue("@Name", item.Name);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SystemCountryCodePoco> GetAll(params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            IList<SystemCountryCodePoco> countrycode = new List<SystemCountryCodePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Code]
                                                ,[Name]
                                                FROM [JOB_PORTAL_DB].[dbo].[System_Country_Codes]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SystemCountryCodePoco temp = new SystemCountryCodePoco();
                temp.Code = reader.GetString(0);
                temp.Name = reader.GetString(1);
                countrycode.Add(temp);
            }
            _connection.Close();
            return countrycode;
        }

        public IList<SystemCountryCodePoco> GetList(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemCountryCodePoco GetSingle(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemCountryCodePoco> pocos = GetAll().AsQueryable();

            return pocos.Where(where).FirstOrDefault();

        }

        public void Remove(params SystemCountryCodePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"DELETE FROM [dbo].[System_Country_Codes]
                                                      WHERE [Code] = @Code";

                cmd.Parameters.AddWithValue("@Code", item.Code);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params SystemCountryCodePoco[] items)
        {
            _connection.Open();

            foreach (SystemCountryCodePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[System_Country_Codes]
                                                 SET [Name] = @Name
                                               WHERE [Code] = @Code";

                cmd.Parameters.AddWithValue("@Code", item.Code);
                cmd.Parameters.AddWithValue("@Name", item.Name);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}