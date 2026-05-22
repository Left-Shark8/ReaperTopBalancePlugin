using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using UnturnedPlugin.Uconomy;

namespace UnturnedPlugin.Commands;

public sealed class CommandTopBalance : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "topbal";

    public string Help => "Displays the 3 richest Uconomy accounts.";

    public string Syntax => "/topbal";

    public List<string> Aliases => new() { "topbalance" };

    public List<string> Permissions => new() { "unturnedplugin.topbal" };

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
            var repository = new UconomyBalanceRepository(configuration);
            var balances = repository.GetTopBalances(3);

            if (balances.Count == 0)
            {
                UnturnedChat.Say(caller, "No Uconomy balances were found.");
                return;
            }

            UnturnedChat.Say(caller, configuration.TopBalanceHeader);

            for (var i = 0; i < balances.Count; i++)
            {
                var entry = balances[i];
                var rank = i + 1;
                UnturnedChat.Say(caller, $"{rank}. {entry.SteamId}: {configuration.TopBalanceMoneyName}{entry.Balance:N0}");
            }
        }
        catch (Exception exception)
        {
            Logger.LogException(exception, "Failed to load top Uconomy balances.");
            UnturnedChat.Say(caller, "Could not load the top balances. Check the server console.");
        }
    }
}
