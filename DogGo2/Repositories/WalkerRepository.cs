using DogGo2.Models;
using DogGo2.Repositories.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
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

        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT walker.Id, walker.[Name], Email, ImageUrl, walker.NeighborhoodId, Neighborhood.[Name] as Neighborhood
                        FROM Walker 
                        JOIN Neighborhood ON walker.NeighborhoodId = Neighborhood.Id
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            ImageUrl = ReaderUtils.GetNullableString(reader, "ImageUrl"),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Neighborhood = new Neighborhood()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                            }
                        };

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Walker.Id AS WalkerId, Walker.[Name] AS WalkerName, Email, ImageUrl, Walker.NeighborhoodId, Neighborhood.Id AS NeighborId , Neighborhood.[Name] AS Neighborhood
                        FROM Walker
                        LEFT JOIN Neighborhood ON Walker.NeighborhoodId = Neighborhood.Id
                        WHERE Walker.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
           

                    if (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            Name = reader.GetString(reader.GetOrdinal("WalkerName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            ImageUrl = ReaderUtils.GetNullableString(reader, "ImageUrl"),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                           
                            Neighborhood = new Neighborhood()
                            {
                             Id = reader.GetInt32(reader.GetOrdinal("NeighborId")),
                             Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                            }
                        };

                        reader.Close();
                        return walker;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

        public void AddWalker(Walker walker)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Walker([Name], Email, ImageUrl, NeighborhoodId)
                     OUTPUT INSERTED.ID
                     VALUES (@name, @email, @imageUrl, @neighborhoodId);
                    ";
                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@email", walker.Email);
                    cmd.Parameters.AddWithValue("@imageUrl", ReaderUtils.GetNullableParam(walker.ImageUrl));
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();
                    walker.Id = id;
                }
            }
        }

        public void UpdateWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Walker
                                       SET
                                       [Name] = @name,
                                       Email = @email,
                                       ImageUrl = @imageUrl,
                                       NeighborhoodId = @neighborhoodId
                                       WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", walker.Id);
                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@email", walker.Email);
                    cmd.Parameters.AddWithValue("@imageUrl", ReaderUtils.GetNullableParam(walker.ImageUrl));
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWalker(int walkerId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Walker
                                       WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", walkerId);

                    cmd.ExecuteNonQuery();

                }
            }
        }


    }
}