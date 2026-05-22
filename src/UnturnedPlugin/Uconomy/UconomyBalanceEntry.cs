namespace ReaperLeaderboardPlugin.Uconomy;

public sealed class UconomyBalanceEntry
{
    public UconomyBalanceEntry(string steamId, string displayName, decimal balance)
    {
        SteamId = steamId;
        DisplayName = displayName;
        Balance = balance;
    }

    public string SteamId { get; }

    public string DisplayName { get; }

    public decimal Balance { get; }
}
