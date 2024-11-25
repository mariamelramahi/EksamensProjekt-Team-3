﻿using Microsoft.Extensions.Configuration; // Til indlæsning af konfigurationsindstillinger fra appsettings.json
using System.Data;
using System.Data.SqlClient; // Til at arbejde med SQL Server via ADO.NET
using Microsoft.Data.SqlClient;

namespace EksamensProjekt.Models.Repositories;

public class StandardAddressRepo : IRepo<Address>
{
    private readonly string _connectionString;

    //Constructor that initialzies the connection string
    public StandardAddressRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Delete(int entity)
    {
        throw new NotImplementedException();
    }

    //Methhod to retrieve a StandardAddress object by its ID using a stored procedure
    public Address GetByID(int id)
    {
        // Intializes a new StandardAddressContext object with the connection string
        Address address = null;

        // Establishes a new SQL database connection
        using (var connection = new SqlConnection(_connectionString))
        {
            // Opens the connection to the database
            connection.Open();

            // Creates a new SQL command object with the stored procedure name and the connection
            var command = new SqlCommand("usp_GetStandardAddressByID", connection)//USP not created in database
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
                    address = new Address
                    {
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        Street = reader.GetString(reader.GetOrdinal("Street")),
                        Number = reader.GetString(reader.GetOrdinal("Number")),
                        FloorNumber = reader.GetString(reader.GetOrdinal("Floor")),
                        Zipcode = reader.GetString(reader.GetOrdinal("ZipCode")),
                        Country = reader.GetString(reader.GetOrdinal("Country"))
                    };
                }
            }
        }
        return address;
    }

    void IRepo<Address>.Create(Address entity)
    {
        throw new NotImplementedException();
    }

    Address IRepo<Address>.GetByID(int id)
    {
        throw new NotImplementedException();
    }

    Address IRepo<Address>.GetByUsername(string userName)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Address> IRepo<Address>.ReadAll()
    {
        throw new NotImplementedException();
    }

    void IRepo<Address>.Update(Address entity)
    {
        throw new NotImplementedException();
    }
    public void Delete(Address entity)
    {
        throw new NotImplementedException();
    }
}
