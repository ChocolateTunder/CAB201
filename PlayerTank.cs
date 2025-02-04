﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class PlayerTank
    {
        private TankController player;
        private Tank tank;
        private Gameplay game;
        private Bitmap bitmap;
        private Gameplay currentGame;
        private int tankX;
        private int tankY;
        private int durability;
        private int power;
        private int weapon;
        private float angle;

        public PlayerTank(TankController player, int tankX, int tankY, Gameplay game)
        {
            this.player = player;
            this.tankX = tankX;
            this.tankY = tankY;
            this.game = game;
            tank = CreateTank();
            durability = tank.GetTankHealth();
            currentGame = game;

            angle = 0;
            power = 25;
            weapon = 0;

            bitmap = tank.CreateTankBMP(player.GetColour(), angle);
        }

        public TankController GetPlayerNumber()
        {
            return player;
        }
        public Tank CreateTank()
        {
            return player.CreateTank();
        }

        public float GetTankAngle()
        {
            return angle;
        }

        public void SetAngle(float angle)
        {
            this.angle = angle;
        }

        public int GetTankPower()
        {
            return power;
        }

        public void SetTankPower(int power)
        {
            this.power = power;
        }

        public int GetPlayerWeapon()
        {
            return weapon;
        }
        public void SetWeapon(int newWeapon)
        {
            weapon = newWeapon;
        }

        public void Display(Graphics graphics, Size displaySize)
        {
            int drawX1 = displaySize.Width * tankX / Terrain.WIDTH;
            int drawY1 = displaySize.Height * tankY / Terrain.HEIGHT;
            int drawX2 = displaySize.Width * (tankX + Tank.WIDTH) / Terrain.WIDTH;
            int drawY2 = displaySize.Height * (tankY + Tank.HEIGHT) / Terrain.HEIGHT;
            graphics.DrawImage(tank.CreateTankBMP(player.GetColour(), angle), new Rectangle(drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1));

            int drawY3 = displaySize.Height * (tankY - Tank.HEIGHT) / Terrain.HEIGHT;
            Font font = new Font("Arial", 8);
            Brush brush = new SolidBrush(Color.White);

            int pct = durability * 100 / tank.GetTankHealth();
            if (pct < 100) {
                graphics.DrawString(pct + "%", font, brush, new Point(drawX1, drawY3));
            }
        }

        public int XPos()
        {
            return tankX;
        }
        public int Y()
        {
            return tankY;
        }

        public void Attack()
        {
            (this.CreateTank()).ActivateWeapon(GetPlayerWeapon(), this, currentGame);
        }

        public void Damage(int damageAmount)
        {
            durability -= damageAmount;
        }

        public bool TankExists()
        {
            if(durability > 0) {
                return true;
            } else {
                return false;
            }
        }

        public bool CalculateGravity()
        {
            //If tank is dead, do nothing
            if (!TankExists()) {
                return false;
            }

            if(currentGame.GetMap().TankFits(XPos(), Y() + 1)){
                return false;
            }else {
                tankY += 1;
                durability -= 1;

                if (tankY == (Terrain.HEIGHT - Tank.HEIGHT)) {
                    durability = 0;
                }

                return true;
            }


        }
    }
}
