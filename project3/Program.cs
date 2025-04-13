﻿using System;

class Program
{
    static Flight[] flights = new Flight[100];
    static Booking[] bookings = new Booking[100];
    static int flightCount = 0;//defined flightCount
    static int bookingCount = 0;//defined bookingCount

    static void Main(string[] args)
    {
        StartSystem();
    }

    static void DisplayWelcomeMessage()
    {
        Console.WriteLine("===========================");
        Console.WriteLine("  Welcome To Codeline Airline");
        Console.WriteLine("===========================");
    }

    //// Print method to replace Console.WriteLine
    //static void p(string message)
    //{
    //    Console.WriteLine(message);
    //}

    static int ShowMainMenu()
    {
        Console.WriteLine("\nMain Menu:");
        Console.WriteLine("1. Book Flight");
        Console.WriteLine("2. Cancel Booking");
        Console.WriteLine("3. View All Flights");
        Console.WriteLine("4. Exit");
        Console.Write("Enter your choice: ");
        int choice = int.Parse(Console.ReadLine()); //not sure if i ahould awitch it to string 
        return choice;
    }

    static void ExitApplication()
    {
        Console.WriteLine("Thank you for using FlyHigh Airline!");
    }

    static void AddFlight(string flightCode, string fromCity, string toCity, DateTime departureTime, int duration) //parameter added 
    {
        flights[flightCount++] = new Flight { FlightCode = flightCode, FromCity = fromCity, ToCity = toCity, Departure = departureTime, Duration = duration };
    }

    static void DisplayAllFlights()
    {
        for (int i = 0; i < flightCount; i++)
        {
            var flight = flights[i];
            Console.WriteLine($"{flight.FlightCode}: {flight.FromCity} to {flight.ToCity} at {flight.Departure}, Duration: {flight.Duration} mins");
        }
    }

    static bool FindFlightByCode(string code)
    {
        for (int i = 0; i < flightCount; i++)
        {
            if (flights[i].FlightCode == code)
                return true;
        }
        return false;
    }

    static void UpdateFlightDeparture(ref DateTime departure) //we used ref because we want to change the value of departure in the class 
    {
        Console.Write("Enter new departure time (yyyy-mm-dd hh:mm): ");
        departure = DateTime.Parse(Console.ReadLine());
    }

    static void CancelFlightBooking(out string passengerName)
    {
        Console.Write("Enter booking ID: ");
        string? id = Console.ReadLine();//i added ? to remove the error 
        for (int i = 0; i < bookingCount; i++)
        {
            if (bookings[i].BookingID == id)
            {
                passengerName = bookings[i].PassengerName;
                for (int j = i; j < bookingCount - 1; j++)
                {
                    bookings[j] = bookings[j + 1];
                }
                bookingCount--;
                Console.WriteLine("Booking canceled successfully.");
                return;
            }
        }
        passengerName = string.Empty;
        Console.WriteLine("Booking not found.");
    }

    static void BookFlight(string passengerName, string flightCode = "Default001")
    {
        if (ValidateFlightCode(flightCode))
        {
            string bookingID = GenerateBookingID(passengerName);
            bookings[bookingCount++] = new Booking { PassengerName = passengerName, FlightCode = flightCode, BookingID = bookingID };
            Console.WriteLine($"Booking confirmed. ID: {bookingID}");
        }
        else
        {
            Console.WriteLine("Invalid flight code.");
        }
    }

    static bool ValidateFlightCode(string flightCode)
    {
        return FindFlightByCode(flightCode);
    }

    static string GenerateBookingID(string passengerName)
    {
        return passengerName + DateTime.Now.Year;
    }

    static void DisplayFlightDetails(string code)
    {
        for (int i = 0; i < flightCount; i++)
        {
            if (flights[i].FlightCode == code)
            {
                Console.WriteLine($"{flights[i].FlightCode}: {flights[i].FromCity} -> {flights[i].ToCity} at {flights[i].Departure}");
                return;
            }
        }
        Console.WriteLine("Flight not found.");
    }

    static void SearchBookingsByDestination(string toCity)
    {
        for (int i = 0; i < bookingCount; i++)
        {
            for (int j = 0; j < flightCount; j++)
            {
                if (flights[j].FlightCode == bookings[i].FlightCode && flights[j].ToCity == toCity)
                {
                    Console.WriteLine($"Booking ID: {bookings[i].BookingID}, Passenger: {bookings[i].PassengerName}");
                }
            }
        }
    }

    static int CalculateFare(int basePrice, int numTickets)
    {
        return basePrice * numTickets;
    }

    static double CalculateFare(double basePrice, int numTickets)
    {
        return basePrice * numTickets;
    }

    static int CalculateFare(int basePrice, int numTickets, int discount)
    {
        return (basePrice * numTickets) - discount;
    }

    static bool ConfirmAction(string action)
    {
        Console.Write($"Are you sure you want to {action}? (y/n): ");
        return Console.ReadLine().ToLower() == "y";
    }

    static void StartSystem()
    {
        DisplayWelcomeMessage();
        AddFlight("Default001", "Muscat", "Dubai", DateTime.Now.AddHours(2), 60);

        while (true)
        {
            int choice = ShowMainMenu();
            switch (choice)
            {
                case 1:
                    Console.Write("Enter your name: ");
                    string? name = Console.ReadLine();
                    Console.Write("Enter flight code (or press Enter for default): ");
                    string? code = Console.ReadLine();
                    if (string.IsNullOrEmpty(code))
                        BookFlight(name);
                    else
                        BookFlight(name, code);
                    break;
                case 2:
                    if (ConfirmAction("cancel your booking"))
                        CancelFlightBooking(out _);
                    break;
                case 3:
                    DisplayAllFlights();
                    break;
                case 4:
                    ExitApplication();
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    class Flight
    {
        public string FlightCode;
        public string FromCity;
        public string ToCity;
        public DateTime Departure;
        public int Duration;
    }

    class Booking
    {
        public string PassengerName;
        public string FlightCode;
        public string BookingID;
    }
}
