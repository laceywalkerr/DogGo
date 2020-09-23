
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

// Repository is like our API

namespace DogGo.Repositories
{
    //to generate IOwnerRepository, you -can- hover over OwnerRepository, go to the screwdriver tool on the left, and select extract interface (unclick connection so it's only selecting "getall")
    //public class calling from the OwnerRepository : IOwnerRepository
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. 
        // This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }
        // SqlcConnection sets up the connection to SQL, like an address to the database
        public SqlConnection Connection
        {
            get
            {
                //this returns the default connection to SQL
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        //method that calls and lists of all the Owners
        public List<Owner> GetAllOwners()
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
                    //This is the content of the command (the caro=go), better check your SQL query to fill this out
                    // FROM Owner = selecting the content from owner
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, NeighborhoodId, Phone
                        FROM Owner
                    ";

                    // if you don't execute the command, it wont do anything. Send it off! (cmd.ExecuteReader)
                    SqlDataReader reader = cmd.ExecuteReader();

                    //looping over everyone in our owners list, also creating a new list
                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            // Setting values of owner properties. 
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone"))
                        };

                        //adding the information to the owner variable
                        owners.Add(owner);
                    }
                    //closing the connection. Remember that if these aren't closed, we will run out of connections.
                    reader.Close();

                    // returning owners variable
                    return owners;
                }
            }
        }

        // Method that is going to be returning Owner data by using the Owner's Id
        // (Owner is the class we set up from the Models folder)
        public Owner GetOwnerById(int id)
        {
            //setting the connection to Sql, aka the "communication tunnel"
            using (SqlConnection conn = Connection)
            {
                //opeining the connection (or communication tunnel) to gain accessibility
                conn.Open();

                //sending the command to send to SQL (sending through the communication tunnel, like a train)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //This is the content of the command (the caro=go), better check your SQL query to fill this out
                    // FROM Owner is selecting the content from owner, WHERE Id is selecting the owner Id
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, NeighborhoodId, Phone
                        FROM Owner
                        WHERE Id = @id
                    ";
                    // PROTIP: Check the ID number here, set "@id" to a 1. See what it returns and that it's correct before continueing.

                    // Add the parameter definition to the command, the name of this parameter is "@id" 
                    cmd.Parameters.AddWithValue("@id", id);

                    // if you don't execute the command it won't do anything ( cmd.ExecuteReader(); ) Send it off!
                    SqlDataReader reader = cmd.ExecuteReader();

                    // creating an if else statement. If it reads it continues through the "if". If it doesn't read, it continues on to the "else".
                    if (reader.Read())
                    {
                        //owner is the name of the variable, new Owner is the new type of variable/class
                        Owner owner = new Owner
                        {
                            //setting the values of the owner properties
                            // VOCAB CHECK: Ordinal--- Being of a specified position in a numbered series. Remember, counting starts at zero.
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone"))
                        };

                        //closing the connection. Remember that if these aren't closed, we will run out of connections.
                        reader.Close();
                        // returning owner variable
                        return owner;
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



        // Method that is going to be returning Owner data by using the Owner's Email to call it
        public Owner GetOwnerByEmail(string email)
        {
            //setting the connection to Sql, aka the "communication tunnel"
            using (SqlConnection conn = Connection)
            {
                //opeining the connection (or communication tunnel) to gain accessibility
                conn.Open();

                //sending the command to send to SQL (sending through the communication tunnel, like a train)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //This is the content of the command (the carogo). PROTIP: check your SQL query to fill this out
                    // FROM Owner is selecting the content from owner, WHERE Email is selecting the owner's email
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, Phone, NeighborhoodId
                        FROM Owner
                        WHERE Email = @email";
                    // PROTIP: Check the email here. See what it returns and that it's correct before continueing.

                    // Add the parameter definition to the command, the name of this parameter is "@email" 
                    cmd.Parameters.AddWithValue("@email", email);

                    // if you don't execute the command it won't do anything ( cmd.ExecuteReader(); ) Send it off!
                    SqlDataReader reader = cmd.ExecuteReader();

                    // creating an if else statement. If it reads it continues through the "if". If it doesn't read, it continues on to the "else".
                    if (reader.Read())
                    {
                        //owner is the name of the variable, new Owner is the new type of variable/class
                        Owner owner = new Owner()
                        {
                            //setting the values of the owner properties
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        //closing the connection. Remember that if these aren't closed, we will run out of connections.
                        reader.Close();
                        // returning owner variable
                        return owner;
                    }
                    //closing the connection. Remember that if these aren't closed, we will run out of connections.
                    reader.Close();
                    // it returns nothing
                    return null;
                }
            }
        }

        // Method that is going to be adding/Creating "Owner".
        // Owner is the type of class, owner is the name of variable
        public void AddOwner(Owner owner)
        {
            //setting the connection to Sql, aka the "communication tunnel"
            using (SqlConnection conn = Connection)
            {
                //opeining the connection (or communication tunnel) to gain accessibility
                conn.Open();

                //sending the command to send to SQL (sending through the communication tunnel, like a train)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //This is the content of the command (the carogo). PROTIP: check your SQL query to fill this out
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    // ExecuteScalar ------ Executes the query, and returns the first column of the first row in the result set returned by the query. 
                    // Additional columns or rows are ignored.
                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }

        // Method that is going to be Updating "Owner".
        public void UpdateOwner(Owner owner)
        {
            //setting the connection to Sql, aka the "communication tunnel"
            using (SqlConnection conn = Connection)
            {
                //opeining the connection (or communication tunnel) to gain accessibility
                conn.Open();

                //sending the command to send to SQL (sending through the communication tunnel, like a train)
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //This is the content of the command (the cargo). PROTIP: check your SQL query to fill this out
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    // PROTIP: Check the id here. Set it to 1 and see what it returns and that it's correct before continueing.

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", owner.Id);

                    // ExecuteNonQuery used for executing queries that does not return any data, as well as executing the command and returns the number of rows affected.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Method that is going to be Deleting "Owner".
        public void DeleteOwner(int ownerId)
        {
            // connectionString links us to SQL, like an address to the database
            using (SqlConnection conn = Connection)
            {
                // Opening the communication tunnel to gain accessibility
                conn.Open();

                // sending the command through the communication tunnel
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // this is the content of the command (the cargo) 
                    // Deleting from the Owner, using the Id to call the specific owner
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    //ExecuteNonQuery used for executing queries that does not return any data. 
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
