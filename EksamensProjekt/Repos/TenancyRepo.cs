using EksamensProjekt.Models;
using EksamensProjekt.Utilities.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace EksamensProjekt.Repos;

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
            var command = new SqlCommand("usp_GetTenancyByID", connection)
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
                        TenancyStatus = Enum.TryParse<TenancyStatus>(reader.GetString(reader.GetOrdinal("TenancyStatus")), out var status) ? status : TenancyStatus.Vacant, // Provide a default value for invalid statuses.
                        MoveInDate = reader.IsDBNull(reader.GetOrdinal("MoveInDate")) ? null : reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                        MoveOutDate = reader.IsDBNull(reader.GetOrdinal("MoveOutDate")) ? null : reader.GetDateTime(reader.GetOrdinal("MoveOutDate")),
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
            string procedureName = "usp_DeleteTenancy"; // Name of the stored procedure

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
                MessageBox.Show("Der opstod en fejl under sletning af lejemålet: " + ex.Message);
                throw;
            }
        }
    }


    public IEnumerable<Tenancy> ReadAll() // classes C# reference type meaning List and Dict points to the same object in memory
    {
        var tenancies = new List<Tenancy>();
        var tenancyMap = new Dictionary<int, Tenancy>();

        using (var conn = new SqlConnection(_connectionString))
        {
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
                        int tenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID"));

                        // Check if we already added this tenancy, otherwise create a new instance
                        if (!tenancyMap.ContainsKey(tenancyID))
                        {
                            var tenancy = SqlDataMapper.PopulateTenancyFromReader(reader);
                            tenancyMap[tenancyID] = tenancy;
                            tenancies.Add(tenancy);
                        }

                        // Add tenant details if they exist
                        if (!reader.IsDBNull(reader.GetOrdinal("TenantID")))
                        {
                            var tenant = SqlDataMapper.PopulateTenantFromReader(reader);
                            tenancyMap[tenancyID].Tenants.Add(tenant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching tenancies: " + ex.Message);
            }
        }

        return tenancies;
    }



    //public IEnumerable<Tenancy> ReadAll()
    //{
    //    var tenancies = new List<Tenancy>();
    //    var tenancyMap = new Dictionary<int, Tenancy>();

    //    using (var conn = new SqlConnection(_connectionString))
    //    {
    //        var cmd = new SqlCommand("usp_ReadAllTenancies", conn)
    //        {
    //            CommandType = CommandType.StoredProcedure
    //        };

    //        try
    //        {
    //            conn.Open();
    //            using (SqlDataReader reader = cmd.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    int tenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID"));

    //                    // Check if we already added this tenancy, otherwise create a new instance
    //                    if (!tenancyMap.ContainsKey(tenancyID))
    //                    {
    //                        var tenancy = PopulateTenancyFromReader(reader);
    //                        tenancyMap[tenancyID] = tenancy;
    //                        tenancies.Add(tenancy);
    //                    }

    //                    // Add tenant details if they exist
    //                    if (!reader.IsDBNull(reader.GetOrdinal("TenantID")))
    //                    {
    //                        var tenant = PopulateTenantFromReader(reader);
    //                        tenancyMap[tenancyID].Tenants.Add(tenant);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Error while fetching tenancies: " + ex.Message);
    //        }
    //    }

    //    return tenancies;
    //}

    //private Tenancy PopulateTenancyFromReader(SqlDataReader reader)
    //{
    //    var tenancy = new Tenancy
    //    {
    //        TenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID")),
    //        TenancyStatus = (TenancyStatus)Enum.Parse(typeof(TenancyStatus), reader.GetString(reader.GetOrdinal("TenancyStatus"))),
    //        MoveInDate = GetValueOrDefault<DateTime?>(reader, "MoveInDate"),
    //        MoveOutDate = GetValueOrDefault<DateTime?>(reader, "MoveOutDate"),
    //        SquareMeter = reader.GetInt32(reader.GetOrdinal("SquareMeter")),
    //        Rent = GetValueOrDefault<decimal?>(reader, "Rent"),
    //        Rooms = reader.GetInt32(reader.GetOrdinal("Rooms")),
    //        Bathrooms = reader.GetInt32(reader.GetOrdinal("BathRooms")),
    //        PetsAllowed = reader.GetBoolean(reader.GetOrdinal("PetsAllowed")),
    //        IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
    //        Address = new Address
    //        {
    //            AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
    //            Street = GetValueOrDefault<string>(reader, "Street"),
    //            Number = GetValueOrDefault<string>(reader, "Number"),
    //            FloorNumber = GetValueOrDefault<string>(reader, "FloorNumber"),
    //            Zipcode = GetValueOrDefault<string>(reader, "Zipcode"),
    //            Country = GetValueOrDefault<string>(reader, "Country"),
    //            IsStandardized = reader.GetBoolean(reader.GetOrdinal("IsStandardized"))
    //        },
    //        Organization = new Organization
    //        {
    //            OrganizationID = reader.GetInt32(reader.GetOrdinal("OrganizationID")),
    //            OrganizationName = GetValueOrDefault<string>(reader, "OrganizationName"),
    //            PhoneNum = GetValueOrDefault<string>(reader, "OrganizationPhoneNum"),
    //            Email = GetValueOrDefault<string>(reader, "OrganizationEmail")
    //        }
    //    };

    //    if (!reader.IsDBNull(reader.GetOrdinal("CompanyID")))
    //    {
    //        tenancy.Company = new Company
    //        {
    //            CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
    //            CompanyName = GetValueOrDefault<string>(reader, "CompanyName"),
    //            PhoneNum = GetValueOrDefault<string>(reader, "CompanyPhoneNum"),
    //            Email = GetValueOrDefault<string>(reader, "CompanyEmail")
    //        };
    //    }

    //    // Initialize tenants list to add later
    //    tenancy.Tenants = new List<Tenant>();

    //    return tenancy;
    //}

    //private Tenant PopulateTenantFromReader(SqlDataReader reader)
    //{
    //    return new Tenant
    //    {
    //        TenantID = reader.GetInt32(reader.GetOrdinal("TenantID")),
    //        FirstName = GetValueOrDefault<string>(reader, "TenantFirstName"),
    //        LastName = GetValueOrDefault<string>(reader, "TenantLastName"),
    //        PhoneNum = GetValueOrDefault<string>(reader, "TenantPhoneNum"),
    //        Email = GetValueOrDefault<string>(reader, "TenantEmail")
    //    };
    //}

    //// Helper method to handle nullable value types and strings in SqlDataReader
    //private T GetValueOrDefault<T>(SqlDataReader reader, string columnName)
    //{
    //    int columnOrdinal = reader.GetOrdinal(columnName);
    //    if (reader.IsDBNull(columnOrdinal))
    //    {
    //        if (typeof(T) == typeof(string))
    //        {
    //            return (T)(object)string.Empty; // Return empty string for strings
    //        }
    //        return default(T); // Return default value for other types
    //    }
    //    return reader.GetFieldValue<T>(columnOrdinal);
    //}




    //public IEnumerable<Tenancy> ReadAll() // Reference type (classes in C#)
    //{
    //    var tenancies = new List<Tenancy>(); // Points to the same object as Dictionary
    //    var tenancyMap = new Dictionary<int, Tenancy>(); // Points to the same object as List

    //    using (var conn = new SqlConnection(_connectionString))
    //    {
    //        var cmd = new SqlCommand("usp_ReadAllTenancies", conn)
    //        {
    //            CommandType = CommandType.StoredProcedure
    //        };

    //        try
    //        {
    //            conn.Open();
    //            using (SqlDataReader reader = cmd.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    int tenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID"));

    //                    // Check if we already added this tenancy, otherwise create a new instance
    //                    if (!tenancyMap.ContainsKey(tenancyID))
    //                    {
    //                        var tenancy = CreateTenancyFromReader(reader);
    //                        tenancyMap[tenancyID] = tenancy;
    //                        tenancies.Add(tenancy);
    //                    }

    //                    // Add tenant details if they exist
    //                    if (!reader.IsDBNull(reader.GetOrdinal("TenantID")))
    //                    {
    //                        var tenant = CreateTenantFromReader(reader);
    //                        tenancyMap[tenancyID].Tenants.Add(tenant);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Error while fetching tenancies: " + ex.Message);
    //        }
    //    }

    //    return tenancies;
    //}
    //private Tenancy CreateTenancyFromReader(SqlDataReader reader)
    //{
    //    var tenancy = new Tenancy
    //    {
    //        TenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID")),
    //        TenancyStatus = (TenancyStatus)Enum.Parse(typeof(TenancyStatus), reader.GetString(reader.GetOrdinal("TenancyStatus"))),
    //        MoveInDate = reader.IsDBNull(reader.GetOrdinal("MoveInDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
    //        MoveOutDate = reader.IsDBNull(reader.GetOrdinal("MoveOutDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MoveOutDate")),
    //        SquareMeter = reader.GetInt32(reader.GetOrdinal("SquareMeter")),
    //        Rent = reader.IsDBNull(reader.GetOrdinal("Rent")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Rent")),
    //        Rooms = reader.GetInt32(reader.GetOrdinal("Rooms")),
    //        Bathrooms = reader.GetInt32(reader.GetOrdinal("BathRooms")),
    //        PetsAllowed = reader.GetBoolean(reader.GetOrdinal("PetsAllowed")),
    //        IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
    //        Address = new Address
    //        {
    //            AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
    //            Street = reader.GetString(reader.GetOrdinal("Street")),
    //            Number = reader.GetString(reader.GetOrdinal("Number")),
    //            FloorNumber = reader.IsDBNull(reader.GetOrdinal("FloorNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("FloorNumber")),
    //            Zipcode = reader.GetString(reader.GetOrdinal("Zipcode")),
    //            Country = reader.GetString(reader.GetOrdinal("Country")),
    //            IsStandardized = reader.GetBoolean(reader.GetOrdinal("IsStandardized"))
    //        },
    //        Organization = new Organization
    //        {
    //            OrganizationID = reader.GetInt32(reader.GetOrdinal("OrganizationID")),
    //            OrganizationName = reader.GetString(reader.GetOrdinal("OrganizationName")),
    //            PhoneNum = reader.GetString(reader.GetOrdinal("OrganizationPhoneNum")),
    //            Email = reader.GetString(reader.GetOrdinal("OrganizationEmail"))
    //        }
    //    };

    //    if (!reader.IsDBNull(reader.GetOrdinal("CompanyID")))
    //    {
    //        tenancy.Company = new Company
    //        {
    //            CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
    //            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
    //            PhoneNum = reader.GetString(reader.GetOrdinal("CompanyPhoneNum")),
    //            Email = reader.GetString(reader.GetOrdinal("CompanyEmail"))
    //        };
    //    }

    //    // Initialize tenants list to add later
    //    tenancy.Tenants = new List<Tenant>();

    //    return tenancy;
    //}
    //private Tenant CreateTenantFromReader(SqlDataReader reader)
    //{
    //    return new Tenant
    //    {
    //        TenantID = reader.GetInt32(reader.GetOrdinal("TenantID")),
    //        FirstName = reader.GetString(reader.GetOrdinal("TenantFirstName")),
    //        LastName = reader.GetString(reader.GetOrdinal("TenantLastName")),
    //        PhoneNum = reader.GetString(reader.GetOrdinal("TenantPhoneNum")),
    //        Email = reader.GetString(reader.GetOrdinal("TenantEmail"))
    //    };
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
            string procedureName = "usp_UpdateTenancy"; // Name of the stored procedure

            var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add parameters to the SqlCommand based on whether they are provided in the entity
            command.Parameters.AddWithValue("@TenancyID", entity.TenancyID);

            // Only add parameters if the corresponding field is provided
            command.Parameters.AddWithValue("@TenancyStatus", entity.TenancyStatus.HasValue ? entity.TenancyStatus.Value.ToString() : DBNull.Value);
            command.Parameters.AddWithValue("@MoveInDate", entity.MoveInDate.HasValue ? entity.MoveInDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@MoveOutDate", entity.MoveOutDate.HasValue ? entity.MoveOutDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@SquareMeter", entity.SquareMeter > 0 ? entity.SquareMeter : DBNull.Value);
            command.Parameters.AddWithValue("@Rent", entity.Rent.HasValue ? entity.Rent.Value : DBNull.Value);
            command.Parameters.AddWithValue("@Rooms", entity.Rooms.HasValue ? entity.Rooms.Value : DBNull.Value);
            command.Parameters.AddWithValue("@Bathrooms", entity.Bathrooms.HasValue ? entity.Bathrooms.Value : DBNull.Value);
            command.Parameters.AddWithValue("@PetsAllowed", entity.PetsAllowed.HasValue ? entity.PetsAllowed.Value : DBNull.Value);
            command.Parameters.AddWithValue("@CompanyID", entity.Company != null && entity.Company.CompanyID != 0 ? entity.Company.CompanyID : DBNull.Value);
            command.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted ? entity.IsDeleted : DBNull.Value);

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
                MessageBox.Show("Der opstod en fejl under opdatering af lejemålet: " + ex.Message);
                throw;
            }
        }
    }
}

