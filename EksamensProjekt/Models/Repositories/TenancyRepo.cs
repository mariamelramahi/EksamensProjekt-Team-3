using Microsoft.Data.SqlClient;
﻿using Microsoft.Extensions.Configuration; // Til indlæsning af konfigurationsindstillinger fra appsettings.json
using System.Data;
using System.Data.SqlClient; // Til at arbejde med SQL Server via ADO.NET
using Microsoft.Data.SqlClient;
using System.Windows;

namespace EksamensProjekt.Models.Repositories;

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
            var command = new SqlCommand("sp_GetTenancyByID", connection)
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
                        TenancyStatus = Enum.TryParse<TenancyStatus>(reader.GetString(reader.GetOrdinal("TenancyStatus")), out var status) ? status: TenancyStatus.Vacant, // Provide a default value for invalid statuses.
                        MoveInDate = reader.IsDBNull(reader.GetOrdinal("MoveInDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                        MoveOutDate = reader.IsDBNull(reader.GetOrdinal("MoveOutDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MoveOutDate")),
                        SquareMeter = reader.GetInt32(reader.GetOrdinal("SquareMeter")),
                        Rent = reader.IsDBNull(reader.GetOrdinal("Rent")) ? null : (int?)reader.GetDecimal(reader.GetOrdinal("Rent")),
                        Rooms = reader.GetInt32(reader.GetOrdinal("Rooms")),
                        Bathrooms = reader.GetInt32(reader.GetOrdinal("Bathrooms")),
                        PetsAllowed = reader.GetBoolean(reader.GetOrdinal("PetsAllowed")),
                        IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        Tenants = new List<Tenant>(),
                        Address = new Address(),
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

    void IRepo<Tenancy>.Delete(Tenancy entity)
    {
        // Validate the entity (check for null)
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "The tenancy entity cannot be null.");
        }

        // Use the GetByID method to retrieve the existing tenancy from the database
        Tenancy existingTenancy = GetByID(entity.TenancyID);

        if (existingTenancy == null)
        {
            throw new InvalidOperationException($"Tenancy with ID {entity.TenancyID} not found.");
        }

        // Call the stored procedure to update the tenancy in the database
        using (var connection = new SqlConnection(_connectionString))
        {
            string procedureName = "sp_DeleteTenancy"; // Name of the stored procedure

            var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add parameters to the SqlCommand based on whether they are provided in the entity
            command.Parameters.AddWithValue("@TenancyID", entity.TenancyID);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"TenancyID {entity.TenancyID} not deleted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the Tenancy: " + ex.Message);
                throw;
            }
        }
    }

    Tenancy IRepo<Tenancy>.GetByUsername(string userName)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Tenancy> ReadAll()
    {
        var tenancies = new List<Tenancy>();

        using (var conn = new SqlConnection(_connectionString))
        {
            // calling stored procedure in sql
            var cmd = new SqlCommand("usp_ReadAllTenancies", conn)
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
                        // Create a Tenancy object from the SQL data reader
                        var tenancy = new Tenancy()
                        {
                            TenancyID = reader.GetInt32(0),
                            TenancyStatus = (TenancyStatus)Enum.Parse(typeof(TenancyStatus), reader.GetString(1)),
                            MoveInDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            MoveOutDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                            SquareMeter = reader.GetInt32(4),
                            Rent = (int?)reader.GetDecimal(5),
                            Rooms = reader.GetInt32(6),
                            Bathrooms = reader.GetInt32(7),
                            PetsAllowed = reader.GetBoolean(8),
                            OrganizationID = reader.GetInt32(10),
                            Company = reader.IsDBNull(11) ? null : GetCompanyById(reader.GetInt32(11)),
                            IsDeleted = reader.GetBoolean(12)
                        };

                        // Fetch the related address for the tenancy
                        int AddressID = reader.GetInt32(9);
                        tenancy.Address = GetAddressById(AddressID);
                        if (tenancy.Address == null)
                        {
                            MessageBox.Show($"No address found for TenancyID {tenancy.TenancyID} with StandardAddressID {AddressID}");
                        }

                        // Fetch the related tenants for the tenancy
                        tenancy.Tenants = GetTenantsByTenancyId(tenancy.TenancyID) ?? new List<Tenant>();

                        tenancies.Add(tenancy);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while reading all Tenancy entries: " + ex.Message);
            }
        }

        return tenancies;
    }

    // Helper method to get a StandardAddress by its ID
    private Address GetAddressById(int standardAddressID)
    {
        Address address = null;

        using (var conn = new SqlConnection(_connectionString))
        {
            string addressQuery = "SELECT StandardAddressID, Street, Number, FloorNumber, Zipcode, Country FROM StandardAddress WHERE StandardAddressID = @StandardAddressID";
            var cmd = new SqlCommand(addressQuery, conn);
            cmd.Parameters.AddWithValue("@StandardAddressID", standardAddressID);

            try
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        address = new Address()
                        {
                            AddressID = reader.GetInt32(0),
                            Street = reader.GetString(1),
                            Number = reader.GetString(2),
                            FloorNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Zipcode = reader.GetString(4),
                            Country = reader.GetString(5)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading the address: " + ex.Message);
            }
        }

        return address;
    }

    // Helper method to get tenants by TenancyID
    private List<Tenant> GetTenantsByTenancyId(int tenancyId)
    {
        var tenants = new List<Tenant>();

        using (var conn = new SqlConnection(_connectionString))
        {
            string tenantQuery = "SELECT p.PartyID, p.FirstName, p.LastName, p.PhoneNum, p.Email, p.PartyRole " +
                                 "FROM TenancyTenant tt " +
                                 "JOIN Tenant t ON tt.TenantID = t.TenantID " +
                                 "JOIN Party p ON t.PartyID = p.PartyID " +
                                 "WHERE tt.TenancyID = @TenancyID";
            var cmd = new SqlCommand(tenantQuery, conn);
            cmd.Parameters.AddWithValue("@TenancyID", tenancyId);

            try
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tenant = new Tenant()
                        {
                            TenantID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            PhoneNum = reader.GetString(3),
                            Email = reader.GetString(4),
                            PartyRole = reader.GetString(5)
                        };

                        tenants.Add(tenant);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading tenants: " + ex.Message);
            }
        }

        return tenants;
    }
    private Company GetCompanyById(int companyId)
    {
        Company company = null;

        using (var conn = new SqlConnection(_connectionString))
        {
            string companyQuery = "SELECT CompanyID, CompanyName, PartyID FROM Company WHERE CompanyID = @CompanyID";
            var cmd = new SqlCommand(companyQuery, conn);
            cmd.Parameters.AddWithValue("@CompanyID", companyId);

            try
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        company = new Company()
                        {
                            CompanyID = reader.GetInt32(0),
                            CompanyName = reader.GetString(1),
                            PartyID = reader.GetInt32(2)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading the company: " + ex.Message);
            }
        }

        return company;
    }

    //IEnumerable<Tenancy> IRepo<Tenancy>.ReadAll()
    //{
    //    throw new NotImplementedException();
    //}

    void IRepo<Tenancy>.Update(Tenancy entity)
    {
        // Validate the entity (check for null)
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "The tenancy entity cannot be null.");
        }

        // Use the GetByID method to retrieve the existing tenancy from the database
        Tenancy existingTenancy = GetByID(entity.TenancyID);

        if (existingTenancy == null)
        {
            throw new InvalidOperationException($"Tenancy with ID {entity.TenancyID} not found.");
        }

        // Call the stored procedure to update the tenancy in the database
        using (var connection = new SqlConnection(_connectionString))
        {
            string procedureName = "sp_UpdateTenancy"; // Name of the stored procedure

            var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add parameters to the SqlCommand based on whether they are provided in the entity
            command.Parameters.AddWithValue("@TenancyID", entity.TenancyID);

            // Only add parameters if the corresponding field is provided
            command.Parameters.AddWithValue("@TenancyStatus", entity.TenancyStatus.HasValue ? (object)entity.TenancyStatus.Value.ToString() : DBNull.Value);
            command.Parameters.AddWithValue("@MoveInDate", entity.MoveInDate.HasValue ? (object)entity.MoveInDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@MoveOutDate", entity.MoveOutDate.HasValue ? (object)entity.MoveOutDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@SquareMeter", entity.SquareMeter > 0 ? (object)entity.SquareMeter : DBNull.Value);
            command.Parameters.AddWithValue("@Rent", entity.Rent.HasValue ? (object)entity.Rent.Value : DBNull.Value);
            command.Parameters.AddWithValue("@Rooms", entity.Rooms.HasValue ? (object)entity.Rooms.Value : DBNull.Value);
            command.Parameters.AddWithValue("@Bathrooms", entity.Bathrooms.HasValue ? (object)entity.Bathrooms.Value : DBNull.Value);
            command.Parameters.AddWithValue("@PetsAllowed", entity.PetsAllowed.HasValue ? (object)entity.PetsAllowed.Value : DBNull.Value);
            command.Parameters.AddWithValue("@CompanyID", entity.Company != null && entity.Company.CompanyID != 0 ? (object)entity.Company.CompanyID : DBNull.Value);
            command.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted ? (object)entity.IsDeleted : DBNull.Value);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"No rows were updated for TenancyID {entity.TenancyID}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the Tenancy: " + ex.Message);
                throw;
            }
        }
    }
}

