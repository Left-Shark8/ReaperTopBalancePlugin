using System;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using ReaperLeaderboardPlugin.Kills;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace ReaperLeaderboardPlugin;

public sealed class Plugin : RocketPlugin<PluginConfiguration>
{
    public static Plugin? Instance { get; private set; }

    private KillStatsRepository? killStatsRepository;

    protected override void Load()
    {
        Instance = this;
        killStatsRepository = new KillStatsRepository(Configuration.Instance);
        killStatsRepository.EnsureTable();

        UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
        ZombieManager.onZombieDead += OnZombieDead;

        Logger.Log($"{Name} loaded.");
    }

    protected override void Unload()
    {
        UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
        ZombieManager.onZombieDead -= OnZombieDead;

        Logger.Log($"{Name} unloaded.");
        killStatsRepository = null;
        Instance = null;
    }

    private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
    {
        if (killStatsRepository == null || murderer == CSteamID.Nil || murderer == player.CSteamID)
        {
            return;
        }

        try
        {
            var killer = UnturnedPlayer.FromCSteamID(murderer);
            killStatsRepository.Increment(murderer.m_SteamID.ToString(), killer.DisplayName, KillCategory.Player);
        }
        catch (Exception exception)
        {
            Logger.LogException(exception, "Failed to record player kill.");
        }
    }

    private void OnZombieDead(Player instigator, Zombie zombie, Vector3 ragdoll, ERagdollEffect ragdollEffect, bool trackKill, ERagdollDestroy ragdollDestroy)
    {
        if (killStatsRepository == null || instigator == null || !trackKill)
        {
            return;
        }

        try
        {
            var playerId = instigator.channel.owner.playerID;
            var steamId = playerId.steamID.m_SteamID.ToString();
            var displayName = string.IsNullOrWhiteSpace(playerId.characterName) ? steamId : playerId.characterName;
            var category = zombie.isMega ? KillCategory.MegaZombie : KillCategory.Zombie;
            killStatsRepository.Increment(steamId, displayName, category);
        }
        catch (Exception exception)
        {
            Logger.LogException(exception, "Failed to record zombie kill.");
        }
    }
}
