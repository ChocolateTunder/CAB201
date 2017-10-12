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
        protected Gameplay currentGame;

        public void SetCurrentGame(Gameplay game)
        {
            currentGame = game;
        }

        public abstract void Tick();
        public abstract void Display(Graphics graphics, Size displaySize);
    }
}
