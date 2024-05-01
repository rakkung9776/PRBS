using HitterGame;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Baseball_Final
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("학번을 입력하세요.");
            string playerID = Console.ReadLine();
            bool playAgain = false;
            string choice;
            bool lobbyMessageShown = false;

            do
            {
                do
                {               
                    Console.Clear();
                    Console.WriteLine("\n===================================================");
                    Console.WriteLine($"\n\n{new string(' ', 16)}|~) _  _ _ |_  _ ||");
                    Console.WriteLine($"{new string(' ', 16)}|_)(_|_\\(/_|_)(_|||");
                    Console.WriteLine($"\n\n{new string(' ', 19)}1. 게임 시작");
                    Console.WriteLine($"{new string(' ', 19)}2. 게임 설명");
                    Console.WriteLine($"{new string(' ', 19)}3. 게임 종료");
                    Console.WriteLine("\n\n===================================================\n\n");

                    Console.Write("\n메뉴를 선택하세요: ");
                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            StartGame(playerID);
                            if (!AskToPlayAgain())
                            {
                                Console.WriteLine("게임을 종료합니다.");
                                return;
                            }
                            break;
                        case "2":
                            ShowGameInstructions();
                            break;
                        case "3":
                            Console.WriteLine("게임을 종료합니다.");
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
                            break;
                    }
                } while (choice != "1" && choice != "2" && choice != "3");

                do
                {
                    if (!lobbyMessageShown) // 메시지가 아직 출력되지 않았다면
                    {
                        Console.Write("\n로비로 돌아가시겠습니까? (예: y / 아니오: n): ");
                        lobbyMessageShown = true; // 메시지 출력 여부를 true로 설정하여 한 번만 출력되도록 합니다.
                    }

                    string backToLobbyInput = Console.ReadLine().ToLower();
                    if (backToLobbyInput == "y" || backToLobbyInput == "yes")
                    {
                        playAgain = true;
                        lobbyMessageShown = false;
                        break; // 루프를 종료하고 게임 메뉴로 돌아감
                    }
                    else if (backToLobbyInput == "n" || backToLobbyInput == "no")
                    {
                        playAgain = true;

                    }
                    else
                    {

                    }
                } while (true);

            } while (playAgain);
        }

        static void ShowGameInstructions()
        {
            Console.Clear();
            Console.WriteLine("\n===================================================");
            Console.WriteLine($"\n{new string(' ', 13)}_  _  _ _  _    _   | _  _");
            Console.WriteLine($"{new string(' ', 12)}(_|(_|| | |(/_  | |_||(/__\\");
            Console.WriteLine($"{new string(' ', 12)} _|                        ");
            Console.WriteLine($"\n\n\n{new string(' ', 3)}1. 플레이어는 타자의 시점에서 게임을 진행한다.");
            Console.WriteLine($"{new string(' ', 3)}2. 게임이 시작되면 4번의 타석에 설 수 있으며");
            Console.WriteLine($"{new string(' ', 6)}선택지를 통해 투수의 공을 예상하여 공을");
            Console.WriteLine($"{new string(' ', 6)}타격하는 방식이다.");
            Console.WriteLine("===================================================\n\n");
        }

        static void StartGame(string playerID)
        {

            do
            {
                int ahnta = 0;
                int homerun = 0;
                int outs = 0;
                int totalTrials = 0;
                double score = 0;

                Random random = new Random();

                Console.Clear();
                DrawingObject bigWindow = new DrawingObject(70, 26);
                bigWindow.DrawIntro();
                Console.ReadKey();

                // 게임이 종료되지 않는 동안 반복
                while (outs < 1) // 아웃이 1회 이상 발생할 때까지 반복
                {
                    Console.Clear();
                    bigWindow.Draw();

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("============================================");
                    Console.SetCursorPosition(1, 1);
                    Console.WriteLine("||" + $"== {new string(' ', 61)} ==||");
                    Console.SetCursorPosition(1, 2);
                    Console.WriteLine("||" + $"== {new string(' ', 16)}{outs} 아웃, {totalTrials} 볼넷, {ahnta} 안타, {homerun} 홈런{new string(' ', 16)}" + "==||");
                    Console.SetCursorPosition(1, 3);
                    Console.WriteLine("||" + $"== {new string(' ', 61)} ==||");
                    Console.SetCursorPosition(0, 4);
                    Console.WriteLine("==========================================================================");

                    Console.SetCursorPosition(3, 8); // 메시지 출력 위치 변경

                    bool swingBat = GetSwingBatDecision();

                    if (swingBat)
                    {
                        Console.SetCursorPosition(3, 8); // 메시지 출력 위치 변경
                        Console.WriteLine("배트를 휘두르셨습니다!");
                        int hitOutcome = random.Next(1, 101); // 1부터 100 사이의 랜덤한 숫자

                        if (hitOutcome <= 5) // 5% 확률로 홈런
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.SetCursorPosition(13, 10);
                            Console.WriteLine("   _ __                     ___                ");
                            Console.SetCursorPosition(13, 11);
                            Console.WriteLine("  /// /  _   _    __       / o |      _    __");
                            Console.SetCursorPosition(13, 12);
                            Console.WriteLine(" / ` / ,'o| / \\','o/     /  ,' /7/7 / \\/7 (c'");
                            Console.SetCursorPosition(13, 13);
                            Console.WriteLine("/_n_/  |_,'/_nn_/|_(     /_/`_\\ /__/ /_n_/ /__)");
                            Console.SetCursorPosition(28, 17);
                            Console.WriteLine("~~~!!홈 런!!~~~");
                            Console.ResetColor();
                            homerun++;
                            score += 1;
                        }
                        else if (hitOutcome <= 70) // 65% 확률로 안타
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(3, 10);
                            Console.WriteLine("   _ __           ");
                            Console.SetCursorPosition(3, 11);
                            Console.WriteLine("  /// /  ()  /7  __");
                            Console.SetCursorPosition(3, 12);
                            Console.WriteLine(" / ` /  /7  /_7 (c'");
                            Console.SetCursorPosition(3, 13);
                            Console.WriteLine("/_n_/  //  //  /__)");
                            Console.SetCursorPosition(3, 17);
                            Console.WriteLine("안타!");
                            Console.ResetColor();
                            ahnta++;
                            score += 0.25;
                        }
                        else // 30% 확률로 아웃
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(3, 10);
                            Console.WriteLine("    _   _ __ _____  ");
                            Console.SetCursorPosition(3, 11);
                            Console.WriteLine("  ,' \\ /// //_  _/ ");
                            Console.SetCursorPosition(3, 12);
                            Console.WriteLine(" / o |/ U /  / /   ");
                            Console.SetCursorPosition(3, 13);
                            Console.WriteLine(" |_,' \\_,'  /_/     ");
                            Console.SetCursorPosition(3, 16);
                            Console.WriteLine("아웃!");
                            Console.ResetColor();
                            outs++;
                            score -= 1;
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(3, 8);
                        Console.WriteLine("배트를 휘두르지 않았습니다!");

                        if (outs < 1) // 아웃이 아닌 상태에서만 볼넷 발생
                        {
                            int hitOutcome = random.Next(1, 101); // 1부터 100 사이의 랜덤한 숫자

                            if (hitOutcome <= 50) // 50% 확률로 볼넷
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.SetCursorPosition(3, 10);
                                Console.WriteLine("   ___                                   ___                ");
                                Console.SetCursorPosition(3, 11);
                                Console.WriteLine("  / o.)  _   __  __       _   _          / o.)  _   /7  /7  __");
                                Console.SetCursorPosition(3, 12);
                                Console.WriteLine(" / o \\ ,'o| (c','o/     ,'o| / \\/7      / o \\ ,'o| //  //  (c'");
                                Console.SetCursorPosition(3, 13);
                                Console.WriteLine("/___,' |_,7/__)|_(      |_,'/_n_/      /___,' |_,7//  //  /__)");
                                Console.SetCursorPosition(3, 17);
                                Console.WriteLine("볼넷!");
                                Console.ResetColor();
                                totalTrials++; // 볼넷 발생 시 볼넷 횟수 증가
                                score += 0.25;
                            }
                            else // 50% 확률로 아웃
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.SetCursorPosition(3, 10);
                                Console.WriteLine("    _   _ __ _____  ");
                                Console.SetCursorPosition(3, 11);
                                Console.WriteLine("  ,' \\ /// //_  _/ ");
                                Console.SetCursorPosition(3, 12);
                                Console.WriteLine(" / o |/ U /  / /   ");
                                Console.SetCursorPosition(3, 13);
                                Console.WriteLine(" |_,' \\_,'  /_/     ");
                                Console.SetCursorPosition(3, 16);
                                Console.WriteLine("아웃!");
                                Console.ResetColor();
                                outs++;
                                score -= 1;
                            }
                        }
                    }

                    Console.SetCursorPosition(3, 24);
                    Console.WriteLine("계속하려면 Enter 키를 누르세요...");
                    Console.SetCursorPosition(3, 24);
                    Console.ReadLine();

                }

                int all = ahnta + homerun + outs + totalTrials; // 총 타수 계산

                Console.SetCursorPosition(3, 18);
                Console.WriteLine("게임 종료!");
                Console.SetCursorPosition(8, 2);
                Console.WriteLine($"최종 결과: {score} 점, {all} 타수, {homerun} 홈런, {outs} 아웃, {totalTrials} 볼넷, {ahnta} 안타");
                Console.SetCursorPosition(3, 24);

                // Google 스프레드시트에 데이터 전송
                SendDataToGoogleSheet(playerID, $"{DateTime.Now.Month}월 {DateTime.Now.Day}일", $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}", score, all, homerun, outs, totalTrials, ahnta);

            } while (AskToPlayAgain());
        }

        // 사용자 입력 받아서 예 또는 아니오 반환
        static bool GetSwingBatDecision()
        {
            Console.SetCursorPosition(3, 6);
            Console.WriteLine("배트를 휘두르시겠습니까? (예: y / 아니오: n)");
            Console.SetCursorPosition(3, 24);
            string input = Console.ReadLine().ToLower();


            return input == "y" || input == "yes";
        }

        static bool AskToPlayAgain()
        {
            Console.SetCursorPosition(3, 24);
            Console.WriteLine("게임을 다시 시작하시겠습니까? (예: y / 아니오: n) : ");
            Console.SetCursorPosition(55, 24);
            string input = Console.ReadLine().ToLower();
            return input == "y" || input == "yes";
        }

        static async Task SendDataToGoogleSheet(string playerID, string logintime, string nowtime, double score, int all, int homerun, int outs, int totalTrials, int ahnta)
        {
            try
            {
                // Google 스프레드시트로 전송할 URL
                string url = "https://script.google.com/macros/s/AKfycby_NB11nqrGzW5T7lQ-UhMpNKVExpPsebaLUm7I2jB_hXqVvdKAbAEX0_Yo4s-6RQ7T/exec"; // AppsScript URL 적기 /exec

                // 파라미터 설정
                string parameters = $"?playerID={playerID}&logintime={logintime}&nowtime={nowtime}&score={score}&all={all}&homerun={homerun}&outs={outs}&totalTrials={totalTrials}&ahnta={ahnta}";

                // 최종 URL 조합
                string fullUrl = url + parameters;

                // HttpClient 생성
                using (HttpClient client = new HttpClient())
                {
                    // GET 요청 보내기
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    // 응답 결과 확인
                    if (response.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(3, 20);
                        Console.WriteLine("Google 스프레드시트에 데이터가 전송되었습니다.");
                        Console.ResetColor();
                        Console.SetCursorPosition(55, 24);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(3, 20);
                        Console.WriteLine($"Google 스프레드시트에 데이터 전송 실패! 상태 코드: {response.StatusCode}");
                        Console.ResetColor();
                        Console.SetCursorPosition(55, 24);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(3, 20);
                Console.WriteLine($"Google 스프레드시트에 데이터 전송 중 오류 발생: {ex.Message}");
                Console.ResetColor();
                Console.SetCursorPosition(55, 24);
            }
        }
    }
}