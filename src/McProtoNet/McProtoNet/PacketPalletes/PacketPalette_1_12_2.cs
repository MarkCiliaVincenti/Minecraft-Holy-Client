﻿using System.Collections.Frozen;

namespace McProtoNet
{
	public class PacketPalette_1_12_2 : IPacketPallete
	{
		public static PacketPalette_1_12_2 Instance { get; } = new();

		private readonly FrozenDictionary<int, PacketIn> typeIn = FrozenDictionary.ToFrozenDictionary(new Dictionary<int, PacketIn>()
		{
			 { 0x00, PacketIn.SpawnEntity },
			{ 0x01, PacketIn.SpawnExperienceOrb },
			{ 0x02, PacketIn.SpawnWeatherEntity },
			{ 0x03, PacketIn.SpawnLivingEntity },
			{ 0x04, PacketIn.SpawnPainting },
			{ 0x05, PacketIn.SpawnPlayer },
			{ 0x06, PacketIn.EntityAnimation },
			{ 0x07, PacketIn.Statistics },
			{ 0x08, PacketIn.BlockBreakAnimation },
			{ 0x09, PacketIn.BlockEntityData },
			{ 0x0A, PacketIn.BlockAction },
			{ 0x0B, PacketIn.BlockChange },
			{ 0x0C, PacketIn.BossBar },
			{ 0x0D, PacketIn.ServerDifficulty },
			{ 0x0E, PacketIn.TabComplete },
			{ 0x0F, PacketIn.ChatMessage },
			{ 0x10, PacketIn.MultiBlockChange },
			{ 0x11, PacketIn.WindowConfirmation },
			{ 0x12, PacketIn.CloseWindow },
			{ 0x13, PacketIn.OpenWindow },
			{ 0x14, PacketIn.WindowItems },
			{ 0x15, PacketIn.WindowProperty },
			{ 0x16, PacketIn.SetSlot },
			{ 0x17, PacketIn.SetCooldown },
			{ 0x18, PacketIn.PluginMessage },
			{ 0x19, PacketIn.NamedSoundEffect },
			{ 0x1A, PacketIn.Disconnect },
			{ 0x1B, PacketIn.EntityStatus },
			{ 0x1C, PacketIn.Explosion },
			{ 0x1D, PacketIn.UnloadChunk },
			{ 0x1E, PacketIn.ChangeGameState },
			{ 0x1F, PacketIn.KeepAlive },
			{ 0x20, PacketIn.ChunkData },
			{ 0x21, PacketIn.Effect },
			{ 0x22, PacketIn.Particle },
			{ 0x23, PacketIn.JoinGame },
			{ 0x24, PacketIn.MapData },
			{ 0x25, PacketIn.EntityMovement },
			{ 0x26, PacketIn.EntityPosition },
			{ 0x27, PacketIn.EntityPositionRotation },
			{ 0x28, PacketIn.EntityRotation },
			{ 0x29, PacketIn.VehicleMove },
			{ 0x2A, PacketIn.OpenSignEditor },
			{ 0x2B, PacketIn.CraftRecipeResponse },
			{ 0x2C, PacketIn.PlayerAbilities },
			{ 0x2D, PacketIn.CombatEvent },
			{ 0x2E, PacketIn.PlayerInfo },
			{ 0x2F, PacketIn.PlayerPositionRotation },
			{ 0x30, PacketIn.UseBed },
			{ 0x31, PacketIn.UnlockRecipes },
			{ 0x32, PacketIn.DestroyEntities },
			{ 0x33, PacketIn.RemoveEntityEffect },
			{ 0x34, PacketIn.ResourcePackSend },
			{ 0x35, PacketIn.Respawn },
			{ 0x36, PacketIn.EntityHeadLook },
			{ 0x37, PacketIn.SelectAdvancementTab },
			{ 0x38, PacketIn.WorldBorder },
			{ 0x39, PacketIn.Camera },
			{ 0x3A, PacketIn.HeldItemChange },
			{ 0x3B, PacketIn.DisplayScoreboard },
			{ 0x3C, PacketIn.EntityMetadata },
			{ 0x3D, PacketIn.AttachEntity },
			{ 0x3E, PacketIn.EntityVelocity },
			{ 0x3F, PacketIn.EntityEquipment },
			{ 0x40, PacketIn.SetExperience },
			{ 0x41, PacketIn.UpdateHealth },
			{ 0x42, PacketIn.ScoreboardObjective },
			{ 0x43, PacketIn.SetPassengers },
			{ 0x44, PacketIn.Teams },
			{ 0x45, PacketIn.UpdateScore },
			{ 0x46, PacketIn.SpawnPosition },
			{ 0x47, PacketIn.TimeUpdate },
			{ 0x48, PacketIn.Title },
			{ 0x49, PacketIn.SoundEffect },
			{ 0x4A, PacketIn.PlayerListHeaderAndFooter },
			{ 0x4B, PacketIn.CollectItem },
			{ 0x4C, PacketIn.EntityTeleport },
			{ 0x4D, PacketIn.Advancements },
			{ 0x4E, PacketIn.EntityProperties },
			{ 0x4F, PacketIn.EntityEffect },
		});

		private readonly FrozenDictionary<PacketOut, int> typeOut = FrozenDictionary.ToFrozenDictionary(new Dictionary<PacketOut, int>()
		{
			 { PacketOut.TeleportConfirm, 0x00 },
			{ PacketOut.TabComplete, 0x01 },
			{ PacketOut.ChatMessage, 0x02 },
			{ PacketOut.ClientStatus, 0x03 },
			{ PacketOut.ClientSettings, 0x04 },
			{ PacketOut.WindowConfirmation, 0x05 },
			{ PacketOut.EnchantItem, 0x06 },
			{ PacketOut.ClickWindow, 0x07 },
			{ PacketOut.CloseWindow, 0x08 },
			{ PacketOut.PluginMessage, 0x09 },
			{ PacketOut.InteractEntity, 0x0A },
			{ PacketOut.KeepAlive, 0x0B },
			{ PacketOut.PlayerMovement, 0x0C },
			{ PacketOut.PlayerPosition, 0x0D },
			{ PacketOut.PlayerPositionRotation, 0x0E },
			{ PacketOut.PlayerRotation, 0x0F },
			{ PacketOut.VehicleMove, 0x10 },
			{ PacketOut.SteerBoat, 0x11 },
			{ PacketOut.CraftRecipeRequest, 0x12 },
			{ PacketOut.PlayerAbilities, 0x13 },
			{ PacketOut.PlayerAction, 0x14 },
			{ PacketOut.EntityAction, 0x15 },
			{ PacketOut.SteerVehicle, 0x16 },
			{ PacketOut.RecipeBookData, 0x17 },
			{ PacketOut.ResourcePackStatus, 0x18 },
			{ PacketOut.AdvancementTab, 0x19 },
			{ PacketOut.HeldItemChange, 0x1A },
			{ PacketOut.CreativeInventoryAction, 0x1B },
			{ PacketOut.UpdateSign, 0x1C },
			{ PacketOut.Animation, 0x1D },
			{ PacketOut.Spectate, 0x1E },
			{ PacketOut.PlayerBlockPlacement, 0x1F },
			{ PacketOut.UseItem, 0x20 },
		});

		public int GetOut(PacketOut packet)
		{
			return typeOut[packet];
		}

		public bool TryGetIn(int id, out PacketIn packetIn)
		{
			if (typeIn.TryGetValue(id, out packetIn))
			{
				return true;
			}
			return false;
		}
	}
}
