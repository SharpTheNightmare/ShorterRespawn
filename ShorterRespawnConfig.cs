using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Terraria.ModLoader.Config;

namespace ShorterRespawn
{
	public class ShorterRespawnConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		// Vanilla RespawnTime
		public const int RegularRespawnTimer = 600;

		[Header("Presets")]
		[JsonIgnore]
		[Label("[i:889] Shorter Respawn Preset")]
		[Tooltip("This preset reduces only Expert Penalty and is the default.")]
		public bool ShorterRespawnPreset
		{
			get => GlobalRespawnScale == 1f && ExpertPenaltyScale == 1.125f && BossPenaltyScale == 2f;
			set
			{
				if (value)
				{
					GlobalRespawnScale = 1f;
					ExpertPenaltyScale = 1.125f;
					BossPenaltyScale = 2f;
				}
			}
		}

		[JsonIgnore]
		[Label("[i:3099] Terraria Defaults Preset")]
		[Tooltip("This preset restores the default Terraria behavior.")]
		public bool TerrariaDefaultsPreset
		{
			get => GlobalRespawnScale == 1f && ExpertPenaltyScale == 1.5f && BossPenaltyScale == 2f;
			set
			{
				if (value)
				{
					GlobalRespawnScale = 1f;
					ExpertPenaltyScale = 1.5f;
					BossPenaltyScale = 2f;
				}
			}
		}

		[Header("Configurable Respawn Scales")]
		[DefaultValue(1f)]
		[Range(0f, 3f)]
		[Label("[i:1175] Global Respawn Scale")]
		public float GlobalRespawnScale { get; set; }

		[DefaultValue(1.125f)]
		[Range(1f, 3f)]
		[Label("[i:3233] Expert Mode Penalty Scale")]
		[Tooltip("Extra respawn time penalty while in Expert Mode. Default Terraria value is 1.5.")]
		public float ExpertPenaltyScale { get; set; }

		[DefaultValue(2f)]
		[Range(1f, 3f)]
		[Label("[i:43] Boss Penalty Scale")]
		[Tooltip("Extra respawn time penalty for deaths during boss fights. Default Terraria value is 2.")]
		public float BossPenaltyScale { get; set; }

		// The 3 fields above are used for the 4 properties below to convey the effect of the players choices.

		[Header("Resulting Respawn Times")]
		[JsonIgnore]
		[Range(0f, 60f)]
		[Label("Normal Respawn Time in Seconds")]
		[Tooltip("Default Terraria time is 10 seconds")]
		public float NormalRespawn => GlobalRespawnScale * RegularRespawnTimer / 60;

		[JsonIgnore]
		[Range(0f, 60f)]
		[Label("Normal Boss Respawn Time in Seconds")]
		[Tooltip("Default Terraria time is 20 seconds")]
		public float NormalBossRespawn => GlobalRespawnScale * RegularRespawnTimer * BossPenaltyScale / 60;

		[JsonIgnore]
		[Range(0f, 60f)]
		[Label("Expert Respawn Time in Seconds")]
		[Tooltip("Default Terraria time is 15 seconds")]
		public float ExpertRespawn => GlobalRespawnScale * RegularRespawnTimer * ExpertPenaltyScale / 60;

		[JsonIgnore]
		[Range(0f, 60f)]
		[Label("Expert Boss Respawn Time in Seconds")]
		[Tooltip("Default Terraria time is 30 seconds")]
		public float ExpertBossRespawn => GlobalRespawnScale * RegularRespawnTimer * BossPenaltyScale * ExpertPenaltyScale / 60;

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (ShorterRespawn.Instance.herosMod != null && ShorterRespawn.Instance.herosMod.Version >= new Version(0, 2, 2))
			{
				if (ShorterRespawn.Instance.herosMod.Call("HasPermission", whoAmI, ShorterRespawn.ModifyGlobalRespawnTime_Permission) is bool result && result)
					return true;
				message = $"You lack the \"{ShorterRespawn.ModifyGlobalRespawnTime_Display}\" permission.";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
	}
}