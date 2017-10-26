using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class SkirmishForm : Form
    {
        private Color landscapeColour;
        private Random rng = new Random();
        private Image backgroundImage = null;
        private int levelWidth = 160;
        private int levelHeight = 120;
        private Gameplay currentGame;

        private BufferedGraphics backgroundGraphics;
        private BufferedGraphics gameplayGraphics;

        string [] imageFilenames = { "Images\\background1.jpg",
                            "Images\\background2.jpg",
                            "Images\\background3.jpg",
                            "Images\\background4.jpg"};
        Color [] landscapeColours = { Color.FromArgb(255, 0, 0, 0),
                             Color.FromArgb(255, 73, 58, 47),
                             Color.FromArgb(255, 148, 116, 93),
                             Color.FromArgb(255, 133, 119, 109) };

        public SkirmishForm(Gameplay game)
        {
            currentGame = game;
            int chosen = rng.Next(0, 4);

            landscapeColour = landscapeColours [chosen];
            backgroundImage = Image.FromFile(imageFilenames [chosen], true);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            InitializeComponent();

            backgroundGraphics = InitBuffer();
            gameplayGraphics = InitBuffer();

            DrawBackground();
            
            DrawGameplay();
            
            NewTurn();
        }  

        // From https://stackoverflow.com/questions/13999781/tearing-in-my-animation-on-winforms-c-sharp
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public void EnableControlPanel()
        {
            controlPanel.Enabled = true;           
        }

        public void SetAngle(float angle)
        {
            angleSelector.Value = (decimal)angle;
        }

        public void SetTankPower(int power)
        {
            powerSelector.Value = power;
        }
        public void SetWeapon(int weapon)
        {
            weaponSelector.SelectedIndex = weapon;
        }

        public void Attack()
        {
            currentGame.CurrentPlayerTank().Attack();
            controlPanel.Enabled = false;
            timer.Enabled = true;
        }

        private void DrawGameplay () {
            backgroundGraphics.Render(gameplayGraphics.Graphics);
            currentGame.DisplayTanks(gameplayGraphics.Graphics, displayPanel.Size);
            currentGame.RenderEffects(gameplayGraphics.Graphics, displayPanel.Size);
        }

        private void NewTurn () {
            TankController player = currentGame.CurrentPlayerTank().GetPlayerNumber();
            PlayerTank tank = currentGame.CurrentPlayerTank();

            this.Text = String.Format("Tank Battle - Round {0} of {1}", currentGame.CurrentRound(), currentGame.GetMaxRounds());
            controlPanel.BackColor = player.GetColour();
            playerLabel.Text = player.Name();
            tank.SetAngle((float)angleSelector.Value);
            tank.SetTankPower(powerSelector.Value);

            string direction; 
            if (currentGame.GetWind() >= 0) {
                direction = "E";
            } else {
                direction = "W";
            }
            windLabel.Text = String.Format("{0} {1}", Math.Abs(currentGame.GetWind()), direction);

            weaponSelector.Items.Clear();

            string [] weapons = tank.CreateTank().GetWeapons();
            foreach (string weapon in weapons) {
                weaponSelector.Items.Add(weapon);
            }

            tank.SetWeapon(weaponSelector.SelectedIndex);
            player.NewTurn(this, currentGame);
        }

        private void DrawBackground()
        {
            Graphics graphics = backgroundGraphics.Graphics;
            Image background = backgroundImage;
            graphics.DrawImage(backgroundImage, new Rectangle(0, 0, displayPanel.Width, displayPanel.Height));

            Terrain battlefield = currentGame.GetMap();
            Brush brush = new SolidBrush(landscapeColour);

            for (int y = 0; y < Terrain.HEIGHT; y++)
            {
                for (int x = 0; x < Terrain.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        int drawX1 = displayPanel.Width * x / levelWidth;
                        int drawY1 = displayPanel.Height * y / levelHeight;
                        int drawX2 = displayPanel.Width * (x + 1) / levelWidth;
                        int drawY2 = displayPanel.Height * (y + 1) / levelHeight;
                        graphics.FillRectangle(brush, drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1);
                    }
                }
            }
        }

        public BufferedGraphics InitBuffer()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            Graphics graphics = displayPanel.CreateGraphics();
            Rectangle dimensions = new Rectangle(0, 0, displayPanel.Width, displayPanel.Height);
            BufferedGraphics bufferedGraphics = context.Allocate(graphics, dimensions);
            return bufferedGraphics;
        }

        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = displayPanel.CreateGraphics();
            gameplayGraphics.Render(graphics);
        }

        private void weaponSelector_SelectedValueChanged (object sender, EventArgs e) {
            PlayerTank tank = currentGame.CurrentPlayerTank();
            tank.SetWeapon(weaponSelector.SelectedIndex);
        }

        private void angleSelector_ValueChanged (object sender, EventArgs e) {
            PlayerTank tank = currentGame.CurrentPlayerTank();
            tank.SetAngle((float)angleSelector.Value);
            DrawGameplay();
            displayPanel.Invalidate();
        }

        private void powerSelector_ValueChanged (object sender, EventArgs e) {
            PlayerTank tank = currentGame.CurrentPlayerTank();
            tank.SetTankPower(powerSelector.Value);
            currentPower.Text = powerSelector.Value.ToString();
        }

        private void timer_Tick (object sender, EventArgs e) {
            currentGame.WeaponEffectStep();

            if (!currentGame.WeaponEffectStep()) {
                currentGame.CalculateGravity();
                DrawBackground();
                DrawGameplay();
                displayPanel.Invalidate();

                if (currentGame.CalculateGravity()) {
                    return;
                }else {
                    timer.Enabled = false;
                    currentGame.FinaliseTurn();

                    if (currentGame.FinaliseTurn()) {
                        NewTurn();
                    }else {
                        Dispose();
                        currentGame.NextRound();
                        return;
                    }
                }
            } else {
                DrawGameplay();
                displayPanel.Invalidate();
                return;
            }
        }

        private void fireButton_Click (object sender, EventArgs e) {
            Attack();
        }
    }
}