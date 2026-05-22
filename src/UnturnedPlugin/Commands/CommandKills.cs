using System;
using System.Collections.Generic;
using ReaperLeaderboardPlugin.Kills;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;

namespace ReaperLeaderboardPlugin.Commands;

public sealed class CommandKills : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Player;

    public string Name => "kills";

    public string Help => "Shows your player, zombie, and mega zombie kills.";

    public string Syntax => "/kills";

    public List<string> Aliases => new();

    public List<string> Permissions => new() { "reaperleaderboard.kills" };

    public void Execute(IRocketPlayer caller, string[] command)
    {
        var configuration = Plugin.Instance?.Configuration.Instance;
        if (configuration == null)
        {
            UnturnedChat.Say(caller, "Plugin configuration is not loaded.");
            return;
        }

        try
        {
            var repository = new KillStatsRepository(configuration);
            var stats = repository.GetStats(caller.Id);

            UnturnedChat.Say(caller, configuration.KillsHeader);
            UnturnedChat.Say(caller, $"Players: {stats.PlayerKills:N0}");
            UnturnedChat.Say(caller, $"Zombies: {stats.ZombieKills:N0}");
            UnturnedChat.Say(caller, $"Mega Zombies: {stats.MegaZombieKills:N0}");
        }
        catch (Exception exception)
        {
            Logger.LogException(exception, "Failed to load kill stats.");
            UnturnedChat.Say(caller, "Could not load your kills. Check the server console.");
        }
    }
}
