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

        static void StartGame(string playerID)
        {
            do
            {
                int ahnta = 0;
                int homerun = 0;
                int outs = 0;
                int totalTrials = 0;
                double score = 0;

                // 게임 종료 후 최종 결과 출력
                int all = ahnta + homerun + outs + totalTrials; // 전체 타석 수 계산
                

                SendDataToGoogleSheet(playerID, $"{DateTime.Now.Month}월 {DateTime.Now.Day}일", $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}", score, all, homerun, outs, totalTrials, ahnta);

            }

            SavePlayerData(); // 게임 종료 후 플레이어 데이터 저장
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
                //Console.SetCursorPosition(3, 20);
                //Console.WriteLine("Google 스프레드시트에 데이터가 전송되었습니다.");
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


    }
}