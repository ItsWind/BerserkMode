using HarmonyLib;
using SandBox.GameComponents;
using TaleWorlds.MountAndBlade;

namespace BerserkMode.Patches
{
    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), nameof(SandboxAgentApplyDamageModel.CalculateDamage))]
    public class BerserkDamageMultiplierPatch
    {
        private static AttackInformation attackInfo;

        [HarmonyPrefix]
        public static void Prefix(ref AttackInformation attackInformation)
        {
            attackInfo = attackInformation;
        }

        [HarmonyPostfix]
        public static float Postfix(float original)
        {
            if (SubModule.isBerserk)
            {
                if (attackInfo.IsAttackerPlayer)
                {
                    float newDamage = original * ((float)SubModule.berserkDamageMultiplier / 10.0f);
                    return newDamage;
                }
                else if (attackInfo.IsVictimPlayer)
                {
                    float newDamage = original / ((float)SubModule.berserkDamageDivider / 10.0f);
                    return newDamage;
                }
            }
            return original;
        }
    }
}