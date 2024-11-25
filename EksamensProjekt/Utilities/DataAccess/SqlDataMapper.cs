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
                MoveInDate = SqlDataReaderHelper.GetValueOrDefault<DateTime?>(reader, "MoveInDate"),
                MoveOutDate = SqlDataReaderHelper.GetValueOrDefault<DateTime?>(reader, "MoveOutDate"),
                SquareMeter = SqlDataReaderHelper.GetValueOrDefault<int?>(reader, "SquareMeter"),
                Rent = SqlDataReaderHelper.GetValueOrDefault<decimal?>(reader, "Rent"),
                Rooms = SqlDataReaderHelper.GetValueOrDefault<int?>(reader, "Rooms"),
                Bathrooms = SqlDataReaderHelper.GetValueOrDefault<int?>(reader, "BathRooms"),
                PetsAllowed = SqlDataReaderHelper.GetValueOrDefault<bool?>(reader, "PetsAllowed"),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),

                Address = new Address
                {
                    AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                    Street = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "Street"),
                    Number = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "Number"),
                    FloorNumber = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "FloorNumber"),
                    Zipcode = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "Zipcode"),
                    Country = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "Country"),
                    IsStandardized = reader.GetBoolean(reader.GetOrdinal("IsStandardized"))
                },

                Organization = new Organization
                {
                    OrganizationID = reader.GetInt32(reader.GetOrdinal("OrganizationID")),
                    OrganizationName = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "OrganizationName"),
                    PhoneNum = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "OrganizationPhoneNum"),
                    Email = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "OrganizationEmail")
                }
            };

            if (!reader.IsDBNull(reader.GetOrdinal("CompanyID")))
            {
                tenancy.Company = new Company
                {
                    CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                    CompanyName = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "CompanyName"),
                    PhoneNum = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "CompanyPhoneNum"),
                    Email = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "CompanyEmail")
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
                FirstName = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "TenantFirstName"),
                LastName = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "TenantLastName"),
                PhoneNum = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "TenantPhoneNum"),
                Email = SqlDataReaderHelper.GetReferenceValueOrDefault<string>(reader, "TenantEmail")
            };
        }
    }
}
