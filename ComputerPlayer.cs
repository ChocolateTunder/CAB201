﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class ComputerPlayer : TankController
    {
        public ComputerPlayer(string name, Tank tank, Color colour) : base(name, tank, colour)
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
