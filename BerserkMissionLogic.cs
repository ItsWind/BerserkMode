using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace BerserkMode {
    public class BerserkMissionLogic : MissionLogic {
        public static BerserkMissionLogic Instance;

        public bool IsBerserking {
            get {
                return berserkActiveFor > 0f;
            }
            set {
                if (value == true) {
                    berserkActiveFor = currentRage;
                    currentRage = 0f;
                    Utils.PrintToMessages("You feel invigorated by rage!", 255, 0, 0);
                }
                else {
                    berserkActiveFor = 0f;
                    Utils.PrintToMessages("You feel exhausted..", 135, 135, 0);
                }
            }
        }

        private float berserkActiveFor = 0f;
        private float currentRage = 0f;
        private float keyDownFor = 0f;
        private float rageYellsCooldown = 0f;

        public BerserkMissionLogic() {
            Instance = this;
        }

        public override void OnMissionTick(float dt) {
            if (Agent.Main == null)
                return;

            if (IsBerserking) {
                DoRageYells(dt);

                berserkActiveFor -= dt;
                if (berserkActiveFor <= 0f)
                    IsBerserking = false;
            }
            else if (currentRage > 0f) {
                float rageToLose = GlobalSettings<MCMConfig>.Instance.RageLostPerSecond * dt;
                ModifyRage(-rageToLose);
            }

            // KEY HOLD
            if (Input.IsKeyDown(GlobalSettings<MCMConfig>.Instance.GetBerserkKey()) && keyDownFor >= 0f) {
                keyDownFor += dt;
                if (keyDownFor >= GlobalSettings<MCMConfig>.Instance.BerserkKeyHoldInSeconds) {
                    BerserkKeyHeld();
                    keyDownFor = -1.0f;
                }
            }
            // KEY NOT HELD DOWN
            else if (!Input.IsKeyDown(GlobalSettings<MCMConfig>.Instance.GetBerserkKey())) {
                // KEY PRESSED
                if (keyDownFor > 0f) {
                    BerserkKeyPressed();
                    keyDownFor = 0f;
                }
                // KEY HOLD TRIGGERED AND NOT HELD ANYMORE
                else if (keyDownFor < 0f) {
                    keyDownFor = 0f;
                }
            }
        }

        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData) {
            if (!affectedAgent.IsHuman || !affectorAgent.IsHuman || affectedAgent.Team.Side == affectorAgent.Team.Side)
                return;
            
            if (IsBerserking)
                return;

            float gain = 0f;
            // PLAYER TAKING DAMAGE
            if (affectedAgent.IsMainAgent)
                gain = (float)attackCollisionData.InflictedDamage * GlobalSettings<MCMConfig>.Instance.PercentDamageTakenAsRage;
            // PLAYER DEALING DAMAGE
            else if (affectorAgent.IsMainAgent)
                gain = (float)attackCollisionData.InflictedDamage * GlobalSettings<MCMConfig>.Instance.PercentDamageDealtAsRage;

            if (gain > 0f) {
                if (attackCollisionData.AttackBlockedWithShield)
                    gain /= 2f;
                ModifyRage(gain);
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow) {
            // Check if human and not same team
            bool doHpGain = affectorAgent != null && affectedAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman && !affectedAgent.Team.Equals(affectorAgent.Team);

            if (!doHpGain)
                return;

            // Check for berserk mode HP gain
            if (IsBerserking && affectorAgent.IsMainAgent) {
                DoAgentHPOnKill(affectorAgent);
                // return gives no more HP to player if Berserk mode HP gained
                return;
            }

            // HP on kill by default HP gain
            if (GlobalSettings<MCMConfig>.Instance.HPOnKillAlwaysOn)
                DoAgentHPOnKill(affectorAgent);
        }

        private void DoRageYells(float dt) {
            if (!GlobalSettings<MCMConfig>.Instance.YellInBerserkMode)
                return;

            if (rageYellsCooldown > 0f) {
                rageYellsCooldown -= dt;
            } else {
                Agent.Main.MakeVoice(SkinVoiceManager.VoiceType.Yell, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
                rageYellsCooldown = Utils.GetRandomFloat(1.5f, 3f);
            }
        }

        private void ModifyRage(float mod) {
            currentRage += mod;
            if (currentRage < 0f)
                currentRage = 0f;

            if (GlobalSettings<MCMConfig>.Instance.ReportPlayerRageGain && mod > 0f) {
                Utils.PrintToMessages("+{RAGE_GAIN} {RAGE_DISPLAY}", 205, 0, 0,
                    ("RAGE_GAIN", mod.ToString()), ("RAGE_DISPLAY", GetRageDisplayString()));
            }
        }

        private void BerserkKeyHeld() {
            if (IsBerserking)
                Utils.PrintToMessages("You are already in berserk mode!", 255, 0, 0);
            else if (currentRage >= GlobalSettings<MCMConfig>.Instance.RageNeededForBerserkTrigger)
                IsBerserking = true;
            else
                Utils.PrintToMessages("You don't have enough rage. {RAGE_DISPLAY}", 155, 0, 0,
                    ("RAGE_DISPLAY", GetRageDisplayString()));
        }

        private void BerserkKeyPressed() {
            if (IsBerserking)
                Utils.PrintToMessages("Berserking for {BERSERK_SECONDS_LEFT} more seconds!", 255, 0, 0,
                    ("BERSERK_SECONDS_LEFT", Math.Round(berserkActiveFor).ToString()));
            else
                Utils.PrintToMessages("RAGE - {RAGE_DISPLAY}", 205, 0, 0,
                    ("RAGE_DISPLAY", GetRageDisplayString()));
        }

        private string GetRageDisplayString() {
            return "(" + currentRage + "/" + GlobalSettings<MCMConfig>.Instance.RageNeededForBerserkTrigger + ")";
        }

        private void DoAgentHPOnKill(Agent a) {
            float min = GlobalSettings<MCMConfig>.Instance.HPOnKill - GlobalSettings<MCMConfig>.Instance.HPOnKillVariance;
            float max = GlobalSettings<MCMConfig>.Instance.HPOnKill + GlobalSettings<MCMConfig>.Instance.HPOnKillVariance;
            float hpGain = Utils.GetRandomFloat(min, max);

            if (hpGain <= 0)
                return;

            a.Health += hpGain;

            if (a.Health > a.HealthLimit)
                a.Health = a.HealthLimit;

            if (a.IsMainAgent && GlobalSettings<MCMConfig>.Instance.ReportPlayerHPGain)
                Utils.PrintToMessages("+{HP_GAIN} HP gained", 255, 102, 102,
                    ("HP_GAIN", hpGain.ToString()));
        }
    }
}
