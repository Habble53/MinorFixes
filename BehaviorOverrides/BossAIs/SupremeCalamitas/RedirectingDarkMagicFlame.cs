using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernumMode.BehaviorOverrides.BossAIs.SupremeCalamitas
{
    public class RedirectingDarkMagicFlame : ModProjectile
    {
        public PrimitiveTrailCopy TrailDrawer = null;
        public ref float Time => ref projectile.ai[0];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Flame");
            Main.projFrames[projectile.type] = 6;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.timeLeft = 480;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            Player closestPlayer = Main.player[Player.FindClosest(projectile.Center, 1, 1)];
            projectile.Opacity = Utils.InverseLerp(0f, 20f, projectile.timeLeft, true) * Utils.InverseLerp(0f, 20f, Time, true);

            if (Time < 25f)
                projectile.velocity *= 1.015f;
            else if (Time < 50f)
                projectile.velocity *= 0.97f;
            else
            {
                if (!projectile.WithinRange(closestPlayer.Center, 220f))
                    projectile.velocity = projectile.velocity.MoveTowards(projectile.SafeDirectionTo(closestPlayer.Center) * projectile.velocity.Length(), 0.45f) * 1.05f;
            }

            if (Time == 50f)
                Main.PlaySound(SoundID.Item74, projectile.Center);

            if (Time > 100f)
            {
                float angularOffset = (float)Math.Cos((projectile.Center * new Vector2(1.4f, 1f)).Length() / 175f + projectile.identity * 0.89f) * 0.024f;
                projectile.velocity = projectile.velocity.RotatedBy(angularOffset);
                projectile.velocity = projectile.velocity.SafeNormalize(Vector2.UnitY) * MathHelper.Clamp(projectile.velocity.Length() * 1.013f, 12f, 27f);
            }

            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;

            Time++;
        }


        public float FlameTrailWidthFunction(float completionRatio)
        {
            return MathHelper.SmoothStep(20f, 5f, completionRatio) * projectile.Opacity;
        }

        public Color FlameTrailColorFunction(float completionRatio)
        {
            float trailOpacity = Utils.InverseLerp(0.8f, 0.27f, completionRatio, true) * Utils.InverseLerp(0f, 0.067f, completionRatio, true) * 0.9f;
            Color startingColor = Color.Lerp(Color.White, Color.IndianRed, 0.25f);
            Color middleColor = Color.Lerp(Color.Maroon, Color.Red, 0.4f);
            Color endColor = Color.Lerp(Color.Purple, Color.Black, 0.35f);
            Color color = CalamityUtils.MulticolorLerp(completionRatio, startingColor, middleColor, endColor) * trailOpacity;
            color.A = 184;
            return color * projectile.Opacity;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 drawPosition = projectile.Center - Main.screenPosition;
            Rectangle frame = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Vector2 origin = frame.Size() * 0.5f;
            spriteBatch.Draw(texture, drawPosition, frame, projectile.GetAlpha(lightColor), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}