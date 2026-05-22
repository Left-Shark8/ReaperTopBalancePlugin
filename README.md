# ReaperLeaderboardPlugin

Drag-and-drop RocketMod Unturned plugin for economy and kill leaderboards.

## Install

Copy these into your server plugin folder:

```text
Servers\<YourServer>\Rocket\Plugins\ReaperLeaderboardPlugin\
```

Required files:

```text
ReaperLeaderboardPlugin.dll
Libraries\MySql.Data.dll
```

Restart the server after copying the files.

## Commands

```text
/topbal
/kills
/topkills
```

## Permissions

```text
reaperleaderboard.topbal
reaperleaderboard.kills
reaperleaderboard.topkills
```

## Database

The plugin uses MySQL. It reads balances from your existing Uconomy table:

```text
uconomy
```

Default balance columns:

```text
steamId
balance
```

The plugin also creates its own kill stats table:

```text
reaper_kill_stats
```

That table stores:

```text
steam_id
display_name
player_kills
zombie_kills
mega_zombie_kills
```

`/topbal` shows saved player names when the plugin has seen the player in the kill tracker. Otherwise it falls back to SteamID64.

## Config

After the first server start, Rocket creates the plugin config. Update the MySQL/Uconomy settings there:

```xml
<UconomyHost>localhost</UconomyHost>
<UconomyPort>3306</UconomyPort>
<UconomyDatabase>unturned</UconomyDatabase>
<UconomyUsername>unturned</UconomyUsername>
<UconomyPassword>password</UconomyPassword>
<UconomyTable>uconomy</UconomyTable>
<UconomySteamIdColumn>steamId</UconomySteamIdColumn>
<UconomyBalanceColumn>balance</UconomyBalanceColumn>
<KillStatsTable>reaper_kill_stats</KillStatsTable>
```
