using Rocket.API;

namespace ReaperLeaderboardPlugin;

public sealed class PluginConfiguration : IRocketPluginConfiguration
{
    public string Greeting { get; set; } = "Welcome to the server!";
    public string UconomyHost { get; set; } = "localhost";
    public uint UconomyPort { get; set; } = 3306;
    public string UconomyDatabase { get; set; } = "unturned";
    public string UconomyUsername { get; set; } = "unturned";
    public string UconomyPassword { get; set; } = "password";
    public string UconomyTable { get; set; } = "uconomy";
    public string UconomySteamIdColumn { get; set; } = "steamId";
    public string UconomyBalanceColumn { get; set; } = "balance";
    public string TopBalanceHeader { get; set; } = "Top 3 richest players:";
    public string TopBalanceMoneyName { get; set; } = "$";
    public string KillStatsTable { get; set; } = "reaper_kill_stats";
    public string KillsHeader { get; set; } = "Your kills:";
    public string TopKillsHeader { get; set; } = "Top kill leaders:";

    public void LoadDefaults()
    {
        Greeting = "Welcome to the server!";
        UconomyHost = "localhost";
        UconomyPort = 3306;
        UconomyDatabase = "unturned";
        UconomyUsername = "unturned";
        UconomyPassword = "password";
        UconomyTable = "uconomy";
        UconomySteamIdColumn = "steamId";
        UconomyBalanceColumn = "balance";
        TopBalanceHeader = "Top 3 richest players:";
        TopBalanceMoneyName = "$";
        KillStatsTable = "reaper_kill_stats";
        KillsHeader = "Your kills:";
        TopKillsHeader = "Top kill leaders:";
    }
}
