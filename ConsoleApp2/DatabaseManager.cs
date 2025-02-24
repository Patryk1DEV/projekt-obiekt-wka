using System;
using System.Data.SQLite;

namespace EmployeeManagementSystem
{
    public static class DatabaseManager
    {
        private static SQLiteConnection dbConnection = new SQLiteConnection("Data Source=employees.db;Version=3;");

        public static void InitializeDatabase()
        {
            dbConnection.Open();

            string createEmployeesTable = "CREATE TABLE IF NOT EXISTS Employees (Id INTEGER PRIMARY KEY AUTOINCREMENT, FirstName TEXT, LastName TEXT, Position TEXT, Email TEXT);";
            string createAccessCardsTable = "CREATE TABLE IF NOT EXISTS AccessCards (EmployeeId INTEGER PRIMARY KEY, CardNumber TEXT, FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE);";
            string createEventsTable = "CREATE TABLE IF NOT EXISTS Events (Id INTEGER PRIMARY KEY AUTOINCREMENT, EmployeeId INTEGER, Name TEXT, Date TEXT, FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE);";
            string createComputerStatusTable = "CREATE TABLE IF NOT EXISTS ComputerStatus (ComputerId INTEGER PRIMARY KEY AUTOINCREMENT, EmployeeId INTEGER NULL, IsOnline INTEGER, OnlineSince TEXT, FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE SET NULL);";
            string createRequestsTable = "CREATE TABLE IF NOT EXISTS Requests (RequestId INTEGER PRIMARY KEY AUTOINCREMENT, EmployeeId INTEGER, Type TEXT, Status TEXT DEFAULT 'Oczekujące', FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE);";
            string createHRMessagesTable = "CREATE TABLE IF NOT EXISTS HRMessages (MessageId INTEGER PRIMARY KEY AUTOINCREMENT, EmployeeId INTEGER, Message TEXT, Status TEXT DEFAULT 'Nieprzeczytane', Timestamp TEXT, FOREIGN KEY(EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE);";


            using (var cmd = new SQLiteCommand(createHRMessagesTable, dbConnection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createRequestsTable, dbConnection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createEmployeesTable, dbConnection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createAccessCardsTable, dbConnection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createEventsTable, dbConnection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createComputerStatusTable, dbConnection)) cmd.ExecuteNonQuery();
        }

        public static void AddEmployee()
        {
            Console.Write("Podaj imię: ");
            string firstName = Console.ReadLine() ?? "";
            Console.Write("Podaj nazwisko: ");
            string lastName = Console.ReadLine() ?? "";
            Console.Write("Podaj stanowisko: ");
            string position = Console.ReadLine() ?? "";
            Console.Write("Podaj email: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Podaj numer karty dostępu: ");
            string cardNumber = Console.ReadLine() ?? "";

            string query = "INSERT INTO Employees (FirstName, LastName, Position, Email) VALUES (@FirstName, @LastName, @Position, @Email);";
            using (var cmd = new SQLiteCommand(query, dbConnection))
            {
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Position", position);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }

            int lastEmployeeId = (int)dbConnection.LastInsertRowId;
            string cardQuery = "INSERT INTO AccessCards (EmployeeId, CardNumber) VALUES (@EmployeeId, @CardNumber);";
            using (var cmd = new SQLiteCommand(cardQuery, dbConnection))
            {
                cmd.Parameters.AddWithValue("@EmployeeId", lastEmployeeId);
                cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"Pracownik ID {lastEmployeeId} dodany!");
        }
        public static void RemoveEmployee()
        {
            Console.Write("Podaj ID pracownika do usunięcia: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                string query = "DELETE FROM Employees WHERE Id = @id";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? $"Pracownik ID {id} został usunięty." : $"Nie znaleziono pracownika o ID {id}.");
                }
            }
            else Console.WriteLine("Nieprawidłowe ID.");
        }

