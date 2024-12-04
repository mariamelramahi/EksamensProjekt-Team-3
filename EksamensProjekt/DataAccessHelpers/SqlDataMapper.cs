using EksamensProjekt.Models;
using Microsoft.Data.SqlClient;

namespace EksamensProjekt.DataAccess
{
    public static class SqlDataMapper
    {

        //
        // Tenancy
        //

        public static Tenancy PopulateTenancyFromReader(SqlDataReader reader)
        {
            var tenancy = new Tenancy
            {
                TenancyID = reader.GetInt32(reader.GetOrdinal("TenancyID")),
                TenancyStatus = SqlDataReaderHelper.GetEnumValue<TenancyStatus>(reader, "TenancyStatus"),
                MoveInDate = SqlDataReaderHelper.GetValueOrNull<DateTime?>(reader, "MoveInDate"),
                MoveOutDate = SqlDataReaderHelper.GetValueOrNull<DateTime?>(reader, "MoveOutDate"),
                SquareMeter = SqlDataReaderHelper.GetValueOrNull<int?>(reader, "SquareMeter"),
                Rent = SqlDataReaderHelper.GetValueOrNull<decimal?>(reader, "Rent"),
                Rooms = SqlDataReaderHelper.GetValueOrNull<int?>(reader, "Rooms"),
                Bathrooms = SqlDataReaderHelper.GetValueOrNull<int?>(reader, "BathRooms"),
                PetsAllowed = SqlDataReaderHelper.GetValueOrNull<bool?>(reader, "PetsAllowed"),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),

                Address = new Address
                {
                    AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                    Street = SqlDataReaderHelper.GetValueOrNull<string?>(reader, "Street"),
                    Number = SqlDataReaderHelper.GetValueOrNull<string?>(reader, "Number"),
                    FloorNumber = SqlDataReaderHelper.GetValueOrNull<string?>(reader, "FloorNumber"),
                    Zipcode = SqlDataReaderHelper.GetValueOrNull<string?>(reader, "Zipcode"),
                    Country = SqlDataReaderHelper.GetValueOrNull<string?>(reader, "Country"),
                    IsStandardized = reader.GetBoolean(reader.GetOrdinal("IsStandardized"))
                },

                Organization = new Organization
                {
                    OrganizationID = reader.GetInt32(reader.GetOrdinal("OrganizationID")),
                    OrganizationName = SqlDataReaderHelper.GetValueOrNull<string>(reader, "OrganizationName"),
                    PhoneNum = SqlDataReaderHelper.GetValueOrNull<string>(reader, "OrganizationPhoneNum"),
                    Email = SqlDataReaderHelper.GetValueOrNull<string>(reader, "OrganizationEmail")
                }
            };

            if (!reader.IsDBNull(reader.GetOrdinal("CompanyID")))
            {
                tenancy.Company = new Company
                {
                    CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                    CompanyName = SqlDataReaderHelper.GetValueOrNull<string>(reader, "CompanyName"),
                    PhoneNum = SqlDataReaderHelper.GetValueOrNull<string>(reader, "CompanyPhoneNum"),
                    Email = SqlDataReaderHelper.GetValueOrNull<string>(reader, "CompanyEmail")
                };
            }

            tenancy.Tenants = new List<Tenant>(); // Initialized for adding tenants later

            return tenancy;
        }


        public static void AddTenancyParameters(SqlCommand command, Tenancy tenancy, bool isUpdate)
        {
            if (isUpdate) 
                command.Parameters.AddWithValue("@TenancyID", tenancy.TenancyID); // Include ID for update operations

            // Add parameters to the SqlCommand based on the tenancy fields
            command.Parameters.AddWithValue("@TenancyStatus", tenancy.TenancyStatus.HasValue ? tenancy.TenancyStatus.Value.ToString() : TenancyStatus.Vacant.ToString());
            command.Parameters.AddWithValue("@MoveInDate", tenancy.MoveInDate.HasValue ? tenancy.MoveInDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@MoveOutDate", tenancy.MoveOutDate.HasValue ? tenancy.MoveOutDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("@SquareMeter", tenancy.SquareMeter > 0 ? tenancy.SquareMeter : DBNull.Value);
            command.Parameters.AddWithValue("@Rent", tenancy.Rent.HasValue ? tenancy.Rent.Value : DBNull.Value);
            command.Parameters.AddWithValue("@Rooms", tenancy.Rooms.HasValue ? tenancy.Rooms.Value : DBNull.Value);
            command.Parameters.AddWithValue("@Bathrooms", tenancy.Bathrooms.HasValue ? tenancy.Bathrooms.Value : DBNull.Value);
            command.Parameters.AddWithValue("@PetsAllowed", tenancy.PetsAllowed.HasValue ? tenancy.PetsAllowed.Value : DBNull.Value);
            command.Parameters.AddWithValue("@CompanyID", tenancy.Company != null && tenancy.Company.CompanyID != 0 ? tenancy.Company.CompanyID : DBNull.Value);
            command.Parameters.AddWithValue("@IsDeleted", tenancy.IsDeleted);
            command.Parameters.AddWithValue("@AddressID", tenancy.Address.AddressID);
            command.Parameters.AddWithValue("OrganizationID", 1);
        }






        //
        // Tenant
        //


        public static Tenant PopulateTenantFromReader(SqlDataReader reader)
        {
            return new Tenant
            {
                TenantID = SqlDataReaderHelper.GetValueOrNull<int>(reader, "TenantID"),
                PartyID = SqlDataReaderHelper.GetValueOrNull<int>(reader, "PartyID"),
                FirstName = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantFirstName"),
                LastName = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantLastName"),
                PhoneNum = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantPhoneNum"),
                Email = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantEmail"),
                PartyRole = SqlDataReaderHelper.GetValueOrNull<string>(reader, "PartyRole")
            };
        }



        public static void AddTenantParameters(SqlCommand command, Tenant tenant, bool isUpdate)
        {
            if (isUpdate)
                command.Parameters.AddWithValue("@TenantID", tenant.TenantID); // Include ID for update operations

            // Add parameters to the SqlCommand based on the tenant fields
            command.Parameters.AddWithValue("@FirstName", tenant.FirstName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LastName", tenant.LastName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNum", tenant.PhoneNum ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", tenant.Email ?? (object)DBNull.Value);
        }











    }
}
