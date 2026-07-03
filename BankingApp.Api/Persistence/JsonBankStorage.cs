using System.Text.Json;
using System.Text.Json.Serialization;
using BankingApp.Api.Persistence.Models;
using BankingApp.Core;
using Microsoft.AspNetCore.Hosting;

namespace BankingApp.Api.Persistence
{
    public class JsonBankStorage : IBankStorage
    {
        private readonly string filePath;

        private readonly JsonSerializerOptions jsonOptions;

        public JsonBankStorage(IWebHostEnvironment environment)
        {
            filePath = Path.Combine(environment.ContentRootPath, "Data", "accounts.json");

            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            jsonOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public List<BankAccount> LoadAccounts()
        {
            if (!File.Exists(filePath))
            {
                return new List<BankAccount>();
            }

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<BankAccount>();
            }

            List<StoredBankAccount> storedAccounts =
                JsonSerializer.Deserialize<List<StoredBankAccount>>(json, jsonOptions)
                ?? new List<StoredBankAccount>();

            List<BankAccount> accounts = new List<BankAccount>();

            foreach(StoredBankAccount storedAccount in storedAccounts)
            {
                List<Transaction> transactions = storedAccount.Transactions
                    .Select(transaction => new Transaction(
                        transaction.Type,
                        transaction.Amount,
                        transaction.Date))
                    .ToList();

                BankAccount account = BankAccount.Restore(
                    storedAccount.OwnerName,
                    storedAccount.AccountNumber,
                    storedAccount.Balance,
                    transactions
                    );

                accounts.Add(account);
            }

            return accounts;
        }

        public void SaveAccounts(IReadOnlyList<BankAccount> accounts)
        {
            string? directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            List<StoredBankAccount> storedAccounts = accounts
                .Select(account => new StoredBankAccount
                {
                    OwnerName = account.OwnerName,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    Transactions = account.GetTransactionHistory()
                        .Select(transaction => new StoredTransaction
                        {
                            Type = transaction.Type,
                            Amount = transaction.Amount,
                            Date = transaction.Date
                        })
                        .ToList()

                })
                .ToList();

            string json = JsonSerializer.Serialize(storedAccounts, jsonOptions);

            File.WriteAllText(filePath, json);
        }
    }
}
