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
            BankSystem bankSystem = new BankSystem();
            BankAccount selectedAccount = null;

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Select Account");
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Withdraw");
                Console.WriteLine("5. Show Balance");
                Console.WriteLine("6. Show Transaction History");
                Console.WriteLine("7. Show All Accounts");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter owner name");
                        string ownerName = Console.ReadLine();
                        BankAccount newAccount = bankSystem.CreateAccount(ownerName);

                        Console.WriteLine($"Account created for {newAccount.OwnerName}");
                        Console.WriteLine($"Account number: {newAccount.AccountNumber}");
                        break;
                    case "2":
                        Console.WriteLine("Enter account number: ");
                        string accountNumber = Console.ReadLine();

                        selectedAccount = bankSystem.FindAccount(accountNumber);

                        if(CheckAccount(selectedAccount))
                        {
                            Console.WriteLine($"Selected account: {selectedAccount.AccountNumber}");
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;
                    case "3":
                        if(!CheckAccount(selectedAccount))
                        {
                            Console.WriteLine("Please select an account first.");
                            break;
                        }
                        Console.Write("Enter amount to deposit: ");
                        if(decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                        {
                            OperationResult result = selectedAccount.Deposit(depositAmount);
                            Console.WriteLine(result.Message);
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount.");
                        }
                        break;
                    case "4":
                        if (!CheckAccount(selectedAccount))
                        {
                            Console.WriteLine("Please select an account first.");
                            break;
                        }
                        Console.Write("Enter amount to withdraw: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                        {
                            OperationResult result = selectedAccount.Withdraw(withdrawAmount);
                            Console.WriteLine(result.Message);
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount.");
                        }
                        break;
                    case "5":
                        if (!CheckAccount(selectedAccount))
                        {
                            Console.WriteLine("Please select an account first.");
                            break;
                        }

                        Console.WriteLine($"Current balance: {selectedAccount.Balance}");
                        break;
                    case "6":
                        if (!CheckAccount(selectedAccount))
                        {
                            Console.WriteLine("Please select an account first.");
                            break;
                        }

                        selectedAccount.ShowTransactionHistory();
                        break;

                    case "7":
                        bankSystem.ShowAllAccounts();
                        break;

                    case "8":
                        isRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;

                }
            }
        }

        private static bool HasSelectedAccount(BankAccount currentAccount)
        {
            return currentAccount != null;
        }
    }
}
