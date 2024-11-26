using Microsoft.Extensions.Configuration; // Til indlæsning af konfigurationsindstillinger fra appsettings.json
using System.Data;
using System.Data.SqlClient; // Til at arbejde med SQL Server via ADO.NET
using Microsoft.Data.SqlClient;
using EksamensProjekt.Models;

namespace EksamensProjekt.Repos;

public class UserRepo : IUserRepo<User>
{

    private readonly string _connectionString;


    public UserRepo(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }


    // Method to get user by usernameInput
    public User GetByUsername(string usernameInput)
    {
        User user = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("usp_GetUserByUsername", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Username", usernameInput);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        UserPasswordHash = reader.GetString(reader.GetOrdinal("UserPasswordHash")),
                        // Populate other properties if needed
                    };
                }
            }
        }

        return user;
    }

}
