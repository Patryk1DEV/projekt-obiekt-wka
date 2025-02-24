using System;

namespace EmployeeManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager.InitializeDatabase();

            while (true)
            {
                AppInterface.OpenInterface();
            }
        }
    }
}
