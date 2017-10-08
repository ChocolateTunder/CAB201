using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TankBattle
{
    public class Gameplay {
        // Private instance variables declared here
        private static TankController[] players;
        private int maxRoundsPlay;
        private int currentRound;
        private int startingTankController;
        private TankController currentPlayer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numPlayers"></param>
        /// <param name="numRounds"></param>
        public Gameplay(int numPlayers, int numRounds) {

            // Check if paremeters are within bounds (given in assignment description)
            // if within bounds, store in private variables
            if ((numPlayers >= 2) && (numPlayers <= 8)) {
                players = new TankController[numPlayers];
            } else {
                throw new ArgumentOutOfRangeException();
            }

            if((numRounds >= 1) && (numRounds <= 100)) {
                maxRoundsPlay = numRounds;
            } else {
                throw new ArgumentOutOfRangeException();
            }

            List<Attack> attack;
        }

        public int GetNumPlayers() {
            return players.Length;
        }

        // not got dis var yet bois
        public int CurrentRound() {
            throw new NotImplementedException();
            // return CurrentRound;
        }

        public int GetMaxRounds()
        {
            return maxRoundsPlay;
        }

        public void SetPlayer(int playerNum, TankController player)
        {
            players[playerNum - 1] = player;
        }

        public TankController GetPlayerNumber(int playerNum)
        {
            return players[playerNum - 1];
        }

        public PlayerTank GetGameplayTank(int playerNum)
        {
            // not ready yet bois
            throw new NotImplementedException();
        }

        public static Color TankColour(int playerNum)
        {
            // initialise color values
            Color playerOne = Color.White;
            Color playerTwo = Color.Blue;
            Color playerThree = Color.Red;
            Color playerFour = Color.Purple;
            Color playerFive = Color.Orange;
            Color playerSix = Color.Green;
            Color playerSeven = Color.Yellow;
            Color playerEight = Color.SaddleBrown;

            switch (playerNum) {
                case 1:
                    return playerOne;
                case 2:
                    return playerTwo;
                case 3:
                    return playerThree;
                case 4:
                    return playerFour;
                case 5:
                    return playerFive;
                case 6:
                    return playerSix;
                case 7:
                    return playerSeven;
                case 8:
                    return playerEight;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }

        public static int[] CalcPlayerLocations(int numPlayers)
        {
            // Ensure numPlayers is reasonable
            if (numPlayers < 2) {
                throw new ArgumentOutOfRangeException();
            }

            // Find distance between players (divide total width by numPlayers)
            float distance = Terrain.WIDTH / numPlayers;
            // Find distance between outside players and the boundaries (divide distance by two)
            double currentPos = Math.Round(distance / 2);
            // Array of player positions
            int[] playerPositions = new int[numPlayers];

            // Set first position 
            playerPositions[0] = (int)currentPos-3;
            
            // sest remaining positions
            for (int i = 1; i < numPlayers; i++) {
                currentPos += Math.Round(distance);
                playerPositions[i] = (int)currentPos;
            }

            // return array
            return playerPositions;
        } // end CalcPlayerLocations()

        public static void Rearrange(int[] array)
        {

            int size = array.Length;
            Random rnd = new Random();
            int temp;

            for (int i = 0; i < size; i++) {
                int index = rnd.Next(i, size);

                temp = array[i];
                array[i] = array[index];
                array[index] = temp;
            }
            foreach(int x in array) {
                Console.Write("{0} ", x);
            }
            /*Random rng = new Random();
            int size = array.Length;
            List<int> randList = new List<int>();
            List<int> temp = new List<int>();
            int i = 0;
            int tempNum;
            
            foreach(int num in array) {
                temp[i] = num;
                i++;
            }

            for(int j = size-1; j > 0; j--) {
                tempNum = rng.Next(0, j);
                randList.Add(temp[tempNum]);
                randList.RemoveAt(tempNum);
            }

            for(int j = 0; j < size-1; j++) {
                array[i] = randList[i];
            }*/

            /*int size = array.Length;
            int[] randArray = new int[size];
            int[] temp = new int[size];
            int holder;
            int tempInt;
            for(int i =0; i < size; i++) {
                Console.Write("{0}  ", array[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < size; i++) {
                do {
                    holder = rng.Next(0, size);
                } while (randArray.Contains(holder));
                randArray[i] = holder;
                Console.Write("{0} ", randArray[i]);
            }

            Console.WriteLine("rand array generated");
            
           for (int i = 0; i < size; i++) {
                temp[i] = array[i];
            }

            Console.WriteLine("temp array generated");
            for (int i = 0; i < size; i++) {
                tempInt = randArray[i];
                array[tempInt-1] = temp[i];
                
            }
            Console.WriteLine("final array done bitches");
            */
        }

        public void NewGame()
        {
            // update starting values
            this.currentRound = 1;
            this.startingTankController = 0;
            // Call method
            NewRound();
        }

        public void NewRound()
        {
            // intialise currentPlayer to startingTankController
            // create new terrain
            // create new TankController positions (get length of TankController array)
            // Call startRound method for each TankController
            // shuffle positions array with Rearrange
            // array of PlayerTank as private field (num PlayerTanks == num TankControllers)
            // initialise PlayerTank by finding horizontal position of the PlayerTank (returned by CalcPlayerLocations and shuffled with the rearrange method)
                
            // 
            throw new NotImplementedException();
        }

        public Terrain GetMap()
        {
            throw new NotImplementedException();
        }

        public void DisplayTanks(Graphics graphics, Size displaySize)
        {
            throw new NotImplementedException();
        }

        public PlayerTank CurrentPlayerTank()
        {
            throw new NotImplementedException();
        }

        public void AddEffect(Attack weaponEffect)
        {
            throw new NotImplementedException();
        }

        public bool WeaponEffectStep()
        {
            throw new NotImplementedException();
        }

        public void RenderEffects(Graphics graphics, Size displaySize)
        {
            throw new NotImplementedException();
        }

        public void CancelEffect(Attack weaponEffect)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfTankHit(float projectileX, float projectileY)
        {
            throw new NotImplementedException();
        }

        public void Damage(float damageX, float damageY, float explosionDamage, float radius)
        {
            throw new NotImplementedException();
        }

        public bool CalculateGravity()
        {
            throw new NotImplementedException();
        }

        public bool FinaliseTurn()
        {
            throw new NotImplementedException();
        }

        public void ScoreWinner()
        {
            throw new NotImplementedException();
        }

        public void NextRound()
        {
            throw new NotImplementedException();
        }
        
        public int GetWind()
        {
            throw new NotImplementedException();
        }
    }
}
