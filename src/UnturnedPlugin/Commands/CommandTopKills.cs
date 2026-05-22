using System;
using System.Collections.Generic;
using ReaperLeaderboardPlugin.Kills;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;

namespace ReaperLeaderboardPlugin.Commands;

public sealed class CommandTopKills : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "topkills";

    public string Help => "Shows the top 3 players for player, zombie, and mega zombie kills.";

    public string Syntax => "/topkills";

    public List<string> Aliases => new();

    public List<string> Permissions => new() { "reaperleaderboard.topkills" };

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

            UnturnedChat.Say(caller, configuration.TopKillsHeader);
            SendTop(caller, repository, "Player Kills", KillCategory.Player);
            SendTop(caller, repository, "Zombie Kills", KillCategory.Zombie);
            SendTop(caller, repository, "Mega Zombie Kills", KillCategory.MegaZombie);
        }
        catch (Exception exception)
        {
            Logger.LogException(exception, "Failed to load top kill stats.");
            UnturnedChat.Say(caller, "Could not load the top kills. Check the server console.");
        }
    }

    private static void SendTop(IRocketPlayer caller, KillStatsRepository repository, string label, KillCategory category)
    {
        var entries = repository.GetTop(category, 3);
        UnturnedChat.Say(caller, label + ":");

        if (entries.Count == 0)
        {
            UnturnedChat.Say(caller, "No kills recorded yet.");
            return;
        }

        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            UnturnedChat.Say(caller, $"{i + 1}. {entry.DisplayName}: {entry.Kills:N0}");
        }
    }
}
