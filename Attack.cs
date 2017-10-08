using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public abstract class Attack
    {
        public void SetCurrentGame(Gameplay game)
        {
            throw new NotImplementedException();
        }

        public abstract void Tick();
        public abstract void Display(Graphics graphics, Size displaySize);
    }
}
