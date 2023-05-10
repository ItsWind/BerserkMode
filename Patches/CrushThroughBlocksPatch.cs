using HarmonyLib;
using MCM.Abstractions.Base.Global;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using SandBox.GameComponents;

namespace BerserkMode.Patches
{
    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), nameof(SandboxAgentApplyDamageModel.DecideCrushedThrough))]
    internal class CrushThroughBlocksPatchSandbox {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, Agent attackerAgent)
        {
            if (attackerAgent.IsPlayerControlled && BerserkMissionLogic.Instance.IsBerserking && GlobalSettings<MCMConfig>.Instance.CrushThroughBlocksInBerserkMode)
                __result = true;
        }
    }

    [HarmonyPatch(typeof(CustomAgentApplyDamageModel), nameof(CustomAgentApplyDamageModel.DecideCrushedThrough))]
    internal class CrushThroughBlocksPatchCustomBattle {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, Agent attackerAgent) {
            if (attackerAgent.IsPlayerControlled && BerserkMissionLogic.Instance.IsBerserking && GlobalSettings<MCMConfig>.Instance.CrushThroughBlocksInBerserkMode)
                __result = true;
        }
    }
}