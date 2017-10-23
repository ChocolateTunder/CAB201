using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle {
    public class Terrain {
        public const int WIDTH = 160;
        public const int HEIGHT = 120;
        private bool [,] initTerrain = new bool [HEIGHT, WIDTH];

        public Terrain () {
            // Create 2D array of bools to represent terrain.
            // true = terrain at location

            Random rand = new Random();
            int trueOrFalse;
            int prob = 3; // probability of element being true is prob-1/prob

            // intialise top four rows as false (leave room for tank)
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    initTerrain [i, j] = false;
                }
            }

            // initialise bottom row as true (must have no bottomless pits)
            for (int i = 0; i < WIDTH; i++) {
                initTerrain [HEIGHT - 1, i] = true;
            }

            //Randomly choose a starting height between the floor and top four rows
            int initH = rand.Next(5, HEIGHT - 2);

            /*
            for (int j = 0; j < WIDTH; j++) {
                for (int i = initH; i <= HEIGHT-2; i++) {
                    initTerrain [j, i] = true;
                }
            }*/



            for (int i = HEIGHT - 2; i > 4; i--) {
                for (int j = 0; j < WIDTH; j++) {

                    trueOrFalse = rand.Next(0, prob);

                    if ((trueOrFalse < prob - 1) && (initTerrain [i + 1, j] == true)) {
                        initTerrain [i, j] = true;
                    } else if (trueOrFalse == prob) {
                        initTerrain [i, j] = false;
                    }
                }
            }

            // use for testing (only prints first section of terrain, rest can't fit on output screen

            /*
            for (int i = 0; i < HEIGHT; i++) {
                for(int j = 0; j < WIDTH/5; j++) {
                    if (initTerrain[i, j] == true) {
                        Console.Write("# ");
                    } else {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }*/

        }// end Terrain()

        public bool TerrainAt (int x, int y) {
            return initTerrain [y, x];
        }

        public bool TankFits (int x, int y) {
            for (int i = x; i < x + Tank.WIDTH; i++) {
                for (int j = y; j < y + Tank.HEIGHT; j++) {
                    if (TerrainAt(i, j)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public int PlaceTankVertically (int x) {
            int y = 0;

            while (!TankFits(x, y)) {
                y++;
            }

            y--;
            return y;
        }

        public void DestroyTerrain (float destroyX, float destroyY, float radius) {
            float distX, distY, hypot;

            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    distX = Math.Abs(destroyX - j);
                    distY = Math.Abs(destroyY - i);

                    hypot = (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));

                    if (hypot < radius) {
                        initTerrain [i, j] = false;
                    }
                }
            }
        }

        public bool CalculateGravity () {
            bool moved = false;

            for (int y = HEIGHT-1; y > 0; y--) {
                for (int x = WIDTH-1; x >= 0; x--) {
                    // Check if there's terrain at x,y
                    if (!TerrainAt(x, y)) {
                        // Check if there's a block above and move it down
                        if (TerrainAt(x, y - 1)) {
                            initTerrain [y, x] = true;
                            initTerrain [y - 1, x] = false;

                            moved = true;
                        }
                    }
                }
            }
            return moved;
        }
    }
}