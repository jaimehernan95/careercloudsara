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
    public class SystemLanguageCodeRepository:IDataRepository<SystemLanguageCodePoco>
    {

        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public SystemLanguageCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

       public void Add(params SystemLanguageCodePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[System_Language_Codes]
                                                        (
                                                           [LanguageID]
                                                          ,[Name]
                                                          ,[Native_Name]
                                                        )
                                                        VALUES
                                                        (
                                                           @LanguageID
                                                          ,@Name
                                                          ,@Native_Name
                                                        )";

                cmd.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Native_Name", item.NativeName);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

       public IList<SystemLanguageCodePoco> GetAll(params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IList<SystemLanguageCodePoco> langcode = new List<SystemLanguageCodePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [LanguageID]
                                                ,[Name]
                                                ,[Native_Name]
                                                FROM [JOB_PORTAL_DB].[dbo].[System_Language_Codes]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SystemLanguageCodePoco temp = new SystemLanguageCodePoco();

                temp.LanguageID = reader.GetString(0);
                temp.Name = reader.GetString(1);
                temp.NativeName = reader.GetString(2);
                     langcode.Add(temp);
            }
            _connection.Close();
            return langcode;
        }

        public IList<SystemLanguageCodePoco> GetList(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

       public SystemLanguageCodePoco GetSingle(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemLanguageCodePoco> pocos = GetAll().AsQueryable();

            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemLanguageCodePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"DELETE FROM System_Language_Codes
                                WHERE LanguageID = @LanguageID";

                cmd.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params SystemLanguageCodePoco[] items)
        {
            _connection.Open();

            foreach (SystemLanguageCodePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE System_Language_Codes
                            SET Name = @Name,
                                Native_Name = @Native_Name
                                WHERE LanguageID = @LanguageID";

                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Native_Name", item.NativeName);
                cmd.Parameters.AddWithValue("@LanguageID", item.LanguageID);

                cmd.ExecuteNonQuery();
            }
        _connection.Close();

            }
        }
}
