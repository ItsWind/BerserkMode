using HarmonyLib;
using MCM.Abstractions.Base.Global;
using TaleWorlds.MountAndBlade;
using SandBox.GameComponents;

namespace BerserkMode.Patches
{
    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), nameof(SandboxAgentApplyDamageModel.CalculateDamage))]
    internal class BerserkDamageMultiplierPatchSandbox {
        [HarmonyPostfix]
        private static void Postfix(ref float __result, ref AttackInformation attackInformation)
        {
            if (BerserkMissionLogic.Instance.IsBerserking)
            {
                if (attackInformation.IsAttackerPlayer)
                {
                    __result = __result * GlobalSettings<MCMConfig>.Instance.BerserkDamageMultiplier;
                }
                else if (attackInformation.IsVictimPlayer)
                {
                    __result = __result * GlobalSettings<MCMConfig>.Instance.BerserkResistanceMultiplier;
                }
            }
        }
    }
    [HarmonyPatch(typeof(CustomAgentApplyDamageModel), nameof(CustomAgentApplyDamageModel.CalculateDamage))]
    internal class BerserkDamageMultiplierPatchCustomBattle {
        [HarmonyPostfix]
        private static void Postfix(ref float __result, ref AttackInformation attackInformation) {
            if (BerserkMissionLogic.Instance.IsBerserking) {
                if (attackInformation.IsAttackerPlayer) {
                    __result = __result * GlobalSettings<MCMConfig>.Instance.BerserkDamageMultiplier;
                } else if (attackInformation.IsVictimPlayer) {
                    __result = __result * GlobalSettings<MCMConfig>.Instance.BerserkResistanceMultiplier;
                }
            }
        }
    }
}