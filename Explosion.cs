using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
   public class Explosion : Attack 
    {
        private int explosionDamage, explosionRadius, destructionRadius;
        private float x, y;
        private float lifespan;
        public Explosion(int explosionDamage, int explosionRadius, int earthDestructionRadius)
        {
            this.explosionDamage = explosionDamage;
            this.explosionRadius = explosionRadius;
            destructionRadius = earthDestructionRadius;
        }

        public void Explode(float x, float y)
        {
            this.x = x;
            this.y = y;

            lifespan = 1.0f;
        }

        public override void Tick()
        {
            if (lifespan <= 0) {
                currentGame.Damage(x, y, explosionDamage, destructionRadius);
                currentGame.GetMap().DestroyTerrain(x, y, destructionRadius);
                currentGame.CancelEffect(this);       
            }

            lifespan -= 0.05f;
        }

        public override void Display(Graphics graphics, Size displaySize)
        {
            float x = (float)this.x * displaySize.Width / Terrain.WIDTH;
            float y = (float)this.y * displaySize.Height / Terrain.HEIGHT;
            float radius = displaySize.Width * (float)((1.0 - lifespan) * destructionRadius * 3.0 / 2.0) / Terrain.WIDTH;

            int alpha = 0, red = 0, green = 0, blue = 0;

            if (lifespan < 1.0 / 3.0) {
                red = 255;
                alpha = (int)(lifespan * 3.0 * 255);
            } else if (lifespan < 2.0 / 3.0) {
                red = 255;
                alpha = 255;
                green = (int)((lifespan * 3.0 - 1.0) * 255);
            } else {
                red = 255;
                alpha = 255;
                green = 255;
                blue = (int)((lifespan * 3.0 - 2.0) * 255);
            }

            RectangleF rect = new RectangleF(x - radius, y - radius, radius * 2, radius * 2);
            Brush b = new SolidBrush(Color.FromArgb(alpha, red, green, blue));

            graphics.FillEllipse(b, rect);
        }
    }
}
