﻿using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
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
        public List<Dog> GetDogsByOwnerId(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId 
                        FROM Dog
                        WHERE OwnerId = @ownerId
                    ";

                    cmd.Parameters.AddWithValue("@ownerId", ownerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        // Check if optional columns are null
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }

        public List<Dog> GetDogsByWalkId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                        d.Name,
                        w.DogId
                        FROM Dog d
                        LEFT JOIN Walks w on w.DogId = w.id;

            ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),

                        };



                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }
        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId
                        FROM Dog
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                           /* Notes = reader.GetString(reader.GetOrdinal("Notes")),*/
                            
                        };
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId 
                        FROM Dog
                        WHERE Id = @id";



                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed"))
                           /* Notes = reader.GetString(reader.GetOrdinal("Notes")),*/

                        };

                        reader.Close();
                        return dog;
                    }

                    reader.Close();
                    return null;
                }
            }
        }
/*        There are more columns in the INSERT statement than values specified in the VALUES clause.
            The number of values in the VALUES clause must match the number of columns specified 
            in the INSERT statement.*/
        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog (Name, OwnerId, Breed, Notes, ImageUrl)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @ownerid, @breed, @notes, @imageurl);
                ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerid", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    /*cmd.Parameters.AddWithValue("@notes", dog.Notes);*/


                    if (dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                    }

                    
                    if (dog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);
                    }

                    dog.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                Name = @name, 
                                OwnerId = @ownerid, 
                                Breed = @breed,
                                Notes = @notes,
                                ImageUrl = @imageurl
                                

                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerid", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    /*cmd.Parameters.AddWithValue("@notes", dog.Notes);*/
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