        public static void EditEmployee()
        {
            Console.Write("Podaj ID pracownika do edycji: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Podaj nowe imię: ");
                string firstName = Console.ReadLine() ?? "";
                Console.Write("Podaj nowe nazwisko: ");
                string lastName = Console.ReadLine() ?? "";
                Console.Write("Podaj nowe stanowisko: ");
                string position = Console.ReadLine() ?? "";
                Console.Write("Podaj nowy email: ");
                string email = Console.ReadLine() ?? "";

                string query = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Position = @Position, Email = @Email WHERE Id = @id";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Position", position);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine($"Dane pracownika ID {id} zostały zaktualizowane.");
            }
            else Console.WriteLine("Nieprawidłowe ID.");
        }
        public static void ManageCalendar()
        {
            Console.WriteLine("\n=== Zarządzanie Kalendarzem ===");
            Console.WriteLine("1. Dodaj wydarzenie");
            Console.WriteLine("2. Wyświetl wydarzenia");
            Console.Write("Wybierz opcję: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddEvent();
                    break;
                case "2":
                    ShowEvents();
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór.");
                    break;
            }
        }
        private static void AddEvent()
        {
            Console.Write("Podaj ID pracownika: ");
            if (int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.Write("Podaj nazwę wydarzenia: ");
                string eventName = Console.ReadLine() ?? "";
                Console.Write("Podaj datę (YYYY-MM-DD): ");
                string date = Console.ReadLine() ?? "";

                string query = "INSERT INTO Events (EmployeeId, Name, Date) VALUES (@EmployeeId, @Name, @Date)";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@Name", eventName);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Dodano nowe wydarzenie.");
            }
            else Console.WriteLine("Nieprawidłowe ID.");
        }


        public static void ShowEvents()
        {
            Console.WriteLine("--- Lista Wydarzeń ---");
            using (var cmd = new SQLiteCommand("SELECT * FROM Events", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Pracownik ID: {reader["EmployeeId"]}, Nazwa: {reader["Name"]}, Data: {reader["Date"]}");
                    }
                }
            }
        }

        public static void ManageComputerStatus()
        {
            Console.Write("Podaj ID pracownika, któremu chcesz przypisać komputer: ");
            if (int.TryParse(Console.ReadLine(), out int employeeId))
            {
                string query = "INSERT INTO ComputerStatus (EmployeeId, IsOnline, OnlineSince) VALUES (@EmployeeId, 0, NULL)";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine($"Komputer przypisany do pracownika ID {employeeId}.");
            }
            else Console.WriteLine("Nieprawidłowe ID.");
        }

        public static void ShowAllData()
        {
            Console.WriteLine("\n=== Pełna zawartość bazy danych ===");

            Console.WriteLine("\n--- Pracownicy ---");
            using (var cmd = new SQLiteCommand("SELECT * FROM Employees", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Imię: {reader["FirstName"]}, Nazwisko: {reader["LastName"]}, Stanowisko: {reader["Position"]}, Email: {reader["Email"]}");
                    }
                }
            }

            Console.WriteLine("\n--- Karty dostępu ---");
            using (var cmd = new SQLiteCommand("SELECT * FROM AccessCards", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Pracownik ID: {reader["EmployeeId"]}, Numer Karty: {reader["CardNumber"]}");
                    }
                }
            }

            ShowEvents();
            ShowComputers();
            ShowRequests();

            Console.WriteLine("\n--- Wiadomości do działu kadr ---");
            using (var cmd = new SQLiteCommand("SELECT Status, COUNT(*) AS Count FROM HRMessages GROUP BY Status", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    int totalMessages = 0;
                    while (reader.Read())
                    {
                        string status = reader["Status"].ToString();
                        int count = Convert.ToInt32(reader["Count"]);
                        totalMessages += count;
                        Console.WriteLine($"{status}: {count}");
                    }
                    Console.WriteLine($"Łączna liczba wiadomości: {totalMessages}");
                }
            }
        }
        public static void AddComputer()
        {
            string query = "INSERT INTO ComputerStatus (IsOnline, OnlineSince) VALUES (0, NULL);";
            using (var cmd = new SQLiteCommand(query, dbConnection))
            {
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Dodano nowy komputer.");
        }

        public static void ShowComputers()
        {
            Console.WriteLine("\n--- Lista komputerów ---");
            using (var cmd = new SQLiteCommand("SELECT ComputerId, EmployeeId, IsOnline, OnlineSince FROM ComputerStatus", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string onlineSince = reader["OnlineSince"].ToString();
                        string workDuration = "N/A"; // Domyślnie brak czasu pracy

                        if (!string.IsNullOrEmpty(onlineSince) && reader["IsOnline"].ToString() == "1") // Jeśli komputer jest online
                        {
                            DateTime onlineTime = DateTime.Parse(onlineSince);
                            TimeSpan duration = DateTime.Now - onlineTime;
                            workDuration = duration.ToString(@"hh\:mm\:ss"); // Formatowanie na godziny:minuty:sekundy
                        }

                        Console.WriteLine($"ID: {reader["ComputerId"]}, Pracownik ID: {reader["EmployeeId"]}, Online: {reader["IsOnline"]}, Czas pracy: {workDuration}");
                    }
                }
            }
        }

        public static void AssignComputerToEmployee()
        {
            Console.Write("Podaj ID komputera: ");
            if (int.TryParse(Console.ReadLine(), out int computerId))
            {
                Console.Write("Podaj ID pracownika: ");
                if (int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    string query = "UPDATE ComputerStatus SET EmployeeId = @EmployeeId WHERE ComputerId = @ComputerId";
                    using (var cmd = new SQLiteCommand(query, dbConnection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        cmd.Parameters.AddWithValue("@ComputerId", computerId);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Przypisano komputer do pracownika.");
                }
                else Console.WriteLine("Nieprawidłowe ID pracownika.");
            }
            else Console.WriteLine("Nieprawidłowe ID komputera.");
        }
        public static void UpdateComputerStatus()
        {
            Console.Write("Podaj ID komputera: ");
            if (int.TryParse(Console.ReadLine(), out int computerId))
            {
                Console.Write("Podaj nowy status (1 - Online, 0 - Offline): ");
                if (int.TryParse(Console.ReadLine(), out int newStatus) && (newStatus == 0 || newStatus == 1))
                {
                    string query = "UPDATE ComputerStatus SET IsOnline = @IsOnline, OnlineSince = CASE WHEN @IsOnline = 1 THEN datetime('now') ELSE NULL END WHERE ComputerId = @ComputerId";
                    using (var cmd = new SQLiteCommand(query, dbConnection))
                    {
                        cmd.Parameters.AddWithValue("@IsOnline", newStatus);
                        cmd.Parameters.AddWithValue("@ComputerId", computerId);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Status komputera został zaktualizowany.");
                }
                else Console.WriteLine("Nieprawidłowy status. Wpisz 1 dla Online lub 0 dla Offline.");
            }
            else Console.WriteLine("Nieprawidłowe ID komputera.");
        }
        public static void AddRequest()
        {
            Console.Write("Podaj ID pracownika: ");
            if (int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.Write("Podaj typ zgłoszenia (np. Urlop, Zapytanie): ");
                string requestType = Console.ReadLine() ?? "";

                string query = "INSERT INTO Requests (EmployeeId, Type) VALUES (@EmployeeId, @Type)";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@Type", requestType);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Zgłoszenie zostało dodane.");
            }
            else Console.WriteLine("Nieprawidłowe ID pracownika.");
        }

        public static void ShowRequests()
        {
            Console.WriteLine("\n=== Lista zgłoszeń ===");
            using (var cmd = new SQLiteCommand("SELECT * FROM Requests", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["RequestId"]}, Pracownik ID: {reader["EmployeeId"]}, Typ: {reader["Type"]}, Status: {reader["Status"]}");
                    }
                }
            }
        }

        public static void UpdateRequestStatus()
        {
            Console.Write("Podaj ID zgłoszenia do aktualizacji: ");
            if (int.TryParse(Console.ReadLine(), out int requestId))
            {
                Console.Write("Podaj nowy status (Oczekujące, Zatwierdzone, Odrzucone): ");
                string newStatus = Console.ReadLine() ?? "";

                string query = "UPDATE Requests SET Status = @Status WHERE RequestId = @RequestId";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Status zgłoszenia został zaktualizowany.");
            }
            else Console.WriteLine("Nieprawidłowe ID zgłoszenia.");
        }
        public static void SendMessageToHR()
        {
            Console.Write("Podaj ID pracownika: ");
            if (int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.Write("Wpisz wiadomość do działu kadr: ");
                string message = Console.ReadLine() ?? "";

                string query = "INSERT INTO HRMessages (EmployeeId, Message, Timestamp) VALUES (@EmployeeId, @Message, datetime('now'))";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Wiadomość została wysłana do działu kadr.");
            }
            else Console.WriteLine("Nieprawidłowe ID pracownika.");
        }
        public static void ShowHRMessages()
        {
            Console.WriteLine("\n=== Wiadomości do działu kadr ===");
            using (var cmd = new SQLiteCommand("SELECT * FROM HRMessages", dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["MessageId"]}, Pracownik ID: {reader["EmployeeId"]}, Treść: {reader["Message"]}, Status: {reader["Status"]}, Data: {reader["Timestamp"]}");
                    }
                }
            }
        }
        public static void MarkMessageAsRead()
        {
            Console.Write("Podaj ID wiadomości do oznaczenia jako przeczytaną: ");
            if (int.TryParse(Console.ReadLine(), out int messageId))
            {
                string query = "UPDATE HRMessages SET Status = 'Przeczytane' WHERE MessageId = @MessageId";
                using (var cmd = new SQLiteCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("@MessageId", messageId);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Wiadomość została oznaczona jako przeczytana.");
            }
            else Console.WriteLine("Nieprawidłowe ID wiadomości.");
        }
        
    }
}