using Microsoft.Data.SqlClient;

namespace EksamensProjekt.Utilities.DataAccess
{
    public static class SqlDataReaderHelper
    {
        // Helper method to handle nullable types in SqlDataReader and return default value if null
        public static T GetValueOrDefault<T>(SqlDataReader reader, string columnName)
        {
            int columnOrdinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(columnOrdinal))
            {
                // Handle reference types (e.g., string) and nullable value types differently
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)string.Empty; // Return empty string if the type is string
                }

                return default; // Return null for nullable structs or default for other types
            }
            else
            {
                return reader.GetFieldValue<T>(columnOrdinal);
            }
        }
    }
}
