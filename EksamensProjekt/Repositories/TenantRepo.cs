using Microsoft.Extensions.Configuration; // Til indlæsning af konfigurationsindstillinger fra appsettings.json
using System.Data;
using System.Data.SqlClient; // Til at arbejde med SQL Server via ADO.NET
using Microsoft.Data.SqlClient;

namespace EksamensProjekt.Repos
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
                var command = new SqlCommand("usp_GetTenantByID", connection)
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
                            PhoneNum = reader.GetString(reader.GetOrdinal("PhoneNum")),
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
            Tenant tenant = entity;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("usp_CreateTenant", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", tenant.FirstName);
                    command.Parameters.AddWithValue("@LastName", tenant.LastName);
                    command.Parameters.AddWithValue("@PhoneNum", tenant.PhoneNum);
                    command.Parameters.AddWithValue("@Email", tenant.Email);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


            public IEnumerable<Tenant> ReadAll()
        {
            var tenants = new List<Tenant>();

            using (var conn = new SqlConnection(_connectionString))
            {
                // Use the stored procedure instead of a raw SQL query
                var cmd = new SqlCommand("usp_ReadAllTenants", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a Tenant object from the SQL data reader
                            var tenant = new Tenant()
                            {
                                TenantID = reader.GetInt32(0),
                                PartyID = reader.GetInt32(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                PhoneNum = reader.GetString(4),
                                Email = reader.GetString(5),
                                PartyRole = reader.GetString(6)
                            };
                            tenants.Add(tenant);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while reading all Tenant entries: " + ex.Message);
                }
            }

            return tenants;
        }

        void IRepo<Tenant>.Update(Tenant entity)
        {
            Tenant tenant = entity;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("usp_UpdateTenant", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TenantID", tenant.TenantID);
                    command.Parameters.AddWithValue("@FirstName", tenant.FirstName);
                    command.Parameters.AddWithValue("@LastName", tenant.LastName);
                    command.Parameters.AddWithValue("@PhoneNum", tenant.PhoneNum);
                    command.Parameters.AddWithValue("@Email", tenant.Email);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Delete(Tenant entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteTenancyTenant(int tenancyID, int tenantID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("usp_DeleteTenancyTenant", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    
                    command.Parameters.AddWithValue("@TenancyID", tenancyID);
                    command.Parameters.AddWithValue("@TenantID", tenantID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddTenantToTenancy(int tenancyID, int tenantID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("usp_AddTenantToTenancy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TenancyID", tenancyID);
                    command.Parameters.AddWithValue("@TenantID", tenantID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

