using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {

        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
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

        public List<Walk> GetAllWalks()
        {
            //setting the connection to Sql, aka the "communication tunnel"
            // "Connection" prompt comes from the BaseRepository which is the common code being applied through the application
            using (SqlConnection conn = Connection)
            {
                //opeining the connection (or communication tunnel) to gain accessibility
                conn.Open();

                //sending the command to send to SQL (sending through the communication tunnel, like a train)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //This is the content of the command (the cargo), better check your SQL query to fill this out
                    // FROM Owner = selecting the content from owner
                    cmd.CommandText = @"
                        SELECT Id, Date, Duration, WalkerId, DogId
                        FROM Walk
                    ";

                    // if you don't execute the command, it wont do anything. Send it off! (cmd.ExecuteReader)
                    SqlDataReader reader = cmd.ExecuteReader();

                    //looping over everyone in our owners list, also creating a new list
                    List<Walk> walks = new List<Walk>();
                    while (reader.Read())
                    {
                        Walk walk = new Walk
                        {
                            // Setting values of owner properties. 
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                        };

                        //adding the information to the owner variable
                        walks.Add(walk);
                    }
                    //closing the connection. Remember that if these aren't closed, we will run out of connections.
                    reader.Close();

                    // returning owners variable
                    return walks;
                }
            }
        }
        // Method that is going to be returning Walk data by using the Walk's Id
        // (Walk is the class we set up from the Models folder)
        public Walk GetWalkById(int id)
        {
            //setting the connection to Sql, aka the "communication tunnel"
            using (SqlConnection conn = Connection)
            {
                //opeining the connection (or communication tunnel) to gain accessibility
                conn.Open();

                //sending the command to send to SQL (sending through the communication tunnel, like a train)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //This is the content of the command (the cargo), better check your SQL query to fill this out
                    // FROM Walk is selecting the content from owner, WHERE Id is selecting the walk Id
                    cmd.CommandText = @"
                        SELECT Id, Date, Duration, WalkerId, DogId
                        FROM Walk
                        WHERE Id = @id";

                    // Add the parameter definition to the command, the name of this parameter is "@id"
                    cmd.Parameters.AddWithValue("@id", id);

                    // if you don't execute the command it won't do anything ( cmd.ExecuteReader(); ) Send it off!
                    SqlDataReader reader = cmd.ExecuteReader();

                    // creating an if else statement. If it reads it continues through the "if". If it doesn't read, it continues on to the "else".
                    if (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            // Setting values of walk properties.
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                        };

                        //closing the connection. Remember that if these aren't closed, we will run out of connections.
                        reader.Close();
                        // returning walk variable
                        return walk;
                    }
                    else
                    {
                        //closing the connection. Remember that if these aren't closed, we will run out of connections.
                        reader.Close();
                        // it returns nothing
                        return null;
                    }
                }
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, Date, Duration, WalkerId, DogId
                FROM Walks
                WHERE WalkerId = @walkerId
            ";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                        };

                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }
    }
}
