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

            Console.Write("Enter amount to deposit: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            account.Deposit(amount);

            Console.WriteLine($"Current Balance: {account.Balance}");
        }
    }
}
