using Microsoft.Data.SqlClient;
using System.Data;

namespace EksamensProjekt.Models.Repositories;

public class StandardAddressRepo : IRepo<StandardAddress>
{
    private readonly string _connectionString;

    //Constructor that initialzies the connection string
    public StandardAddressRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Delete(int entity)
    {
        throw new NotImplementedException();
    }

    //Methhod to retrieve a StandardAddress object by its ID using a stored procedure
    public StandardAddress GetByID(int id)
    {
        // Intializes a new StandardAddressContext object with the connection string
        StandardAddress address = null;

        // Establishes a new SQL database connection
        using (var connection = new SqlConnection(_connectionString))
        {
            // Opens the connection to the database
            connection.Open();

            // Creates a new SQL command object with the stored procedure name and the connection
            var command = new SqlCommand("GetStandardAddressByID", connection)
            {
                // Specifies that the command is a stored procedure
                CommandType = CommandType.StoredProcedure
            };

            // Adds a parameter to the SqlCommand for the AddressID, used by the stored procedure to identify the Address record
            command.Parameters.AddWithValue("@AddressID", id);

            // Executes the command and stores the result in a SqlDataReader object
            using (var reader = command.ExecuteReader())
            {
                // Checks if the reader has any rows to read
                if (reader.Read())
                {
                    // Initializes a new StandardAddress object with the data from the reader
                    address = new StandardAddress
                    {
                        StandardAddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        Street = reader.GetString(reader.GetOrdinal("Street")),
                        Number = reader.GetString(reader.GetOrdinal("Number")),
                        FloorNumber = reader.GetString(reader.GetOrdinal("Floor")),
                        Zipcode = reader.GetString(reader.GetOrdinal("ZipCode")),
                        Country = reader.GetString(reader.GetOrdinal("Country"))
                    };
                }
            }
        }
        return address;
    }

    void IRepo<StandardAddress>.Create(StandardAddress entity)
    {
        throw new NotImplementedException();
    }

    StandardAddress IRepo<StandardAddress>.GetByID(int id)
    {
        throw new NotImplementedException();
    }

    StandardAddress IRepo<StandardAddress>.GetByUsername(string userName)
    {
        throw new NotImplementedException();
    }

    IEnumerable<StandardAddress> IRepo<StandardAddress>.ReadAll()
    {
        throw new NotImplementedException();
    }

    void IRepo<StandardAddress>.Update(StandardAddress entity)
    {
        throw new NotImplementedException();
    }
    public void Delete(StandardAddress entity)
    {
        throw new NotImplementedException();
    }
}
