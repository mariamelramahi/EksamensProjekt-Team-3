using Microsoft.Extensions.Configuration; // Til indlæsning af konfigurationsindstillinger fra appsettings.json
using System.Data;
using System.Data.SqlClient; // Til at arbejde med SQL Server via ADO.NET

namespace EksamensProjekt.Models.Repositories
{
    public class TenantRepo : IRepo<Tenant>
    {
        private readonly string _connectionString;

        //Constructor, that initializes the connection string for the database
        public TenantRepo(string connectionString)
        {
            //the connection string is assigned if it is not null; otherwise, it throws an exception
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Delete(int entity)
        {
            throw new NotImplementedException();
        }

        // Method to retrieve a Tenant object by its ID using a stored procedure
        public Tenant GetByID(int tenantID)
        {
            // intializies a Tenant object to null. If the query finds a tenant record, this variable will hold hold the data. 
            Tenant tenant = null;

            // Establishes a new SQL database connection using the provided connection string
            using (var connection = new SqlConnection(_connectionString))
            {
                // Opens the connection to the database to allow database operations
                connection.Open();

                // Creates a new SQL command object with the stored procedure name and the connection
                var command = new SqlCommand("GetTenantByID", connection)
                {
                    // Specifies that the command is a stored procedure
                    CommandType = CommandType.StoredProcedure
                };
                
                // Adds a parameter to the SqlCommand for the TenantID, used by the stored procedure to identify the Tenant record
                command.Parameters.AddWithValue("@TenantID", tenantID);

                // Executes the command and stores the result in a SqlDataReader object
                using (var reader = command.ExecuteReader())
                {
                    // Checks if the reader has any rows to read
                    if (reader.Read())
                    {
                        // Initializes a new Tenant object with the data from the reader
                        tenant = new Tenant
                        {
                            TenantID = reader.GetInt32(reader.GetOrdinal("TenantID")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                    }
                }
            }

            // Returns the Tenant object
            return tenant;
        }

        void IRepo<Tenant>.Create(Tenant entity)
        {
            throw new NotImplementedException();
        }

        Tenant IRepo<Tenant>.GetByUsername(string userName)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Tenant> IRepo<Tenant>.ReadAll()
        {
            throw new NotImplementedException();
        }

        void IRepo<Tenant>.Update(Tenant entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(Tenant entity)
        {
            throw new NotImplementedException();
        }
    }
}
