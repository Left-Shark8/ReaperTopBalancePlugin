using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace UnturnedPlugin;

public sealed class Plugin : RocketPlugin<PluginConfiguration>
{
    public static Plugin? Instance { get; private set; }

    protected override void Load()
    {
        Instance = this;
        Logger.Log($"{Name} loaded. Greeting: {Configuration.Instance.Greeting}");
    }

    protected override void Unload()
    {
        Logger.Log($"{Name} unloaded.");
        Instance = null;
    }
}
