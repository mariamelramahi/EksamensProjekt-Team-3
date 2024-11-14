using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamensProjekt.Models;
using System.Data.SqlClient;
using System.Data;

namespace EksamensProjekt.Models.Repositories
{
    public class TenancyRepo : IRepo<Tenancy>
    {
        private readonly string _connectionString;

        //Constructor, that initializes the connection string for the database
        public TenancyRepo(string connectionString)
        {
            //the connection string is assigned if it is not null; otherwise, it throws an exception
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Delete(int entity)
        {
            throw new NotImplementedException();
        }

        // Method to retrieve a Tenancy object by its ID using a stored procedure
        public Tenancy GetByID(int tenancyID)
        {
            // intializies a Tenancy object to null. If the query finds a tenancy record, this variable will hold hold the data. 
            Tenancy tenancy = null;

            // Establishes a new SQL database connection using the provided connection string
            using (var connection = new SqlConnection(_connectionString))
            {
                // Opens the connection to the database to allow database operations
                connection.Open();

                // Creates a new SQL command object with the stored procedure name and the connection
                var command = new SqlCommand("GetTenancyByID", connection)
                {
                    // Specifies that the command is a stored procedure
                    CommandType = CommandType.StoredProcedure
                };
                
                // Adds a parameter to the SqlCommand for the TenancyID, used by the stored procedure to identify the Tenancy record
                command.Parameters.AddWithValue("@TenancyID", tenancyID);

                // Executes the command and stores the result in a SqlDataReader object
                using (var reader = command.ExecuteReader())
                {
                    // Checks if the reader has any rows to read
                    if (reader.Read())
                    {
                        // Initializes a new Tenancy object with the data from the reader
                        tenancy = new Tenancy
                        {
                            TenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID")),
                            TenancyStatus = (TenancyStatus)reader.GetInt32(reader.GetOrdinal("TenancyStatus")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            MoveOutDate = reader.GetDateTime(reader.GetOrdinal("MoveOutDate")),
                            SqaureMeter = reader.GetString(reader.GetOrdinal("SqaureMeter")),
                            Rent = reader.GetInt32(reader.GetOrdinal("Rent")),
                            Rooms = reader.GetInt32(reader.GetOrdinal("Rooms")),
                            Bathrooms = reader.GetInt32(reader.GetOrdinal("Bathrooms")),
                            PetsAllowed = reader.GetBoolean(reader.GetOrdinal("PetsAllowed")),
                            tentants = new List<Tentant>(),
                            adress = new StandardAdress(),
                            Company = new Company()
                        };
                    }
                }
            }

            // Returns the Tenancy object, which is either null or contains the data from the database
            return tenancy;
        }

        void IRepo<Tenancy>.Create(Tenancy entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Tenancy> IRepo<Tenancy>.ReadAll()
        {
            throw new NotImplementedException();
        }

        void IRepo<Tenancy>.Update(Tenancy entity)
        {
            throw new NotImplementedException();
        }
    }
}

                  