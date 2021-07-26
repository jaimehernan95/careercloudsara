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
    public class ApplicantSkillRepository : IDataRepository<ApplicantSkillPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public ApplicantSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params ApplicantSkillPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Skills]
                                                        (
                                                           [Id]
                                                          ,[Applicant]
                                                          ,[Skill]
                                                          ,[Skill_Level]
                                                          ,[Start_Month]
                                                          ,[Start_Year]
                                                          ,[End_Month]
                                                          ,[End_Year]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Applicant
                                                          ,@Skill
                                                          ,@Skill_Level
                                                          ,@Start_Month
                                                          ,@Start_Year
                                                          ,@End_Month
                                                          ,@End_Year
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Skill", item.Skill);
                cmd.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                cmd.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                cmd.Parameters.AddWithValue("@Start_Year", item.StartYear);
                cmd.Parameters.AddWithValue("@End_Month", item.EndMonth);
                cmd.Parameters.AddWithValue("@End_Year", item.EndYear);
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantSkillPoco> GetAll(params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IList<ApplicantSkillPoco> applicantskill = new List<ApplicantSkillPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Applicant]
                                                ,[Skill]
                                                ,[Skill_Level]
                                                ,[Start_Month]
                                                ,[Start_Year]
                                                ,[End_Month]
                                                ,[End_Year]
                                                ,[Time_Stamp]
                                                FROM [JOB_PORTAL_DB].[dbo].[Applicant_Skills]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ApplicantSkillPoco temp = new ApplicantSkillPoco();
                temp.Id = reader.GetGuid(0);
                temp.Applicant = reader.GetGuid(1);
                temp.Skill = reader.GetString(2);
                temp.SkillLevel = reader.GetString(3);
                temp.StartMonth = reader.GetByte(4);
                temp.StartYear = reader.GetInt32(5);
                temp.EndMonth = reader.GetByte(6);
                temp.EndYear = reader.GetInt32(7);
                temp.TimeStamp = (byte[])reader[8];
                applicantskill.Add(temp);
            }
            _connection.Close();
            return applicantskill;
        }

        public IList<ApplicantSkillPoco> GetList(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantSkillPoco GetSingle(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantSkillPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantSkillPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from [dbo].[Applicant_Skills] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();

            }
            _connection.Close();
        }

        public void Update(params ApplicantSkillPoco[] items)
        {
            _connection.Open();
            foreach (ApplicantSkillPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE Applicant_Skills
                            SET Applicant = @Applicant,
                                Skill = @Skill,
                                Skill_Level = @Skill_Level,
                                Start_Month = @Start_Month,
                                Start_Year = @Start_Year,
                                End_Month = @End_Month,
                                End_Year = @End_Year
                                WHERE ID = @Id";

                cmd.Parameters.AddWithValue("@Applicant", item.Applicant);
                cmd.Parameters.AddWithValue("@Skill", item.Skill);
                cmd.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                cmd.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                cmd.Parameters.AddWithValue("@Start_Year", item.StartYear);
                cmd.Parameters.AddWithValue("@End_Month", item.EndMonth);
                cmd.Parameters.AddWithValue("@End_Year", item.EndYear);
                cmd.Parameters.AddWithValue("@Id", item.Id);
            }
            _connection.Close();
        }
    }
}
