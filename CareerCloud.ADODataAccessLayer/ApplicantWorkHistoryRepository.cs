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
    public class ApplicantWorkHistoryRepository : IDataRepository<ApplicantWorkHistoryPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public ApplicantWorkHistoryRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params ApplicantWorkHistoryPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO APPLICANT_WORK_HISTORY 
(Id,
Applicant,
Company_Name,
Country_Code,
Location,
Job_Title,
Job_Description,
Start_Month,
Start_Year,
End_Month,
End_Year)
                VALUES
(@Id,
@Applicant,
@Company_Name,
@Country_Code,
@Location,
@Job_Title,
@Job_Description,
@Start_Month,
@Start_Year,
@End_Month,
@End_Year)";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                cmd.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                cmd.Parameters.AddWithValue("@Location", item.Location);
                cmd.Parameters.AddWithValue("@Job_Title", item.JobTitle);
                cmd.Parameters.AddWithValue("@Job_Description", item.JobDescription);
                cmd.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                cmd.Parameters.AddWithValue("@Start_Year", item.StartYear);
                cmd.Parameters.AddWithValue("@End_Month", item.EndMonth);
                cmd.Parameters.AddWithValue("@End_Year", item.EndYear);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantWorkHistoryPoco> GetAll(params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IList<ApplicantWorkHistoryPoco> applicantworkhistory = new List<ApplicantWorkHistoryPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id], 
                        [Applicant], 
                        [Company_Name], 
                        [Country_Code], 
                        [Location],
                        [Job_Title],
                        [Job_Description],
                        [Start_Month],
                        [Start_Year],
                        [End_Month],
                        [End_Year],
                        [Time_Stamp]
                    
                    FROM [JOB_PORTAL_DB].[dbo].[Applicant_Work_History]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ApplicantWorkHistoryPoco temp = new ApplicantWorkHistoryPoco();
                temp.Id = reader.GetGuid(0);
                temp.Applicant = reader.GetGuid(1);
                temp.CompanyName = reader.GetString(2);
                temp.CountryCode = reader.GetString(3);
                temp.Location = reader.GetString(4);
                temp.JobTitle = reader.GetString(5);
                temp.JobDescription = reader.GetString(6);
                temp.StartMonth = reader.GetInt16(7);
                temp.StartYear = reader.GetInt32(8);
                temp.EndMonth = reader.GetInt16(9);
                temp.EndYear = reader.GetInt32(10);
                temp.TimeStamp = (byte[])reader[11];
                applicantworkhistory.Add(temp);
            }
            _connection.Close();
            return applicantworkhistory;

        }

        public IList<ApplicantWorkHistoryPoco> GetList(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantWorkHistoryPoco GetSingle(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantWorkHistoryPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantWorkHistoryPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Applicant_Work_History] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params ApplicantWorkHistoryPoco[] items)
        {
            _connection.Open();
            foreach (ApplicantWorkHistoryPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Applicant_Work_History]
                    SET [Id] = @Id, 
                        [Applicant] = @Applicant,  
                        [Company_Name] = @Company_Name, 
                        [Country_Code] = @Country_Code, 
                        [Location] = @Location,
                        [Job_Title] = @Job_Title,
                        [Job_Description] = @Job_Description,
                        [Start_Month] = @Start_Month,
                        [Start_Year] = @Start_Year,
                        [End_Month] = @End_Month,
                        [End_Year] = @End_Year
                    
                    WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                cmd.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                cmd.Parameters.AddWithValue("@Location", item.Location);
                cmd.Parameters.AddWithValue("@Job_Title", item.JobTitle);
                cmd.Parameters.AddWithValue("@Job_Description", item.JobDescription);
                cmd.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                cmd.Parameters.AddWithValue("@Start_Year", item.StartYear);
                cmd.Parameters.AddWithValue("@End_Month", item.EndMonth);
                cmd.Parameters.AddWithValue("@End_Year", item.EndYear);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
