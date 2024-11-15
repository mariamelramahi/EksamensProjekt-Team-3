using Microsoft.Extensions.Configuration; // Til indlæsning af konfigurationsindstillinger fra appsettings.json
using System.Data;
using System.Data.SqlClient; // Til at arbejde med SQL Server via ADO.NET
using Microsoft.Data.SqlClient;


namespace EksamensProjekt.Models.Repositories;

public class UserRepo : IRepo<User>
{
    private readonly string _connectionString;

    public UserRepo(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public void Create(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int entity)
    {
        throw new NotImplementedException();
    }

    public User GetByID(int id)
    {
        throw new NotImplementedException();
    }

    public User GetByName(string userName)
    {
        throw new NotImplementedException();
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
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        // Populate other properties if needed
                    };
                }
            }
        }

        return user;
    }

    public IEnumerable<User> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }

    void IRepo<User>.Delete(User entity)
    {
        throw new NotImplementedException();
    }
}
