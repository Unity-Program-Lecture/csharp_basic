using System;
using System.Collections.Generic;
using LiteDB;

namespace GameDatabaseLab
{
    public class QuestProgress
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string QuestId { get; set; } = "";
        public int KillCount { get; set; }
        public int TargetCount { get; set; }
        public bool IsCompleted { get; set; }
        public List<string> RewardIds { get; set; } = new List<string>();
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            using (LiteDatabase database = new LiteDatabase("QuestProgress.db"))
            {
                ILiteCollection<QuestProgress> quests =
                    database.GetCollection<QuestProgress>("quests");
                quests.EnsureIndex(x => x.PlayerId);

                QuestProgress quest = quests.FindOne(x =>
                    x.PlayerId == 1 && x.QuestId == "GoblinHunt");

                if (quest == null)
                {
                    quest = new QuestProgress
                    {
                        PlayerId = 1,
                        QuestId = "GoblinHunt",
                        TargetCount = 3,
                        RewardIds = new List<string> { "Potion" }
                    };
                    quests.Insert(quest);
                }

                Console.WriteLine("고블린 퀘스트를 불러왔습니다.");
                Console.WriteLine("처치 수: " + quest.KillCount + "/" + quest.TargetCount);
                Console.WriteLine("완료 여부: " + quest.IsCompleted);

                while (true)
                {
                    Console.Write("1: 고블린 한 마리 처치 | r: 진행 상태 초기화 | q: 종료 > ");
                    string input = Console.ReadLine();

                    if (input == null || input == "q" || input == "Q")
                    {
                        break;
                    }

                    if (input == "r" || input == "R")
                    {
                        quest.KillCount = 0;
                        quest.IsCompleted = false;
                        quests.Update(quest);

                        Console.WriteLine("고블린 퀘스트 진행 상태를 초기화했습니다.");
                        Console.WriteLine("처치 수: " + quest.KillCount + "/" + quest.TargetCount);
                        Console.WriteLine("완료 여부: " + quest.IsCompleted);
                        continue;
                    }

                    if (input != "1")
                    {
                        Console.WriteLine("1, r 또는 q를 입력하세요.");
                        continue;
                    }

                    if (quest.IsCompleted)
                    {
                        Console.WriteLine("이미 완료한 퀘스트입니다.");
                        continue;
                    }

                    quest.KillCount++;
                    quest.IsCompleted = quest.KillCount >= quest.TargetCount;
                    quests.Update(quest);

                    Console.WriteLine("고블린을 한 마리 처치했습니다.");
                    Console.WriteLine("처치 수: " + quest.KillCount + "/" + quest.TargetCount);
                    Console.WriteLine("완료 여부: " + quest.IsCompleted);
                }
            }
        }
    }
}
