using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class TitlescreenForm : Form
    {
        public TitlescreenForm()
        {
            InitializeComponent();
        }

        private void newGameButton_Click (object sender, EventArgs e) {
            Gameplay game = new Gameplay(2, 1);
            TankController player1 = new Human("Player 1", Tank.CreateTank(1), Gameplay.TankColour(1));
            TankController player2 = new Human("Player 2", Tank.CreateTank(1), Gameplay.TankColour(2));
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);
            game.NewGame();
        }
    }
}
