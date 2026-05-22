namespace UnturnedPlugin.Uconomy;

public sealed class UconomyBalanceEntry
{
    public UconomyBalanceEntry(string steamId, decimal balance)
    {
        SteamId = steamId;
        Balance = balance;
    }

    public string SteamId { get; }

    public decimal Balance { get; }
}
