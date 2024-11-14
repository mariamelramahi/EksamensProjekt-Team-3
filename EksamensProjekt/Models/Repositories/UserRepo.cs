using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Models.Repositories
{
    public class UserRepo : IRepo<User>
    {
        private readonly string _connectionString;

        public UserRepo(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Delete(int entity)
        {
            throw new NotImplementedException();
        }

        // Method to get user by usernameInput
        public User GetByUsername(string usernameInput)
        {
            User user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("usp_GetUserByUsername", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Username", usernameInput);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            // Populate other properties if needed
                        };
                    }
                }
            }

            return user;
        }

        void IRepo<User>.Create(User entity)
        {
            throw new NotImplementedException();
        }

        User IRepo<User>.GetByID(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<User> IRepo<User>.ReadAll()
        {
            throw new NotImplementedException();
        }

        void IRepo<User>.Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
