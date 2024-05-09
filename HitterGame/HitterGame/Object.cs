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
            Console.SetCursorPosition(22, 1);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 3);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 5);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 7);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 9);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 11);
            Console.WriteLine($"*");
            Console.SetCursorPosition(11, 13);
            Console.WriteLine("야구 게임을 시작합니다!");
            Console.SetCursorPosition(22, 15);
            Console.WriteLine($"*");
            Console.SetCursorPosition(14, 17);
            Console.WriteLine("아무키나 누르세요");
            Console.SetCursorPosition(22, 19);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 21);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 23);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 25);
            Console.WriteLine($"*");
            Console.SetCursorPosition(22, 27);
            Console.WriteLine($"*");
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
        public void MainDraw()
        {
            Console.Clear();
            DrawBorder02();
            MainView();
        }

        public void manualView()
        {
            Console.Clear();
            DrawBorder02();
            MainView();
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

        private void manual()
        {
            Console.Clear();

            Console.SetCursorPosition(28, 4);
            Console.WriteLine("._ _  _ ._     _ |");
            Console.SetCursorPosition(28, 5);
            Console.WriteLine("| | |(_|| ||_|(_||");
            Console.SetCursorPosition(13, 8);
            Console.WriteLine($"1. 플레이어는 타자의 시점에서 게임을 진행한다.");
            Console.SetCursorPosition(13, 9);
            Console.WriteLine($"2. 게임이 시작되면 4번의 타석에 설 수 있으며");
            Console.SetCursorPosition(16, 10);
            Console.WriteLine($"선택지를 통해 투수의 공을 예상하여 공을");
            Console.SetCursorPosition(16, 11);
            Console.WriteLine($"타격하는 방식이다.");

            Console.SetCursorPosition(13, 21);
            Console.Write("로비로 돌아가시겠습니까? (예: y / 아니오: n): ");
        }
        private void MainView()
        {
            Console.SetCursorPosition(3, 3);
            Console.WriteLine("._____      _        _____._______._____      _      __      __      ");
            Console.SetCursorPosition(3, 4);
            Console.WriteLine("|  _  \\    / \\      /     ||  ___||  _  \\    / \\    |  |    |  |     ");
            Console.SetCursorPosition(3, 5);
            Console.WriteLine("| |_)  |  / ^ \\    |   (--`| |___ | |_)  |  / ^ \\   |  |    |  |     ");
            Console.SetCursorPosition(3, 6);
            Console.WriteLine("|  _  <  / /_\\ \\    \\   \\  |  ___||  _  <  / /_\\ \\  |  |    |  |     ");
            Console.SetCursorPosition(3, 7);
            Console.WriteLine("| |_)  |/  ___  \\ --)|   | | |___ | |_)  |/  ___  \\ |  `---.|  `---.");
            Console.SetCursorPosition(3, 8);
            Console.WriteLine("|_____//__/   \\__\\|_____/  |_____||_____//__/   \\__\\|______||______|");

            Console.SetCursorPosition(29, 11);
            Console.WriteLine("- 내일은 타격왕 -");

            Console.SetCursorPosition(31, 15);
            Console.WriteLine("1. 게임 시작");
            Console.SetCursorPosition(31, 16);
            Console.WriteLine("2. 게임 설명");
            Console.SetCursorPosition(31, 17);
            Console.WriteLine("3. 게임 종료");
        }

    }
}
