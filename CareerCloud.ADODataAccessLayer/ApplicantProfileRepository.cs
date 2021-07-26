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
    public class ApplicantProfileRepository : IDataRepository<ApplicantProfilePoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public ApplicantProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

        public void Add(params ApplicantProfilePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Profiles]
                                                        (
                                                           [Id]
                                                          ,[Login]
                                                          ,[Current_Salary]
                                                          ,[Current_Rate]
                                                          ,[Currency]
                                                          ,[Country_Code]
                                                          ,[State_Province_Code]
                                                          ,[Street_Address]
                                                          ,[City_Town]
                                                          ,[Zip_Postal_Code]
 )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Login
                                                          ,@Current_Salary
                                                          ,@Current_Rate
                                                          ,@Currency
                                                          ,@Country_Code
                                                          ,@State_Province_Code
                                                          ,@Street_Address
                                                          ,@City_Town
                                                          ,@Zip_Postal_Code
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Current_Salary", item.CurrentSalary);
                cmd.Parameters.AddWithValue("@Current_Rate", item.CurrentRate);
                cmd.Parameters.AddWithValue("@Currency", item.Currency);
                cmd.Parameters.AddWithValue("@Country_Code", item.Country);
                cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                cmd.Parameters.AddWithValue("@City_Town", item.City);
                cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();

        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantProfilePoco> GetAll(params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IList<ApplicantProfilePoco> applicantprofile = new List<ApplicantProfilePoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                            ,[Login]
                                            ,[Current_Salary]
                                            ,[Current_Rate]
                                            ,[Currency]
                                            ,[Country_Code]
                                            ,[State_Province_Code]
                                            ,[Street_Address]
                                            ,[City_Town]
                                            ,[Zip_Postal_Code]
                                            ,[Time_Stamp]
                                    FROM [JOB_PORTAL_DB].[dbo].[Applicant_Profiles]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ApplicantProfilePoco temp = new ApplicantProfilePoco();
                temp.Id = reader.GetGuid(0);
                temp.Login = reader.GetGuid(1);
                temp.CurrentSalary = (Decimal?)reader[2];
                temp.CurrentRate = (Decimal?)reader[3];
                temp.Currency = reader.GetString(4);
                temp.Country = reader.GetString(5);
                temp.Province = reader.GetString(6);
                temp.Street = reader.GetString(7);
                temp.City = reader.GetString(8);
                temp.PostalCode = reader.GetString(9);
                temp.TimeStamp = (byte[])reader[10];
                applicantprofile.Add(temp);
            }
            _connection.Close();
            return applicantprofile;
        }

        public IList<ApplicantProfilePoco> GetList(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantProfilePoco GetSingle(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantProfilePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantProfilePoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from [dbo].[Applicant_Profiles] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();

            }
            _connection.Close();
        }

        public void Update(params ApplicantProfilePoco[] items)
        {

            _connection.Open();
            foreach (ApplicantProfilePoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE Applicant_Profiles
                            SET Login = @Login,
                                Current_Salary = @Current_Salary,
                                Current_Rate = @Current_Rate,
                                Currency = @Currency,
                                Country_Code = @Country_Code,
                                State_Province_Code = @State_Province_Code,
                                Street_Address = @Street_Address,
                                City_Town = @City_Town,
                                Zip_Postal_Code = @Zip_Postal_Code
                                WHERE ID = @Id";

                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Current_Salary", item.CurrentSalary);
                cmd.Parameters.AddWithValue("@Current_Rate", item.CurrentRate);
                cmd.Parameters.AddWithValue("@Currency", item.Currency);
                cmd.Parameters.AddWithValue("@Country_Code", item.Country);
                cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                cmd.Parameters.AddWithValue("@City_Town", item.City);
                cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);
                cmd.Parameters.AddWithValue("@Id", item.Id);
            }
            _connection.Close();
        }
    }
}