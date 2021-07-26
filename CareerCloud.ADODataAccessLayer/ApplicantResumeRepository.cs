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
    public class ApplicantResumeRepository: IDataRepository<ApplicantResumePoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public ApplicantResumeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params ApplicantResumePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Resumes]
                                                        (
                                                           [Id]
                                                          ,[Applicant]
                                                          ,[Resume]
                                                          ,[Last_Updated]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Applicant
                                                          ,@Resume
                                                          ,@Last_Updated
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Resume", item.Resume);
                cmd.Parameters.AddWithValue("@Last_Updated", item.LastUpdated);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantResumePoco> GetAll(params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            IList<ApplicantResumePoco> applicantresume = new List<ApplicantResumePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                            ,[Applicant]
                                            ,[Resume]
                                            ,[Last_Updated]
                                            FROM [JOB_PORTAL_DB].[dbo].[Applicant_Resumes]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ApplicantResumePoco temp = new ApplicantResumePoco();
                temp.Id = reader.GetGuid(0);
                temp.Applicant = reader.GetGuid(1);
                temp.Resume = reader.GetString(2);
                if (!reader.IsDBNull(3)) temp.LastUpdated = reader.GetDateTime(3);
                applicantresume.Add(temp);
            }
            _connection.Close();
            return applicantresume;
        }

        public IList<ApplicantResumePoco>  GetList(Expression<Func<ApplicantResumePoco, bool>> where, params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantResumePoco GetSingle(Expression<Func<ApplicantResumePoco, bool>> where, params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantResumePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantResumePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Applicant_Resumes] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();

            }

            _connection.Close();
        }

        public void Update(params ApplicantResumePoco[] items)
        {
            _connection.Open();
            foreach (ApplicantResumePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Applicant_Resumes]
                    SET [Id] = @Id, 
                        [Applicant] = @Applicant, 
                        [Resume] = @Resume,  
                        [Last_Updated] = @Last_Updated
                    WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Resume", item.Resume);
                cmd.Parameters.AddWithValue("@Last_Updated", item.LastUpdated);
            }
            _connection.Close();
        }
    }
}
