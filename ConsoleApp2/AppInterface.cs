namespace EmployeeManagementSystem;

public static class AppInterface
{
    public static void OpenInterface()
    {
        Console.WriteLine("\n=== System Zarządzania Pracownikami ===");
        Console.WriteLine("1. Dodaj pracownika");
        Console.WriteLine("2. Usuń pracownika");
        Console.WriteLine("3. Edytuj dane pracownika");
        Console.WriteLine("4. Zarządzaj kalendarzem");
        Console.WriteLine("5. Zarządzaj statusami komputerów");
        Console.WriteLine("6. Wyświetl dane");
        Console.WriteLine("7. Zarządzaj zgłoszeniami");
        Console.WriteLine("8. Komunikacja z działem kadr");
        Console.WriteLine("9. Zamknij");


        Console.Write("Wybierz opcję: ");
        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1": DatabaseManager.AddEmployee(); break;
            case "2": DatabaseManager.RemoveEmployee(); break;
            case "3": DatabaseManager.EditEmployee(); break;
            case "4": DatabaseManager.ManageCalendar(); break;
            case "5":
                Console.WriteLine("\n=== Zarządzanie komputerami ===");
                Console.WriteLine("1. Dodaj komputer");
                Console.WriteLine("2. Lista obecnych komputerów");
                Console.WriteLine("3. Przypisz komputer do ID pracownika");
                Console.WriteLine("4. Zmień status komputera (Online/Offline)");
                Console.Write("Wybierz opcję: ");
                string computerChoice = Console.ReadLine() ?? "";

                switch (computerChoice)
                {
                    case "1":
                        DatabaseManager.AddComputer();
                        break;
                    case "2":
                        DatabaseManager.ShowComputers();
                        break;
                    case "3":
                        DatabaseManager.AssignComputerToEmployee();
                        break;
                    case "4":
                        DatabaseManager.UpdateComputerStatus();
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
                break;

            case "6": DatabaseManager.ShowAllData(); break;
            case "7":
                Console.WriteLine("\n=== Zarządzanie zgłoszeniami ===");
                Console.WriteLine("1. Dodaj zgłoszenie");
                Console.WriteLine("2. Lista zgłoszeń");
                Console.WriteLine("3. Zmień status zgłoszenia");
                Console.Write("Wybierz opcję: ");
                string requestChoice = Console.ReadLine() ?? "";

                switch (requestChoice)
                {
                    case "1":
                        DatabaseManager.AddRequest();
                        break;
                    case "2":
                        DatabaseManager.ShowRequests();
                        break;
                    case "3":
                        DatabaseManager.UpdateRequestStatus();
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
                break;
            case "8":
                Console.WriteLine("\n=== Komunikacja z działem kadr ===");
                Console.WriteLine("1. Wyślij wiadomość");
                Console.WriteLine("2. Wyświetl wiadomości");
                Console.WriteLine("3. Oznacz wiadomość jako przeczytaną");
                Console.Write("Wybierz opcję: ");
                string hrChoice = Console.ReadLine() ?? "";

                switch (hrChoice)
                {
                    case "1":
                        DatabaseManager.SendMessageToHR();
                        break;
                    case "2":
                        DatabaseManager.ShowHRMessages();
                        break;
                    case "3":
                        DatabaseManager.MarkMessageAsRead();
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
                break;

            case "9": Environment.Exit(0); return;
            default: Console.WriteLine("Nieprawidłowy wybór."); break;
        }
    }
}