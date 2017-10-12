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
        private string name;
        private Color colour;
        private Tank tank;
        private int roundsWon;

        public TankController(string name, Tank tank, Color colour)
        {
            this.name = name;
            this.tank = tank;
            this.colour = colour;
            roundsWon = 0;
        }
        public Tank CreateTank()
        {
            return tank;
        }
        public string Name()
        {
            return name;
        }
        public Color GetColour()
        {
            return colour;
        }
        public void WonRound()
        {
            roundsWon += 1;
        }
        public int GetVictories()
        {
            return roundsWon;
        }

        public abstract void StartRound();

        public abstract void NewTurn(SkirmishForm gameplayForm, Gameplay currentGame);

        public abstract void ReportHit(float x, float y);
    }
}
