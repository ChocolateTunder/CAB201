using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle {
    public abstract class Tank {
        public const int WIDTH = 4;
        public const int HEIGHT = 3;
        public const int NUM_TANKS = 1;
        
      
        public abstract int[,] DisplayTank(float angle);
        
        public static void CreateLine(int[,] graphic, int X1, int Y1, int X2, int Y2) {
            /*Console.WriteLine("X1: {0}, Y1: {1}", X1, Y1);
            Console.WriteLine("X2: {0}, Y2: {1}", X2, Y2);*/
            
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
                    graphic[y, x] = 1;
                } else {
                    graphic[x, y] = 1;
                }

                error = error - Math.Abs(dy);
                if (error < 0) {
                    y = y + yStep;
                    error = error + dx;
                }
            }
            /*
            for (int i = 0; i < graphic.GetLength(0); i++) {
                for (int j = 0; j < graphic.GetLength(1); j++) {
                    Console.Write(graphic[i, j]);
                }
                Console.WriteLine();
            }*/
        }

        public Bitmap CreateTankBMP(Color tankColour, float angle) {
            int[,] tankGraphic = DisplayTank(angle);
            int height = tankGraphic.GetLength(0);
            int width = tankGraphic.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);
            Color transparent = Color.FromArgb(0, 0, 0, 0);
            Color tankOutline = Color.FromArgb(255, 0, 0, 0);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (tankGraphic[y, x] == 0) {
                        bmp.SetPixel(x, y, transparent);
                    } else {
                        bmp.SetPixel(x, y, tankColour);
                    }
                }
            }

            // Outline each pixel
            for (int y = 1; y < height - 1; y++) {
                for (int x = 1; x < width - 1; x++) {
                    if (tankGraphic[y, x] != 0) {
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

        public static Tank CreateTank(int tankNumber) {
            //TODO Add if statement to select tank type
            return new BasicTank();
        }
    }
    //TODO Add 5 more tank types
    public class BasicTank : Tank {
        public PlayerTank playerTank;
        public TankController player;
        float xPos, yPos;
        //Creation of the default tank
        public override void ActivateWeapon(int weapon, PlayerTank playerTank, Gameplay currentGame) {
            this.playerTank = playerTank;
            xPos = (float)playerTank.XPos() + (float)0.5 * WIDTH;
            yPos = (float)playerTank.Y() + (float)0.5 * HEIGHT;
            player = playerTank.GetPlayerNumber();
        }

        /// <summary>
        /// This method takes in an angle between -90 degress and 90 degrees and converts
        /// this to a position for the tank turret. The method the returns an array that 
        /// displays the tank with the turret in position.
        /// 
        /// Author: Sophie Rogers, n9925100
        /// Date created: 12/10/2017
        /// Date last modified: 12/10/2017
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public override int[,] DisplayTank(float angle) {
            // declare variables, store if angle is -ve
            const double LENGTH_TURRET = 5;
            double degAngle = angle;
            double radAngle;
            int xi = 7;
            int yi = 6;
            int x;
            int y;
            double adj;
            double opp;
            bool isNeg = false;
            //Console.WriteLine(degAngle);
            
            int[,] graphic = { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                               { 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                               { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

            if (degAngle == 0) {
                Tank.CreateLine(graphic, 7, 6, 1, 7);
                /*for (int j = 0; j < graphic.GetLength(0); j++) {
                    for (int i = 0; i < graphic.GetLength(1); i++) {
                        Console.Write(graphic[j, i]);
                    }
                    Console.WriteLine();
                }*/
                return graphic;
            }

            if (degAngle < 0) {
                isNeg = true;
                //Console.WriteLine("angle is negative");
            } else {
                isNeg = false;
            }

            // convert to radians
            if (degAngle < 0) {
                radAngle = Math.Abs(degAngle + 90) * (Math.PI / 180);
            } else {


                radAngle = Math.Abs(degAngle - 90) * (Math.PI / 180);
            }

            //Console.WriteLine("Angle in rad:{0}", radAngle);

            // find opp and adj using sin(x) and cos(x)
            opp = Math.Sin(radAngle) * LENGTH_TURRET;
            adj = Math.Cos(radAngle) * LENGTH_TURRET;
            //Console.WriteLine("Opp = {0}, adj = {1}", opp, adj);

            // find x and y coordinates
            // test if x is a very small value, if it is round down, if not round up.
            if (adj < 0.5 && !isNeg) {
                x = xi + 4;
            } else if (adj < 0.5 && isNeg) {
                x = xi - 4;
            } else { 
                x = (int)Math.Ceiling(adj);
                if (isNeg) {
                    x = xi - x;
                } else {
                    x = xi + x;
                }
            }

            // round y up 
            if (opp < 0.5) {
                y = yi - (int)Math.Floor(opp);
            } else {
                y = yi - (int)Math.Ceiling(opp) ;
            }
            
            //Console.WriteLine("x = {0}, y = {1}", x, y);

            // determine quadrant
            
            //Console.WriteLine("x = {0}, y = {1}", x, y);
            
            // call CreateLine()
            CreateLine(graphic, xi, yi, y, x);

            /*for (int j = 0; j < graphic.GetLength(0); j++) {
                for (int i = 0; i < graphic.GetLength(1); i++) {
                    Console.Write(graphic[j, i]);
                }
                Console.WriteLine();
            }*/
            return graphic;
        }

        /// <summary>
        /// Returns the current tank health
        /// 
        /// Author: Sophie Rogers, n9935100
        /// Date created: 12/10/2017
        /// Date last modified: 12/10/2017
        /// </summary>
        /// <returns></returns>
        public override int GetTankHealth() {
            return 100;
        }

        /// <summary>
        /// Returns a list of weapons allocated to each tank
        /// 
        /// Author: Sophie Rogers, n9935100
        /// Date created: 12/10/2017
        /// Date last modified: 12/10/2017
        /// </summary>
        /// <returns></returns>
        public override string[] GetWeapons() {
            string[] weapons = new string[] {"Standard Shell", "Missile", "Landmine", "Nuke" };
            return weapons;
        }
    }
}
