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
        private int maxRoundsPlay,currentRound;
        private int [] positions;
        private Terrain map = new Terrain(); 
        private TankController startingTankController;
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

        public int CurrentRound() {
            return currentRound;
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
            // Progresses through the input array, swapping the position of members of the array with 
            // another member at a random position.
            for (int i = 0; i < size; i++) {
                int index = rnd.Next(i, size);

                temp = array[i];
                array[i] = array[index];
                array[index] = temp;
            }
        }

        public void NewGame()
        {
            // Update starting values
            currentRound = 1;
            startingTankController = players[0];
            // Call method
            NewRound();
        }

        public void NewRound()
        {
            startingTankController = currentPlayer;
            map = new Terrain();
            positions = CalcPlayerLocations(players.Length);
            // TODO: Loop through all Tank Controllers and call TankController.StartRound() method
            Rearrange(positions);
            // TODO: Create an array of PlayerTanks that is same size as players[]
            // TODO: Initialize array of PlayerTank by finding HorizontalPosition, VerticalPositon and then calling PlayerTank constructor
            // TODO: Initialize wind speed between -100 and 100
            // TODO: Create new skirmish form and Show() it
            throw new NotImplementedException();
        }

        public Terrain GetMap()
        {
            // This method returns the current Terrain used by the game. This is stored in a private field and is initialised by NewRound().
            // Implement after New Round
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
