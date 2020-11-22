using EmployeeClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace UserUi
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string inputName = LoginAndValidation.UserLogin();

            var EmployeeList = CSVOperations.GetEmployeesFromCSV();

            while (true)
            {

                Console.WriteLine("Write 1 to print all Employees. \n" +
                                  "Write 2 to view your info. \n" +
                                  "Write 3 to edit your info. \n" +
                                  "Write 4 to save Employees to CSV-file.\n");

                string x = "";
                x = Console.ReadLine();

                if (x == "1")
                {
                    Console.Clear();
                    CSVOperations.PrintEmployees(EmployeeList);
                    continue;
                }

                if (x == "2")
                {
                    Console.Clear();
                    CSVOperations.PrintCurrentUser(inputName, EmployeeList);
                }

                if (x == "3")
                {
                    CSVOperations.EditCurrentUser(inputName, EmployeeList);
                    continue;
                }


                if (x == "4")
                {
                    CSVOperations.SaveToCSV(EmployeeList);
                    continue;
                }

                x = "";
            }
        }
    }
}
