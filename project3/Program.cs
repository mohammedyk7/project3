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
        p("[===========================]");
        p(" Welcome To Codeline Airline ");
        p("[===========================]");
    }

    static void p(string message)
    {
        Console.WriteLine(message);
    }

    static int ShowMainMenu()
    {
        p("\nAmin Menu:");
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
        return -1;
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

    static void CancelFlightBooking(out string passengerName)
    {
        passengerName = string.Empty;
        Console.Write("Enter Booking ID: ");
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
        passengerName = bookings[index].PassengerName ?? string.Empty;
        bookings[index].PassengerName = string.Empty;
        bookings[index].FlightCode = string.Empty;
        bookings[index].Date = default;
        bookings[index].IsConfirmed = false;
        bookings[index].BookingID = null;
        p($"Booking canceled. Passenger: {passengerName}");
    }

    static void BookFlight(string passengerName, string flightCode = "Default001")
    {
        if (ValidateFlightCode(flightCode))
        {
            string bookingID = GenerateBookingID(passengerName);

            // Assume a base price for the flight
            int basePrice = 100; // You can change this value
            int numTickets = 1;  // Assuming 1 ticket per booking

            // Calculate the fare
            int totalFare = CalculateFare(basePrice, numTickets);

            // Create the booking
            bookings[bookingCount++] = new Booking
            {
                PassengerName = passengerName,
                FlightCode = flightCode,
                BookingID = bookingID
            };

            // Display the booking confirmation and fare
            p($"Booking confirmed. ID: {bookingID}");
            p($"Total fare for {passengerName}: {totalFare} units.");
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

    static void DisplayFlightDetails(string code)
    {
        for (int i = 0; i < flightCount; i++)
        {
            if (flights[i].FlightCode == code)
            {
                var f = flights[i];
                p($"Flight {f.FlightCode}: {f.FromCity} ➜ {f.ToCity}");
                p($"Departure: {f.Departure}, Duration: {f.Duration} minutes");
                return;
            }
        }
        p("Flight not found.");
    }

    static void SearchBookingsByDestination(string toCity)
    {
        bool found = false;
        for (int i = 0; i < bookingCount; i++)
        {
            string flightCode = bookings[i].FlightCode;
            for (int j = 0; j < flightCount; j++)
            {
                if (flights[j].FlightCode == flightCode && flights[j].ToCity == toCity)
                {
                    p($"Passenger: {bookings[i].PassengerName} | Flight: {flightCode} ➜ {toCity}");
                    found = true;
                }
            }
        }
        if (!found) p("No bookings found for that destination.");
    }

    static void UpdateFlightDeparture(ref DateTime departure) //REF TO KEEP IT UPDATES 
    {
        Console.Write("Enter flight code to update departure: ");
        string? code = Console.ReadLine();
        for (int i = 0; i < flightCount; i++)
        {
            if (flights[i].FlightCode == code)
            {
                Console.Write("Enter new departure time (yyyy-MM-dd HH:mm): ");
                string? input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime newTime))
                {
                    departure = newTime;
                    flights[i].Departure = newTime;
                    p($"Departure time updated for flight {code} to {newTime}");
                }
                else
                {
                    p("Invalid date format.");
                }
                return;
            }
        }
        p("Flight not found.");
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
        int total = basePrice * numTickets;
        return total - discount;
    }

    static void StartSystem()
    {
        DisplayWelcomeMessage();
        AddFlight("Default001", "Muscat", "Dubai", DateTime.Now.AddHours(2), 60);
        while (true)
        {
            int choice = ShowMainMenu();
            if (choice == -1) continue;
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
