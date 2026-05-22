using System;
using System.Collections.Generic;
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
    private readonly Dictionary<Zombie, Player> zombieInstigators = new();

    protected override void Load()
    {
        Instance = this;
        killStatsRepository = new KillStatsRepository(Configuration.Instance);
        killStatsRepository.EnsureTable();

        UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
        DamageTool.damageZombieRequested += OnDamageZombieRequested;
        DamageTool.zombieDamaged += OnZombieDamaged;

        Rocket.Core.Logging.Logger.Log($"{Name} loaded.");
    }

    protected override void Unload()
    {
        UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
        DamageTool.damageZombieRequested -= OnDamageZombieRequested;
        DamageTool.zombieDamaged -= OnZombieDamaged;

        Rocket.Core.Logging.Logger.Log($"{Name} unloaded.");
        zombieInstigators.Clear();
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
            Rocket.Core.Logging.Logger.LogException(exception, "Failed to record player kill.");
        }
    }

    private void OnDamageZombieRequested(ref DamageZombieParameters parameters, ref bool shouldAllow)
    {
        if (!shouldAllow || parameters.zombie == null || parameters.instigator is not Player player)
        {
            return;
        }

        zombieInstigators[parameters.zombie] = player;
    }

    private void OnZombieDamaged(Zombie zombie, ref Vector3 direction, ref float damage, ref float times, ref bool canRepair)
    {
        if (killStatsRepository == null || zombie == null || !zombie.isDead)
        {
            return;
        }

        if (!zombieInstigators.TryGetValue(zombie, out var instigator))
        {
            return;
        }

        zombieInstigators.Remove(zombie);

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
            Rocket.Core.Logging.Logger.LogException(exception, "Failed to record zombie kill.");
        }
    }
}
