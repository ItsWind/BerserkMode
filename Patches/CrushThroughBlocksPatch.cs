using HarmonyLib;
using SandBox.GameComponents;
using TaleWorlds.MountAndBlade;

namespace BerserkMode.Patches
{
    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), nameof(SandboxAgentApplyDamageModel.DecideCrushedThrough))]
    public class CrushThroughBlocksPatch
    {
        private static Agent attacker;

        [HarmonyPrefix]
        public static void Prefix(Agent attackerAgent)
        {
            if (SubModule.breakThroughBlocks)
            {
                attacker = attackerAgent;
            }
        }

        [HarmonyPostfix]
        public static bool Postfix(bool original)
        {
            if (SubModule.breakThroughBlocks)
            {
                if (attacker.IsPlayerControlled && SubModule.isBerserk)
                {
                    return true;
                }
            }
            return original;
        }
    }
}