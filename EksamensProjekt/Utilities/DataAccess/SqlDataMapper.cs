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
                TenancyStatus = (TenancyStatus)Enum.Parse(typeof(TenancyStatus), reader.GetString(reader.GetOrdinal("TenancyStatus"))),
                MoveInDate = SqlDataReaderHelper.GetValueOrDefault<DateTime?>(reader, "MoveInDate"),
                MoveOutDate = SqlDataReaderHelper.GetValueOrDefault<DateTime?>(reader, "MoveOutDate"),
                SquareMeter = reader.GetInt32(reader.GetOrdinal("SquareMeter")),
                Rent = SqlDataReaderHelper.GetValueOrDefault<decimal?>(reader, "Rent"),
                Rooms = reader.GetInt32(reader.GetOrdinal("Rooms")),
                Bathrooms = reader.GetInt32(reader.GetOrdinal("BathRooms")),
                PetsAllowed = reader.GetBoolean(reader.GetOrdinal("PetsAllowed")),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                Address = new Address
                {
                    AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                    Street = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "Street"),
                    Number = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "Number"),
                    FloorNumber = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "FloorNumber"),
                    Zipcode = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "Zipcode"),
                    Country = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "Country"),
                    IsStandardized = reader.GetBoolean(reader.GetOrdinal("IsStandardized"))
                },
                Organization = new Organization
                {
                    OrganizationID = reader.GetInt32(reader.GetOrdinal("OrganizationID")),
                    OrganizationName = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "OrganizationName"),
                    PhoneNum = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "OrganizationPhoneNum"),
                    Email = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "OrganizationEmail")
                }
            };

            if (!reader.IsDBNull(reader.GetOrdinal("CompanyID")))
            {
                tenancy.Company = new Company
                {
                    CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                    CompanyName = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "CompanyName"),
                    PhoneNum = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "CompanyPhoneNum"),
                    Email = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "CompanyEmail")
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
                FirstName = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "TenantFirstName"),
                LastName = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "TenantLastName"),
                PhoneNum = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "TenantPhoneNum"),
                Email = SqlDataReaderHelper.GetValueOrDefault<string>(reader, "TenantEmail")
            };
        }
    }
}
