using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString)
        {
        }
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {


                    cmd.CommandText = @"
                    SELECT c.Id, c.Name
                    FROM Chore c
                    LEFT JOIN Roommate r ON c.Id = r.RoomId
                    WHERE r.Id IS NULL
                ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> unassignedChores = new List<Chore>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));


                        Chore chore = new Chore
                        {
                            Id = id,
                            Name = name,

                        };

                        unassignedChores.Add(chore);
                    }

                    reader.Close();

                    return unassignedChores;
                }
            }
        }
    }
}
    
