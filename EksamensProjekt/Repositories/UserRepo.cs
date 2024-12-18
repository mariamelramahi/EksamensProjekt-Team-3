using Microsoft.Extensions.Configuration; // To read the configuration settings from appsettings.json
using System.Data;
using System.Data.SqlClient; // To work with SQL Server via ADO.NET
using Microsoft.Data.SqlClient;
using EksamensProjekt.Models;
using EksamensProjekt.DataAccess;

namespace EksamensProjekt.Repos;

public class UserRepo : IUserRepo<User>
{

    private readonly string _connectionString;


    public UserRepo(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }


    // Method to get user by usernameInput
    public User GetByUsername(string username)
    {
        User user = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("usp_GetUserByUsername", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Username", username);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = SqlDataMapper.PopulateUserFromReader(reader);
                }
            }
        }

        return user;
    }

}
