using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitterGame
{
    internal class DrawingObject
    {
        private readonly int width;
        private readonly int height;
        private int strikes;
        private int balls;
        private int outs;
        private int totalTrials;
        private int ahnta;
        private int jangta;
        public void DrawIntro()
        {
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine("\n 야구 게임을 시작합니다!\n");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
            Console.WriteLine($"\n{new string(' ', 12)}*");
        }

        public DrawingObject(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Draw()
        {
            Console.Clear();
            DrawBorder();
        }

        public void Draw02()
        {
            Console.Clear();
            DrawBorder02();
        }

        private void DrawBorder()
        {
            Console.WriteLine(new string('=', width + 4));
            for (int i = 0; i < height; i++)
            {
                Console.WriteLine($"||{new string(' ', width)}||");
            }
            Console.WriteLine(new string('=', width + 4));
            Console.SetCursorPosition(2, 22);
            Console.WriteLine("======================================================================");
        }

        private void DrawBorder02()
        {
            Console.WriteLine(new string('=', width + 4));
            for (int i = 0; i < height; i++)
            {
                Console.WriteLine($"||{new string(' ', width)}||");
            }
            Console.WriteLine(new string('=', width + 4));
        }

    }
}
