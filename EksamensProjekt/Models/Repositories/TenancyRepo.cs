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


            }
        }
    }
}

                  