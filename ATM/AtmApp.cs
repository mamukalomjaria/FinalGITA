using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace ATM
{
    public static class AtmApp
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("=== ATM ===");
                Console.WriteLine("1) Register");
                Console.WriteLine("2) Login");
                Console.WriteLine("0) Exit");
                Console.Write("Choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        var user = Login();
                        if (user != null)
                        {
                            UserMenu(user);
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.\n");
                        break;
                }
            }
        }

        private static void Register()
        {
            try
            {
                Console.Write("First name: ");
                string? first = Console.ReadLine();
                Console.Write("Last name: ");
                string? last = Console.ReadLine();
                Console.Write("Personal ID: ");
                string? pid = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(first) ||
                    string.IsNullOrWhiteSpace(last) ||
                    string.IsNullOrWhiteSpace(pid))
                {
                    Console.WriteLine("All fields are required.\n");
                    return;
                }

                if (UserRepository.FindByPersonalId(pid!) != null)
                {
                    Console.WriteLine("User with this personal ID already exists.\n");
                    return;
                }

                var rnd = new Random();
                string password = rnd.Next(0, 10000).ToString("D4");

                var user = new User
                {
                    Id = UserRepository.GetNextId(),
                    FirstName = first!,
                    LastName = last!,
                    PersonalId = pid!,
                    Password = password,
                    Balance = 0
                };

                UserRepository.AddUser(user);

                Console.WriteLine($"Registration successful! Your password is: {password}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while registering: {ex.Message}\n");
            }
        }

        private static User Login()
        {
            Console.Write("Personal ID: ");
            string? pid = Console.ReadLine();
            Console.Write("Password (4 digits): ");
            string? pass = Console.ReadLine();

            var user = UserRepository.FindByPersonalId(pid ?? "");
            if (user == null || user.Password != (pass ?? ""))
            {
                Console.WriteLine("Invalid credentials.\n");
                return null;
            }

            Console.WriteLine($"Welcome, {user.FirstName} {user.LastName}!\n");
            return user;
        }

        private static void UserMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("=== ATM MENU ===");
                Console.WriteLine("1) Check balance");
                Console.WriteLine("2) Deposit money");
                Console.WriteLine("3) Withdraw money");
                Console.WriteLine("4) Show operation history");
                Console.WriteLine("0) Logout");
                Console.Write("Choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowBalance(user);
                        break;
                    case "2":
                        Deposit(user);
                        break;
                    case "3":
                        Withdraw(user);
                        break;
                    case "4":
                        ShowHistory(user);
                        break;
                    case "0":
                        Console.WriteLine("Logged out.\n");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.\n");
                        break;
                }
            }
        }

        private static void ShowBalance(User user)
        {
            Console.WriteLine($"Your balance is: {user.Balance} GEL\n");

            string desc =
                $"User {user.FirstName} {user.LastName} checked balance on {DateTime.Now:dd.MM.yyyy}. Balance: {user.Balance} GEL.";

            LogService.AddLog(new TransactionLog
            {
                PersonalId = user.PersonalId,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Operation = "CheckBalance",
                Amount = 0,
                BalanceAfter = user.Balance,
                Date = DateTime.Now,
                Description = desc
            });
        }

        private static void Deposit(User user)
        {
            Console.Write("Enter amount to deposit: ");
            string? input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.\n");
                return;
            }

            user.Balance += amount;
            UserRepository.UpdateUser(user);

            string desc =
                $"User {user.FirstName} {user.LastName} deposited {amount} GEL on {DateTime.Now:dd.MM.yyyy}. Balance is {user.Balance} GEL.";

            LogService.AddLog(new TransactionLog
            {
                PersonalId = user.PersonalId,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Operation = "Deposit",
                Amount = amount,
                BalanceAfter = user.Balance,
                Date = DateTime.Now,
                Description = desc
            });

            Console.WriteLine("Deposit successful.\n");
        }

        private static void Withdraw(User user)
        {
            Console.Write("Enter amount to withdraw: ");
            string? input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.\n");
                return;
            }

            if (amount > user.Balance)
            {
                Console.WriteLine("Not enough balance.\n");
                return;
            }

            user.Balance -= amount;
            UserRepository.UpdateUser(user);

            string desc =
                $"User {user.FirstName} {user.LastName} withdrew {amount} GEL on {DateTime.Now:dd.MM.yyyy}. Balance is {user.Balance} GEL.";

            LogService.AddLog(new TransactionLog
            {
                PersonalId = user.PersonalId,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Operation = "Withdraw",
                Amount = amount,
                BalanceAfter = user.Balance,
                Date = DateTime.Now,
                Description = desc
            });

            Console.WriteLine("Withdraw successful.\n");
        }

        private static void ShowHistory(User user)
        {
            var logs = LogService.LoadLogsForUser(user.PersonalId);

            Console.WriteLine("\n=== OPERATION HISTORY ===");
            if (logs.Count == 0)
            {
                Console.WriteLine("No operations yet.");
            }
            else
            {
                foreach (var log in logs)
                {
                    Console.WriteLine($"{log.Date:dd.MM.yyyy HH:mm} - {log.Description}");
                }
            }
            Console.WriteLine("=========================\n");
        }
    }
}
