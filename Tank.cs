using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public abstract class Tank
    {
        public const int WIDTH = 4;
        public const int HEIGHT = 3;
        public const int NUM_TANKS = 1;

        public abstract int[,] DisplayTank(float angle);

        public static void CreateLine(int[,] graphic, int X1, int Y1, int X2, int Y2)
        {
            int dx = X2 - X1;
            int dy = Y2 - Y1;

            // Determine how steep the line is
            bool isSteep;
            if (Math.Abs(dy) > Math.Abs(dx)) {
                isSteep = true;
            } else {
                isSteep = false;
            }

            // Rotate line if needed
            if (isSteep) {
                int tempX = X1;
                X1 = Y1;
                Y1 = tempX;

                tempX = X2;
                X2 = Y2;
                Y2 = tempX;
            }

            // Swap start and end points if necessary
            if (X1 > X2) {
                int tempX = X1;
                X1 = X2;
                X2 = tempX;

                int tempY = Y1;
                Y1 = Y2;
                Y2 = tempY;
            }

            // Recalculate deltas
            dx = X2 - X1;
            dy = Y2 - Y1;

            // Calculate error 
            int error = dx / 2;
            int yStep;
            if (Y1 < Y2) {
                yStep = 1;
            } else {
                yStep = -1;
            }

            // Iterate to generate the line
            int y = Y1;
            for (int x = X1; x < X2 + 1; x++) {
                if (isSteep) {
                    graphic [y, x] = 1;
                } else {
                    graphic [x, y] = 1;
                }

                error = error - Math.Abs(dy);
                if (error < 0) {
                    y = y + yStep;
                    error = error + dx;
                }
            }
        }

        public Bitmap CreateTankBMP(Color tankColour, float angle)
        {
            int[,] tankGraphic = DisplayTank(angle);
            int height = tankGraphic.GetLength(0);
            int width = tankGraphic.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);
            Color transparent = Color.FromArgb(0, 0, 0, 0);
            Color tankOutline = Color.FromArgb(255, 0, 0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tankGraphic[y, x] == 0)
                    {
                        bmp.SetPixel(x, y, transparent);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, tankColour);
                    }
                }
            }

            // Outline each pixel
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (tankGraphic[y, x] != 0)
                    {
                        if (tankGraphic[y - 1, x] == 0)
                            bmp.SetPixel(x, y - 1, tankOutline);
                        if (tankGraphic[y + 1, x] == 0)
                            bmp.SetPixel(x, y + 1, tankOutline);
                        if (tankGraphic[y, x - 1] == 0)
                            bmp.SetPixel(x - 1, y, tankOutline);
                        if (tankGraphic[y, x + 1] == 0)
                            bmp.SetPixel(x + 1, y, tankOutline);
                    }
                }
            }

            return bmp;
        }

        public abstract int GetTankHealth();

        public abstract string[] GetWeapons();

        public abstract void ActivateWeapon(int weapon, PlayerTank playerTank, Gameplay currentGame);

        public static Tank CreateTank(int tankNumber)
        {
            //TODO Add if statement to select tank type
            return new BasicTank();
        }
    }
    //TODO Add 5 more tank types
    public class BasicTank : Tank {
        //Creation of the default tank
        public override void ActivateWeapon(int weapon, PlayerTank playerTank, Gameplay currentGame) {
            throw new NotImplementedException();
        }

        public override int [,] DisplayTank(float angle) {
            throw new NotImplementedException();
        }

        public override int GetTankHealth() {
            return 100;
        }

        public override string [] GetWeapons() {
            throw new NotImplementedException();
        }
    }
}
