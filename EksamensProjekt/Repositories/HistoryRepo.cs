using EksamensProjekt.DataAccess;
using EksamensProjekt.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;


namespace EksamensProjekt.Repos
{
    public class HistoryRepo : IRepo<History>
    {

        private readonly string _connectionString;

        //Constructor that initialzies the connection string
        public HistoryRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(History entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public History GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<History> ReadAll()
        {
            var histories = new List<History>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("usp_ReadAllHistories", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var history = SqlDataMapper.PopulateHistoryFromReader(reader);
                            histories.Add(history);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while fetching histories: " + ex.Message);
                }
            }

            return histories;
        }


        public void Update(History entity)
        {
            throw new NotImplementedException();
        }
    }
}

