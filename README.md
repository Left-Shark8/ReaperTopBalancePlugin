# ReaperLeaderboardPlugin

RocketMod Unturned plugin for economy and kill leaderboards.

## Drag And Drop Install

Drop the folders from this repo into your server's Rocket folder so the final layout looks like this:

```text
Servers\<YourServer>\Rocket\
  Plugins\
    ReaperLeaderboardPlugin.dll
  Libraries\
    MySql.Data.dll
    BouncyCastle.Crypto.dll
    Google.Protobuf.dll
    K4os.Compression.LZ4.dll
    K4os.Compression.LZ4.Streams.dll
    K4os.Hash.xxHash.dll
    Renci.SshNet.dll
    System.Buffers.dll
    System.Memory.dll
    System.Numerics.Vectors.dll
    System.Runtime.CompilerServices.Unsafe.dll
    Ubiety.Dns.Core.dll
    ZstdNet.dll
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

The plugin creates its own kill stats table:

```text
reaper_kill_stats
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
