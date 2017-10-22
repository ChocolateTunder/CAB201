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
        private float  gravity;
        private float x, y, angle, power;
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
            throw new NotImplementedException();
        }
    }
}
