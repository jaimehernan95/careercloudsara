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
    public class CompanyJobSkillRepository:IDataRepository<CompanyJobSkillPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public CompanyJobSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params CompanyJobSkillPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Company_Job_Skills]
                                                        (
                                                           [Id]
                                                          ,[Job]
                                                          ,[Skill]
                                                          ,[Skill_Level]
                                                          ,[Importance]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Job
                                                          ,@Skill
                                                          ,@Skill_Level
                                                          ,@Importance
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Skill", item.Skill);
                cmd.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                cmd.Parameters.AddWithValue("@Importance", item.Importance);
                cmd.ExecuteNonQuery();
            }

            _connection.Close();

        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyJobSkillPoco> GetAll(params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobSkillPoco> companyjob = new List<CompanyJobSkillPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Job]
                                                ,[Skill]
                                                ,[Skill_Level]
                                                ,[Importance]
                                                ,[Time_Stamp]
                                                FROM [JOB_PORTAL_DB].[dbo].[Company_Job_Skills]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyJobSkillPoco temp = new CompanyJobSkillPoco();
                temp.Id = reader.GetGuid(0);
                temp.Job = reader.GetGuid(1);
                temp.Skill = reader.GetString(2);
                temp.SkillLevel = reader.GetString(3);
                temp.Importance = reader.GetInt32(4);
                temp.TimeStamp = (byte[])reader[5];
                companyjob.Add(temp);
            }
            _connection.Close();
            return companyjob;
        }

        public IList<CompanyJobSkillPoco> GetList(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobSkillPoco GetSingle(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobSkillPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobSkillPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Job_Skills] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params CompanyJobSkillPoco[] items)
        {
            _connection.Open();
           
                foreach (CompanyJobSkillPoco item in items)
                {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Company_Job_Skills]
                    SET [Id] = @Id, 
                        [Job] = @Job, 
                        [Skill] = @Skill, 
                        [Skill_Level] = @Skill_Level,
                        [Importance] = @Importance
                    
                    WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Job", item.Job);
                cmd.Parameters.AddWithValue("@Skill", item.Skill);
                cmd.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                cmd.Parameters.AddWithValue("@Importance", item.Importance);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
