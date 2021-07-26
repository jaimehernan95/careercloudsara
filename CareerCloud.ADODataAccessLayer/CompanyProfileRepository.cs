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
    public class CompanyProfileRepository:IDataRepository<CompanyProfilePoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public CompanyProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params CompanyProfilePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Company_Profiles]
                                                        (
                                                           [Id]
                                                          ,[Registration_Date]
                                                          ,[Company_Website]
                                                          ,[Contact_Phone]
                                                          ,[Contact_Name]
                                                          ,[Company_Logo]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Registration_Date
                                                          ,@Company_Website
                                                          ,@Contact_Phone
                                                          ,@Contact_Name
                                                          ,@Company_Logo
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Registration_Date", item.RegistrationDate);
                cmd.Parameters.AddWithValue("@Company_Website", item.CompanyWebsite);
                cmd.Parameters.AddWithValue("@Contact_Phone", item.ContactPhone);
                cmd.Parameters.AddWithValue("@Contact_Name", item.ContactName);
                cmd.Parameters.AddWithValue("@Company_Logo", item.CompanyLogo);

                cmd.ExecuteNonQuery();
            }

            _connection.Close();

        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyProfilePoco> GetAll(params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IList<CompanyProfilePoco> companyprofile = new List<CompanyProfilePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                            ,[Registration_Date]
                                            ,[Company_Website]
                                            ,[Contact_Phone]
                                            ,[Contact_Name]
                                            ,[Company_Logo]
                                            ,[Time_Stamp]
                                    FROM [JOB_PORTAL_DB].[dbo].[Company_Profiles]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyProfilePoco temp = new CompanyProfilePoco();
                temp.Id = reader.GetGuid(0);
                temp.RegistrationDate = (DateTime)reader[1];
                temp.CompanyWebsite = (reader[2] == DBNull.Value) ? null : (string)reader[2];
                temp.ContactPhone = reader.GetString(3);
                temp.ContactName = (reader[4] == DBNull.Value) ? null : (string)reader[4];
                temp.CompanyLogo = (reader[5] == DBNull.Value) ? null : (byte[])reader[5];
                temp.TimeStamp = (byte[])reader[6];
                companyprofile.Add(temp);
            }
            _connection.Close();
            return companyprofile;

        }

        public IList<CompanyProfilePoco>  GetList(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyProfilePoco GetSingle(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyProfilePoco> pocos = GetAll().AsQueryable();

            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyProfilePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Profiles] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params CompanyProfilePoco[] items)
        {
            _connection.Open();

            foreach (CompanyProfilePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Company_Profiles]
                                                 SET [Registration_Date] = @Registration_Date
                                                    ,[Company_Website] = @Company_Website
                                                    ,[Contact_Phone] = @Contact_Phone
                                                    ,[Contact_Name] = @Contact_Name
                                                    ,[Company_Logo] = @Company_Logo
                                               WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Registration_Date", item.RegistrationDate);
                cmd.Parameters.AddWithValue("@Company_Website", item.CompanyWebsite);
                cmd.Parameters.AddWithValue("@Contact_Phone", item.ContactPhone);
                cmd.Parameters.AddWithValue("@Contact_Name", item.ContactName);
                cmd.Parameters.AddWithValue("@Company_Logo", item.CompanyLogo);

                cmd.ExecuteNonQuery();
            }

            _connection.Close();
        }
    }
    }

