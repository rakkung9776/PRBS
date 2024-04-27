using System;
using System.IO;
class Program
{
    static void Main(string[] args)
    {
        bool playAgain = false;
        bool lobbyMessageShown = false;
        string choice;

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
                        StartGame(args);
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

    static void StartGame(string[] args)
    {
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

        // 파일 경로
        string filePath = "savedata.txt";

        int strikes = 0;
        int ahnta = 0;
        int jangta = 0;
        int balls = 0;
        int outs = 0;
        int totalTrials = 0;
        int swingLocation = 0;
        int all = 0;

        Random random = new Random();

        // 게임이 종료되지 않는 동안 반복
        while (outs + totalTrials + ahnta + jangta < 4) // 아웃과 볼넷의 합이 4가 될 때까지 반복
        {
            Console.WriteLine("\n\n===================================================");
            Console.WriteLine("||" + $"== {strikes} 스트라이크, {balls} 볼, {outs} 아웃, {totalTrials} 볼넷{new string(' ', 7)} " + "==||");
            Console.WriteLine("||" + $"== {ahnta} 안타, {jangta} 장타{new string(' ', 28)}" + "==||");
            Console.WriteLine("===================================================\n\n");


            bool swingBat = GetSwingBatDecision();

            if (swingBat)
            {
                swingLocation = GetSwingLocation();
                Console.WriteLine($"\n배트를 휘두르셨습니다! 선택한 위치: {GetLocationName(swingLocation)}");
            }
            else
            {
                Console.WriteLine("\n배트를 휘두르지 않았습니다!");
            }

            Console.WriteLine("\n투구!");
            int pitchDirection = GetPitchDirection();
            Console.WriteLine($"투구 방향: {GetLocationName(pitchDirection)}");
            DeterminePitchResult(swingBat, swingLocation, pitchDirection, ref strikes, ref balls, ref outs, ref totalTrials, ref jangta, ref ahnta);


        }

        Console.WriteLine("\n\n게임 종료! \n\n");

        Console.WriteLine("===================================================");
        Console.WriteLine("||" + $"최종 결과: 4타석 {outs} 아웃, {totalTrials} 볼넷, {ahnta} 안타, {jangta} 장타||");
        Console.WriteLine("===================================================\n"); ;

        all += 4;


        // 파일에 저장할 결과 적음
        SaveGameResult(filePath, all, outs, totalTrials, ahnta, jangta);
    }

    // 사용자 입력 받아서 예 또는 아니오 반환
    static bool GetSwingBatDecision()
    {
        Console.WriteLine("배트를 휘두르시겠습니까? (예: y / 아니오: n)");
        string input = Console.ReadLine().ToLower();

        return input == "y" || input == "yes";
    }

    // 사용자로부터 배트 휘두를 위치 입력 받기
    static int GetSwingLocation()
    {
        Console.WriteLine("\n배트를 휘두를 위치를 선택하세요:");
        Console.WriteLine("1. 위쪽");
        Console.WriteLine("2. 가운데");
        Console.WriteLine("3. 아래쪽");

        int choice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 3)
            {
                break;
            }
            Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
        }

        return choice;
    }

    // 위치 이름 반환
    static string GetLocationName(int location)
    {
        switch (location)
        {
            case 1:
                return "위쪽";
            case 2:
                return "가운데";
            case 3:
                return "아래쪽";
            default:
                return "알 수 없음";
        }
    }

    // 투구 방향 랜덤하게 반환
    static int GetPitchDirection()
    {
        Random random = new Random();
        return random.Next(1, 4); // 1부터 3까지의 랜덤한 숫자 반환
    }

    // 투구 결과 결정
    static void DeterminePitchResult(bool swungBat, int swingLocation, int pitchDirection, ref int strikes, ref int balls, ref int outs, ref int totalTrials, ref int jangta, ref int ahnta)
    {
        Random random = new Random();

        if (swungBat)
        {
            if (swingLocation == pitchDirection) // 사용자가 선택한 위치와 투구 방향이 일치하는 경우
            {
                int hitOutcome = random.Next(1, 101); // 1부터 100 사이의 랜덤한 숫자

                if (hitOutcome <= 25) // 25% 확률로 장타
                {
                    Console.WriteLine("장타!");

                    jangta++;
                    strikes = 0; // 아웃 후 스트라이크 초기화
                    balls = 0; // 아웃 후 볼 초기화
                               // 장타 처리에 필요한 로직 추가
                }
                else if (hitOutcome <= 60) // 35% 확률로 안타
                {
                    Console.WriteLine("안타!");
                    ahnta++;
                    strikes = 0; // 아웃 후 스트라이크 초기화
                    balls = 0; // 아웃 후 볼 초기화
                               // 안타 처리에 필요한 로직 추가
                }
                else // 40% 확률로 아웃
                {
                    Console.WriteLine("아웃!");
                    outs++;
                    strikes = 0; // 아웃 후 스트라이크 초기화
                    balls = 0; // 아웃 후 볼 초기화
                }
            }
            else // 선택한 위치와 투구 방향이 일치하지 않는 경우는 스트라이크 처리
            {
                Console.WriteLine("스트라이크!");
                strikes++;
            }
        }
        else
        {
            if (pitchDirection != 1) // 스윙을 하지 않았고 위쪽이 아닌 경우에만 볼 카운트
            {
                Console.WriteLine("볼!");
                balls++;
            }
            else
            {
                Console.WriteLine("스트라이크!");
                strikes++;
            }
        }

        if (strikes == 3)
        {
            Console.WriteLine("삼진 아웃!");
            outs++;
            strikes = 0; // 아웃 후 스트라이크 초기화
            balls = 0; // 아웃 후 볼 초기화
        }
        else if (balls == 4)
        {
            Console.WriteLine("볼넷 진루!");
            balls = 0; // 볼넷 시 볼 초기화
            strikes = 0; // 볼넷 시 스트라이크 초기화
            totalTrials++; // 볼넷 발생 시 볼넷 횟수 증가
        }
    }

    // 데이터 누적 시키면서 총 타석까지 같이 결과 추가
    static void SaveGameResult(string filePath, int all, int outs, int totalTrials, int ahnta, int jangta)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine("==============================");
            writer.WriteLine($"총 타석: {all}");
            writer.WriteLine($"아웃: {outs}");
            writer.WriteLine($"볼넷: {totalTrials}");
            writer.WriteLine($"안타: {ahnta}");
            writer.WriteLine($"장타: {jangta}");
            writer.WriteLine("==============================");
        }

        Console.WriteLine($"결과값이 {filePath}에 저장되었습니다. 결과는 누적됩니다.");
    }



    static bool AskToPlayAgain()
    {
        Console.WriteLine("게임을 다시 시작하시겠습니까? (예: y / 아니오: n)");
        string input = Console.ReadLine().ToLower();

        return input == "y" || input == "yes";
    }
}

