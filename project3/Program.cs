using System;

class Program
{
    static Flight[] flights = new Flight[100];
    static Booking[] bookings = new Booking[100];
    static int flightCount = 0;
    static int bookingCount = 0;

    static void Main(string[] args)
    {
        StartSystem();
    }

    static void DisplayWelcomeMessage()
    {
        p("===========================");
        p("  Welcome To Codeline Airline");
        p("===========================");
    }

    // Print method to replace Console.WriteLine
    static void p(string message)
    {
        Console.WriteLine(message);
    }

    static int ShowMainMenu()
    {
        p("\nMain Menu:");
        p("1. Book Flight");
        p("2. Cancel Booking");
        p("3. View All Flights");
        p("4. Exit");
        Console.Write("Enter your choice: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int choice))
        {
            return choice;
        }
        p("Invalid input. Please enter a number.");
        return 0; // Return 0 for invalid input
    }

    static void ExitApplication()
    {
        p("Thank you for using FlyHigh Airline!");
    }

    static void AddFlight(string flightCode, string fromCity, string toCity, DateTime departureTime, int duration)
    {
        flights[flightCount++] = new Flight { FlightCode = flightCode, FromCity = fromCity, ToCity = toCity, Departure = departureTime, Duration = duration };
    }

    static void DisplayAllFlights()
    {
        for (int i = 0; i < flightCount; i++)
        {
            var flight = flights[i];
            p($"{flight.FlightCode}: {flight.FromCity} to {flight.ToCity} at {flight.Departure}, Duration: {flight.Duration} mins");
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

    static void CancelFlightBooking()
    {
        p("Enter Booking ID: ");
        string? bookingId = Console.ReadLine();

        int index = -1;
        for (int i = 0; i < bookingCount; i++)
        {
            if (bookings[i] != null && bookings[i].BookingID == bookingId)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            p("Booking not found.");
            return;
        }

        bookings[index].PassengerName = string.Empty;//to clear the passenger name 
        bookings[index].FlightNumber = string.Empty; //to clear the flight number 
        bookings[index].Date = default;
        bookings[index].IsConfirmed = false;//bool
        bookings[index].BookingID = null;//meaning no booking id exists anymore 

        p("Booking has been successfully canceled.");
    }

    static void BookFlight(string passengerName, string flightCode = "Default001")
    {
        if (ValidateFlightCode(flightCode))
        {
            string bookingID = GenerateBookingID(passengerName);
            bookings[bookingCount++] = new Booking { PassengerName = passengerName, FlightCode = flightCode, BookingID = bookingID };
            p($"Booking confirmed. ID: {bookingID}");
        }
        else
        {
            p("Invalid flight code.");
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
                    p("Enter your name: ");
                    string? name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        p("Name cannot be empty. Please try again.");
                        break;
                    }
                    p("Enter flight code (or press Enter for default): ");
                    string? code = Console.ReadLine();
                    if (string.IsNullOrEmpty(code))
                        BookFlight(name);
                    else
                        BookFlight(name, code);
                    break;
                case 2:
                    if (ConfirmAction("cancel your booking"))
                        CancelFlightBooking();
                    break;
                case 3:
                    DisplayAllFlights();
                    break;
                case 4:
                    ExitApplication();
                    return;
                default:
                    p("Invalid choice.");
                    break;
            }
        }
    }

    static bool ConfirmAction(string action)
    {
        Console.Write($"Are you sure you want to {action}? (y/n): ");
        return Console.ReadLine()?.ToLower() == "y";
    }

    class Flight
    {
        public string? FlightCode;
        public string? FromCity;
        public string? ToCity;
        public DateTime Departure;
        public int Duration;
    }

    class Booking
    {
        public string? PassengerName;
        public string? FlightCode;
        public string? BookingID;
        public bool IsConfirmed;
        public string? FlightNumber;
        public DateTime Date;
    }
}
