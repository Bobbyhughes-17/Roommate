using Microsoft.Data.SqlClient;
using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;


namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {

        public RoommateRepository(string connectionString) : base(connectionString) { }


        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        Roommate roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("Name")),
                            Room = null
                        };

                        roommates.Add(roommate);
                    }

                    reader.Close();
                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT r.Id, r.FirstName, r.LastName, r.RentPortion, r.MoveInDate, rm.Name AS RoomName, rm.Id AS RoomId, rm.MaxOccupancy AS RoomMaxOccupancy FROM Roommate r LEFT JOIN Room rm ON r.RoomId = rm.Id WHERE r.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            RoomName = reader.IsDBNull(reader.GetOrdinal("RoomName")) ? null : reader.GetString(reader.GetOrdinal("RoomName")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("RoomName"))
                              
                            }


                        };

                    }


                    reader.Close();
                    return roommate;
                }

            }

        }
        public List<Roommate> GetRoommatesByRoomId(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT r.Id, r.FirstName, r.LastName, r.RentPortion, r.MoveInDate, rm.Name AS RoomName, rm.Id AS RoomId, rm.MaxOccupancy AS RoomMaxOccupancy
                        FROM Roommate r
                        LEFT JOIN Room rm ON r.RoomId = rm.Id
                        WHERE r.RoomId = @roomId";

                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        Roommate roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            RoomName = reader.IsDBNull(reader.GetOrdinal("RoomName")) ? null : reader.GetString(reader.GetOrdinal("RoomName")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("RoomName")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("RoomMaxOccupancy"))
                            }
                        };

                        roommates.Add(roommate);
                    }

                    reader.Close();
                    return roommates;
                }
            }
        }
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate, RoomId)
                        OUTPUT INSERTED.Id
                        VALUES (@firstName, @lastName, @rentPortion, @moveInDate, @roomId);";

                        cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                        cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                        cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                        cmd.Parameters.AddWithValue("@roomId", (object)roommate.Room?.Id ?? DBNull.Value);

                    int id = (int)cmd.ExecuteScalar();
                    roommate.Id = id;
                    
                }
            }
        }

        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Roommate
                        SET FirstName = @firstName,
                        LastName = @lastName,
                        RentPortion = @rentPortion,
                        MoveInDate = @moveInDate,
                        RoomId = @roomId
                     WHERE Id = @id
                   ";

                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@roomId", (object)roommate.Room?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", roommate.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
