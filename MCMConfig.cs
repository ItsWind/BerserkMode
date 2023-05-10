using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.InputSystem;

namespace BerserkMode {
	internal sealed class MCMConfig : AttributeGlobalSettings<MCMConfig> {
		public override string Id => "BerserkMode";
		public override string DisplayName => "Berserk Mode";
		public override string FolderName => "BerserkMode";
		public override string FormatType => "xml";

		public InputKey GetBerserkKey() {
			InputKey key;
			try {
				string toUse = BerserkKey;
				toUse = toUse.Length == 1 ? toUse.ToUpper() : toUse;
				key = (InputKey)Enum.Parse(typeof(InputKey), toUse);
			} catch (Exception) { return InputKey.M; }
			return key;
		}

		[SettingPropertyText("Berserk Trigger Key", HintText = "Key to press to trigger berserk mode. If this value is not set correctly, it will default to M.", Order = 1, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public string BerserkKey { get; set; } = "M";

		[SettingPropertyFloatingInteger("Berserk Trigger Key Hold In Seconds", 0f, 3f, HintText = "The amount of seconds the key needs to be held in order for the key hold function to trigger.", Order = 2, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public float BerserkKeyHoldInSeconds { get; set; } = 1f;

		[SettingPropertyBool("Always Berserk", HintText = "Enable this to always be in berserk mode.", Order = 3, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public bool AlwaysBerserk { get; set; } = false;

		[SettingPropertyBool("Crush Through Blocks In Berserk Mode", HintText = "Enable to crush through blocks while berserking.", Order = 4, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public bool CrushThroughBlocksInBerserkMode { get; set; } = true;

		[SettingPropertyBool("Yell While In Berserk Mode", HintText = "Enable to have your player constantly yell while in berserk mode.", Order = 5, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public bool YellInBerserkMode { get; set; } = true;

		// RAGE

		[SettingPropertyBool("Report Player Rage Gain", HintText = "Enable to report any rage gain the player gets.", Order = 1, RequireRestart = false)]
		[SettingPropertyGroup("Rage")]
		public bool ReportPlayerRageGain { get; set; } = false;

		[SettingPropertyFloatingInteger("Percent Damage Taken Generated As Rage", 0f, 3f, HintText = "The amount of damage you take will be multiplied by this number and generated into rage while not in berserk mode.", Order = 2, RequireRestart = false)]
		[SettingPropertyGroup("Rage")]
		public float PercentDamageTakenAsRage { get; set; } = 1f;

		[SettingPropertyFloatingInteger("Percent Damage Dealt Generated As Rage", 0f, 3f, HintText = "The amount of damage you deal will be multiplied by this number and generated into rage while not in berserk mode.", Order = 3, RequireRestart = false)]
		[SettingPropertyGroup("Rage")]
		public float PercentDamageDealtAsRage { get; set; } = 0.25f;

		[SettingPropertyFloatingInteger("Berserk Mode Rage Needed To Trigger", 0f, 300f, HintText = "Set the minimum amount of rage needed to trigger berserk mode. 1 rage = 1 second in berserk mode on trigger.", Order = 4, RequireRestart = false)]
		[SettingPropertyGroup("Rage")]
		public float RageNeededForBerserkTrigger { get; set; } = 60f;

		[SettingPropertyFloatingInteger("Rage Lost Per Second", 0f, 5f, HintText = "Set the amount of rage lost per second.", Order = 5, RequireRestart = false)]
		[SettingPropertyGroup("Rage")]
		public float RageLostPerSecond { get; set; } = 0.1f;

		// DAMAGE CHANGES

		[SettingPropertyFloatingInteger("Berserk Mode Damage Multiplier", 0f, 5f, HintText = "The amount of damage you do will be multiplied by this number while in berserk mode.", Order = 1, RequireRestart = false)]
		[SettingPropertyGroup("Damage Changes")]
		public float BerserkDamageMultiplier { get; set; } = 2.5f;

		[SettingPropertyFloatingInteger("Berserk Mode Resistance Multiplier", 0f, 1f, HintText = "The amount of damage enemies will do to you will be multiplied by this number while in berserk mode.", Order = 2, RequireRestart = false)]
		[SettingPropertyGroup("Damage Changes")]
		public float BerserkResistanceMultiplier { get; set; } = 0.4f;

		// HP CHANGES

		[SettingPropertyBool("HP On Kill Always On", HintText = "Enables HP gain on kill for ALL troops ALWAYS. Otherwise, only for the player while berserking.", Order = 1, RequireRestart = false)]
		[SettingPropertyGroup("HP Changes")]
		public bool HPOnKillAlwaysOn { get; set; } = false;

		[SettingPropertyBool("Report Player HP Gain", HintText = "Enable to report any HP gain the player gets.", Order = 2, RequireRestart = false)]
		[SettingPropertyGroup("HP Changes")]
		public bool ReportPlayerHPGain { get; set; } = false;

		[SettingPropertyInteger("HP Gain", 0, 100, HintText = "Set the hp you will gain from kills.", Order = 3, RequireRestart = false)]
		[SettingPropertyGroup("HP Changes")]
		public int HPOnKill { get; set; } = 25;

		[SettingPropertyInteger("HP Gain Variance", 0, 100, HintText = "Set the minimum and maximum.", Order = 4, RequireRestart = false)]
		[SettingPropertyGroup("HP Changes")]
		public int HPOnKillVariance { get; set; } = 7;
	}
}
