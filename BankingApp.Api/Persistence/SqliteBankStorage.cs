using BankingApp.Api.Persistence.Entities;
using BankingApp.Core;
using Microsoft.EntityFrameworkCore;


namespace BankingApp.Api.Persistence
{
    public class SqliteBankStorage : IBankStorage
    {
        private readonly BankDbContext context;

        public SqliteBankStorage(BankDbContext context)
        {
            this.context = context;

            context.Database.EnsureCreated();
        }

        public List<BankAccount> LoadAccounts()
        {

            List<BankAccountEntity> accountEntities = context.Accounts
                .Include(account => account.Transactions)
                .ToList();

            List<BankAccount> accounts = new List<BankAccount>();

            foreach(BankAccountEntity accountEntity in accountEntities)
            {
                List<Transaction> transactions = accountEntity.Transactions
                    .OrderBy(transaction => transaction.Date)
                    .Select(transaction => new Transaction(
                        transaction.Type,
                        transaction.Amount,
                        transaction.Date))
                    .ToList();

                BankAccount account = BankAccount.Restore(
                    accountEntity.OwnerName,
                    accountEntity.AccountNumber,
                    accountEntity.Balance,
                    transactions);

                accounts.Add(account);
            }

            return accounts;
        }

        public void SaveAccounts(IReadOnlyList<BankAccount> accounts)
        {
            List<BankAccountEntity> existingAccounts = context.Accounts
                .Include(account => account.Transactions)
                .ToList();

            context.Accounts.RemoveRange(existingAccounts);

            List<BankAccountEntity> accountEntities = accounts
                .Select(account => new BankAccountEntity
                {
                    OwnerName = account.OwnerName,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    Transactions = account.GetTransactionHistory()
                    .Select(transaction => new TransactionEntity
                    {
                        Type = transaction.Type,
                        Amount = transaction.Amount,
                        Date = transaction.Date,
                    })
                    .ToList()
                })
                .ToList();

            context.Accounts.AddRange(accountEntities);

            context.SaveChanges();
        }
    }
}
