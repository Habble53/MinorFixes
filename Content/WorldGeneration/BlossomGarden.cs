﻿using CalamityMod.Schematics;
using InfernumMode.Core.GlobalInstances.Systems;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using static CalamityMod.Schematics.SchematicManager;


namespace InfernumMode.Content.WorldGeneration
{
    public class BlossomGarden
    {
        public static void Generate(GenerationProgress progress, GameConfiguration _2)
        {
            progress.Message = "Growing a garden...";

            SchematicMetaTile[,] schematic = TileMaps["BlossomGarden"];
            Point placementPoint = default;
            SchematicAnchor schematicAnchor = SchematicAnchor.Center;
            for (int i = 0; i < 10000; i++)
            {
                int placementPositionX = WorldGen.genRand.Next(WorldGen.tLeft, WorldGen.tRight);
                int placementPositionY = WorldGen.tTop < Main.rockLayer - 10.0 ? WorldGen.tBottom + 240 : WorldGen.tTop - 240;
                placementPoint = new(placementPositionX, placementPositionY);
                Rectangle area = CalamityMod.CalamityUtils.GetSchematicProtectionArea(schematic, placementPoint, schematicAnchor);

                // Check the spot is valid.
                if (WorldGen.structures.CanPlace(area, 10))
                    break;
            }
            WorldSaveSystem.BlossomGardenCenter = placementPoint;
            bool _ = false;
            PlaceSchematic<Action<Chest>>("BlossomGarden", placementPoint, schematicAnchor, ref _);
        }
    }
}