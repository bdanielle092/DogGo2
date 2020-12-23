using DogGo2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Repositories
{
    public class WalkStatusRepository : IWalkStatusRepository 
    {
        private readonly IConfiguration _config;

        public WalkStatusRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<WalkStatus> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, description FROM WalkStatus";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<WalkStatus> walkStatuses = new List<WalkStatus>();

                   while(reader.Read())
                    {
                        WalkStatus walkStatus = new WalkStatus
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        };
                        walkStatuses.Add(walkStatus);
                    }
                    reader.Close();
                    return walkStatuses;
                }
            }
        }

        public WalkStatus GetWalkStatusById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Description FROM WalkStatus WHERE Id = @id";
                    cmd.Parameters.AddWithValue(@"id", id);
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())
                    {
                        WalkStatus walkStatus = new WalkStatus
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        };
                        reader.Close();
                        return walkStatus;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

    }
}
