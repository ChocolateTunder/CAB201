using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class Bullet : Attack
    {
        private float xVelocity;
        private float yVelocity;
        private float x, y;
        private Explosion explosion;
        private TankController player;
        
        public Bullet(float x, float y, float angle, float power, float gravity, Explosion explosion, TankController player)
        {
            this.x = x;
            this.y = y;
            this.explosion = explosion;
            this.player = player;
            float angleRadians = (90 - angle) * (float)Math.PI / 180;
            float magnitude = power / 50;
            xVelocity = (float)Math.Cos(angleRadians) * magnitude;
            yVelocity = (float)Math.Sin(angleRadians) * -magnitude;
        }

        public override void Tick () {
            for (int i = 0; i < 10; i++) {
                x += xVelocity;
                y += yVelocity;
                x += currentGame.GetWind() / 1000.0f;

                if ((x > Terrain.WIDTH) || (x < 0) || (y < 0)){
                currentGame.CancelEffect(this);
                }
            }
        }

        public override void Display(Graphics graphics, Size size)
        {
            float x = (float)this.x * size.Width / Terrain.WIDTH;
            float y = (float)this.y * size.Height / Terrain.HEIGHT;
            float s = size.Width / Terrain.WIDTH;

            RectangleF r = new RectangleF(x - s / 2.0f, y - s / 2.0f, s, s);
            Brush b = new SolidBrush(Color.WhiteSmoke);

            graphics.FillEllipse(b, r);
        }
    }
}
