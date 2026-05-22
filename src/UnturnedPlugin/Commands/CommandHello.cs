using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;

namespace UnturnedPlugin.Commands;

public sealed class CommandHello : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "hello";

    public string Help => "Sends the configured greeting.";

    public string Syntax => "/hello";

    public List<string> Aliases => new();

    public List<string> Permissions => new() { "unturnedplugin.hello" };

    public void Execute(IRocketPlayer caller, string[] command)
    {
        var greeting = Plugin.Instance?.Configuration.Instance.Greeting ?? "Hello!";
        UnturnedChat.Say(caller, greeting);
    }
}
