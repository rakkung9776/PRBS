using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System.Text;
using Google.Apis.Sheets.v4.Data;
using HitterGame;
using Newtonsoft.Json.Linq;
using System.Threading;
using NAudio.Wave;

namespace Baseball_Final
{
    class Program
    {
        static Dictionary<string, int> playerAValues = new Dictionary<string, int>(); // 아이디 별 J열 값 저장
        static void Main(string[] args)
        {
            LoadPlayerData();

            Console.WriteLine("학번을 입력하세요.");
            string playerID = Console.ReadLine();
            bool playAgain = false;
            string choice;
            bool lobbyMessageShown = false;


            Task backgroundMusicTask = Task.Run(() => PlayBackgroundMusicAsync());
            DrawingObject bigWindow = new DrawingObject(70, 26);

            do
            {
                do
                {
                    Console.Clear();
                    bigWindow.MainDraw();

                    Console.SetCursorPosition(37, 19);
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
                        Console.Clear();
                        playAgain = true;
                        ShowGameInstructions();
                    }
                    else
                    {
                        Console.Clear();
                        playAgain = true;
                        ShowGameInstructions();
                    }
                } while (true);

            } while (playAgain);
        
        }

        static void LoadPlayerData()
        {
            string dataFilePath = "player_data.txt";

            if (File.Exists(dataFilePath))
            {
                // 파일이 존재시 데이터를 불러옴.
                string[] lines = File.ReadAllLines(dataFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        string playerID = parts[0];
                        int aValue = int.Parse(parts[1]);
                        playerAValues[playerID] = aValue;
                    }
                }
            }
        }

        static void ShowGameInstructions()
        {
            Console.Clear();
            DrawingObject bigWindow = new DrawingObject(70, 26);
            bigWindow.Draw02();

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

                while (outs < 1)
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

                    Console.SetCursorPosition(3, 8);

                    bool swingBat = GetSwingBatDecision();

                    if (swingBat)
                    {
                        Console.SetCursorPosition(3, 8);
                        Console.WriteLine("배트를 휘두르셨습니다!");
                        int hitOutcome = random.Next(1, 101);

                        if (hitOutcome <= 5)
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
                        else if (hitOutcome <= 70)
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
                        else
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

                        if (outs < 1)
                        {
                            int hitOutcome = random.Next(1, 101);

                            if (hitOutcome <= 50)
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
                                totalTrials++;
                                score += 0.25;
                            }
                            else
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
                    Console.ReadKey();
                }

                // 게임 종료 후 최종 결과 출력
                int all = ahnta + homerun + outs + totalTrials; // 전체 타석 수 계산
                Console.SetCursorPosition(3, 18);
                Console.WriteLine("게임 종료!");
                Console.SetCursorPosition(8, 2);
                Console.WriteLine($"최종 결과: {score} 점, {all} 타수, {homerun} 홈런, {outs} 아웃, {totalTrials} 볼넷, {ahnta} 안타");
                Console.SetCursorPosition(3, 24);

                SendDataToGoogleSheet(playerID, $"{DateTime.Now.Month}월 {DateTime.Now.Day}일", $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}", score, all, homerun, outs, totalTrials, ahnta);

            } while (AskToPlayAgain());

            SavePlayerData(); // 게임 종료 후 플레이어 데이터 저장
        }

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
                string jsonPath = "gamedata-202327036-4236f51ed467.json"; // OAuth2.0 클라이언트 JSON 파일 경로
                string[] scopes = { SheetsService.Scope.Spreadsheets }; // Google Sheets API 사용 스코프

                // 인증 파일에서 자격 증명을 가져옴
                GoogleCredential credential;
                using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(scopes);
                }

                // Google Sheets 서비스 생성
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Baseball_Final"
                });

                // 스프레드시트 ID 및 시트 이름 설정
                string spreadsheetId = "11aUuY8GS1kjUC4re-_sQolyFeq5SD7nZgAw6HDMY9eE"; // 스프레드시트 ID
                string range = "GameData!A2"; // 시트 이름 또는 범위

                // 데이터 입력
                var valueRange = new ValueRange();
                var oblist = new List<object>() { GetNextJValue(playerID), playerID, logintime, nowtime, score, all, homerun, ahnta, totalTrials, outs };
                valueRange.Values = new List<IList<object>> { oblist };

                // 데이터를 스프레드시트에 추가
                SpreadsheetsResource.ValuesResource.AppendRequest request =
                    service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
                request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
                request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
                await request.ExecuteAsync();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(3, 20);
                Console.WriteLine("Google 스프레드시트에 데이터가 전송되었습니다.");
                Console.ResetColor();
                Console.SetCursorPosition(55, 24);
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

        static void SavePlayerData()
        {
            string dataFilePath = "player_data.txt";

            // 플레이어 데이터를 파일에 저장합니다.
            using (StreamWriter writer = new StreamWriter(dataFilePath))
            {
                foreach (var kvp in playerAValues)
                {
                    writer.WriteLine($"{kvp.Key},{kvp.Value}");
                }
            }
        }

        static int GetNextJValue(string playerID)
        {
            if (!playerAValues.ContainsKey(playerID))
            {
                playerAValues[playerID] = 0; // 처음 값을 설정합니다.
            }

            return ++playerAValues[playerID];
        }

        static async Task PlayBackgroundMusicAsync()
        {
            try
            {
                string executablePath = AppDomain.CurrentDomain.BaseDirectory;
                string backgroundMusicPath = Path.Combine(executablePath, "Background.wav");

                while (true)
                {
                    if (File.Exists(backgroundMusicPath))
                    {
                        using (var audioFile = new AudioFileReader(backgroundMusicPath))
                        using (var outputDevice = new WaveOutEvent())
                        {
                            outputDevice.Init(audioFile);
                            outputDevice.Play();

                            // 재생이 완료될 때까지 대기
                            while (outputDevice.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("배경 음악 파일을 찾을 수 없습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"배경 음악을 재생하는 중 오류 발생: {ex.Message}");
            }
        }

    }
}