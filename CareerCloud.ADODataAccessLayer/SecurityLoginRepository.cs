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
   public class SecurityLoginRepository: IDataRepository<SecurityLoginPoco>
    {
        protected readonly SqlConnection _connection;
        protected readonly string _connectionStr;

        public SecurityLoginRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            _connection = new SqlConnection(_connectionStr);
        }

       public void Add(params SecurityLoginPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"INSERT INTO [dbo].[Security_Logins]
                                                        (
                                                           [Id]
                                                          ,[Login]
                                                          ,[Password]
                                                          ,[Created_Date]
                                                          ,[Password_Update_Date]
                                                          ,[Agreement_Accepted_Date]
                                                          ,[Is_Locked]
                                                          ,[Is_Inactive]
                                                          ,[Email_Address]
                                                          ,[Phone_Number]
                                                          ,[Full_Name]
                                                          ,[Force_Change_Password]
                                                          ,[Prefferred_Language]
                                                        )
                                                        VALUES
                                                        (
                                                           @Id
                                                          ,@Login
                                                          ,@Password
                                                          ,@Created_Date
                                                          ,@Password_Update_Date
                                                          ,@Agreement_Accepted_Date
                                                          ,@Is_Locked
                                                          ,@Is_Inactive
                                                          ,@Email_Address
                                                          ,@Phone_Number
                                                          ,@Full_Name
                                                          ,@Force_Change_Password
                                                          ,@Prefferred_Language
                                                        )";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Password", item.Password);
                cmd.Parameters.AddWithValue("@Created_Date", item.Created);
                cmd.Parameters.AddWithValue("@Password_Update_Date", item.PasswordUpdate);
                cmd.Parameters.AddWithValue("@Agreement_Accepted_Date", item.AgreementAccepted);
                cmd.Parameters.AddWithValue("@Is_Locked", item.IsLocked);
                cmd.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                cmd.Parameters.AddWithValue("@Email_Address", item.EmailAddress);
                cmd.Parameters.AddWithValue("@Phone_Number", item.PhoneNumber);
                cmd.Parameters.AddWithValue("@Full_Name", item.FullName);
                cmd.Parameters.AddWithValue("@Force_Change_Password", item.ForceChangePassword);
                cmd.Parameters.AddWithValue("@Prefferred_Language", item.PrefferredLanguage);
                cmd.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginPoco> GetAll(params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IList<SecurityLoginPoco> securitylogin = new List<SecurityLoginPoco>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = @"SELECT [Id]
                                                ,[Login]
                                                ,[Password]
                                                ,[Created_Date]
                                                ,[Password_Update_Date]
                                                ,[Agreement_Accepted_Date]
                                                ,[Is_Locked]
                                                ,[Is_Inactive]
                                                ,[Email_Address]
                                                ,[Phone_Number]
                                                ,[Full_Name]
                                                ,[Force_Change_Password]
                                                ,[Prefferred_Language]
                                                ,[Time_Stamp]
                                                FROM [JOB_PORTAL_DB].[dbo].[Security_Logins]";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SecurityLoginPoco temp = new SecurityLoginPoco();
                temp.Id = reader.GetGuid(0);
                temp.Login = (string)reader[1];
                temp.Password = reader.GetString(2);
                temp.Created = (DateTime)reader[3];
                temp.PasswordUpdate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
                temp.AgreementAccepted = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                temp.IsLocked = (bool)reader[6];
                temp.IsInactive = (bool)reader[7];
                temp.EmailAddress = reader.GetString(8);
                temp.PhoneNumber = (reader[9] == DBNull.Value) ? null : (string)reader[9];
                temp.FullName = (reader[10] == DBNull.Value) ? null : (string)reader[10];
                temp.ForceChangePassword = (bool)reader[11];
                temp.PrefferredLanguage = (reader[12] == DBNull.Value) ? null : (string)reader[12];
                temp.TimeStamp = (byte[])reader[13];
                securitylogin.Add(temp);
            }
            _connection.Close();
            return securitylogin;

        }

        public IList<SecurityLoginPoco>  GetList(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginPoco GetSingle(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginPoco> pocos = GetAll().AsQueryable();

            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginPoco[] items)
        {
            _connection.Open();
            foreach (var item in items)

            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"Delete from  [dbo].[Security_Logins] where id=@Id";
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Update(params SecurityLoginPoco[] items)
        {
            _connection.Open();

            foreach (SecurityLoginPoco item in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = @"UPDATE [dbo].[Security_Logins]
                                                 SET [Login] = @Login
                                                    ,[Password] = @Password
                                                    ,[Created_Date] = @Created_Date
                                                    ,[Password_Update_Date] = @Password_Update_Date
                                                    ,[Agreement_Accepted_Date] = @Agreement_Accepted_Date
                                                    ,[Is_Locked] = @Is_Locked
                                                    ,[Is_Inactive] = @Is_Inactive
                                                    ,[Email_Address] = @Email_Address
                                                    ,[Phone_Number] = @Phone_Number
                                                    ,[Full_Name] = @Full_Name
                                                    ,[Force_Change_Password] = @Force_Change_Password
                                                    ,[Prefferred_Language] = @Prefferred_Language
                                               WHERE [Id] = @Id";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Login", item.Login);
                cmd.Parameters.AddWithValue("@Password", item.Password);
                cmd.Parameters.AddWithValue("@Created_Date", item.Created);
                cmd.Parameters.AddWithValue("@Password_Update_Date", item.PasswordUpdate);
                cmd.Parameters.AddWithValue("@Agreement_Accepted_Date", item.AgreementAccepted);
                cmd.Parameters.AddWithValue("@Is_Locked", item.IsLocked);
                cmd.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                cmd.Parameters.AddWithValue("@Email_Address", item.EmailAddress);
                cmd.Parameters.AddWithValue("@Phone_Number", item.PhoneNumber);
                cmd.Parameters.AddWithValue("@Full_Name", item.FullName);
                cmd.Parameters.AddWithValue("@Force_Change_Password", item.ForceChangePassword);
                cmd.Parameters.AddWithValue("@Prefferred_Language", item.PrefferredLanguage);

                cmd.ExecuteNonQuery();
            }
            _connection.Close();
        }
        }
}
