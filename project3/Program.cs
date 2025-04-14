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
        p("\nMain Menu:");
        p("1. Book Flight");
        p("2. Cancel Booking");
        p("3. View All Flights");
        p("4. Exit");
        p("Enter your choice: ");
        string? input = Console.ReadLine();//null if no more available 
        if (int.TryParse(input, out int choice)) // to convert a string to an integer,checks whether the TryParse method successfully converted the input string into an integer.

            //If successful, the method will return true, 
        {
            return choice;
        }
        p("Invalid input. Please enter a number.");
        return -1; //input os invalid
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
            var flight = flights[i]; //So i don't need to update the type in all places where the variable is used.
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

    static void UpdateFlightDeparture(ref DateTime departure) //REF TO KEEP IT UPDATES 
    {
        p("Enter flight code to update departure: ");
        string? code = Console.ReadLine();
        for (int i = 0; i < flightCount; i++)
        {
            if (flights[i].FlightCode == code)
            {
                Console.Write("Enter new departure time (yyyy-MM-dd HH:mm): ");
                string? input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime newTime))//because its numiracal 
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

    static void CancelFlightBooking(out string passengerName)
    {
        passengerName = string.Empty;
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
        passengerName = bookings[index].PassengerName ?? string.Empty;
        bookings[index].PassengerName = string.Empty;// TO CLEAR THE ARRAY 
        bookings[index].FlightCode = string.Empty;//TO CLEAR THE ARRAY 
        bookings[index].Date = default;
        bookings[index].IsConfirmed = false;
        bookings[index].BookingID = null;
        p($"Booking canceled. Passenger: {passengerName}");
    }

    static void BookFlight(string passengerName, string flightCode = "Default001")
    {
        if (ValidateFlightCode(flightCode))//input validate + no retrun becuase of void
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
                PassengerName = passengerName,//this will be the name of the passenger
                FlightCode = flightCode,//this will be the flight code
                BookingID = bookingID//this will be the booking ID
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
        // Checks if the given flight code exists in the flights array.
        // Returns true if the flight code is found, otherwise false.
        return FindFlightByCode(flightCode);
    }

    static string GenerateBookingID(string passengerName)
    {
        // Generates a unique booking ID by appending the current year to the passenger's name.
        // This ensures that each booking ID is distinct for the given passenger.
        return passengerName + DateTime.Now.Year;
    }

    static void DisplayFlightDetails(string code)
    {
        // Searches for a flight by its flight code and displays its details.
        // If the flight is found, it prints the flight's code, origin, destination, departure time, and duration.
        // If the flight is not found, it notifies the user.
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
            string? flightCode = bookings[i]?.FlightCode; // Use nullable type to handle potential null values
            if (flightCode == null) continue; // Skip if flightCode is null

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
        // Display a welcome message to the user
        DisplayWelcomeMessage();

        // Add a default flight to the system
        AddFlight("Default001", "Muscat", "Dubai", DateTime.Now.AddHours(2), 60);

        // Main loop to keep the system running
        while (true)
        {
            // Show the main menu and get the user's choice
            int choice = ShowMainMenu();
            if (choice == -1) continue; // If input is invalid, show the menu again

            // Handle the user's choice
            switch (choice)
            {
                case 1:
                    // Booking a flight
                    p("Enter your name: ");
                    string? name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        // Ensure the name is not empty
                        p("Name cannot be empty. Please try again.");
                        break;
                    }
                    p("Enter flight code (or press Enter for default): ");
                    string? code = Console.ReadLine();
                    if (string.IsNullOrEmpty(code))
                        BookFlight(name); // Book with the default flight code
                    else
                        BookFlight(name, code); // Book with the provided flight code
                    break;

                case 2:
                    // Canceling a booking
                    if (ConfirmAction("cancel your booking"))
                        CancelFlightBooking(out _); // Cancel the booking and ignore the passenger name
                    break;

                case 3:
                    // Display all available flights
                    DisplayAllFlights();
                    break;

                case 4:
                    // Exit the application
                    ExitApplication();
                    return;

                default:
                    // Handle invalid menu choices
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
