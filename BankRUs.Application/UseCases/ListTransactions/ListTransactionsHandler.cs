using BankRUs.Application.Repositories;

namespace BankRUs.Application.UseCases.ListTransactions;

public class ListTransactionsHandler
{
    private readonly IBankAccountRepository _repo;

    public ListTransactionsHandler(IBankAccountRepository repo)
    {
        _repo = repo;
    }

    public async Task<ListTransactionsResult?> HandleAsync(ListTransactionsQuery query)
    {
        // 1) Hämta konto
        var account = await _repo.GetByIdAsync(query.BankAccountId);
        if (account is null)
            return null;

        // 2) Ägarskap
        if (account.UserId != query.UserId)
            return null;

        // 3) Paging-validering
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 20 : query.PageSize;
        var sort = query.Sort?.ToLower() == "asc" ? "asc" : "desc";


        // 4) Hämta total + sida
        var totalCount = await _repo.CountTransactionsAsync(account.Id);
        var transactions = await _repo.ListTransactionsAsync(account.Id, page, pageSize, sort);

        // 5) Mappa
        var items = transactions.Select(t => new ListTransactionItem(
            TransactionId: t.Id,
            Type: t.Type.ToString(),
            Amount: t.Amount,
            Reference: t.Reference,
            CreatedAt: t.CreatedAt,
            BalanceAfter: t.BalanceAfter
        )).ToList();

        // 6) Returnera
        return new ListTransactionsResult(
            AccountId: account.Id,
            Balance: account.Balance,
            Page: page,
            PageSize: pageSize,
            TotalCount: totalCount,
            Items: items
        );
    }
}
