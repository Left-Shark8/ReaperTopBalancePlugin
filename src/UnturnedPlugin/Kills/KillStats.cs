namespace ReaperLeaderboardPlugin.Kills;

public sealed class KillStats
{
    public KillStats(string steamId, int playerKills, int zombieKills, int megaZombieKills)
    {
        SteamId = steamId;
        PlayerKills = playerKills;
        ZombieKills = zombieKills;
        MegaZombieKills = megaZombieKills;
    }

    public string SteamId { get; }

    public int PlayerKills { get; }

    public int ZombieKills { get; }

    public int MegaZombieKills { get; }
}
