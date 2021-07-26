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
    public class ApplicantEducationRepository : IDataRepository<ApplicantEducationPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public ApplicantEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params ApplicantEducationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Educations]
([Id]
,[Applicant]
,[Major
,[Certificate_Diploma]
,[Start_Date]
,[Completion_Date]
,[Completion_Percent])
VALUES
(@Id
,@Applicant
,@Major
,@Certificate_Diploma
,@Start_Date
,@Completion_Date
,@Completion_Percent
)";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Major", item.Major);
                cmd.Parameters.AddWithValue("@Certificate_Diploma", item.CertificateDiploma);
                cmd.Parameters.AddWithValue("@Start_Date", item.StartDate);
                cmd.Parameters.AddWithValue("@Completion_Date", item.CompletionDate);
                cmd.Parameters.AddWithValue("@Completion_Percent", item.CompletionPercent);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();

        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

       public IList<ApplicantEducationPoco> GetAll(params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            IList<ApplicantEducationPoco> applicanteducation = new List<ApplicantEducationPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
,[Applicant]
,[Major
,[Certificate_Diploma]
,[Start_Date]
,[Completion_Date]
,[Completion_Percent]
,[Time_Stamp]
FROM [dbo].[Applicant_Educations]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ApplicantEducationPoco temp = new ApplicantEducationPoco();
                temp.Id = reader.GetGuid(0);
                temp.Applicant = reader.GetGuid(1);
                temp.Major = reader.GetString(2);
                temp.CertificateDiploma = reader.GetString(3);
                temp.StartDate = (DateTime?)reader[4];
                temp.CompletionDate = (DateTime?)reader[5];
                temp.CompletionPercent = (byte?)reader[6];
                temp.TimeStamp = (byte[])reader[7];
                applicanteducation.Add(temp);
            }
            _connection.Close();
            return applicanteducation;
        }

       public IList<ApplicantEducationPoco> GetList(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

       public ApplicantEducationPoco GetSingle(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantEducationPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

       public void Remove(params ApplicantEducationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from [dbo].[Applicant_Educations] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();

            }
            _connection.Close();
        }

       public void Update(params ApplicantEducationPoco[] items)
        {
            _connection.Open();
            foreach (ApplicantEducationPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Applicant_Educations]
                                SET Applicant = @Applicant,
                                Major = @Major,
                                Certificate_Diploma = @Certificate_Diploma,
                                Start_Date = @Start_Date,
                                Completion_Date = @Completion_Date,
                                Completion_Percent = @Completion_Percent
                                WHERE ID = @Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Major", item.Major);
                cmd.Parameters.AddWithValue("@Certificate_Diploma", item.CertificateDiploma);
                cmd.Parameters.AddWithValue("@Start_Date", item.StartDate);
                cmd.Parameters.AddWithValue("@Completion_Date", item.CompletionDate);
                cmd.Parameters.AddWithValue("@Completion_Percent", item.CompletionPercent);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}