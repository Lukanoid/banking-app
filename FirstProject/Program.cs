using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter account owner name: ");
            string ownerName = Console.ReadLine();

            BankAccount account = new BankAccount(ownerName);

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("1. Deposit:");
                Console.WriteLine("2. Withdraw:");
                Console.WriteLine("3. Show Balance:");
                Console.WriteLine("4. Show Transactions:");
                Console.WriteLine("5. Exit:");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter deposit amount: ");
                        decimal depositAmount = decimal.Parse(Console.ReadLine());
                        account.Deposit(depositAmount);
                        break;
                    case "2":
                        Console.WriteLine("Enter withdraw amount: ");
                        decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                        account.Withdraw(withdrawAmount);
                        break;
                    case "3":
                        Console.WriteLine($"Current balance: {account.Balance}");
                        break;
                    case "4":
                        account.ShowTransactions();
                        break;
                    case "5":
                        isRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

            }
        }
    }
}
