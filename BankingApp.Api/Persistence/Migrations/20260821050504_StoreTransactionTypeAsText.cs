using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StoreTransactionTypeAsText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Transactions"
                ALTER COLUMN "Type" TYPE text
                USING CASE "Type"
                WHEN 0 THEN 'Deposit'
                WHEN 1 THEN 'Withdraw'
                WHEN 2 THEN 'TransferOut'
                WHEN 3 THEN 'TransferIn'
                ELSE "Type"::text
             END;
             """);
                
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                 ALTER TABLE "Transactions"
                 ALTER COLUMN "Type" TYPE integer
                 USING CASE "Type"
                 WHEN 'Deposit' THEN 0
                 WHEN 'Withdraw' THEN 1
                 WHEN 'TransferOut' THEN 2
                 WHEN 'TransferIn' THEN 3
                 ELSE "Type"::integer
             END;
             """);
        }
    }
}
