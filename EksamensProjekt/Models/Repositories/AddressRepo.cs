//using Microsoft.Data.SqlClient;
//using System.Data;
//using System.Text;

//namespace EksamensProjekt.Models.Repositories;

//public class AddressRepo : IRepo<Tenancy>
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

//    // Method to retrieve an Tenancy object by its ID using a stored procedure
//    public Tenancy GetByID(int address)
//    {
//        // Intializes an Tenancy object to null. If the query finds an address record, this variable will hold the data.
//        Tenancy address = null;

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

//            // Adds a parameter to the SqlCommand for the AddressID, used by the stored procedure to identify the Tenancy record
//            command.Parameters.AddWithValue("@AddressID", addressID);

//            // Executes the command and stores the result in a SqlDataReader object
//            using (var reader = command.ExecuteReader())
//            {
//                // Checks if the reader has any rows to read
//                if (reader.Read())
//                {
//                    // Initializes a new Tenancy object with the data from the reader
//                    address = new Tenancy
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

//    void IRepo<Tenancy>.Create(Tenancy entity)
//    {
//        throw new NotImplementedException();
//    }

//    Tenancy IRepo<Tenancy>.GetByUsername(string userName)
//    {
//        throw new NotImplementedException();
//    }

//    IEnumerable<Tenancy> IRepo<Tenancy>.ReadAll()
//    {
//        throw new NotImplementedException();
//    }

//    void IRepo<Tenancy>.Update(Tenancy entity)
//    {
//        throw new NotImplementedException();
//    }
//}
