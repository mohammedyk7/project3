using System;

class Program
{
    static Flight[] flights = new Flight[100];
    static Booking[] bookings = new Booking[100];
    static int flightCount = 0;
    static int bookingCount = 0;
    static List<Flight> flightsList2 = new List<Flight>();
    static List<Booking> bookings2 = new List<Booking>();


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
        try
        {
            p("\nMain Menu:");
            p("1. Book Flight");
            p("2. Cancel Booking");
            p("3. View All Flights");
            p("4. Exit");
            p("Enter your choice: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                return choice;
            }
            p("Invalid input. Please enter a number.");
        }
        catch (Exception ex)
        {
            p($"An error occurred: {ex.Message}");
        }
        return -1; // Return -1 for invalid input or errors
    }

    static void ExitApplication()
    {
        p("Thank you for using FlyHigh Airline!");
    }

    static void AddFlight(string flightCode, string fromCity, string toCity, DateTime departureTime, int duration)
    {
        flights[flightCount++] = new Flight
        { FlightCode = flightCode, FromCity = fromCity, ToCity = toCity, Departure = departureTime, Duration = duration };
    }

  
    static void AddFlightToList2(string flightCode, string fromCity, string toCity, DateTime departureTime, int duration)
    {
        flightsList2.Add(new Flight
        {
            FlightCode = flightCode,
            FromCity = fromCity,
            ToCity = toCity,
            Departure = departureTime,
            Duration = duration
        });
    }

    static void DisplayAllFlights()
    {
        for (int i = 0; i < flightCount; i++)
        {
            var flight = flights[i]; //So i don't need to update the type in all places where the variable is used.
            p($"{flight.FlightCode}: {flight.FromCity} to {flight.ToCity} at {flight.Departure}, Duration: {flight.Duration} mins");
        }
    }//array

    static void DisplayAllFlights2()//list
    {
        foreach (var flight in flightsList2)
        {
            p($"{flight.FlightCode}: {flight.FromCity} to {flight.ToCity} at {flight.Departure}, Duration: {flight.Duration} mins");
        }
    }


    static bool FindFlightByCode(string code)//array
    {
        for (int i = 0; i < flightCount; i++)
        {
            if (flights[i].FlightCode == code)
                return true;
        }
        return false;
    }
    static bool FindFlightByCode2(string code) //list 
    {
        foreach (var flight in flightsList2)
        {
            if (flight.FlightCode == code)//condition
                return true;
        }
        return false;
    }


    static void UpdateFlightDeparture(ref DateTime departure)
    {
        try
        {
            p("Enter flight code to update departure: ");
            string? code = Console.ReadLine();
            for (int i = 0; i < flightCount; i++)
            {
                if (flights[i].FlightCode == code)
                {
                    p("Enter new departure time (yyyy-MM-dd HH:mm): ");
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
        catch (Exception ex)
        {
            p($"An error occurred while updating the flight departure: {ex.Message}");
        }
    }//array
    //ref because date time already has a value the change will be updated by the reference
    static void UpdateFlightDeparture2(ref DateTime departure )//list
    {
        try
        {
            p("Enter flight code to update departure: ");
            string? code = Console.ReadLine();

            foreach (var flight in flights)
            {
                if (flight.FlightCode == code)
                {
                    p("Enter new departure time (yyyy-MM-dd HH:mm): ");
                    string? input = Console.ReadLine();
                    if (DateTime.TryParse(input, out DateTime newTime))
                    {
                        flight.Departure = newTime;
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
        catch (Exception ex)
        {
            p($"An error occurred while updating the flight departure: {ex.Message}");
        }
    }



    // In this case, 'out' is used to return the passenger's name after canceling the booking.
    static void CancelFlightBooking(out string passengerName)
    {
        passengerName = string.Empty;
        try
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
            passengerName = bookings[index].PassengerName ?? string.Empty;
            bookings[index].PassengerName = string.Empty;
            bookings[index].FlightCode = string.Empty;
            bookings[index].Date = default;
            bookings[index].IsConfirmed = false;
            bookings[index].BookingID = null;
            p($"Booking canceled. Passenger: {passengerName}");
        }
        catch (Exception ex)
        {
            p($"An error occurred while canceling the booking: {ex.Message}");
        }
    }//array
    static void CancelFlightBooking(List<Booking> bookings, out string passengerName)
    {
        passengerName = string.Empty;
        try
        {
            p("Enter Booking ID: ");
            string? bookingId = Console.ReadLine();

            string? bookingToRemove;

            foreach (var booking in bookings)
            {
                if (booking.BookingID == bookingId)
                {
                    bookingToRemove = booking;
                    break;
                }
            }

            if (bookingToRemove == null)
            {
                p("Booking not found.");
                return;
            }

            passengerName = bookingToRemove.PassengerName ?? string.Empty;
            bookings.Remove(bookingToRemove);
            p($"Booking canceled. Passenger: {passengerName}");
        }
        catch (Exception ex)
        {
            p($"An error occurred while canceling the booking: {ex.Message}");
        }
    }


    static void BookFlight(string passengerName, string flightCode = "Default001")
    {
        try
        {
            if (ValidateFlightCode(flightCode))
            {
                string bookingID = GenerateBookingID(passengerName);
                int basePrice = 100;
                int numTickets = 1;
                int totalFare = CalculateFare(basePrice, numTickets);

                bookings[bookingCount++] = new Booking
                {
                    PassengerName = passengerName,
                    FlightCode = flightCode,
                    BookingID = bookingID
                };

                p($"Booking confirmed. ID: {bookingID}");
                p($"Total fare for {passengerName}: {totalFare} units.");
            }
            else
            {
                p("Invalid flight code.");
            }
        }
        catch (Exception ex)
        {
            p($"An error occurred while booking the flight: {ex.Message}");
        }
    }


    static bool ValidateFlightCode(string flightCode)
    {
        // Checks if the given flight code exists in the flights array.
        // Returns true if the flight code is found, otherwise false.
        return FindFlightByCode(flightCode);
        //uses the bool return type because its purpose is to validate whether a given flight code exists in the system.
    }
    static bool ValidateFlightCode2(string flightCode, List<Flight> flightList)
    {
        foreach (var flight in flightList2)
        {
            if (flight.FlightCode == flightCode)
                return true;
        }
        return false;
    }


    static string GenerateBookingID(string passengerName)
    {
        // Generates a unique booking ID by appending the current year to the passenger's name.
        // This ensures that each booking ID is distinct for the given passenger.
        return passengerName + DateTime.Now.Year;
    }
    static string GenerateBookingID2(string passengerName)
    {
        foreach (var flight in flightList)
        {
            passengerName + DateTime.Now.Year;
            return true;
        }
        return false;
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
        // Calculate the total fare by multiplying the base price with the number of tickets
        return basePrice * numTickets;
    }

    static double CalculateFare(double basePrice, int numTickets)
    {
        return basePrice * numTickets;
    }

    static int CalculateFare(int basePrice, int numTickets, int discount)
    {
        // Calculate the total fare by multiplying the base price with the number of tickets
        int total = basePrice * numTickets;
        // Subtract the discount from the total fare and return the result
        return total - discount;
    }

    static void StartSystem()
    {
        // Display a welcome message to the user
        DisplayWelcomeMessage();

        // Adding a default flight to the system with flight code "Default001",
        // departing from Muscat to Dubai, scheduled 2 hours from now, with a duration of 60 minutes.
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
