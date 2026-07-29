using BankingApp.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api.Persistence
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) 
            : base(options)
        { 
        }

        public DbSet<BankAccountEntity> Accounts => Set<BankAccountEntity>();

        public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccountEntity>()
                .HasIndex(account => account.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<BankAccountEntity>()
                .HasMany(account => account.Transactions)
                .WithOne(transaction => transaction.Account)
                .HasForeignKey(transaction => transaction.BankAccountEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
