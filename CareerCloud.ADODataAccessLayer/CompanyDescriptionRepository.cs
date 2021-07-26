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
    public class CompanyDescriptionRepository: IDataRepository<CompanyDescriptionPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public CompanyDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params CompanyDescriptionPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Company_Descriptions]
                                                        (
                                                           [Id]
                                                          ,[Company]
                                                          ,[LanguageID]
                                                          ,[Company_Name]
                                                          ,[Company_Description]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Company
                                                          ,@LanguageID
                                                          ,@Company_Name
                                                          ,@Company_Description
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Company", item.Company);
                cmd.Parameters.AddWithValue("@LanguageID", item.LanguageId);
                cmd.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                cmd.Parameters.AddWithValue("@Company_Description", item.CompanyDescription);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyDescriptionPoco> GetAll(params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IList<CompanyDescriptionPoco> companydescription = new List<CompanyDescriptionPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id], 
                        [Company], 
                        [LanguageID], 
                        [Company_Name],
                        [Company_Description]
                    
                    FROM [JOB_PORTAL_DB].[dbo].[Company_Descriptions]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyDescriptionPoco temp = new CompanyDescriptionPoco();
                temp.Id = reader.GetGuid(0);
                temp.Company = reader.GetGuid(1);
                temp.LanguageId = reader.GetString(2);
                temp.CompanyName = reader.GetString(3);
                temp.CompanyDescription = reader.GetString(4);
                temp.TimeStamp = (byte[])reader[5];
                companydescription.Add(temp);
            }
            _connection.Close();
            return companydescription;

        }

            

        public IList<CompanyDescriptionPoco>  GetList(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyDescriptionPoco GetSingle(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyDescriptionPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyDescriptionPoco[] items)
        {

            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Descriptions]  where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params CompanyDescriptionPoco[] items)
        {
            _connection.Open();
            foreach (CompanyDescriptionPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Company_Descriptions]
                    SET [Id] = @Id, 
                        [Company] = @Company, 
                        [LanguageID] = @LanguageID, 
                        [Company_Name] = @Company_Name,
                        [Company_Description] = @Company_Description
                    
                    WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Company", item.Company);
                cmd.Parameters.AddWithValue("@LanguageID", item.LanguageId);
                cmd.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                cmd.Parameters.AddWithValue("@Company_Description", item.CompanyDescription);
cmd.ExecuteNonQuery();
            }
            _connection.Close();

            }
        }
}
