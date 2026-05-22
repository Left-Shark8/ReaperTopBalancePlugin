# ReaperTopBalancePlugin

Simple RocketMod-style Unturned plugin that displays the top 3 richest players from a Uconomy database.

## What is included

- `Plugin.cs`: plugin load/unload entry point.
- `PluginConfiguration.cs`: generated RocketMod configuration.
- `CommandTopBalance.cs`: `/topbal` command that displays the 3 richest Uconomy accounts.
- `CommandHello.cs`: starter `/hello` command.
- `Libs/`: place your Unturned, RocketMod, and MySQL reference DLLs here.

## Required references

Copy these DLLs from your Unturned server/RocketMod install into `Libs/`:

- `Rocket.API.dll`
- `Rocket.Core.dll`
- `Rocket.Unturned.dll`
- `UnityEngine.dll`
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

Grant the permission `unturnedplugin.topbal` to players who should use `/topbal`.

## Build

```powershell
dotnet build .\UnturnedPlugin.sln -c Release
```

The compiled plugin DLL will be at:

```text
src\UnturnedPlugin\bin\Release\UnturnedPlugin.dll
```

## Install

1. Create this folder on your server:

   ```text
   Servers\<YourServer>\Rocket\Plugins\UnturnedPlugin\
   ```

2. Copy `UnturnedPlugin.dll` into that folder.
3. Start the server once so RocketMod creates the configuration file.
