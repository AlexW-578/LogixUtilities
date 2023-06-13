using FrooxEngine;
using FrooxEngine.LogiX;
using HarmonyLib;
using NeosModLoader;

namespace LogixUtils_v2
{
    public class LogixUtils_v2 : NeosMod
    {
        public override string Name => "LogixUtils-v2";
        public override string Author => "AlexW-578";
        public override string Version => "0.0.1";
        public override string Link => "https://github.com/AlexW-578/LogixUtils-v2/";
        private static ModConfiguration Config;

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Enabled =
            new ModConfigurationKey<bool>("Enabled", "Enable/Disable the Mod", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> WireBeGone =
            new ModConfigurationKey<bool>("Wire Be Gone", "Disable the connecting wire for InterfaceProxies", () => true);

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("co.uk.AlexW-578.LogixUtils-v2");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(LogixInterfaceProxy), "GetInterface")]
        class WireBeGone_Patch
        {
            static void Postfix(LogixInterfaceProxy __instance, SyncRef<Slot> ____interfaceSlot)
            {
                if (Config.GetValue(Enabled) && Config.GetValue(WireBeGone))
                {
                    ____interfaceSlot.Target.FindChild(wire => wire.Name.Equals("Wire"),5).ActiveSelf = false;
                }
            }
        }
    }
}