using Microsoft.Data.SqlClient;

namespace EksamensProjekt.Utilities.DataAccess
{
    public static class SqlDataReaderHelper
    {
        /// <summary>
        /// Gets the value of a value type column from SqlDataReader.
        /// If the column value is DBNull, returns the default value for the type.
        /// Handles both nullable and non-nullable value types.
        /// </summary>
        /// <typeparam name="T">The type of the value (e.g., int, decimal, DateTime).</typeparam>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name in the database.</param>
        /// <returns>The value of the column if it is not null; otherwise, returns the default value for the type.</returns>
        public static T GetValueOrDefault<T>(SqlDataReader reader, string columnName)
        {
            int columnOrdinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(columnOrdinal))
            {
                // Return null for both reference and nullable value types
                return default;
            }
            else
            {
                return reader.GetFieldValue<T>(columnOrdinal);
            }
        }

        /// <summary>
        /// Gets the value of a reference type column from SqlDataReader.
        /// Returns a default value if the column value is DBNull (e.g., empty string for strings).
        /// </summary>
        /// <typeparam name="T">The type of the reference value (e.g., string).</typeparam>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name in the database.</param>
        /// <returns>The value of the column if it is not null; otherwise, returns the default value for the type.</returns>
        public static T GetReferenceValueOrDefault<T>(SqlDataReader reader, string columnName) where T : class
        {
            int columnOrdinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(columnOrdinal) ? default : reader.GetFieldValue<T>(columnOrdinal);
        }

        /// <summary>
        /// Gets the value of a column representing an enum type from SqlDataReader.
        /// Parses the string representation of the enum from the database.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <param name="columnName">The column name in the database.</param>
        /// <returns>The parsed enum value if successful; otherwise, throws an exception.</returns>
        public static T GetEnumValue<T>(SqlDataReader reader, string columnName) where T : Enum
        {
            int columnOrdinal = reader.GetOrdinal(columnName);
            string enumString = reader.GetString(columnOrdinal);
            return (T)Enum.Parse(typeof(T), enumString);
        }
    }
}
