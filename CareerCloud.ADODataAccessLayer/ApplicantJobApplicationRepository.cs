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
   public class ApplicantJobApplicationRepository : IDataRepository<ApplicantJobApplicationPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public ApplicantJobApplicationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params ApplicantJobApplicationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Job_Applications]
([Id]
      ,[Applicant]
      ,[Job]
      ,[Application_Date]
      )
VALUES
(@Id
,@Applicant
,@Job
,@Application_Date
)";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Application_Date", item.ApplicationDate);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();

        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantJobApplicationPoco> GetAll(params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            IList<ApplicantJobApplicationPoco> applicantjob = new List<ApplicantJobApplicationPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
      ,[Applicant]
      ,[Job]
      ,[Application_Date]
      ,[Time_Stamp]
FROM [dbo].[Applicant_Job_Applications]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ApplicantJobApplicationPoco temp = new ApplicantJobApplicationPoco();
                temp.Id = reader.GetGuid(0);
                temp.Applicant = reader.GetGuid(1);
                temp.Job = reader.GetGuid(2);
                temp.ApplicationDate = reader.GetDateTime(3);
                temp.TimeStamp = (byte[])reader[4];
                applicantjob.Add(temp);
            }
            _connection.Close();
            return applicantjob;
        }

        public IList<ApplicantJobApplicationPoco>  GetList(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantJobApplicationPoco GetSingle(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantJobApplicationPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantJobApplicationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from [dbo].[Applicant_Job_Applications] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();

            }
            _connection.Close();
        }

        public void Update(params ApplicantJobApplicationPoco[] items)
        {
            _connection.Open();

            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Applicant_Job_Applications]
                                        SET [Applicant] = @Applicant
                                           ,[Job] = @Job
                                           ,[Application_Date] = @Application_Date
                                        WHERE[Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Application_Date", item.ApplicationDate);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
