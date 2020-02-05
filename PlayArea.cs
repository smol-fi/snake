using System;
using System.Linq;

namespace Snake
{
    class PlayArea
    {
        public static void ClearConsole(int playW, int playH)
        {
            var blackLine = string.Join("", new byte[playW].Select(b => " ").ToArray());
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i=0; i < playH; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(blackLine);
            }
        }
    }
}
