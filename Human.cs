using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class Human : TankController
    {
        public Human(string name, Tank tank, Color colour) : base(name, tank, colour)
        {
            throw new NotImplementedException();
        }

        public override void StartRound()
        {
            throw new NotImplementedException();
        }

        public override void NewTurn(SkirmishForm gameplayForm, Gameplay currentGame)
        {
            throw new NotImplementedException();
        }

        public override void ReportHit(float x, float y)
        {
            throw new NotImplementedException();
        }
    }
}
