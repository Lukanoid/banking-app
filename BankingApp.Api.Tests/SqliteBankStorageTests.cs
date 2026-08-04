using BankingApp.Api.Persistence;
using BankingApp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace BankingApp.Api.Tests
{
    public class SqliteBankStorageTests
    {
        [Fact]
        public void LoadAccounts_ShouldReturnEmptyList_WhenDatabaseIsEmpty()
        {
            string databasePath = CreateDatabasePath();

            try
            {
                using BankDbContext context = CreateContext(databasePath);
                context.Database.EnsureCreated();

                SqliteBankStorage storage = new SqliteBankStorage(context);

                List<BankAccount> accounts = storage.LoadAccounts();

                Assert.Empty(accounts);
            }
            finally
            {
                DeleteDatabaseFiles(databasePath);
            }
        }

        [Fact]
        public void SaveAccounts_ShouldPersistAccountsToDatabase()
        {
            string databasePath = CreateDatabasePath();

            try
            {
                using (BankDbContext saveContext = CreateContext(databasePath))
                {
                    saveContext.Database.EnsureCreated();

                    SqliteBankStorage saveStorage = new SqliteBankStorage(saveContext);

                    BankAccount account = new BankAccount("John Doe", "123");

                    saveStorage.SaveAccounts(new List<BankAccount>
                    {
                        account
                    });
                }

                using (BankDbContext loadContext = CreateContext(databasePath))
                {
                    SqliteBankStorage loadStorage = new SqliteBankStorage(loadContext);

                    List<BankAccount> loadedAccounts = loadStorage.LoadAccounts();

                    BankAccount loadedAccount = Assert.Single(loadedAccounts);

                    Assert.Equal("John Doe", loadedAccount.OwnerName);
                    Assert.Equal("123", loadedAccount.AccountNumber);
                    Assert.Equal(0m, loadedAccount.Balance);
                    Assert.Empty(loadedAccount.GetTransactionHistory());
                }
            }
            finally
            {
                DeleteDatabaseFiles(databasePath);
            }
        }

        [Fact]
        public void SaveAccounts_ShouldPersistTransactionsToDatabase()
        {
            string databasePath = CreateDatabasePath();

            try
            {
                using (BankDbContext saveContext = CreateContext(databasePath))
                {
                    saveContext.Database.EnsureCreated();

                    SqliteBankStorage saveStorage = new SqliteBankStorage(saveContext);

                    BankAccount account = new BankAccount("John Doe", "123");

                    account.Deposit(500m);
                    account.Withdraw(100m);

                    saveStorage.SaveAccounts(new List<BankAccount>
                    {
                        account
                    });
                }

                using (BankDbContext loadContext = CreateContext(databasePath))
                {
                    SqliteBankStorage loadStorage = new SqliteBankStorage(loadContext);

                    List<BankAccount> loadedAccounts = loadStorage.LoadAccounts();

                    BankAccount loadedAccount = Assert.Single(loadedAccounts);
                    IReadOnlyList<Transaction> transactions = loadedAccount.GetTransactionHistory();

                    Assert.Equal(400m, loadedAccount.Balance);
                    Assert.Equal(2, transactions.Count);

                    Assert.Contains(transactions, transaction =>
                        transaction.Type == TransactionType.Deposit &&
                        transaction.Amount == 500m);

                    Assert.Contains(transactions, transaction =>
                        transaction.Type == TransactionType.Withdraw &&
                        transaction.Amount == 100m);
                }
            }
            finally
            {
                DeleteDatabaseFiles(databasePath);
            }
        }

        [Fact]
        public void SaveAccounts_ShouldPersistMultipleAccountsToDatabase()
        {
            string databasePath = CreateDatabasePath();

            try
            {
                using (BankDbContext saveContext = CreateContext(databasePath))
                {
                    saveContext.Database.Migrate();

                    SqliteBankStorage saveStorage = new SqliteBankStorage(saveContext);

                    BankAccount firstAccount = new BankAccount("John Doe", "123");
                    BankAccount secondAccount = new BankAccount("Vasil", "456");

                    firstAccount.Deposit(1000m);
                    secondAccount.Deposit(500m);

                    saveStorage.SaveAccounts(new List<BankAccount>
                    {
                        firstAccount,
                        secondAccount
                    });
                }

                using (BankDbContext loadContext = CreateContext(databasePath))
                {
                    SqliteBankStorage loadStorage = new SqliteBankStorage(loadContext);

                    List<BankAccount> loadedAccounts = loadStorage.LoadAccounts();

                    Assert.Equal(2, loadedAccounts.Count);

                    Assert.Contains(loadedAccounts, account =>
                        account.OwnerName == "John Doe" &&
                        account.AccountNumber == "123" &&
                        account.Balance == 1000m);

                    Assert.Contains(loadedAccounts, account =>
                        account.OwnerName == "Vasil" &&
                        account.AccountNumber == "456" &&
                        account.Balance == 500m);
                }
            }
            finally
            {
                DeleteDatabaseFiles(databasePath);
            }
        }

        private static BankDbContext CreateContext(string databasePath)
        {
            DbContextOptions<BankDbContext> options = new DbContextOptionsBuilder<BankDbContext>()
                .UseSqlite($"Data Source={databasePath};Pooling=False")
                .Options;

            return new BankDbContext(options);
        }

        private static string CreateDatabasePath()
        {
            return Path.Combine(
                Path.GetTempPath(),
                $"banking-test-{Guid.NewGuid()}.db");
        }

        private static void DeleteDatabaseFiles(string databasePath)
        {
            SqliteConnection.ClearAllPools();
            
            DeleteFileIfExists(databasePath);
            DeleteFileIfExists(databasePath + "-wal");
            DeleteFileIfExists(databasePath + "-shm");
        }

        private static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}