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

            var EmployeeList = Library.GetEmployeesFromCSV();
            
            LoginAndValidation.AdminLogin();
            string inputName = "";

            while (true)
            {
                
                Console.WriteLine("Write 1 to print all Employees. \n" +
                                  "Write 2 to create a new Employee. \n" +
                                  "Write 3 to edit a Employee. \n" +
                                  "Write 4 to save Employees to CSV-file.\n" +
                                  "Write 5 to remove a Employee.");
                
                string x = "";
                x = Console.ReadLine();

                if (x == "1")
                {
                    Console.Clear();
                    Library.PrintEmployees(EmployeeList);
                    continue;
                }

                if (x == "2")
                {
                    Console.Clear();
                    Library.AddEmployee(EmployeeList);
                    continue;
                }

                if (x == "3")
                {
                    Console.Clear();
                    Library.EditEmployee(inputName, EmployeeList);
                    continue;
                }

                if (x == "4")
                {
                    Console.Clear();
                    Library.SaveToCSV(EmployeeList);
                    continue;
                }

                if (x == "5")
                {
                    Library.RemoveEmployee(EmployeeList);
                    continue;
                }
            }
        }
    }
}
