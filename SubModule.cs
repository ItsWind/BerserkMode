using TaleWorlds.MountAndBlade;
using TaleWorlds.InputSystem;
using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using HarmonyLib;

namespace BerserkMode
{
    public class SubModule : MBSubModuleBase
    {
        private static readonly string modPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 26) + "Modules\\BerserkMode";
        private static Config config = new(modPath);

        public static Random rand = new();

        // Config vars
        private int minHPPerKill;
        private int maxHPPerKill;
        private int minHPPerKillByDefault;
        private int maxHPPerKillByDefault;

        private int cooldownInSeconds;
        private int berserkTimeInSeconds;
        public static int berserkDamageMultiplier;
        public static int berserkDamageDivider;

        public static bool alwaysBerserk;
        public static bool breakThroughBlocks;
        private bool hpOnKillByDefault;
        private bool reportPlayerHPGain;
        // Config vars end

        // Berserk mode vars
        public static bool isBerserk = false;
        private float currentBerserkTime = 0.0f;

        private bool cooldownActive = false;
        private float currentCooldownTime = 0.0f;
        
        protected override void OnSubModuleLoad()
        {
            // Setting config values
            this.minHPPerKill = config.GetConfigValues()["minHPPerKill"];
            this.maxHPPerKill = config.GetConfigValues()["maxHPPerKill"];
            this.minHPPerKillByDefault = config.GetConfigValues()["minHPPerKillByDefault"];
            this.maxHPPerKillByDefault = config.GetConfigValues()["maxHPPerKillByDefault"];

            this.cooldownInSeconds = config.GetConfigValues()["cooldownInSeconds"];
            this.berserkTimeInSeconds = config.GetConfigValues()["berserkTimeInSeconds"];
            berserkDamageMultiplier = config.GetConfigValues()["berserkDamageMultiplier"];
            berserkDamageDivider = config.GetConfigValues()["berserkDamageDivider"];

            alwaysBerserk = Convert.ToBoolean(config.GetConfigValues()["alwaysBerserk"]);
            breakThroughBlocks = Convert.ToBoolean(config.GetConfigValues()["breakThroughBlocks"]);
            this.hpOnKillByDefault = Convert.ToBoolean(config.GetConfigValues()["hpOnKillByDefault"]);
            this.reportPlayerHPGain = Convert.ToBoolean(config.GetConfigValues()["reportPlayerHPGain"]);

            if (this.minHPPerKill > this.maxHPPerKill)
            {
                this.minHPPerKill = this.maxHPPerKill;
            }

            new Harmony("BerserkMode").PatchAll();
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            mission.AddMissionBehavior(new MissionKillLogic(this.minHPPerKill, this.maxHPPerKill, this.minHPPerKillByDefault, this.maxHPPerKillByDefault,
                this.hpOnKillByDefault, this.reportPlayerHPGain));
        }

        protected override void OnApplicationTick(float dt)
        {
            // Get if in scene/battle
            bool isInBattle = Game.Current != null && Mission.Current != null && Mission.Current.Scene != null && Agent.Main != null && Agent.Main.IsActive();
            
            if (isInBattle)
            {
                // Set always berserk config
                if (alwaysBerserk)
                {
                    isBerserk = true;
                    return;
                }

                // Berserk mode tick timer
                if (isBerserk)
                {
                    this.currentBerserkTime += dt;
                    if (this.currentBerserkTime >= this.berserkTimeInSeconds)
                    {
                        // Turn off berserk and set cooldown
                        isBerserk = false;
                        this.currentBerserkTime = 0.0f;
                        this.cooldownActive = true;
                        Utils.PrintToMessages("You tire from berserking...", 255, 255, 102);
                    }
                }
                // Cooldown tick timer
                else if (this.cooldownActive)
                {
                    this.currentCooldownTime += dt;
                    if (this.currentCooldownTime >= this.cooldownInSeconds)
                    {
                        // Turn off cooldown
                        this.cooldownActive = false;
                        this.currentCooldownTime = 0.0f;
                        Utils.PrintToMessages("You feel as if you could go berserk once again!", 255, 51, 51);
                    }
                }

                if (Input.IsKeyPressed(InputKey.M))
                {
                    if (!isBerserk && !this.cooldownActive)
                    {
                        isBerserk = true;
                        Utils.PrintToMessages("You go berserk!", 255, 0, 0);
                    }
                    else if (this.cooldownActive)
                    {
                        Utils.PrintToMessages("You cannot go berserk yet!", 204, 204, 0);
                    }
                    else if (isBerserk)
                    {
                        Utils.PrintToMessages("You are already in berserk mode!", 204, 0, 0);
                    }
                }
            }
            else
            {
                if (isBerserk || this.cooldownActive)
                {
                    isBerserk = false;
                    this.currentBerserkTime = 0.0f;
                    this.cooldownActive = false;
                    this.currentCooldownTime = 0.0f;
                }
            }
        }
    }
}