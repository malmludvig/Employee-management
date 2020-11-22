using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Net;

namespace EmployeeClassLibrary
{
    public class Employee
    {
        public string Id;
        public string Name;
        public string Password;
        public string Address;
        public bool IsAdmin;

    }


    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }

    public class LoginAndValidation
    {

        public static void AdminLogin()
        {
            string inputName = "";
            while (true)
            {
                Console.WriteLine("Welcome to the admin interface. Please login:");
                Console.WriteLine("Name:"); inputName = Console.ReadLine();
                Console.WriteLine("Password"); string inputPassword = Console.ReadLine();

                inputPassword = Encryptor.MD5Hash(inputPassword);

                if (CSVOperations.LoginVerification(inputName, inputPassword))
                {
                    if (CSVOperations.IsAdminOrNot(inputName, inputPassword))
                    {
                        Console.Clear();
                        Console.WriteLine("Welcome, " + inputName + "!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Access denied. Only Admins can login here.");

                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid login");
                    continue;
                }
            }
        }

        public static string UserLogin()
        {
            string inputName = "";

            while (true)
            {

                Console.WriteLine("Welcome to the user interface. Please login:");
                Console.WriteLine("Name:"); inputName = Console.ReadLine();
                Console.WriteLine("Password"); string inputPassword = Console.ReadLine();

                inputPassword = Encryptor.MD5Hash(inputPassword);

                if (CSVOperations.LoginVerification(inputName, inputPassword))
                {
                    if (!CSVOperations.IsAdminOrNot(inputName, inputPassword))
                    {
                        Console.Clear();
                        Console.WriteLine("Welcome, " + inputName + "!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Access denied. Only non-Admins can login here.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid login");
                    continue;
                }
            }

            return inputName;
        }


        public static bool ValidateInput(string input)
        {
            bool check = false;

            if (!string.IsNullOrEmpty(input))
            {
                if (input.All(c => Char.IsLetterOrDigit(c) || c == ' '))
                {
                    check = true;
                    return check;
                }
                else
                {
                    Console.WriteLine("Only letters and numbers are allowed.");
                    check = false;
                    return check;
                }
            }
            else
            {
                Console.WriteLine("You have entered an empty string.");
                check = false;
                return check;
            }
        }

        public static bool ValidatePasswordInput(string input)
        {
            bool check = false;

            if (!string.IsNullOrEmpty(input))
            {
                check = true;
                return check;
            }
            else
            {
                Console.WriteLine("You have entered an empty string.");
                check = false;
                return check;
            }
        }

        public static bool ValidateBoolInput(string input)
        {
            bool check = false;

            if (!string.IsNullOrEmpty(input))
            {
                if (input.All(c => Char.IsLetterOrDigit(c) || c == ' '))
                {

                    if (input.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        check = true;

                    if (input.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                        check = true;

                    return check;
                }
                else
                {
                    Console.WriteLine("Only letters and numbers are allowed.");
                    check = false;
                    return check;
                }
            }
            else
            {
                Console.WriteLine("You have entered an empty string.");
                check = false;
                return check;
            }
        }
    }


    public class CSVOperations
    {
        public static void PrintEmployees(List<object> EmployeeList)

        {

            Console.WriteLine("Employees from the CSV file:\n");
            foreach (Employee item in EmployeeList)

            {
                Console.WriteLine("Id: " + item.Id);
                Console.WriteLine("Name: " + item.Name);
                Console.WriteLine("Password: " + item.Password);
                Console.WriteLine("Address: " + item.Address);
                Console.WriteLine("Admin: " + item.IsAdmin + "\n");
            }
        }

        public static void PrintCurrentUser(string inputName, List<object> EmployeeList)

        {

            Console.WriteLine("Your info from the CSV file:");
            foreach (Employee item in EmployeeList)

            {

                if (item.Name == inputName)

                {
                    Console.WriteLine("Id: " + item.Id);
                    Console.WriteLine("Name: " + item.Name);
                    Console.WriteLine("Address: " + item.Address);
                    Console.WriteLine("Admin: " + item.IsAdmin + "\n");
                }
            }
        }

        public static void AddEmployee(List<object> EmployeeList)

        {

            string newName = "";
            string newAddress = "";
            string newPassword = "";
            string isAdmin = "";

            while (true)
            {

                Console.WriteLine("What is the name of the Employee?");
                newName = Console.ReadLine();
                Console.WriteLine("What is the address of the Employee?");
                newAddress = Console.ReadLine();
                Console.WriteLine("What is the password of the Employee?");
                newPassword = Console.ReadLine();
                Console.WriteLine("Is the Employee an admin? true/false");
                isAdmin = Console.ReadLine();



                if (!LoginAndValidation.ValidateInput(newName) ||
                   !LoginAndValidation.ValidateInput(newAddress) ||
                   !LoginAndValidation.ValidateInput(newPassword))
                {
                    Console.WriteLine("Incorrect validation");
                    continue;
                }
                Console.WriteLine("New Employee " + newName + " has been added.\n");
                break;
            }

            string inputPassword = Encryptor.MD5Hash(newPassword);

            Employee newEmployee = new Employee();

            //Här nedan så skapar jag ett Guid och använder sedan dess värde som en sträng för id.
            var originalGuid = Guid.NewGuid();
            string stringGuid = originalGuid.ToString("B");

            var charsToRemove = new string[] { "{", "}" };
            foreach (var c in charsToRemove)
            {
                stringGuid = stringGuid.Replace(c, string.Empty);
            }

            newEmployee.Id = stringGuid;
            newEmployee.Name = newName;
            newEmployee.Password = inputPassword;
            newEmployee.Address = newAddress;

            if (isAdmin == "true")
            {
                newEmployee.IsAdmin = true;
            }

            EmployeeList.Add(newEmployee);

        }

        public static void EditEmployee(string inputName, List<object> EmployeeList)
        {
            Console.Clear();
            Console.WriteLine("What is the name of the Employee you want to edit?");

            foreach (Employee item in EmployeeList)
            {
                Console.WriteLine(item.Name);
            }

            string editEmployee = Console.ReadLine();
            int i = 0;
            string inputNumber = "";
            foreach (Employee item in EmployeeList)

            {
                if (item.Name == editEmployee)
                {
                    Console.Clear();
                    Console.WriteLine(
                  "Write 1 to edit name. \n" +
                  "Write 2 to edit password. \n" +
                  "Write 3 to edit address.\n" +
                  "Write 4 to change admin status. \n" +
                  "Write 5 to exit. \n");

                    inputNumber = Console.ReadLine();

                    while (true)
                    {

                        if (inputNumber == "1")
                        {
                            while (true)
                            {

                                Console.WriteLine(item.Name);
                                Console.WriteLine("Enter a new name:");
                                string newName = Console.ReadLine();

                                if (!LoginAndValidation.ValidateInput(newName))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }
                                Console.WriteLine("Employee " + item.Name + " has been edited.\n");
                                item.Name = newName;

                                break;
                            }

                        }
                        else if (inputNumber == "2")
                        {
                            while (true)
                            {

                                Console.WriteLine("Edit password:");
                                string newPassword = Console.ReadLine();

                                if (!LoginAndValidation.ValidatePasswordInput(newPassword))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }

                                string inputPassword = Encryptor.MD5Hash(newPassword);
                                item.Password = inputPassword;
                                Console.WriteLine("Employee " + item.Name + " has been edited.\n");

                                break;
                            }


                        }
                        else if (inputNumber == "3")
                        {
                            while (true)
                            {

                                Console.WriteLine(item.Address);
                                Console.WriteLine("Enter a new address:");
                                string newAddress = Console.ReadLine();

                                if (!LoginAndValidation.ValidateInput(newAddress))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }
                                Console.WriteLine("Employee " + item.Address + " has been edited.\n");
                                item.Address = newAddress;

                                break;
                            }
                        }
                        else if (inputNumber == "4")
                        {

                            while (true)
                            {

                                Console.WriteLine(item.IsAdmin);
                                Console.WriteLine("Enter true or false to change admin status.");
                                string newAdminStatus = Console.ReadLine();

                                if (!LoginAndValidation.ValidateBoolInput(newAdminStatus))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }
                                Console.WriteLine("Employee " + item.Address + " has been edited.\n");
                                item.IsAdmin = bool.Parse(newAdminStatus);

                                break;
                            }

                        }

                        break;
                    }
                }
                i++;
            }

            Console.Clear();
            if (inputNumber == "5")
            {
                Console.WriteLine("Editing complete. No changes were made. Press any key to exit.");

            }
            else
            {
                Console.WriteLine(editEmployee + " has been edited successfully. Press any key to exit.");
            }
            Console.ReadKey();
            Console.WriteLine();
            Console.Clear();

        }

        public static void EditCurrentUser(string inputName, List<object> EmployeeList)
        {
            Console.Clear();
            string inputNumber = "";
            int i = 0;
            foreach (Employee item in EmployeeList)

            {
                if (item.Name == inputName)
                {
                    Console.WriteLine(
                  "Write 1 to edit name. \n" +
                  "Write 2 to edit password. \n" +
                  "Write 3 to edit address.\n" +
                  "Write 4 to exit. \n");

                    inputNumber = Console.ReadLine();

                    while (true)
                    {

                        if (inputNumber == "1")
                        {
                            while (true)
                            {

                                Console.WriteLine(item.Name);
                                Console.WriteLine("Enter a new name:");
                                string newName = Console.ReadLine();

                                if (!LoginAndValidation.ValidateInput(newName))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }
                                Console.WriteLine("Employee " + item.Name + " has been edited.\n");
                                item.Name = newName;
                                inputName = newName;

                                break;
                            }
                        }
                        else if (inputNumber == "2")
                        {

                            while (true)
                            {

                                Console.WriteLine("Enter a new password:");
                                string newPassword = Console.ReadLine();
                                Console.WriteLine("Enter a the same password again:");
                                string newPassword2 = Console.ReadLine();

                                if (newPassword == newPassword2)
                                {
                                    string inputPassword = Encryptor.MD5Hash(newPassword);
                                    item.Password = inputPassword;
                                    Console.WriteLine("The passwords did match. Employee " + item.Name + " has been edited.\n");

                                }
                                else
                                {
                                    Console.WriteLine("The passwords did not match. Try again.");
                                    continue;
                                }

                                if (!LoginAndValidation.ValidatePasswordInput(newPassword))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }



                                break;
                            }






                        }
                        else if (inputNumber == "3")
                        {
                            while (true)
                            {

                                Console.WriteLine(item.Address);
                                Console.WriteLine("Enter a new address:");
                                string newAddress = Console.ReadLine();

                                if (!LoginAndValidation.ValidateInput(newAddress))

                                {
                                    Console.WriteLine("Incorrect validation");
                                    continue;
                                }
                                Console.WriteLine("Employee " + item.Address + " has been edited.\n");
                                item.Address = newAddress;

                                break;
                            }
                        }

                        break;

                    }
                }
                i++;
            }

            Console.Clear();
            if (inputNumber == "4")
            {
                Console.WriteLine("Editing complete. No changes were made. Press any key to exit.");

            }
            else
            {
                Console.WriteLine(inputName + " has been edited successfully. Press any key to exit.");
            }
            Console.ReadKey();
            Console.WriteLine();
            Console.Clear();
        }

        public static void RemoveEmployee(List<object> EmployeeList)
        {
            Console.Clear();
            Console.WriteLine("What is the Name of the Employee you want to remove?");

            foreach (Employee item in EmployeeList)
            {
                Console.WriteLine(item.Name);
            }

            string removeEmployee = Console.ReadLine();

            int i = 0;
            foreach (Employee item in EmployeeList)

            {
                if (item.Name == removeEmployee)
                {
                    EmployeeList.RemoveAt(i);
                    break;
                }
                i++;
            }

            Console.WriteLine(removeEmployee + " was removed from the list.");
        }


        public static void SaveToCSV(List<object> EmployeeList)
        {

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataFolder, "Employees.csv");

            var myFile = File.Create(filePath);
            myFile.Close();

            string objectToFile = "";

            foreach (Employee item in EmployeeList)

            {

                objectToFile = item.Id + "," + item.Name + "," + item.Password + "," + item.Address +
                  "," + item.IsAdmin;

                using (StreamWriter stream = File.AppendText(filePath))
                {
                    stream.WriteLine(objectToFile);
                }
            }

            Console.WriteLine("Employees were written to CSV file.\n");
        }

        public static bool IsAdminOrNot(string loginName, string loginPassword)
        {
            bool returnedBool2 = false;
            List<object> loginList = GetEmployeesFromCSV();

            foreach (Employee item in loginList)

            {

                if (loginName == item.Name && loginPassword == item.Password)
                {
                    if (item.IsAdmin == true)
                    {
                        Console.WriteLine("Admin detected");
                        returnedBool2 = true;
                    }
                }
            }
            return returnedBool2;
        }

        public static bool LoginVerification(string loginName, string loginPassword)
        {
            bool returnedBool = false;
            List<object> loginList = GetEmployeesFromCSV();

            foreach (Employee item in loginList)

            {
                if (loginName == item.Name && loginPassword == item.Password)
                {
                    returnedBool = true;
                }
            }

            return returnedBool;
        }

        public static List<object> GetEmployeesFromCSV()
        {
            List<object> EmployeeList = new List<object>();

            //https://stackoverflow.com/questions/3243782/reading-from-a-file-in-current-user-appdata-folder-for-c-sharp

            string appDataFolder = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);

            string filePath = Path.Combine(appDataFolder, "Employees.csv");
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string getId = line.Split(',')[0];
                    string getName = line.Split(',')[1];
                    string getPassword = line.Split(',')[2];
                    string getAddress = line.Split(',')[3];
                    string getAdmin = line.Split(',')[4];

                    Employee newEmployee = new Employee();
                    newEmployee.Id = getId;
                    newEmployee.Name = getName;
                    newEmployee.Password = getPassword;
                    newEmployee.Address = getAddress;

                    if (getAdmin == "True")
                    {
                        newEmployee.IsAdmin = true;
                    }

                    EmployeeList.Add(newEmployee);
                }
            }

            return EmployeeList;
        }
    }
}
