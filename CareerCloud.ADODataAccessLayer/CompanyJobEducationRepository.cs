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
    public class CompanyJobEducationRepository : IDataRepository<CompanyJobEducationPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public CompanyJobEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params CompanyJobEducationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO COMPANY_JOB_EDUCATIONS 
                         (Id,Job,Major,Importance)
                         VALUES
                         (@Id,@Job,@Major,@Importance)";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Major", item.Major);
                cmd.Parameters.AddWithValue("@Importance", item.Importance);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyJobEducationPoco> GetAll(params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobEducationPoco> companyeducation = new List<CompanyJobEducationPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id], 
                        [Job], 
                        [Major], 
                        [Importance]
                    
                    FROM [JOB_PORTAL_DB].[dbo].[Company_Job_Educations]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyJobEducationPoco temp = new CompanyJobEducationPoco();

                temp.Id = reader.GetGuid(0);
                temp.Job = reader.GetGuid(1);
                temp.Major = reader.GetString(2);
                temp.Importance = (short)reader[3];
                temp.TimeStamp = (byte[])reader[4];
                companyeducation.Add(temp);
            }
            _connection.Close();
            return companyeducation;
        }

        public IList<CompanyJobEducationPoco> GetList(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobEducationPoco GetSingle(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobEducationPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobEducationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Job_Educations] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

       public void Update(params CompanyJobEducationPoco[] items)
        {
            _connection.Open();
            foreach (CompanyJobEducationPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Company_Job_Educations]
                    SET [Id] = @Id, 
                        [Job] = @Job, 
                        [Major] = @Major, 
                        [Importance] = @Importance
                    
                    WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Major", item.Major);
                cmd.Parameters.AddWithValue("@Importance", item.Importance);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}