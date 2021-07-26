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
    public class CompanyJobRepository : IDataRepository<CompanyJobPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public CompanyJobRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params CompanyJobPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO COMPANY_JOBS 
                         (Id,Company,Profile_Created,
                          Is_Inactive,Is_Company_Hidden)
                         VALUES
                         (@Id,@Company,@Profile_Created,
                          @Is_Inactive,@Is_Company_Hidden)";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Company", item.Company);
                cmd.Parameters.AddWithValue("@Profile_Created", item.ProfileCreated);
                cmd.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                cmd.Parameters.AddWithValue("@Is_Company_Hidden", item.IsCompanyHidden);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyJobPoco> GetAll(params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobPoco> companyjob = new List<CompanyJobPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Company]
                                                ,[Profile_Created]
                                                ,[Is_Inactive]
                                                ,[Is_Company_Hidden]
                                                ,[Time_Stamp]
                                                FROM [JOB_PORTAL_DB].[dbo].[Company_Jobs]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyJobPoco temp = new CompanyJobPoco();
                temp.Id = reader.GetGuid(0);
                temp.Company = reader.GetGuid(1);
                temp.ProfileCreated = (DateTime)reader[2];
                temp.IsInactive = (bool)reader[3];
                temp.IsCompanyHidden = (bool)reader[4];
                temp.TimeStamp = (byte[])reader[5];

                companyjob.Add(temp);
            }
            _connection.Close();
            return companyjob;
        }

        public IList<CompanyJobPoco> GetList(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobPoco GetSingle(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Jobs] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params CompanyJobPoco[] items)
        {
            _connection.Open();
            foreach (CompanyJobPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
               
                    cmd.CommandText = @"UPDATE [dbo].[Company_Jobs]
                    SET [Id] = @Id, 
                        [Company] = @Company, 
                        [Profile_Created] = @Profile_Created,
                        [Is_Inactive] = @Is_Inactive,
                        [Is_Company_Hidden] = @Is_Company_Hidden
                    
                    WHERE [Id] = @Id";

                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Company", item.Company);
                    cmd.Parameters.AddWithValue("@Profile_Created", item.ProfileCreated);
                    cmd.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    cmd.Parameters.AddWithValue("@Is_Company_Hidden", item.IsCompanyHidden);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
