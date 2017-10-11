using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class Terrain
    {
        public const int WIDTH = 160;
        public const int HEIGHT = 120;
        private bool[,] initTerrain = new bool[HEIGHT, WIDTH];

        public Terrain()
        {
            // Create 2D array of bools to represent terrain.
            // true = terrain at location

            Random rand = new Random();
            int trueOrFalse;
            int prob = 3; // probability of element being true is prob-1/prob

            // intialise top four rows as false (leave room for tank)
            for(int i = 0; i < 4; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    initTerrain[i, j] = false;
                }
            }
            
            // initialise bottom row as true (must have no bottomless pits)
            for (int i = 0; i < WIDTH; i++) {
                initTerrain[HEIGHT - 1, i] = true;
            }

            //Randomly choose a starting height between the floor and top four rows
            int initH = rand.Next(5, HEIGHT - 2);
            
            /*
            for (int j = 0; j < WIDTH; j++) {
                for (int i = initH; i <= HEIGHT-2; i++) {
                    initTerrain [j, i] = true;
                }
            }*/


            
            for(int i = HEIGHT-2; i > 4; i--) {
                for(int j = 0; j < WIDTH; j++) {
                    
                    trueOrFalse = rand.Next(0, prob);

                    if ((trueOrFalse < prob-1) && (initTerrain[i + 1, j] == true)) {
                        initTerrain[i, j] = true;
                    } else if (trueOrFalse == prob) {
                        initTerrain[i, j] = false;
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

        public bool TerrainAt(int x, int y)
        {
            return initTerrain[y, x];
        }

        public bool TankFits(int x, int y)
        {
            throw new NotImplementedException();
        }

        public int PlaceTankVertically(int x)
        {
            throw new NotImplementedException();
        }

        public void DestroyTerrain(float destroyX, float destroyY, float radius)
        {
            throw new NotImplementedException();
        }

        public bool CalculateGravity()
        {
            throw new NotImplementedException();
        }
    }
}
