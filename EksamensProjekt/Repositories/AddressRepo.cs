using EksamensProjekt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EksamensProjekt.Repos;

public class AddressRepo : IRepo<Address>
{
    private readonly string _connectionString;

    //Constructor that initialzies the connection string
    public AddressRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Delete(int entity)
    {
        throw new NotImplementedException();
    }

    //Methhod to retrieve a Address object by its ID using a stored procedure
    public Address GetByID(int id)
    {
        // Intializes a new AddressContext object with the connection string
        Address address = null;

        // Establishes a new SQL database connection
        using (var connection = new SqlConnection(_connectionString))
        {
            // Opens the connection to the database
            connection.Open();

            // Creates a new SQL command object with the stored procedure name and the connection
            var command = new SqlCommand("usp_GetAddressByID", connection)//USP not created in database
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
                    // Initializes a new Address object with the data from the reader
                    address = new Address
                    {
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
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

    void IRepo<Address>.Create(Address entity)
    {
        throw new NotImplementedException();
    }

    Address IRepo<Address>.GetByID(int id)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<Address> ReadAll()
    {
        var addresses = new List<Address>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var cmd = new SqlCommand("usp_ReadAllAddresses", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                connection.Open(); // Use synchronous Open method
                using (SqlDataReader reader = cmd.ExecuteReader()) // Use synchronous ExecuteReader
                {
                    while (reader.Read())
                    {
                        var address = new Address
                        {
                            AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                            Street = reader.GetString(reader.GetOrdinal("Street")),
                            Number = reader.GetString(reader.GetOrdinal("Number")),
                            FloorNumber = reader.IsDBNull(reader.GetOrdinal("FloorNumber")) ? null : reader.GetString(reader.GetOrdinal("FloorNumber")),
                            Zipcode = reader.GetString(reader.GetOrdinal("Zipcode")),
                            Country = reader.GetString(reader.GetOrdinal("Country")),
                            IsStandardized = reader.GetBoolean(reader.GetOrdinal("IsStandardized"))
                        };

                        addresses.Add(address);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching addresses: " + ex.Message);
            }
        }

        return addresses;
    }

    void IRepo<Address>.Update(Address entity)
    {
        throw new NotImplementedException();
    }
    public void Delete(Address entity)
    {
        throw new NotImplementedException();
    }
}
