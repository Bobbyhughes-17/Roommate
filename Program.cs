using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;

namespace Roommates
{
    class Program
    {
       

        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=True;"; 

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                            Console.Write("Press any key to continue");
                            Console.ReadKey();
                        break;

                        case ("Search for room"):
                            Console.Write("Room Id: ");
                        string input = Console.ReadLine();
                        int id;
                        if (int.TryParse(input, out id))
                        {
                            Room room = roomRepo.GetById(id);

                            if (room != null)
                            {
                                Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                            }
                            else
                            {
                                Console.WriteLine("Room not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }

                            Console.Write("Press any key to continue");
                            Console.ReadKey();
                            break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case "View unassigned chores":
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        Console.WriteLine("Unassigned Chores");
                        foreach (Chore chore in unassignedChores)
                        {
                            Console.WriteLine($"Chore ID: {chore.Id}");
                            Console.WriteLine($"Chore Name: {chore.Name}");
                            Console.WriteLine();
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        string roommateInput = Console.ReadLine();
                        int roommateId;
                        if (int.TryParse(roommateInput, out roommateId))
                        {
                            Roommate roommate = roommateRepo.GetById(roommateId);

                            if (roommate != null)
                            {
                                Console.WriteLine($"Roommate ID: {roommate.Id}");
                                Console.WriteLine($"First Name: {roommate.FirstName}");
                                Console.WriteLine($"Last Name: {roommate.LastName}");
                                Console.WriteLine($"Rent Portion: {roommate.RentPortion}");
                                Console.WriteLine($"Moved In Date: {roommate.MoveInDate}");
                                Console.WriteLine($"Room Name: {roommate.RoomName ?? "Unassigned"}"); // Handle null value
                            }
                            else
                            {
                                Console.WriteLine("Roommate not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case "Add a roommate":
                        Console.Write("First Name: ");
                        string firstName = Console.ReadLine();

                        Console.Write("Last Name: ");
                        string lastName = Console.ReadLine();

                        Console.Write("Rent Portion: ");
                        int rentPortion = int.Parse(Console.ReadLine());

                        Console.Write("Move-in Date (YYYY-MM-DD): ");
                        DateTime moveInDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Room ID: ");
                        int roomId = int.Parse(Console.ReadLine());

                        Roommate newRoommate = new Roommate()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            RentPortion = rentPortion,
                            MoveInDate = moveInDate,
                            Room = roomRepo.GetById(roomId)
                        };

                        roommateRepo.Insert(newRoommate);

                        Console.WriteLine($"Roommate {newRoommate.FirstName} {newRoommate.LastName} has been added and assigned an ID of {newRoommate.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case "Update roommate":
                        Console.Write("Roommate ID: ");
                        int updateId = int.Parse(Console.ReadLine());

                        Roommate existingRoommate = roommateRepo.GetById(updateId);

                        if (existingRoommate != null)
                        {
                            Console.WriteLine("Enter updated information:");

                            Console.Write("First Name: ");
                            string updatedFirstName = Console.ReadLine();

                            Console.Write("Last Name: ");
                            string updatedLastName = Console.ReadLine();

                            Console.Write("Rent Portion: ");
                            int updatedRentPortion = int.Parse(Console.ReadLine());

                            Console.Write("Move-in Date (YYYY-MM-DD): ");
                            DateTime updatedMoveInDate = DateTime.Parse(Console.ReadLine());

                            Console.Write("Room ID: ");
                            int updatedRoomId = int.Parse(Console.ReadLine());

                            existingRoommate.FirstName = updatedFirstName;
                            existingRoommate.LastName = updatedLastName;
                            existingRoommate.RentPortion = updatedRentPortion;
                            existingRoommate.MoveInDate = updatedMoveInDate;
                            existingRoommate.Room = roomRepo.GetById(updatedRoomId);

                            roommateRepo.Update(existingRoommate);

                            Console.WriteLine($"Roommate {existingRoommate.FirstName} {existingRoommate.LastName} has been updated");
                        }
                        else
                        {
                            Console.WriteLine("Roommate not found");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case "Delete roommate":
                        Console.Write("Roommate ID: ");
                        int deleteId = int.Parse(Console.ReadLine());

                        Roommate roommateToDelete = roommateRepo.GetById(deleteId);

                        if (roommateToDelete != null)
                        {
                            roommateRepo.Delete(deleteId);
                            Console.WriteLine($"Roommate {roommateToDelete.FirstName} {roommateToDelete.LastName} has been deleted");
                        }
                        else
                        {
                            Console.WriteLine("Roommate not found");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                            break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "View unassigned chores",
                "Search for roommate",
                "Add a roommate",
                "Update roommate",
                "Delete roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}