using FrooxEngine;
using FrooxEngine.LogiX;
using HarmonyLib;
using NeosModLoader;

namespace LogixUtilities
{
    public class LogixUtilities : NeosMod
    {
        public override string Name => "LogiX Utilities";
        public override string Author => "AlexW-578";
        public override string Version => "0.0.1";
        public override string Link => "https://github.com/AlexW-578/LogixUtilities/";
        private static ModConfiguration Config;

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> Enabled =
            new ModConfigurationKey<bool>("Enabled", "Enable/Disable the Mod", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> WireBeGone =
            new ModConfigurationKey<bool>("Wire Be Gone", "Disable the connecting wire for InterfaceProxies", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> VrLogixRotate =
            new ModConfigurationKey<bool>("VR LogiX Rotate", "Fixes the Rotation of spawned Logix nodes in VR", () => true);
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
        
        [HarmonyPatch(typeof(LogixTip), "PositionSpawnedNode")]
        class VRLogixRotate_Patch
        {
            static void Postfix(LogixTip __instance, Slot node)
            {
                if (Config.GetValue(Enabled) && __instance.InputInterface.VR_Active && Config.GetValue(VrLogixRotate))
                {
                    node.Up = __instance.Slot.ActiveUserRoot.Slot.Up;
                }
            }
        }
        
    }
}