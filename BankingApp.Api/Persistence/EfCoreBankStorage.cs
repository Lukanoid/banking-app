using BankingApp.Api.Persistence.Entities;
using BankingApp.Core;
using Microsoft.EntityFrameworkCore;


namespace BankingApp.Api.Persistence
{
    public class EfCoreBankStorage : IBankStorage
    {
        private readonly BankDbContext context;

        public EfCoreBankStorage(BankDbContext context)
        {
            this.context = context;

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
                        transaction.Date,
                        transaction.Description))
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
            List<BankAccountEntity> existingAccountsEntities = context.Accounts
                .Include(account => account.Transactions)
                .ToList();

            HashSet<string> currentAccountNumber = accounts
                .Select(account => account.AccountNumber)
                .ToHashSet();

            foreach(BankAccountEntity existingAccountEntity in existingAccountsEntities)
            {
                if (!currentAccountNumber.Contains(existingAccountEntity.AccountNumber))
                {
                    context.Accounts.Remove(existingAccountEntity);
                }
            }

            foreach(BankAccount account in accounts)
            {
                BankAccountEntity? accountEntity = existingAccountsEntities
                    .FirstOrDefault(existingAccount => existingAccount.AccountNumber == account.AccountNumber);

                if(accountEntity == null)
                {
                    BankAccountEntity newAccountEntity = new BankAccountEntity
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
                               Description = transaction.Description,
                           })
                           .ToList()
                    };

                    context.Accounts.Add(newAccountEntity);

                    continue;
                }

                accountEntity.OwnerName = account.OwnerName;
                accountEntity.Balance = account.Balance;

                context.Transactions.RemoveRange(accountEntity.Transactions);
                accountEntity.Transactions.Clear();

                foreach(Transaction transaction in account.GetTransactionHistory())
                {
                    accountEntity.Transactions.Add(new TransactionEntity
                    {
                        Type = transaction.Type,
                        Amount = transaction.Amount, 
                        Date = transaction.Date,
                        Description = transaction.Description,
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
