using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BerserkMode
{
    public class MissionKillLogic : MissionLogic
    {
        private int minHPGain;
        private int maxHPGain;
        private int minHPGainByDefault;
        private int maxHPGainByDefault;

        private bool hpGainDefault;
        private bool reportPlayerHPGain;

        public MissionKillLogic(int minHP, int maxHP, int minHPD, int maxHPD, bool hpDefault, bool report)
        {
            this.minHPGain = minHP;
            this.maxHPGain = maxHP;
            this.minHPGainByDefault = minHPD;
            this.maxHPGainByDefault = maxHPD;

            this.hpGainDefault = hpDefault;
            this.reportPlayerHPGain = report;
        }

        private void doAgentHPGain(Agent a, int min, int max)
        {
            int hpGain = SubModule.rand.Next(min, max);
            a.Health += hpGain;
            if (a.Health > a.HealthLimit)
            {
                a.Health = a.HealthLimit;
            }
            if (this.reportPlayerHPGain && a.IsMainAgent)
            {
                Utils.PrintToMessages("You got " + hpGain + " HP for that kill!", 255, 102, 102);
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            // Check if human and not same team
            bool doHpGain = affectorAgent != null && affectedAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman && !affectedAgent.Team.Equals(affectorAgent.Team);
            
            // Check for berserk mode HP gain
            bool doBerserkHPGain = doHpGain && SubModule.isBerserk && affectorAgent.IsMainAgent;
            if (doBerserkHPGain)
            {
                doAgentHPGain(affectorAgent, this.minHPGain, this.maxHPGain);
                // return gives no more HP to player if Berserk mode HP gained
                return;
            }

            // HP on kill by default HP gain
            bool doHPGainDefault = this.hpGainDefault && doHpGain;
            if (doHPGainDefault)
            {
                doAgentHPGain(affectorAgent, this.minHPGainByDefault, this.maxHPGainByDefault);
            }
        }
    }
}
