using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShorterRespawn
{
	// This class is the actual mod code that reduces the respawn timer when the player dies.
	public class ShorterRespawnPlayer : ModPlayer
	{
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			// If we are cheating
			/*
			if (ShorterRespawn.Instance.instantRespawn)
			{
				Player.respawnTimer = 0;
				return;
			}
			*/
			// otherwise, if we just want the time reduced to a more typical level
			//if (Main.expertMode)
			//{
			//	player.respawnTimer = (int)(player.respawnTimer * .75);
			//}

			ShorterRespawnConfig config = (ShorterRespawnConfig) Mod.GetConfig("ShorterRespawnConfig");

			// Reimplement vanilla respawnTimer logic
			Player.respawnTimer = ShorterRespawnConfig.RegularRespawnTimer;

			bool bossAlive = false;
			if (Main.netMode != NetmodeID.SinglePlayer && !pvp)
			{
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && (Main.npc[k].boss || Main.npc[k].type == NPCID.EaterofWorldsHead || Main.npc[k].type == NPCID.EaterofWorldsBody || Main.npc[k].type == NPCID.EaterofWorldsTail) && Math.Abs(Player.Center.X - Main.npc[k].Center.X) + Math.Abs(Player.Center.Y - Main.npc[k].Center.Y) < 4000f)
					{
						bossAlive = true;
						break;
					}
				}
			}
			if (bossAlive)
			{
				Player.respawnTimer = (int)(Player.respawnTimer * config.BossPenaltyScale);
			}
			if (Main.expertMode)
			{
				Player.respawnTimer = (int)(Player.respawnTimer * config.ExpertPenaltyScale);
			}
			Player.respawnTimer = (int)(Player.respawnTimer * config.GlobalRespawnScale);
		}
	}
}