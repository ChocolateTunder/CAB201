using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TankBattle {
    public class Gameplay {
        // Private instance variables declared here
        private static TankController[] players;
        private PlayerTank[] tankPlayers;
        private List<Attack> attack;
        private TankController currentPlayer;
        private TankController startingTankController;
        private Terrain map = new Terrain();
        private int maxRoundsPlay;
        private int currentRound;
        private int[] positions;
        private int windSpeed;
        SkirmishForm form;
        Random rng = new Random();

        public Gameplay (int numPlayers, int numRounds) {

            // Check if paremeters are within bounds (given in assignment description)
            // if within bounds, store in private variables
            if ((numPlayers >= 2) && (numPlayers <= 8)) {
                players = new TankController[numPlayers];
            } else {
                throw new ArgumentOutOfRangeException();
            }

            if ((numRounds >= 1) && (numRounds <= 100)) {
                maxRoundsPlay = numRounds;
            } else {
                throw new ArgumentOutOfRangeException();
            }

            attack = new List<Attack>();
        }

        public int GetNumPlayers () {
            return players.Length;
        }

        public int CurrentRound () {
            return currentRound;
        }

        public int GetMaxRounds () {
            return maxRoundsPlay;
        }

        public void SetPlayer (int playerNum, TankController player) {
            players[playerNum - 1] = player;
        }

        public TankController GetPlayerNumber (int playerNum) {
            return players[playerNum - 1];
        }

        public PlayerTank GetGameplayTank (int playerNum) {
            if (playerNum < 1 && playerNum > players.Length) {
                throw new IndexOutOfRangeException("playerNum is out of range in GamePlay.GetGameplayTank()");
            } else {
                return tankPlayers[playerNum - 1];
            }
        }

        public static Color TankColour (int playerNum) {
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

        public static int[] CalcPlayerLocations (int numPlayers) {
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
            playerPositions[0] = (int)currentPos - 3;

            // sest remaining positions
            for (int i = 1; i < numPlayers; i++) {
                currentPos += Math.Round(distance);
                playerPositions[i] = (int)currentPos;
            }

            // return array
            return playerPositions;
        } // end CalcPlayerLocations()

        public static void Rearrange (int[] array) {

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

        public void NewGame () {
            // Update starting values
            currentRound = 1;
            startingTankController = players[0];
            // Call method
            NewRound();
        }


        /// <summary>
        /// Begins new round of gameplay. 
        /// Initialises currentPlayer, the terrain map, and finds
        /// positions of each tank. Finishes by displaying the SkirmishForm.
        /// 
        /// </summary>
        /// <Created>Sophie Rogers, n9935100, 19/10/2017</Created>
        /// <changed>Sophie Rogers, n9935100, 19/10/2017</changed>
        public void NewRound () {

            currentPlayer = startingTankController;
            map = new Terrain();
            positions = CalcPlayerLocations(players.Length);

            int xPos = 0;
            int yPos = 0;


            for (int i = 0; i < players.Length; i++) {
                players[i].StartRound();
            }

            Rearrange(positions);

            tankPlayers = new PlayerTank[players.Length];

            for (int i = 0; i < tankPlayers.Length; i++) {
                xPos = positions[i];
                yPos = map.PlaceTankVertically(xPos);
                tankPlayers[i] = new PlayerTank(players[i], xPos, yPos, this);
            }

            windSpeed = rng.Next(-100, 101);
            form = new SkirmishForm(this);
            form.Show();
        }

        /// <summary>
        /// Gets the current terrain map used by the game that is initialised in NewRound().
        ///  
        /// </summary>
        /// <returns>Current Terrain map</returns>
        /// <created>Sophie Rogers, n9935100, 19.10.2017, 19.10.2017</created>
        /// <changed>Sophie Rogers, n9935100, 19.10.2017</changed>
        public Terrain GetMap () {
            return map;
        }

        /// <summary>
        /// Displays all PlayerTanks currently still alive/
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="displaySize"></param>
        /// <created>Sophie Rogers, n9935100, 19.10.2017</created>
        /// <changed>Sophie Rogers, n9935100, 19.10.2017</changed>
        public void DisplayTanks (Graphics graphics, Size displaySize) {

            for (int i = 0; i < tankPlayers.Length; i++) {
                if (tankPlayers[i].TankExists()) {
                    tankPlayers[i].Display(graphics, displaySize);
                }
            }
        }

        public PlayerTank CurrentPlayerTank () {
            int index = 0;
            bool found = false;
            // return PlayerTank associated with currentPlayer
            //int index = Array.Find(players, currentPlayer => currentPlayer.);
            for (int i = 0; i < players.Length; i++) {
                if (players[i] == currentPlayer) {
                    index = i;
                    found = true;
                    i = players.Length;
                } else {
                    found = false;
                }
            }
            if (found == false) {
                throw new Exception("CurrentPlayer not found");
            }
            return tankPlayers[index];
        }

        /// <summary>
        /// Adds new Attack to attack list and
        /// allows the attack to access this GamePlay
        /// </summary>
        /// <param name="weaponEffect"></param>
        /// <created>Sophie Rogers, n9935100, 19.10.2017</created>
        /// <changed>Sophie Rogers, n9935100, 19.10.2017</changed>
        public void AddEffect (Attack weaponEffect) {
            attack.Add(weaponEffect);
            attack[(attack.Count() - 1)].SetCurrentGame(this);
        }
        /// <summary>
        /// Calls Tick() on each Attack in attack list.
        /// </summary>
        /// <returns>False if no Attack objects in list</returns>
        /// <returns>True if Attack objects in list</returns>
        /// <Created>Sophie Rogers, n9935100, 19.10.2017</Created>
        /// <Changed>Sophie Rogers, n9935100, 19.10.2017</Changed>
        public bool WeaponEffectStep () {
            if (attack.Count() == 0) {
                return false;
            }

            for (int i = 0; i < attack.Count(); i++) {
                attack[i].Tick();
            }

            return true;
        }

        /// <summary>
        /// Call Display() on every Attack in attack array.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="displaySize"></param>
        /// <created>Sophie Rogers, n9935100, 19.10.2017</created>
        /// <changed>Sophie Rogers, n9935100, 19.10.2017</changed>
        public void RenderEffects (Graphics graphics, Size displaySize) {
            foreach (Attack x in attack) {
                x.Display(graphics, displaySize);
            }
        }

        /// <summary>
        /// Removes given Attack from attack array.
        /// </summary>
        /// <param name="weaponEffect"></param>
        /// <created>Sophie Rogers, n9935100, 19.10.2017</created>
        /// <changed>Sophie Rogers, n9935100, 19.10.2017</changed>
        public void CancelEffect (Attack weaponEffect) {
            int index = -1;
            for (int i = 0; i < attack.Count(); i++) {
                if (attack[i] == weaponEffect) {
                    index = i;
                }
            }
            if (index == -1) {
                throw new Exception("weaponEffect not in attack array");
            }
            attack.RemoveAt(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectileX"></param>
        /// <param name="projectileY"></param>
        /// <returns>true if terrain </returns>
        /// <created>Sophie Rogers, n9935100, 21/10/2017</created>
        /// <changed>Sophie Rogers, n9935100, 21/10/2017</changed>
        public bool CheckIfTankHit (float projectileX, float projectileY) {
            int tankX;
            int tankY;
            bool ifX = false;
            bool ifY = false;
            bool tankExists = false;
            // return false if the coordinates are out of the map bounds
            if (projectileX > Terrain.WIDTH || projectileY > Terrain.HEIGHT) {
                return false;
            }
            // return true if there is something at the coordinate location
            if (map.TerrainAt((int)projectileX, (int)projectileY)) {
                return true;
            }
            // if there is a PlayerTank at location that exists, return true
            for (int i = 0; i < tankPlayers.Length; i++) {
                tankX = tankPlayers[i].XPos();
                tankY = tankPlayers[i].Y(); ;
                for (int j = 0; j < Tank.WIDTH; j++) {
                    if (projectileX == Tank.WIDTH - j) {
                        ifX = true;
                    }// end if
                } // end for
                for (int j = 0; j < Tank.HEIGHT; j++) {
                    if (projectileX == Tank.HEIGHT - j) {
                        ifY = true;
                    }// end if
                } // end for
                if (tankPlayers[i].TankExists()) {
                    tankExists = true;
                } // end if
                if (ifX == true && ifY == true && tankExists == true && tankPlayers[i] != CurrentPlayerTank()) {
                    return true;
                } // end if
            }

            return false;
        }

        public void Damage (float damageX, float damageY, float explosionDamage, float radius) {
            foreach (PlayerTank player in tankPlayers) {
                // Variables to hold distance between tank and explosion 
                float distX, distY, hypot;
                int damage;

                // Calculating the centre coordinates of the tanks
                float centreX = (player.XPos() + (float)(0.5 * Tank.WIDTH));
                float centreY = (player.Y() + (float)(0.5 * Tank.HEIGHT));

                // If Tank is still alive, calculate the distance between centre of tank
                // and origin of explosion
                if (player.TankExists()) {
                    distX = Math.Abs(damageX - centreX);
                    distY = Math.Abs(damageY - centreY);

                    hypot = (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));

                    if ((hypot < radius) && (hypot > radius / 2)) {
                        damage = (int)(explosionDamage * ((radius - hypot) / radius));
                        player.Damage(damage);
                    } else if (hypot < radius / 2) {
                        damage = (int)explosionDamage;
                        player.Damage(damage);
                    }
                }
            }
        }

        public bool CalculateGravity () {
            bool moved = false;

            if (map.CalculateGravity()) {
                moved = true;
            }

            foreach (PlayerTank tanks in tankPlayers) {
                if (tanks.CalculateGravity()) {
                    moved = true;
                }
            }

            return moved;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        /// <created>Sophie Rogers, n9935100, 21/10/2017</created>
        /// <changed>Sophie Rogers, n9935100, 21/10/2017</changed>
        public bool FinaliseTurn () {
            int count = 0;
            PlayerTank currentTank = tankPlayers[0];
            int index = 0;
            Random wind = new Random();
            // loop through tankPlayers, check if tank exists
            for (int i = 0; i < tankPlayers.Length; i++) {
                if (tankPlayers[i].TankExists()) {
                    count++;
                }
            }
            // if number of existing tanks > 2
            if (count >= 2) {
                // increment current player, check if exists
                for (int i = 0; i < tankPlayers.Length; i++) {
                    if (CurrentPlayerTank() == tankPlayers[i]) {
                        currentTank = tankPlayers[i];
                        index = i;
                        i = tankPlayers.Length;
                    }
                }
                // if current player doesn't exist, keep looping until it does.
                while (!currentTank.TankExists()) {
                    index++;
                    currentTank = tankPlayers[index];
                    // go back to 0 if necessary
                    if (index == tankPlayers.Length) {
                        index = -1;
                    }
                }
                // adjust wind speed by number between -10 and 10, set to -100 or 100 if outside
                windSpeed += wind.Next(-10, 11);
                if (windSpeed > 100) {
                    windSpeed = 100;
                } else if (windSpeed < -100) {
                    windSpeed = -100;
                }
                // return true
                return true;
            }
            // if number of existing tanks < 2, score the winner and end the game

            ScoreWinner();
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <created>Sophie Rogers, n9935100, 21/10/2017</created>
        /// <changed>Sophie Rogers, n9935100, 21/10/2017</changed>
        public void ScoreWinner () {
            for (int i = 0; i < tankPlayers.Length; i++) {
                if (tankPlayers[i].TankExists()) {
                    players[i].WonRound();
                }
            }
        }

        public void NextRound () {
            int currentIndex = 0;
            // increment currentRound
            currentRound++;
            // if currentRound less than or equal to max rounds
            if (currentRound <= maxRoundsPlay) {
                // switch to next starting player
                for (int i = 0; i < players.Length; i++) {
                    if (startingTankController == players[i]) {
                        currentIndex = i;
                        i = players.Length;
                    }
                }
                currentIndex++;
                if (currentIndex == players.Length) {
                    currentIndex = 0;
                }
                NewRound();
            }
            // if currentRound is greater than max rounds, game over.
            if (currentRound > maxRoundsPlay) {
                TitlescreenForm f = new TitlescreenForm();
                f.Show();
            }
            // show Leaderboard or TitleScreenForm
        }


        public int GetWind () {
            return windSpeed;
        }
    }
}