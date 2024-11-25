using EksamensProjekt.Models;
using Microsoft.Data.SqlClient;

namespace EksamensProjekt.Utilities.DataAccess
{
    public static class SqlDataMapper
    {
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
                    Street = SqlDataReaderHelper.GetValueOrNull<string>(reader, "Street"),
                    Number = SqlDataReaderHelper.GetValueOrNull<string>(reader, "Number"),
                    FloorNumber = SqlDataReaderHelper.GetValueOrNull<string>(reader, "FloorNumber"),
                    Zipcode = SqlDataReaderHelper.GetValueOrNull<string>(reader, "Zipcode"),
                    Country = SqlDataReaderHelper.GetValueOrNull<string>(reader, "Country"),
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

        public static Tenant PopulateTenantFromReader(SqlDataReader reader)
        {
            return new Tenant
            {
                TenantID = reader.GetInt32(reader.GetOrdinal("TenantID")),
                FirstName = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantFirstName"),
                LastName = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantLastName"),
                PhoneNum = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantPhoneNum"),
                Email = SqlDataReaderHelper.GetValueOrNull<string>(reader, "TenantEmail")
            };
        }
    }
}
