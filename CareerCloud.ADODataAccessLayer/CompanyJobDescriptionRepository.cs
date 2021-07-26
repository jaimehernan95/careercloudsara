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
    public class CompanyJobDescriptionRepository : IDataRepository<CompanyJobDescriptionPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public CompanyJobDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params CompanyJobDescriptionPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Company_Jobs_Descriptions]
                                                        (
                                                           [Id]
                                                          ,[Job]
                                                          ,[Job_Name]
                                                          ,[Job_Descriptions]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Job
                                                          ,@Job_Name
                                                          ,@Job_Descriptions
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Job_Name", item.JobName);
                cmd.Parameters.AddWithValue("@Job_Descriptions", item.JobDescriptions);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyJobDescriptionPoco> GetAll(params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobDescriptionPoco> companydescription = new List<CompanyJobDescriptionPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Job]
                                                ,[Job_Name]
                                                ,[Job_Descriptions]
                                                ,[Time_Stamp]
                                                FROM [JOB_PORTAL_DB].[dbo].[Company_Jobs_Descriptions]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyJobDescriptionPoco temp = new CompanyJobDescriptionPoco();
                temp.Id = reader.GetGuid(0);
                temp.Job = reader.GetGuid(1);
                temp.JobName = (string)reader[2];
                temp.JobDescriptions = (string)reader[3];
                temp.TimeStamp = (byte[])reader[4];
                companydescription.Add(temp);
            }
            _connection.Close();
            return companydescription;
        }

        public IList<CompanyJobDescriptionPoco> GetList(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobDescriptionPoco GetSingle(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {

            IQueryable<CompanyJobDescriptionPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobDescriptionPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Jobs_Descriptions] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params CompanyJobDescriptionPoco[] items)
        {
            _connection.Open();
            foreach (CompanyJobDescriptionPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Company_Jobs_Descriptions]
                                                 SET [Job] = @Job
                                                    ,[Job_Name] = @Job_Name
                                                    ,[Job_Descriptions] = @Job_Descriptions
                                               WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Job_Name", item.JobName);
                cmd.Parameters.AddWithValue("@Job_Descriptions", item.JobDescriptions);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
