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
    public class CompanyLocationRepository:IDataRepository<CompanyLocationPoco>
    {
      
            protected readonly SqlConnection _connection;
            protected readonly string _connectionStr;

            public CompanyLocationRepository()
            {
                var config = new ConfigurationBuilder();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                config.AddJsonFile(path, false);
                var root = config.Build();
                _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
                _connection = new SqlConnection(_connectionStr);
            }

        public void Add(params CompanyLocationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Company_Locations]
                                                        (
                                                           [Id]
                                                          ,[Company]
                                                          ,[Country_Code]
                                                          ,[State_Province_Code]
                                                          ,[Street_Address]
                                                          ,[City_Town]
                                                          ,[Zip_Postal_Code]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Company
                                                          ,@Country_Code
                                                          ,@State_Province_Code
                                                          ,@Street_Address
                                                          ,@City_Town
                                                          ,@Zip_Postal_Code
                                                        )";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Company", item.Company);
                cmd.Parameters.AddWithValue("@Country_Code", item.CountryCode);
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

        public IList<CompanyLocationPoco> GetAll(params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IList<CompanyLocationPoco> companyloc = new List<CompanyLocationPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Company]
                                                ,[Country_Code]
                                                ,[State_Province_Code]
                                                ,[Street_Address]
                                                ,[City_Town]
                                                ,[Zip_Postal_Code]
                                                ,[Time_Stamp]
                                                FROM [JOB_PORTAL_DB].[dbo].[Company_Locations]";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyLocationPoco temp = new CompanyLocationPoco();
                temp.Id = reader.GetGuid(0);
                temp.Company = reader.GetGuid(1);
                temp.CountryCode = reader.GetString(2);
                temp.Province = (string)reader[3];
                temp.Street = (string)reader[4];
                temp.City = (reader[5] == DBNull.Value) ? null : (string)reader[5];
                temp.PostalCode = (reader[6] == DBNull.Value) ? null : (string)reader[6];
                temp.TimeStamp = (byte[])reader[7];
                companyloc.Add(temp);
            }
            _connection.Close();
            return companyloc;

        }

        public IList<CompanyLocationPoco> GetList(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyLocationPoco GetSingle(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyLocationPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyLocationPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Company_Locations] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params CompanyLocationPoco[] items)
        {
            _connection.Open();

            foreach (CompanyLocationPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Company_Locations]
                    SET [Id] = @Id, 
                        [Company] = @Company,
                        [Country_Code] = @Country_Code, 
                        [State_Province_Code] = @State_Province_Code,
                        [Street_Address] = @Street_Address,
                        [City_Town] = @City_Town,
                        [Zip_Postal_Code] = @Zip_Postal_Code  
                    
                    WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Company", item.Company);
                cmd.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                cmd.Parameters.AddWithValue("@City_Town", item.City);
                cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
