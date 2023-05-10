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
        public static Random Random = new();
        
        protected override void OnSubModuleLoad()
        {
            new Harmony("BerserkMode").PatchAll();
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            mission.AddMissionBehavior(new BerserkMissionLogic());
        }
    }
}