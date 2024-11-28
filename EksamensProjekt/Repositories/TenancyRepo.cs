using EksamensProjekt.DataAccess;
using EksamensProjekt.Models;
using EksamensProjekt.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EksamensProjekt.Repos;

public class TenancyRepo : IRepo<Tenancy>, ITenantTenancy
{

    private readonly string _connectionString;


    //Constructor, that initializes the connection string for the database
    public TenancyRepo(string connectionString)
    {
        //the connection string is assigned if it is not null; otherwise, it throws an exception
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }


    public Tenancy GetByID(int tenancyID)
    {
        // Initializes a Tenancy object to null
        Tenancy tenancy = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("usp_GetTenancyByID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@TenancyID", tenancyID);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tenancy = SqlDataMapper.PopulateTenancyFromReader(reader);
                }
            }
        }
        return tenancy;
    }


    public void Create(Tenancy tenancy)
    {
        // Use service layer for validation
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_CreateTenancy", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Explicitly set isUpdate to false
            SqlDataMapper.AddTenancyParameters(command, tenancy, isUpdate: false);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("The tenancy could not be created. No rows affected.");
            }
        }
    }


    // Not in use for now - We use soft delete (Update() IsDeleted field instead) 
    public void Delete(int tenancyID)
    {
        // Validate the tenancyID (check if it's a valid ID)
        if (tenancyID <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tenancyID), "The tenancy ID must be greater than zero.");
        }

        // Call the stored procedure to delete the tenancy in the database
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_DeleteTenancy", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TenancyID", tenancyID);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No rows were deleted for TenancyID {tenancyID}");
            }
        }
    }


    public IEnumerable<Tenancy> ReadAll() // classes C# reference type meaning List and Dict points to the same object in memory
    {
        var tenancies = new List<Tenancy>();
        var tenancyMap = new Dictionary<int, Tenancy>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var cmd = new SqlCommand("usp_ReadAllTenancies", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                connection.Open();
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


    public void Update(Tenancy tenancy)
    {
        // Use service layer for validation
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_UpdateTenancy", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Explicitly set isUpdate to true
            SqlDataMapper.AddTenancyParameters(command, tenancy, isUpdate: true);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No rows were updated for TenancyID {tenancy.TenancyID}");
            }
        }
    }






    // Implemented through ITenancyTenant
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

    public void RemoveTenantFromTenancy(int tenancyID, int tenantID)
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



}






