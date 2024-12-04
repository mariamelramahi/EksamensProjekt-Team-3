using EksamensProjekt.DataAccess;
using EksamensProjekt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EksamensProjekt.Repos;

public class AddressRepo : IRepo<Address>
{

    private readonly string _connectionString;

    //Constructor that initialzies the connection string
    public AddressRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    // USPs:
    // usp_GetAddressByID
    // usp_CreateAddress
    // usp_ReadAllAddresses
    // usp_UpdateAddress
    // usp_DeleteAddress
     
    public Address GetByID(int id)
    {
        Address address = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("usp_GetAddressByID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@AddressID", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    address = SqlDataMapper.PopulateAddressFromReader(reader);
                }
            }
        }

        return address;
    }


    public void Create(Address address)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_CreateAddress", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataMapper.AddAddressParameters(command, address, isUpdate: false);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("The address could not be created. No rows affected.");
            }
        }
    }

     
    public IEnumerable<Address> ReadAll()
    {
        var addresses = new List<Address>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_ReadAllAddresses", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    addresses.Add(SqlDataMapper.PopulateAddressFromReader(reader));
                }
            }
        }

        return addresses;
    }


    public void Update(Address address)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_UpdateAddress", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataMapper.AddAddressParameters(command, address, isUpdate: true);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("usp_DeleteAddress", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@AddressID", id);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No rows were deleted for AddressID {id}");
            }
        }
    }

}
