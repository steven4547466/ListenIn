using HarmonyLib;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenIn
{
    public class Plugin
    {
        [PluginConfig]
        public Config Config;

        public static Plugin Singleton { get; private set; }
        public static PluginHandler PluginHandler { get; private set; }

        public static Harmony Harmony { get; private set; }

        [PluginEntryPoint("ListenIn", "1.1.1", "Listen to voice chat happening in your server", "Steven4547466")]
        void LoadPlugin()
        {
            Singleton = this;
            PluginHandler = PluginHandler.Get(this);
            EventManager.RegisterEvents<EventHandlers>(this);

            PluginHandler.SaveConfig(this, nameof(Config));

            Harmony = new Harmony($"listenin-{DateTime.Now.Ticks}");
            Harmony.PatchAll();
        }

        [PluginUnload]
        void UnloadPlugin()
        {
            EventManager.UnregisterEvents<EventHandlers>(this);
            Harmony.UnpatchAll(Harmony.Id);
            Harmony = null;
            Singleton = null;
            PluginHandler = null;
        }
    }
}
