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

            Console.WriteLine($"Account creater for {account.OwnerName}");
            Console.WriteLine($"Current balance {account.Balance}");
        }
    }
}
