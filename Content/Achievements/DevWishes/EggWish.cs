﻿using InfernumMode.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace InfernumMode.Content.Achievements.DevWishes
{
    public class EggWish : Achievement
    {
        public override void Initialize()
        {
            Name = "The Chosen One";
            Description = "Legends tell of a mighty warrior who will one day venture deep into the world and find the legendary blade hidden within\n" +
                "[c/777777:Find an egg sword shrine after defeating Golem]";
            TotalCompletion = 1;
            PositionInMainList = 10;
            UpdateCheck = AchievementUpdateCheck.TileBreak;
            IsDevWish = true;
        }

        public override void ExtraUpdate(Player player, int extraInfo)
        {
            // TODO: Check whether the extraInfo is equal to the shrine tile ID.
        }

        public override void OnCompletion(Player player)
        {
            WishCompletionEffects(player, ModContent.ItemType<CallUponTheEggs>());
        }

        public override void SaveProgress(TagCompound tag)
        {
            tag["EggCurrentCompletion"] = CurrentCompletion;
            tag["EggDoneCompletionEffects"] = DoneCompletionEffects;
        }

        public override void LoadProgress(TagCompound tag)
        {
            CurrentCompletion = tag.Get<int>("EggCurrentCompletion");
            DoneCompletionEffects = tag.Get<bool>("EggDoneCompletionEffects");
        }
    }
}