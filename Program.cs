using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Snake
{
    class Program
    {
        // Random
        private static readonly Random rand = new Random();

        static void Main(string[] args)
        {
            /* *
             * Declare + assign the variables
             */

            // first we store the original size for post-gameover
            int conW = Console.WindowWidth;
            int conH = Console.WindowHeight;
            // we set the console size to be the same as the playing field
            int playW = 20;
            int playH = 15;
            Console.WindowWidth = playW;
            Console.WindowHeight = playH;

            // set the snake's head and its position
            snek head = new snek();
            head.colour = ConsoleColor.Cyan;
            // start at a random position with intentional bullshit spawns
            // we still want to be at least 1px off of the wall
            head.posY = rand.Next(1,14);
            head.posX = rand.Next(1,20);

            // declare the tail
            List<int> tailX = new List<int>();
            List<int> tailY = new List<int>();

            // set the fruit's position to a random position on the playing field
            int fruitX = rand.Next(playW);
            int fruitY = rand.Next(playH);

            // initial movement direction, randomly generated for additional bullshit
            string dir = "";
            int dirRand = rand.Next(4);
            switch (dirRand)
            {
                case 0:
                    dir = "u";
                    break;
                case 1:
                    dir = "d";
                    break;
                case 2:
                    dir = "l";
                    break;
                case 3:
                    dir = "r";
                    break;
            }

            // score and gameover
            int score = 0;
            int gameover = 0;

            // read high score from file in Snake/bin/Debug/netcoreapp3.1/highscore.txt
            int highscore = 0;
            if (File.Exists("highscore.txt"))
            {
                Int32.TryParse(File.ReadAllText("highscore.txt"), out highscore);
            }

            // these are needed to set the game at a pace of once per second
            // i would like the game to run faster than this, but that is in the future
            DateTime time;
            DateTime time2;

            /* *
             * Gameplay
             */
            while (true)
            {
                // Clear the console every movement iteration
                PlayArea.ClearConsole(playW, playH);

                // Draw the head
                Console.SetCursorPosition(head.posX, head.posY);
                Console.ForegroundColor = head.colour;
                Console.Write("O");
                // Draw the tail
                for (int i = 0; i < tailX.Count(); i++)
                {
                    Console.SetCursorPosition(tailX[i], tailY[i]);
                    Console.ForegroundColor = head.colour;
                    Console.Write("#");
                    // if head hits tail, declare a gameover
                    if (tailX[i] == head.posX && tailY[i] == head.posY)
                    {
                        gameover = 1;
                    }
                }
                tailX.Add(head.posX);
                tailY.Add(head.posY);
                // make tail only as long as the score
                if (tailX.Count() > score || tailY.Count() > score)
                {
                    tailX.RemoveAt(0);
                    tailY.RemoveAt(0);
                }
                // Draw the fruit
                Console.SetCursorPosition(fruitX, fruitY);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("X");
                Console.CursorVisible = false;

                // Sync to time
                time = DateTime.Now;

                /* *
                 * Movement
                 */
                while (true)
                {
                    time2 = DateTime.Now;
                    // scaling difficulty through different game speeds
                    if (score <= 5)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 500)
                        {
                            break;
                        }
                    }
                    else if (score > 5 && score <= 10)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 400)
                        {
                            break;
                        }
                    }
                    else if (score > 10 && score <= 15)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 300)
                        {
                            break;
                        }
                    }
                    else if (score > 15 && score <= 20)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 300)
                        {
                            break;
                        }
                    }
                    else if (score > 20 && score <= 25)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 200)
                        {
                            break;
                        }
                    }
                    else if (score > 25 && score <= 30)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 100)
                        {
                            break;
                        }
                    }
                    else if (score > 30)
                    {
                        if (time2.Subtract(time).TotalMilliseconds > 50)
                        {
                            break;
                        }
                    }

                    if (Console.KeyAvailable)
                    {
                        // read input
                        ConsoleKeyInfo input = Console.ReadKey(true);

                        // interpet the input
                        if (input.Key.Equals(ConsoleKey.UpArrow))
                        {
                            dir = "u";
                        }
                        if (input.Key.Equals(ConsoleKey.DownArrow))
                        {
                            dir = "d";
                        }
                        if (input.Key.Equals(ConsoleKey.LeftArrow))
                        {
                            dir = "l";
                        }
                        if (input.Key.Equals(ConsoleKey.RightArrow))
                        {
                            dir = "r";
                        }
                    }
                }
                
                /* END MOVEMENT */

                switch (dir)
                {
                    case "u":
                        head.posY--;
                        break;
                    case "d":
                        head.posY++;
                        break;
                    case "l":
                        head.posX--;
                        break;
                    case "r":
                        head.posX++;
                        break;
                }
                    
                /* *
                 * Eating fruit
                 */
                 // if head collides with fruit, award score and draw new fruit
                if (head.posY == fruitY && head.posX == fruitX)
                {
                    score++;
                    fruitY = rand.Next(playH);
                    fruitX = rand.Next(playW);
                }

                /* *
                 * The best part: death
                 */
                // kill if head goes outside boundaries
                if (head.posY == -1 || head.posX == -1 || head.posY == playH || head.posX == playW)
                {
                    gameover = 1;
                }

                // end the application if player reaches a gameover
                if (gameover == 1)
                {
                    break;
                }

            } /* END GAMEPLAY */

            // The game-over screen runs after gameplay
            // clear the area first
            PlayArea.ClearConsole(playW, playH);
            // write the score
            Console.ForegroundColor = ConsoleColor.Red;
            // we place the end text elsewhere than the first line
            Console.SetCursorPosition(3, playH / 3);
            // if score is below high we show both, otherwise just the new high score
            if (score <= highscore)
            {
                Console.WriteLine("Game over!");
                Console.SetCursorPosition(3, playH / 3 + 1);
                Console.WriteLine("Score: " + score);
                Console.SetCursorPosition(3, playH / 3 + 2);
                Console.WriteLine("High Score: " + highscore);
            }
            else if (score > highscore)
            {
                Console.WriteLine("Game over!");
                Console.SetCursorPosition(1, playH / 3 + 1);
                Console.WriteLine("New High Score: " + score);
                // overwrite content of highscore file or create it if it does not exist
                File.WriteAllText ("highscore.txt", score.ToString());
            }
            // place console output lines after game
            // expect an input before actually closing the program to make it more interactive
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            // reset console size
            Console.WindowWidth = conW;
            Console.WindowHeight = conH;

        } /* END MAIN */

    }
}
