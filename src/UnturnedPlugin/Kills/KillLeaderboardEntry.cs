namespace ReaperLeaderboardPlugin.Kills;

public sealed class KillLeaderboardEntry
{
    public KillLeaderboardEntry(string steamId, string displayName, int kills)
    {
        SteamId = steamId;
        DisplayName = displayName;
        Kills = kills;
    }

    public string SteamId { get; }

    public string DisplayName { get; }

    public int Kills { get; }
}
