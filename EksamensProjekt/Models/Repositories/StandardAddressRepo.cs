using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Models.Repositories
{
    public class StandardAddressRepo : IRepo<StandardAddress>
    {
        private readonly string_ _connectionString;

        //Constructor that initialzies the connection string
        public StandardAddressRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Methhod to retrieve a StandardAddress object by its ID using a stored procedure
        public StandardAddress Get(int id)
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
                            AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                            Street = reader.GetString(reader.GetOrdinal("Street")),
                            Number = reader.GetString(reader.GetOrdinal("Number")),
                            Floor = reader.GetString(reader.GetOrdinal("Floor")),
                            ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                            Country = reader.GetString(reader.GetOrdinal("Country"))
                        };
                    }
                }
            }
            return address;
        }
    }
}
