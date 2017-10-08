using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    abstract public class TankController
    {
        public TankController(string name, Tank tank, Color colour)
        {
            throw new NotImplementedException();
        }
        public Tank CreateTank()
        {
            throw new NotImplementedException();
        }
        public string Name()
        {
            throw new NotImplementedException();
        }
        public Color GetColour()
        {
            throw new NotImplementedException();
        }
        public void WonRound()
        {
            throw new NotImplementedException();
        }
        public int GetVictories()
        {
            throw new NotImplementedException();
        }

        public abstract void StartRound();

        public abstract void NewTurn(SkirmishForm gameplayForm, Gameplay currentGame);

        public abstract void ReportHit(float x, float y);
    }
}
