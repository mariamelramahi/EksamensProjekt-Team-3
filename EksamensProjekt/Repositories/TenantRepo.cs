using EksamensProjekt.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EksamensProjekt.Repos;

public class TenantRepo : IRepo<Tenant>
{
    private readonly string _connectionString;

    //Constructor, that initializes the connection string for the database
    public TenantRepo(string connectionString)
    {
        //the connection string is assigned if it is not null; otherwise, it throws an exception
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }


    public Tenant GetByID(int tenantID)
    {
        Tenant tenant = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("usp_GetTenantByID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@TenantID", tenantID);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tenant = SqlDataMapper.PopulateTenantFromReader(reader);
                }
            }
        }

        return tenant;
    }


    public void Create(Tenant tenant)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_CreateTenant", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Explicitly set isUpdate to false
            SqlDataMapper.AddTenantParameters(command, tenant, isUpdate: false);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("The tenant could not be created. No rows affected.");
            }
        }
    }


    public IEnumerable<Tenant> ReadAll()
    {
        var tenants = new List<Tenant>();

        using (var conn = new SqlConnection(_connectionString))
        {
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
                        // Use SqlDataMapper to map the reader to a Tenant object
                        var tenant = SqlDataMapper.PopulateTenantFromReader(reader);
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


    public void Update(Tenant tenant)
    {
        // Use service layer for validation
        if (tenant == null)
        {
            throw new ArgumentNullException(nameof(tenant), "The tenant entity cannot be null.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_UpdateTenant", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Explicitly set isUpdate to true
            SqlDataMapper.AddTenantParameters(command, tenant, isUpdate: true);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No rows were updated for TenantID {tenant.TenantID}");
            }
        }
    }


    public void Delete(int tenantID)
    {
        if (tenantID <= 0)
        {
            throw new ArgumentException("TenantID must be greater than 0.", nameof(tenantID));
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_DeleteTenant", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TenantID", tenantID);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No tenant was deleted with TenantID {tenantID}.");
            }
        }
    }




}