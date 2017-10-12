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
        private double xVelocity;
        private double yVelocity;
        private double gravity = 9.81;
        private Explosion explosion;
        private TankController player;
        
        public Bullet(float x, float y, float angle, float power, float gravity, Explosion explosion, TankController player)
        {
            throw new NotImplementedException();
        }

        public override void Tick()
        {
            throw new NotImplementedException();
        }

        public override void Display(Graphics graphics, Size size)
        {
            throw new NotImplementedException();
        }
    }
}
