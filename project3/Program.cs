using System;

class Program
{
    static Flight[] flights = new Flight[100];
    static Booking[] bookings = new Booking[100];
    static int flightCount = 0; //++ or  --
    static int bookingCount = 0; //++ or  --

    static void Main(string[] args)
    {
        StartSystem();
    }

    static void DisplayWelcomeMessage()
    {
                                                                   p("[===========================]");
                                                                   p(" Welcome To Codeline Airline ");
                                                                   p("[===========================]");
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
        p("Enter your choice: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int choice))
        {
            return choice;
        }
        p("Invalid input. Please enter a number.");
        return  -1; // Return -1 for invalid input
    }

    static void ExitApplication()
    {
        p("Thank you for using FlyHigh Airline!");
    }

    static void AddFlight(string flightCode, string fromCity, string toCity, DateTime departureTime, int duration) // no return 
    {
        flights[flightCount++] = new Flight { FlightCode = flightCode, FromCity = fromCity, ToCity = toCity, Departure = departureTime, Duration = duration };
        //flights[flightCount++]  flight[increment] //[flightCount] keeping track of how many flights have been added.
    }

    static void DisplayAllFlights() //longer solution ..
    {
        //p("Available Flights:");
        //if (flightCount == 0)
        //{
        //    p("No flights available.");
        //    return;
        //}
        //for (int i = 0; i < flightCount; i++)
        //{
        //    var flight = flights[i]; //i used var so it can be integer or string
        //    p($"{flight.FlightCode}: {flight.FromCity} to {flight.ToCity} at {flight.Departure}, Duration: {flight.Duration} mins");
        //}
    
        for (int i = 0; i < flightCount; i++) //shorter one ..
        {
            var flight = flights[i]; //I used var so it can be integer or string
            p($"{flight.FlightCode}: {flight.FromCity} to {flight.ToCity} at {flight.Departure}, Duration: {flight.Duration} mins");
        }
    }

    static bool FindFlightByCode(string code) //return true or falso "bool"
        //string code consists of numbers and letters 
    {
        for (int i = 0; i < flightCount; i++)//search starts from 0 to flightcount 0 1 2 = flight count = 3
        {
            if (flights[i].FlightCode == code) // comparison check 
                //The dot operator is used to access the FlightCode property of that particular Flight object at index i.
                return true;
        }
        return false;
    }

    static void CancelFlightBooking(out string passengerName)
    {
        passengerName = string.Empty; // Ensure out parameter is always assigned

        Console.Write("Enter Booking ID: ");
        string? bookingId = Console.ReadLine();

        // Find the booking by ID
        int index = -1; //  i didnt add "if (!int.TryParse(Console.ReadLine()," because the entry could be alphanumetric like da32
        for (int i = 0; i < bookingCount; i++)
        {
            if (bookings[i] != null && bookings[i].BookingID == bookingId) //null = "An empty chair, waiting for someone to sit down."
            //bookings[i] != null check added
            //bookings[i].BookingID == bookingId check added
            {
                index = i; //when i =1 index =1 
                break;
            }
            {
                index = i; //when i =1 index =1 
                break;
            }
        }

        if (index == -1)
        {
            p("Booking not found.");
            return;
        }

        // Retrieve passenger name from the found booking
        passengerName = bookings[index].PassengerName ?? string.Empty;

        // Cancel the reservation by resetting the booking details
        bookings[index].PassengerName = string.Empty;
        bookings[index].FlightCode = string.Empty;
        bookings[index].Date = default;
        bookings[index].IsConfirmed = false;
        bookings[index].BookingID = null;

        p($"Booking canceled. Passenger: {passengerName}");
    }

    static void BookFlight(string passengerName, string flightCode = "Default001") //no return because "void"
    {
        if (ValidateFlightCode(flightCode)) //calling another method !
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
            if (choice == -1) continue; // Skip invalid input

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
                        CancelFlightBooking(out _);
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
        public DateTime Date;
    }
}
