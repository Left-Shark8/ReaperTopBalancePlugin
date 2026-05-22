# ReaperLeaderboardPlugin

Simple RocketMod-style Unturned plugin that displays economy and kill leaderboards.

## What is included

- `Plugin.cs`: plugin load/unload entry point.
- `PluginConfiguration.cs`: generated RocketMod configuration.
- `CommandTopBalance.cs`: `/topbal` command that displays the 3 richest Uconomy accounts.
- `CommandKills.cs`: `/kills` command that displays your player, zombie, and mega zombie kills.
- `CommandTopKills.cs`: `/topkills` command that displays the top 3 players for each kill category.
- `Libs/`: place your Unturned, RocketMod, and MySQL reference DLLs here.

## Required references

Copy these DLLs from your Unturned server/RocketMod install into `Libs/`:

- `Rocket.API.dll`
- `Rocket.Core.dll`
- `Rocket.Unturned.dll`
- `UnityEngine.dll`
- `Assembly-CSharp.dll`
- `com.rlabrecque.steamworks.net.dll`
- `MySql.Data.dll`

Depending on your server build and plugin features, you may also need references such as
`Assembly-CSharp.dll`, `UnityEngine.CoreModule.dll`, or `SDG.NetTransport.dll`.

## Uconomy setup

The `/topbal` command assumes the common Uconomy MySQL schema:

- Table: `uconomy`
- Steam ID column: `steamId`
- Balance column: `balance`

These values are configurable in the Rocket-generated plugin config:

```xml
<UconomyHost>localhost</UconomyHost>
<UconomyPort>3306</UconomyPort>
<UconomyDatabase>unturned</UconomyDatabase>
<UconomyUsername>unturned</UconomyUsername>
<UconomyPassword>password</UconomyPassword>
<UconomyTable>uconomy</UconomyTable>
<UconomySteamIdColumn>steamId</UconomySteamIdColumn>
<UconomyBalanceColumn>balance</UconomyBalanceColumn>
```

Grant the permission `reaperleaderboard.topbal` to players who should use `/topbal`.

## Kill leaderboard setup

The plugin creates this table automatically in the same MySQL database:

```xml
<KillStatsTable>reaper_kill_stats</KillStatsTable>
```

Commands:

- `/kills`: shows your player, zombie, and mega zombie kills.
- `/topkills`: shows the top 3 players in each kill category.

Permissions:

- `reaperleaderboard.kills`
- `reaperleaderboard.topkills`

## Build

```powershell
dotnet build .\ReaperLeaderboardPlugin.sln -c Release
```

The compiled plugin DLL will be at:

```text
src\UnturnedPlugin\bin\Release\ReaperLeaderboardPlugin.dll
```

## Install

1. Create this folder on your server:

   ```text
   Servers\<YourServer>\Rocket\Plugins\ReaperLeaderboardPlugin\
   ```

2. Copy `ReaperLeaderboardPlugin.dll` into that folder.
3. Start the server once so RocketMod creates the configuration file.
