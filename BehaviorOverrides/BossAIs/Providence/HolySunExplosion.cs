using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace InfernumMode.BehaviorOverrides.BossAIs.Providence
{
    public class HolySunExplosion : ModProjectile
    {
        public float MaxRadius;

        public PrimitiveTrailCopy FireDrawer;

        public ref float Time => ref Projectile.ai[0];
        public ref float Radius => ref Projectile.ai[1];
        public override void SetStaticDefaults() => DisplayName.SetDefault("Holy Explosion");

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 84;
            Projectile.MaxUpdates = 2;
            Projectile.scale = 1f;
            Projectile.Calamity().canBreakPlayerDefense = true;
        }

        public override void AI()
        {
            Projectile.scale += 0.08f;
            Radius = MathHelper.Lerp(Radius, MaxRadius, 0.1f);
            Projectile.Opacity = Utils.GetLerpValue(8f, 42f, Projectile.timeLeft, true) * 0.55f;

            Time++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Utilities.CircularCollision(targetHitbox.Center.ToVector2(), projHitbox, Radius * 0.8f);

        public float SunWidthFunction(float completionRatio) => Radius * (float)Math.Sin(MathHelper.Pi * completionRatio);

        public Color SunColorFunction(float completionRatio)
        {
            Color sunColor = Main.dayTime ? Color.Yellow : Color.Cyan;
            return Color.Lerp(sunColor, Color.White, (float)Math.Sin(MathHelper.Pi * completionRatio) * 0.5f + 0.3f) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (FireDrawer is null)
                FireDrawer = new PrimitiveTrailCopy(SunWidthFunction, SunColorFunction, null, true, GameShaders.Misc["Infernum:Fire"]);

            GameShaders.Misc["Infernum:Fire"].UseSaturation(0.45f);
            GameShaders.Misc["Infernum:Fire"].UseImage1("Images/Misc/Perlin");

            List<float> rotationPoints = new();
            List<Vector2> drawPoints = new();

            for (float offsetAngle = -MathHelper.PiOver2; offsetAngle <= MathHelper.PiOver2; offsetAngle += MathHelper.Pi / 10f)
            {
                rotationPoints.Clear();
                drawPoints.Clear();

                float adjustedAngle = offsetAngle + MathHelper.Pi * -0.2f;
                Vector2 offsetDirection = adjustedAngle.ToRotationVector2();
                for (int i = 0; i < 16; i++)
                {
                    rotationPoints.Add(adjustedAngle);
                    drawPoints.Add(Vector2.Lerp(Projectile.Center - offsetDirection * Radius / 2f, Projectile.Center + offsetDirection * Radius / 2f, i / 16f));
                }

                FireDrawer.Draw(drawPoints, -Main.screenPosition, 24);
            }
            return false;
        }
    }
}