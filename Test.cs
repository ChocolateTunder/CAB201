using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TankBattle;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TankBattleTestSuite
{
    class RequirementException : Exception
    {
        public RequirementException()
        {
        }

        public RequirementException(string message) : base(message)
        {
        }

        public RequirementException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    class Test
    {
        #region Testing Code

        private delegate bool TestCase();

        private static string ErrorDescription = null;

        private static void SetErrorDescription(string desc)
        {
            ErrorDescription = desc;
        }

        private static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.01) return true;
            return false;
        }

        private static Dictionary<string, string> unitTestResults = new Dictionary<string, string>();

        private static void Passed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[passed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                throw new Exception("ErrorDescription found for passing test case");
            }
            Console.WriteLine();
        }
        private static void Failed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[failed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                Console.Write("\n{0}", ErrorDescription);
                ErrorDescription = null;
            }
            Console.WriteLine();
        }
        private static void FailedToMeetRequirement(string name, string comment)
        {
            Console.Write("[      ] ");
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("{0}", comment);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        private static void DoTest(TestCase test)
        {
            // Have we already completed this test?
            if (unitTestResults.ContainsKey(test.Method.ToString()))
            {
                return;
            }

            bool passed = false;
            bool metRequirement = true;
            string exception = "";
            try
            {
                passed = test();
            }
            catch (RequirementException e)
            {
                metRequirement = false;
                exception = e.Message;
            }
            catch (Exception e)
            {
                exception = e.GetType().ToString();
            }

            string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
            string fnName = test.Method.ToString().Split('0')[1];

            if (metRequirement)
            {
                if (passed)
                {
                    unitTestResults[test.Method.ToString()] = "Passed";
                    Passed(string.Format("{0}.{1}", className, fnName), exception);
                }
                else
                {
                    unitTestResults[test.Method.ToString()] = "Failed";
                    Failed(string.Format("{0}.{1}", className, fnName), exception);
                }
            }
            else
            {
                unitTestResults[test.Method.ToString()] = "Failed";
                FailedToMeetRequirement(string.Format("{0}.{1}", className, fnName), exception);
            }
            Cleanup();
        }

        private static Stack<string> errorDescriptionStack = new Stack<string>();


        private static void Requires(TestCase test)
        {
            string result;
            bool wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

            if (!wasTested)
            {
                // Push the error description onto the stack (only thing that can change, not that it should)
                errorDescriptionStack.Push(ErrorDescription);

                // Do the test
                DoTest(test);

                // Pop the description off
                ErrorDescription = errorDescriptionStack.Pop();

                // Get the proper result for out
                wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

                if (!wasTested)
                {
                    throw new Exception("This should never happen");
                }
            }

            if (result == "Failed")
            {
                string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
                string fnName = test.Method.ToString().Split('0')[1];

                throw new RequirementException(string.Format("-> {0}.{1}", className, fnName));
            }
            else if (result == "Passed")
            {
                return;
            }
            else
            {
                throw new Exception("This should never happen");
            }

        }

        #endregion

        #region Test Cases
        private static Gameplay InitialiseGame()
        {
            Requires(TestGameplay0Gameplay);
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);
            Requires(TestGameplay0SetPlayer);

            Gameplay game = new Gameplay(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player1 = new Human("player1", tank, Color.Orange);
            TankController player2 = new Human("player2", tank, Color.Purple);
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);
            return game;
        }
        private static void Cleanup()
        {
            while (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Dispose();
            }
        }
        private static bool TestGameplay0Gameplay()
        {
            Gameplay game = new Gameplay(2, 1);
            return true;
        }
        private static bool TestGameplay0GetNumPlayers()
        {
            Requires(TestGameplay0Gameplay);

            Gameplay game = new Gameplay(2, 1);
            return game.GetNumPlayers() == 2;
        }
        private static bool TestGameplay0GetMaxRounds()
        {
            Requires(TestGameplay0Gameplay);

            Gameplay game = new Gameplay(3, 5);
            return game.GetMaxRounds() == 5;
        }
        private static bool TestGameplay0SetPlayer()
        {
            Requires(TestGameplay0Gameplay);
            Requires(TestTank0CreateTank);

            Gameplay game = new Gameplay(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player = new Human("playerName", tank, Color.Orange);
            game.SetPlayer(1, player);
            return true;
        }
        private static bool TestGameplay0GetPlayerNumber()
        {
            Requires(TestGameplay0Gameplay);
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);

            Gameplay game = new Gameplay(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player = new Human("playerName", tank, Color.Orange);
            game.SetPlayer(1, player);
            return game.GetPlayerNumber(1) == player;
        }
        private static bool TestGameplay0TankColour()
        {
            Color[] arrayOfColours = new Color[8];
            for (int i = 0; i < 8; i++)
            {
                arrayOfColours[i] = Gameplay.TankColour(i + 1);
                for (int j = 0; j < i; j++)
                {
                    if (arrayOfColours[j] == arrayOfColours[i]) return false;
                }
            }
            return true;
        }
        private static bool TestGameplay0CalcPlayerLocations()
        {
            int[] positions = Gameplay.CalcPlayerLocations(8);
            for (int i = 0; i < 8; i++)
            {
                if (positions[i] < 0) return false;
                if (positions[i] > 160) return false;
                for (int j = 0; j < i; j++)
                {
                    if (positions[j] == positions[i]) return false;
                }
            }
            return true;
        }
        private static bool TestGameplay0Rearrange()
        {
            int[] ar = new int[100];
            for (int i = 0; i < 100; i++)
            {
                ar[i] = i;
            }
            Gameplay.Rearrange(ar);
            for (int i = 0; i < 100; i++)
            {
                if (ar[i] != i)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestGameplay0NewGame()
        {
            Gameplay game = InitialiseGame();
            game.NewGame();

            foreach (Form f in Application.OpenForms)
            {
                if (f is SkirmishForm)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestGameplay0GetMap()
        {
            Requires(TestTerrain0Terrain);
            Gameplay game = InitialiseGame();
            game.NewGame();
            Terrain battlefield = game.GetMap();
            if (battlefield != null) return true;

            return false;
        }
        private static bool TestGameplay0CurrentPlayerTank()
        {
            Requires(TestGameplay0Gameplay);
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);
            Requires(TestGameplay0SetPlayer);
            Requires(TestPlayerTank0GetPlayerNumber);

            Gameplay game = new Gameplay(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player1 = new Human("player1", tank, Color.Orange);
            TankController player2 = new Human("player2", tank, Color.Purple);
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);

            game.NewGame();
            PlayerTank ptank = game.CurrentPlayerTank();
            if (ptank.GetPlayerNumber() != player1 && ptank.GetPlayerNumber() != player2)
            {
                return false;
            }
            if (ptank.CreateTank() != tank)
            {
                return false;
            }

            return true;
        }

        private static bool TestTank0CreateTank()
        {
            Tank tank = Tank.CreateTank(1);
            if (tank != null) return true;
            else return false;
        }
        private static bool TestTank0DisplayTank()
        {
            Requires(TestTank0CreateTank);
            Tank tank = Tank.CreateTank(1);

            int[,] tankGraphic = tank.DisplayTank(45);
            if (tankGraphic.GetLength(0) != 12) return false;
            if (tankGraphic.GetLength(1) != 16) return false;
            // We don't really care what the tank looks like, but the 45 degree tank
            // should at least look different to the -45 degree tank
            int[,] tankGraphic2 = tank.DisplayTank(-45);
            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (tankGraphic2[y, x] != tankGraphic[y, x])
                    {
                        return true;
                    }
                }
            }

            SetErrorDescription("Tank with turret at -45 degrees looks the same as tank with turret at 45 degrees");

            return false;
        }
        private static void DisplayLine(int[,] array)
        {
            string report = "";
            report += "A line drawn from 3,0 to 0,3 on a 4x4 array should look like this:\n";
            report += "0001\n";
            report += "0010\n";
            report += "0100\n";
            report += "1000\n";
            report += "The one produced by Tank.CreateLine() looks like this:\n";
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    report += array[y, x] == 1 ? "1" : "0";
                }
                report += "\n";
            }
            SetErrorDescription(report);
        }
        private static bool TestTank0CreateLine()
        {
            int[,] ar = new int[,] { { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 } };
            Tank.CreateLine(ar, 3, 0, 0, 3);

            // Ideally, the line we want to see here is:
            // 0001
            // 0010
            // 0100
            // 1000

            // However, as we aren't that picky, as long as they have a 1 in every row and column
            // and nothing in the top-left and bottom-right corners

            int[] rows = new int[4];
            int[] cols = new int[4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (ar[y, x] == 1)
                    {
                        rows[y] = 1;
                        cols[x] = 1;
                    }
                    else if (ar[y, x] > 1 || ar[y, x] < 0)
                    {
                        // Only values 0 and 1 are permitted
                        SetErrorDescription(string.Format("Somehow the number {0} got into the array.", ar[y, x]));
                        return false;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (rows[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
                if (cols[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
            }
            if (ar[0, 0] == 1)
            {
                DisplayLine(ar);
                return false;
            }
            if (ar[3, 3] == 1)
            {
                DisplayLine(ar);
                return false;
            }

            return true;
        }
        private static bool TestTank0GetTankHealth()
        {
            Requires(TestTank0CreateTank);
            // As long as it's > 0 we're happy
            Tank tank = Tank.CreateTank(1);
            if (tank.GetTankHealth() > 0) return true;
            return false;
        }
        private static bool TestTank0GetWeapons()
        {
            Requires(TestTank0CreateTank);
            // As long as there's at least one result and it's not null / a blank string, we're happy
            Tank tank = Tank.CreateTank(1);
            if (tank.GetWeapons().Length == 0) return false;
            if (tank.GetWeapons()[0] == null) return false;
            if (tank.GetWeapons()[0] == "") return false;
            return true;
        }

        private static TankController CreateTestingPlayer()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);

            Tank tank = Tank.CreateTank(1);
            TankController player = new Human("player1", tank, Color.Aquamarine);
            return player;
        }

        private static bool TestTankController0Human()
        {
            Requires(TestTank0CreateTank);

            Tank tank = Tank.CreateTank(1);
            TankController player = new Human("player1", tank, Color.Aquamarine);
            if (player != null) return true;
            return false;
        }
        private static bool TestTankController0CreateTank()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);

            Tank tank = Tank.CreateTank(1);
            TankController p = new Human("player1", tank, Color.Aquamarine);
            if (p.CreateTank() == tank) return true;
            return false;
        }
        private static bool TestTankController0Name()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);

            const string PLAYER_NAME = "kfdsahskfdajh";
            Tank tank = Tank.CreateTank(1);
            TankController p = new Human(PLAYER_NAME, tank, Color.Aquamarine);
            if (p.Name() == PLAYER_NAME) return true;
            return false;
        }
        private static bool TestTankController0GetColour()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0Human);

            Color playerColour = Color.Chartreuse;
            Tank tank = Tank.CreateTank(1);
            TankController p = new Human("player1", tank, playerColour);
            if (p.GetColour() == playerColour) return true;
            return false;
        }
        private static bool TestTankController0WonRound()
        {
            TankController p = CreateTestingPlayer();
            p.WonRound();
            return true;
        }
        private static bool TestTankController0GetVictories()
        {
            Requires(TestTankController0WonRound);

            TankController p = CreateTestingPlayer();
            int wins = p.GetVictories();
            p.WonRound();
            if (p.GetVictories() == wins + 1) return true;
            return false;
        }
        private static bool TestHuman0StartRound()
        {
            TankController p = CreateTestingPlayer();
            p.StartRound();
            return true;
        }
        private static bool TestHuman0NewTurn()
        {
            Requires(TestGameplay0NewGame);
            Requires(TestGameplay0GetPlayerNumber);
            Gameplay game = InitialiseGame();

            game.NewGame();

            // Find the gameplay form
            SkirmishForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is SkirmishForm)
                {
                    gameplayForm = f as SkirmishForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Gameplay.NewGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in SkirmishForm");
                return false;
            }

            // Disable the control panel to check that NewTurn enables it
            controlPanel.Enabled = false;

            game.GetPlayerNumber(1).NewTurn(gameplayForm, game);

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after HumanPlayer.NewTurn()");
                return false;
            }
            return true;

        }
        private static bool TestHuman0ReportHit()
        {
            TankController p = CreateTestingPlayer();
            p.ReportHit(0, 0);
            return true;
        }

        private static bool TestPlayerTank0PlayerTank()
        {
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            return true;
        }
        private static bool TestPlayerTank0GetPlayerNumber()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            if (playerTank.GetPlayerNumber() == p) return true;
            return false;
        }
        private static bool TestPlayerTank0CreateTank()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestTankController0CreateTank);
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            if (playerTank.CreateTank() == playerTank.GetPlayerNumber().CreateTank()) return true;
            return false;
        }
        private static bool TestPlayerTank0GetTankAngle()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            float angle = playerTank.GetTankAngle();
            if (angle >= -90 && angle <= 90) return true;
            return false;
        }
        private static bool TestPlayerTank0SetAngle()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0GetTankAngle);
            float angle = 75;
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.SetAngle(angle);
            if (FloatEquals(playerTank.GetTankAngle(), angle)) return true;
            return false;
        }
        private static bool TestPlayerTank0GetTankPower()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);

            playerTank.GetTankPower();
            return true;
        }
        private static bool TestPlayerTank0SetTankPower()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0GetTankPower);
            int power = 65;
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.SetTankPower(power);
            if (playerTank.GetTankPower() == power) return true;
            return false;
        }
        private static bool TestPlayerTank0GetPlayerWeapon()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);

            playerTank.GetPlayerWeapon();
            return true;
        }
        private static bool TestPlayerTank0SetWeapon()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0GetPlayerWeapon);
            int weapon = 3;
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.SetWeapon(weapon);
            if (playerTank.GetPlayerWeapon() == weapon) return true;
            return false;
        }
        private static bool TestPlayerTank0Display()
        {
            Requires(TestPlayerTank0PlayerTank);
            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.Display(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestPlayerTank0XPos()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, x, y, game);
            if (playerTank.XPos() == x) return true;
            return false;
        }
        private static bool TestPlayerTank0Y()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, x, y, game);
            if (playerTank.Y() == y) return true;
            return false;
        }
        private static bool TestPlayerTank0Attack()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.Attack();
            return true;
        }
        private static bool TestPlayerTank0Damage()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();

            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.Damage(10);
            return true;
        }
        private static bool TestPlayerTank0TankExists()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0Damage);

            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            if (!playerTank.TankExists()) return false;
            playerTank.Damage(playerTank.CreateTank().GetTankHealth());
            if (playerTank.TankExists()) return false;
            return true;
        }
        private static bool TestPlayerTank0CalculateGravity()
        {
            Requires(TestGameplay0GetMap);
            Requires(TestTerrain0DestroyTerrain);
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0Damage);
            Requires(TestPlayerTank0TankExists);
            Requires(TestPlayerTank0CreateTank);
            Requires(TestTank0GetTankHealth);

            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            game.NewGame();
            // Unfortunately we need to rely on DestroyTerrain() to get rid of any terrain that may be in the way
            game.GetMap().DestroyTerrain(Terrain.WIDTH / 2.0f, Terrain.HEIGHT / 2.0f, 20);
            PlayerTank playerTank = new PlayerTank(p, Terrain.WIDTH / 2, Terrain.HEIGHT / 2, game);
            int oldX = playerTank.XPos();
            int oldY = playerTank.Y();

            playerTank.CalculateGravity();

            if (playerTank.XPos() != oldX)
            {
                SetErrorDescription("Caused X coordinate to change.");
                return false;
            }
            if (playerTank.Y() != oldY + 1)
            {
                SetErrorDescription("Did not cause Y coordinate to increase by 1.");
                return false;
            }

            int initialArmour = playerTank.CreateTank().GetTankHealth();
            // The tank should have lost 1 armour from falling 1 tile already, so do
            // (initialArmour - 2) damage to the tank then drop it again. That should kill it.

            if (!playerTank.TankExists())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.Damage(initialArmour - 2);
            if (!playerTank.TankExists())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.CalculateGravity();
            if (playerTank.TankExists())
            {
                SetErrorDescription("Tank survived despite taking enough falling damage to destroy it");
                return false;
            }

            return true;
        }
        private static bool TestTerrain0Terrain()
        {
            Terrain battlefield = new Terrain();
            return true;
        }
        private static bool TestTerrain0TerrainAt()
        {
            Requires(TestTerrain0Terrain);

            bool foundTrue = false;
            bool foundFalse = false;
            Terrain battlefield = new Terrain();
            for (int y = 0; y < Terrain.HEIGHT; y++)
            {
                for (int x = 0; x < Terrain.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        foundTrue = true;
                    }
                    else
                    {
                        foundFalse = true;
                    }
                }
            }

            if (!foundTrue)
            {
                SetErrorDescription("IsTileAt() did not return true for any tile.");
                return false;
            }

            if (!foundFalse)
            {
                SetErrorDescription("IsTileAt() did not return false for any tile.");
                return false;
            }

            return true;
        }
        private static bool TestTerrain0TankFits()
        {
            Requires(TestTerrain0Terrain);
            Requires(TestTerrain0TerrainAt);

            Terrain battlefield = new Terrain();
            for (int y = 0; y <= Terrain.HEIGHT - Tank.HEIGHT; y++)
            {
                for (int x = 0; x <= Terrain.WIDTH - Tank.WIDTH; x++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < Tank.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < Tank.WIDTH; ix++)
                        {

                            if (battlefield.TerrainAt(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        if (battlefield.TankFits(x, y))
                        {
                            SetErrorDescription("Found collision where there shouldn't be one");
                            return false;
                        }
                    }
                    else
                    {
                        if (!battlefield.TankFits(x, y))
                        {
                            SetErrorDescription("Didn't find collision where there should be one");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private static bool TestTerrain0PlaceTankVertically()
        {
            Requires(TestTerrain0Terrain);
            Requires(TestTerrain0TerrainAt);

            Terrain battlefield = new Terrain();
            for (int x = 0; x <= Terrain.WIDTH - Tank.WIDTH; x++)
            {
                int lowestValid = 0;
                for (int y = 0; y <= Terrain.HEIGHT - Tank.HEIGHT; y++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < Tank.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < Tank.WIDTH; ix++)
                        {

                            if (battlefield.TerrainAt(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        lowestValid = y;
                    }
                }

                int placedY = battlefield.PlaceTankVertically(x);
                if (placedY != lowestValid)
                {
                    SetErrorDescription(string.Format("Tank was placed at {0},{1} when it should have been placed at {0},{2}", x, placedY, lowestValid));
                    return false;
                }
            }
            return true;
        }
        private static bool TestTerrain0DestroyTerrain()
        {
            Requires(TestTerrain0Terrain);
            Requires(TestTerrain0TerrainAt);

            Terrain battlefield = new Terrain();
            for (int y = 0; y < Terrain.HEIGHT; y++)
            {
                for (int x = 0; x < Terrain.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        battlefield.DestroyTerrain(x, y, 0.5f);
                        if (battlefield.TerrainAt(x, y))
                        {
                            SetErrorDescription("Attempted to destroy terrain but it still exists");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            SetErrorDescription("Did not find any terrain to destroy");
            return false;
        }
        private static bool TestTerrain0CalculateGravity()
        {
            Requires(TestTerrain0Terrain);
            Requires(TestTerrain0TerrainAt);
            Requires(TestTerrain0DestroyTerrain);

            Terrain battlefield = new Terrain();
            for (int x = 0; x < Terrain.WIDTH; x++)
            {
                if (battlefield.TerrainAt(x, Terrain.HEIGHT - 1))
                {
                    if (battlefield.TerrainAt(x, Terrain.HEIGHT - 2))
                    {
                        // Seek up and find the first non-set tile
                        for (int y = Terrain.HEIGHT - 2; y >= 0; y--)
                        {
                            if (!battlefield.TerrainAt(x, y))
                            {
                                // Do a gravity step and make sure it doesn't slip down
                                battlefield.CalculateGravity();
                                if (!battlefield.TerrainAt(x, y + 1))
                                {
                                    SetErrorDescription("Moved down terrain even though there was no room");
                                    return false;
                                }

                                // Destroy the bottom-most tile
                                battlefield.DestroyTerrain(x, Terrain.HEIGHT - 1, 0.5f);

                                // Do a gravity step and make sure it does slip down
                                battlefield.CalculateGravity();

                                if (battlefield.TerrainAt(x, y + 1))
                                {
                                    SetErrorDescription("Terrain didn't fall");
                                    return false;
                                }

                                // Otherwise this seems to have worked
                                return true;
                            }
                        }


                    }
                }
            }
            SetErrorDescription("Did not find any appropriate terrain to test");
            return false;
        }
        private static bool TestAttack0SetCurrentGame()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestGameplay0Gameplay);

            Attack weaponEffect = new Explosion(1, 1, 1);
            Gameplay game = new Gameplay(2, 1);
            weaponEffect.SetCurrentGame(game);
            return true;
        }
        private static bool TestBullet0Bullet()
        {
            Requires(TestExplosion0Explosion);
            TankController player = CreateTestingPlayer();
            Explosion explosion = new Explosion(1, 1, 1);
            Bullet projectile = new Bullet(25, 25, 45, 30, 0.02f, explosion, player);
            return true;
        }
        private static bool TestBullet0Tick()
        {
            Requires(TestGameplay0NewGame);
            Requires(TestExplosion0Explosion);
            Requires(TestBullet0Bullet);
            Requires(TestAttack0SetCurrentGame);
            Gameplay game = InitialiseGame();
            game.NewGame();
            TankController player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);

            Bullet projectile = new Bullet(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.SetCurrentGame(game);
            projectile.Tick();

            // We can't really test this one without a substantial framework,
            // so we just call it and hope that everything works out

            return true;
        }
        private static bool TestBullet0Display()
        {
            Requires(TestGameplay0NewGame);
            Requires(TestGameplay0GetPlayerNumber);
            Requires(TestExplosion0Explosion);
            Requires(TestBullet0Bullet);
            Requires(TestAttack0SetCurrentGame);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the projectile
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            game.NewGame();
            TankController player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);

            Bullet projectile = new Bullet(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.SetCurrentGame(game);
            projectile.Display(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestExplosion0Explosion()
        {
            TankController player = CreateTestingPlayer();
            Explosion explosion = new Explosion(1, 1, 1);

            return true;
        }
        private static bool TestExplosion0Explode()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestAttack0SetCurrentGame);
            Requires(TestGameplay0GetPlayerNumber);
            Requires(TestGameplay0NewGame);

            Gameplay game = InitialiseGame();
            game.NewGame();
            TankController player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);
            explosion.SetCurrentGame(game);
            explosion.Explode(25, 25);

            return true;
        }
        private static bool TestExplosion0Tick()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestAttack0SetCurrentGame);
            Requires(TestGameplay0GetPlayerNumber);
            Requires(TestGameplay0NewGame);
            Requires(TestExplosion0Explode);

            Gameplay game = InitialiseGame();
            game.NewGame();
            TankController player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);
            explosion.SetCurrentGame(game);
            explosion.Explode(25, 25);
            explosion.Tick();

            // Again, we can't really test this one without a full framework

            return true;
        }
        private static bool TestExplosion0Display()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestAttack0SetCurrentGame);
            Requires(TestGameplay0GetPlayerNumber);
            Requires(TestGameplay0NewGame);
            Requires(TestExplosion0Explode);
            Requires(TestExplosion0Tick);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the explosion
            TankController p = CreateTestingPlayer();
            Gameplay game = InitialiseGame();
            game.NewGame();
            TankController player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(10, 10, 10);
            explosion.SetCurrentGame(game);
            explosion.Explode(25, 25);
            // Step it for a bit so we can be sure the explosion is visible
            for (int i = 0; i < 10; i++)
            {
                explosion.Tick();
            }
            explosion.Display(graphics, bitmapSize);

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }

        private static SkirmishForm InitialiseSkirmishForm(out NumericUpDown angleCtrl, out TrackBar powerCtrl, out Button fireCtrl, out Panel controlPanel, out ListBox weaponSelect)
        {
            Requires(TestGameplay0NewGame);

            Gameplay game = InitialiseGame();

            angleCtrl = null;
            powerCtrl = null;
            fireCtrl = null;
            controlPanel = null;
            weaponSelect = null;

            game.NewGame();
            SkirmishForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is SkirmishForm)
                {
                    gameplayForm = f as SkirmishForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay.NewGame() did not create a SkirmishForm and that is the only way SkirmishForm can be tested");
                return null;
            }

            bool foundDisplayPanel = false;
            bool foundControlPanel = false;

            foreach (Control c in gameplayForm.Controls)
            {
                // The only controls should be 2 panels
                if (c is Panel)
                {
                    // Is this the control panel or the display panel?
                    Panel p = c as Panel;

                    // The display panel will have 0 controls.
                    // The control panel will have separate, of which only a few are mandatory
                    int controlsFound = 0;
                    bool foundFire = false;
                    bool foundAngle = false;
                    bool foundAngleLabel = false;
                    bool foundPower = false;
                    bool foundPowerLabel = false;


                    foreach (Control pc in p.Controls)
                    {
                        controlsFound++;

                        // Mandatory controls for the control panel are:
                        // A 'Fire!' button
                        // A NumericUpDown for controlling the angle
                        // A TrackBar for controlling the power
                        // "Power:" and "Angle:" labels

                        if (pc is Label)
                        {
                            Label lbl = pc as Label;
                            if (lbl.Text.ToLower().Contains("angle"))
                            {
                                foundAngleLabel = true;
                            }
                            else
                            if (lbl.Text.ToLower().Contains("power"))
                            {
                                foundPowerLabel = true;
                            }
                        }
                        else
                        if (pc is Button)
                        {
                            Button btn = pc as Button;
                            if (btn.Text.ToLower().Contains("fire"))
                            {
                                foundFire = true;
                                fireCtrl = btn;
                            }
                        }
                        else
                        if (pc is TrackBar)
                        {
                            foundPower = true;
                            powerCtrl = pc as TrackBar;
                        }
                        else
                        if (pc is NumericUpDown)
                        {
                            foundAngle = true;
                            angleCtrl = pc as NumericUpDown;
                        }
                        else
                        if (pc is ListBox)
                        {
                            weaponSelect = pc as ListBox;
                        }
                    }

                    if (controlsFound == 0)
                    {
                        foundDisplayPanel = true;
                    }
                    else
                    {
                        if (!foundFire)
                        {
                            SetErrorDescription("Control panel lacks a \"Fire!\" button OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngle)
                        {
                            SetErrorDescription("Control panel lacks an angle NumericUpDown OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPower)
                        {
                            SetErrorDescription("Control panel lacks a power TrackBar OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngleLabel)
                        {
                            SetErrorDescription("Control panel lacks an \"Angle:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPowerLabel)
                        {
                            SetErrorDescription("Control panel lacks a \"Power:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }

                        foundControlPanel = true;
                        controlPanel = p;
                    }

                }
                else
                {
                    SetErrorDescription(string.Format("Unexpected control ({0}) named \"{1}\" found in SkirmishForm", c.GetType().FullName, c.Name));
                    return null;
                }
            }

            if (!foundDisplayPanel)
            {
                SetErrorDescription("No display panel found");
                return null;
            }
            if (!foundControlPanel)
            {
                SetErrorDescription("No control panel found");
                return null;
            }
            return gameplayForm;
        }

        private static bool TestSkirmishForm0SkirmishForm()
        {
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            SkirmishForm gameplayForm = InitialiseSkirmishForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            return true;
        }
        private static bool TestSkirmishForm0EnableControlPanel()
        {
            Requires(TestSkirmishForm0SkirmishForm);
            Gameplay game = InitialiseGame();
            game.NewGame();

            // Find the gameplay form
            SkirmishForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is SkirmishForm)
                {
                    gameplayForm = f as SkirmishForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Gameplay.NewGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in SkirmishForm");
                return false;
            }

            // Disable the control panel to check that EnableControlPanel enables it
            controlPanel.Enabled = false;

            gameplayForm.EnableControlPanel();

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after SkirmishForm.EnableControlPanel()");
                return false;
            }
            return true;

        }
        private static bool TestSkirmishForm0SetAngle()
        {
            Requires(TestSkirmishForm0SkirmishForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            SkirmishForm gameplayForm = InitialiseSkirmishForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            float testAngle = 27;

            gameplayForm.SetAngle(testAngle);
            if (FloatEquals((float)angle.Value, testAngle)) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set angle to {0} but angle is {1}", testAngle, (float)angle.Value));
                return false;
            }
        }
        private static bool TestSkirmishForm0SetTankPower()
        {
            Requires(TestSkirmishForm0SkirmishForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            SkirmishForm gameplayForm = InitialiseSkirmishForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            int testPower = 71;

            gameplayForm.SetTankPower(testPower);
            if (power.Value == testPower) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set power to {0} but power is {1}", testPower, power.Value));
                return false;
            }
        }
        private static bool TestSkirmishForm0SetWeapon()
        {
            Requires(TestSkirmishForm0SkirmishForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            SkirmishForm gameplayForm = InitialiseSkirmishForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            gameplayForm.SetWeapon(0);

            // WeaponSelect is optional behaviour, so it's okay if it's not implemented here, as long as the method works.
            return true;
        }
        private static bool TestSkirmishForm0Attack()
        {
            Requires(TestSkirmishForm0SkirmishForm);
            // This is something we can't really test properly without a proper framework, so for now we'll just click
            // the button and make sure it disables the control panel
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            SkirmishForm gameplayForm = InitialiseSkirmishForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            controlPanel.Enabled = true;
            fire.PerformClick();
            if (controlPanel.Enabled)
            {
                SetErrorDescription("Control panel still enabled immediately after clicking fire button");
                return false;
            }

            return true;
        }
        private static void UnitTests()
        {
            DoTest(TestGameplay0Gameplay);
            DoTest(TestGameplay0GetNumPlayers);
            DoTest(TestGameplay0GetMaxRounds);
            DoTest(TestGameplay0SetPlayer);
            DoTest(TestGameplay0GetPlayerNumber);
            DoTest(TestGameplay0TankColour);
            DoTest(TestGameplay0CalcPlayerLocations);
            DoTest(TestGameplay0Rearrange);
            DoTest(TestGameplay0NewGame);
            DoTest(TestGameplay0GetMap);
            DoTest(TestGameplay0CurrentPlayerTank);
            DoTest(TestTank0CreateTank);
            DoTest(TestTank0DisplayTank);
            DoTest(TestTank0CreateLine);
            DoTest(TestTank0GetTankHealth);
            DoTest(TestTank0GetWeapons);
            DoTest(TestTankController0Human);
            DoTest(TestTankController0CreateTank);
            DoTest(TestTankController0Name);
            DoTest(TestTankController0GetColour);
            DoTest(TestTankController0WonRound);
            DoTest(TestTankController0GetVictories);
            DoTest(TestHuman0StartRound);
            DoTest(TestHuman0NewTurn);
            DoTest(TestHuman0ReportHit);
            DoTest(TestPlayerTank0PlayerTank);
            DoTest(TestPlayerTank0GetPlayerNumber);
            DoTest(TestPlayerTank0CreateTank);
            DoTest(TestPlayerTank0GetTankAngle);
            DoTest(TestPlayerTank0SetAngle);
            DoTest(TestPlayerTank0GetTankPower);
            DoTest(TestPlayerTank0SetTankPower);
            DoTest(TestPlayerTank0GetPlayerWeapon);
            DoTest(TestPlayerTank0SetWeapon);
            DoTest(TestPlayerTank0Display);
            DoTest(TestPlayerTank0XPos);
            DoTest(TestPlayerTank0Y);
            DoTest(TestPlayerTank0Attack);
            DoTest(TestPlayerTank0Damage);
            DoTest(TestPlayerTank0TankExists);
            DoTest(TestPlayerTank0CalculateGravity);
            DoTest(TestTerrain0Terrain);
            DoTest(TestTerrain0TerrainAt);
            DoTest(TestTerrain0TankFits);
            DoTest(TestTerrain0PlaceTankVertically);
            DoTest(TestTerrain0DestroyTerrain);
            DoTest(TestTerrain0CalculateGravity);
            DoTest(TestAttack0SetCurrentGame);
            DoTest(TestBullet0Bullet);
            DoTest(TestBullet0Tick);
            DoTest(TestBullet0Display);
            DoTest(TestExplosion0Explosion);
            DoTest(TestExplosion0Explode);
            DoTest(TestExplosion0Tick);
            DoTest(TestExplosion0Display);
            DoTest(TestSkirmishForm0SkirmishForm);
            DoTest(TestSkirmishForm0EnableControlPanel);
            DoTest(TestSkirmishForm0SetAngle);
            DoTest(TestSkirmishForm0SetTankPower);
            DoTest(TestSkirmishForm0SetWeapon);
            DoTest(TestSkirmishForm0Attack);
        }
        
        #endregion
        
        #region CheckClasses

        private static bool CheckClasses()
        {
            string[] classNames = new string[] { "Program", "ComputerPlayer", "Terrain", "Explosion", "SkirmishForm", "Gameplay", "Human", "Bullet", "TankController", "PlayerTank", "Tank", "Attack" };
            string[][] classFields = new string[][] {
                new string[] { "Main" }, // Program
                new string[] { }, // ComputerPlayer
                new string[] { "TerrainAt","TankFits","PlaceTankVertically","DestroyTerrain","CalculateGravity","WIDTH","HEIGHT"}, // Terrain
                new string[] { "Explode" }, // Explosion
                new string[] { "EnableControlPanel","SetAngle","SetTankPower","SetWeapon","Attack","InitBuffer"}, // SkirmishForm
                new string[] { "GetNumPlayers","CurrentRound","GetMaxRounds","SetPlayer","GetPlayerNumber","GetGameplayTank","TankColour","CalcPlayerLocations","Rearrange","NewGame","NewRound","GetMap","DisplayTanks","CurrentPlayerTank","AddEffect","WeaponEffectStep","RenderEffects","CancelEffect","CheckIfTankHit","Damage","CalculateGravity","FinaliseTurn","ScoreWinner","NextRound","GetWind"}, // Gameplay
                new string[] { }, // Human
                new string[] { }, // Bullet
                new string[] { "CreateTank","Name","GetColour","WonRound","GetVictories","StartRound","NewTurn","ReportHit"}, // TankController
                new string[] { "GetPlayerNumber","CreateTank","GetTankAngle","SetAngle","GetTankPower","SetTankPower","GetPlayerWeapon","SetWeapon","Display","XPos","Y","Attack","Damage","TankExists","CalculateGravity"}, // PlayerTank
                new string[] { "DisplayTank","CreateLine","CreateTankBMP","GetTankHealth","GetWeapons","ActivateWeapon","CreateTank","WIDTH","HEIGHT","NUM_TANKS"}, // Tank
                new string[] { "SetCurrentGame","Tick","Display"} // Attack
            };

            Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine("Checking classes for public methods...");
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsPublic)
                {
                    if (type.Namespace != "TankBattle")
                    {
                        Console.WriteLine("Public type {0} is not in the TankBattle namespace.", type.FullName);
                        return false;
                    }
                    else
                    {
                        int typeIdx = -1;
                        for (int i = 0; i < classNames.Length; i++)
                        {
                            if (type.Name == classNames[i])
                            {
                                typeIdx = i;
                                classNames[typeIdx] = null;
                                break;
                            }
                        }
                        foreach (MemberInfo memberInfo in type.GetMembers())
                        {
                            string memberName = memberInfo.Name;
                            bool isInherited = false;
                            foreach (MemberInfo parentMemberInfo in type.BaseType.GetMembers())
                            {
                                if (memberInfo.Name == parentMemberInfo.Name)
                                {
                                    isInherited = true;
                                    break;
                                }
                            }
                            if (!isInherited)
                            {
                                if (typeIdx != -1)
                                {
                                    bool fieldFound = false;
                                    if (memberName[0] != '.')
                                    {
                                        foreach (string allowedFields in classFields[typeIdx])
                                        {
                                            if (memberName == allowedFields)
                                            {
                                                fieldFound = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fieldFound = true;
                                    }
                                    if (!fieldFound)
                                    {
                                        Console.WriteLine("The public field \"{0}\" is not one of the authorised fields for the {1} class.\n", memberName, type.Name);
                                        Console.WriteLine("Remove it or change its access level.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    //Console.WriteLine("{0} passed.", type.FullName);
                }
            }
            for (int i = 0; i < classNames.Length; i++)
            {
                if (classNames[i] != null)
                {
                    Console.WriteLine("The class \"{0}\" is missing.", classNames[i]);
                    return false;
                }
            }
            Console.WriteLine("All public methods okay.");
            return true;
        }
        
        #endregion

        public static void Main()
        {
            if (CheckClasses())
            {
                UnitTests();

                int passed = 0;
                int failed = 0;
                foreach (string key in unitTestResults.Keys)
                {
                    if (unitTestResults[key] == "Passed")
                    {
                        passed++;
                    }
                    else
                    {
                        failed++;
                    }
                }

                Console.WriteLine("\n{0}/{1} unit tests passed", passed, passed + failed);
                if (failed == 0)
                {
                    Console.WriteLine("Starting up TankBattle...");
                    Program.Main();
                    return;
                }
            }

            Console.WriteLine("\nPress enter to exit.");
            Console.ReadLine();
        }
    }
}
