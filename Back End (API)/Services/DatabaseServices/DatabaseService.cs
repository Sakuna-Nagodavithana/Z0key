using System.Data.SqlClient;
using Z0key.Models;

namespace Z0key.Services.DatabaseServices
{
    public class DatabaseService:IDatabaseService
    {

        private readonly string _serverName = "DESKTOP-MINE\\TEW_SQLEXPRESS";
        private string _connectionString ;
        private SqlConnection _connection;


        public DatabaseService()
        {
            if (!DoesDatabaseExist("UserInfo"))
            {
                using (SqlCommand creatDatabaseCommand = new SqlCommand("CREATE DATABASE UserInfo", _connection))
                using (SqlCommand creatTableCommand = new SqlCommand(
                           "CREATE TABLE UserInfo.dbo.UserInfo (UserName NVARCHAR(100) NOT NULL," +
                           "Password NVARCHAR(100) NOT NULL,Email NVARCHAR(100) NOT NULL," +
                           "[Key] NVARCHAR(100) NOT NULL," +
                           " LastModifiedTime DATETIME NOT NULL);", _connection))
                {
                    try
                    {
                        _connection.Open();
                        creatDatabaseCommand.ExecuteNonQuery();
                        creatTableCommand.ExecuteNonQuery();
                        _connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating database: {ex.Message}");
                    }
                }
            }
                

        }

        private bool DoesDatabaseExist(string databaseName)
        {

            _connectionString = $"Server={_serverName};Integrated Security=True;";


            _connection = new SqlConnection(_connectionString);

            bool result = false;
            _connection.Open();
            SqlCommand command = new SqlCommand($"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'", _connection);
            object databaseId = command.ExecuteScalar();
            if (databaseId != null && databaseId != DBNull.Value)
            {
                result = true;
            }
            _connection.Close();
            return result;
        }

        public void CreatDataEntry(User user)
        {

            

            string insertQuery =
                "INSERT INTO UserInfo.dbo.UserInfo (UserName, Password, Email, [Key], LastModifiedTime) " +
                "VALUES (@UserName, @Password, @Email, @Key, @LastModifiedTime)";

            using (SqlCommand insertCommand = new SqlCommand(insertQuery, _connection))
            {
                insertCommand.Parameters.AddWithValue("@UserName", user.UserName);
                insertCommand.Parameters.AddWithValue("@Password", user.Password);
                insertCommand.Parameters.AddWithValue("@Email", user.Email);
                insertCommand.Parameters.AddWithValue("@Key", user.Key);
                insertCommand.Parameters.AddWithValue("@LastModifiedTime", DateTime.UtcNow);


                _connection.Open();
                insertCommand.ExecuteNonQuery();
                _connection.Close();

            }

            
        }


        public ReturnKey VerifyUser(VerifyUser user)
        {
            ReturnKey? returnKey = null;
            string query = "SELECT * FROM UserInfo.dbo.UserInfo WHERE UserName = @UserName";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (user.Password == reader["Password"].ToString())
                        {
                            returnKey = new ReturnKey(reader["Key"].ToString(), (DateTime)reader["LastModifiedTime"]);
                            break;
                        }
                        
                    }
                }

                _connection.Close();
            }

            return returnKey;
        }

        public void ChangeUserPassword(ChangeUserPassword user)
        {
            string query = "UPDATE UserInfo.dbo.UserInfo SET Password = @NewPassword WHERE UserName = @UserName";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@NewPassword", user.NewPassword);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
                
            }

        }


        public void DeleteUser(DeleteUser user)
        {
            string query = "DELETE FROM UserInfo.dbo.UserInfo WHERE UserName = @UserName";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }

        }

    }
}


