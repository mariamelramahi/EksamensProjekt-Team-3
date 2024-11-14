//using Microsoft.Data.SqlClient;
//using System.Data;
//using System.Text;

//namespace EksamensProjekt.Models.Repositories;

//public class AddressRepo : IRepo<Address>
//{
//    private readonly string _connectionString;

//    // Constructor that intialisizes the connection string for the database
//    public AddressRepo(string connectionString)
//    {
//        // Sets the connection string to the connectionString parameter
//        _connectionString = connectionString;
//    }

//    public void Delete(int entity)
//    {
//        throw new NotImplementedException();
//    }

//    // Method to retrieve an Address object by its ID using a stored procedure
//    public Address GetByID(int address)
//    {
//        // Intializes an Address object to null. If the query finds an address record, this variable will hold the data.
//        Address address = null;

//        // Establishes a new SQL database connection using the provided connection string
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            //Opens the connection to the database to allow database operations
//            connection.Open();

//            // Creates a new SQL command object with the stored procedure name and the connection
//            var command = new SqlCommand("GetAddressByID", connection)
//            {
//                // Specifies that the command is a stored procedure
//                CommandType = CommandType.StoredProcedure
//            };

//            // Adds a parameter to the SqlCommand for the AddressID, used by the stored procedure to identify the Address record
//            command.Parameters.AddWithValue("@AddressID", addressID);

//            // Executes the command and stores the result in a SqlDataReader object
//            using (var reader = command.ExecuteReader())
//            {
//                // Checks if the reader has any rows to read
//                if (reader.Read())
//                {
//                    // Initializes a new Address object with the data from the reader
//                    address = new Address
//                    {
//                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
//                        Street = reader.GetString(reader.GetOrdinal("Street")),
//                        Number = reader.GetString(reader.GetOrdinal("Number")),
//                        Floor = reader.GetString(reader.GetOrdinal("Floor")),
//                        ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
//                        Country = reader.GetString(reader.GetOrdinal("Country"))
//                    };
//                }
//            }
//        }

//        return address;

//    }

//    void IRepo<Address>.Create(Address entity)
//    {
//        throw new NotImplementedException();
//    }

//    Address IRepo<Address>.GetByUsername(string userName)
//    {
//        throw new NotImplementedException();
//    }

//    IEnumerable<Address> IRepo<Address>.ReadAll()
//    {
//        throw new NotImplementedException();
//    }

//    void IRepo<Address>.Update(Address entity)
//    {
//        throw new NotImplementedException();
//    }
//}
