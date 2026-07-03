using BankingApp.Core;

namespace BankingApp.Api.Persistence
{
    public interface IBankStorage
    {
        List<BankAccount> LoadAccounts();

        void SaveAccounts(IReadOnlyList<BankAccount> accounts);
    }
}
