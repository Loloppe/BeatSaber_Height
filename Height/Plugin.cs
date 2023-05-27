using BeatSaberMarkupLanguage.GameplaySetup;
using HarmonyLib;
using Height.Configuration;
using IPA;
using IPA.Config.Stores;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace Height
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static Harmony harmony { get; private set; }

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config conf)
        {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            harmony = new Harmony("Loloppe.BeatSaber.Height");
        }

        [OnEnable]
        public void OnEnable()
        {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            GameplaySetup.instance.AddTab("Height", "Height.Views.settings.bsml", PluginConfig.Instance, MenuType.All);
        }

        [OnDisable]
        public void OnDisable()
        {
            harmony.UnpatchSelf();
            GameplaySetup.instance.RemoveTab("Height");
        }
    }
}
