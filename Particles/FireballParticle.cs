﻿using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace InfernumMode.Particles
{
    public class FireballParticle : Particle
    {
        private float Opacity;

        private float Spin;


        private static int FrameAmount = 6;

        public override bool SetLifetime => true;

        public override int FrameVariants => 7;

        public override bool UseCustomDraw => true;

        public override bool UseAdditiveBlend => true;

        public override string Texture => "CalamityMod/Particles/HeavySmoke";

        public FireballParticle(Vector2 position, Vector2 velocity, Color color, int lifetime, float scale, float opacity, float rotationSpeed = 0f)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            Scale = scale;
            Variant = Main.rand.Next(7);
            Lifetime = lifetime;
            Opacity = opacity;
            Spin = rotationSpeed;
        }

        public override void Update()
        {
            if ((float)Time / (float)Lifetime < 0.1f)
            {
                Scale += 0.01f;
            }
            if ((float)Time / (float)Lifetime > 0.9f)
            {
                Scale *= 0.975f;
            }

            Color = Main.hslToRgb((Main.rgbToHsl(Color).X) % 1f, Main.rgbToHsl(Color).Y, Main.rgbToHsl(Color).Z);
            Opacity *= 0.98f;
            Rotation += Spin * ((Velocity.X > 0f) ? 1f : (-1f));
            float lerpValue = Utils.GetLerpValue(1f, 0.85f, LifetimeCompletion, clamped: true);
            Color *= lerpValue;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            int num = (int)Math.Floor((float)Time / ((float)Lifetime / (float)FrameAmount));
            Rectangle rectangle = new(80 * Variant, 80 * num, 80, 80);
            spriteBatch.Draw(texture, Position - Main.screenPosition, rectangle, Color * Opacity, Rotation, rectangle.Size() / 2f, Scale, SpriteEffects.None, 0f);
        }
    }
}