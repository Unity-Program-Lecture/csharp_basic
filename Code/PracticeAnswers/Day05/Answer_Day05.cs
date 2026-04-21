using System;

namespace PracticeAnswers.Day05
{
    class Monster
    {
        public string name;
        public Monster(string name) { this.name = name; }
        public void Attack() { Console.WriteLine($"{name}이(가) 공격합니다!"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1. [배열선언]
            int[] scores = new int[5];

            // 2. [값넣기]
            scores[0] = 100;
            scores[4] = 50;

            // 3. [출력]
            foreach (int s in scores) Console.Write(s + " ");
            Console.WriteLine();

            // 4. [검색]
            int[] data = { 10, 50, 80, 20, 40 };
            int max = data[0];
            foreach (int d in data) if (d > max) max = d;
            Console.WriteLine("최댓값: " + max);

            // 5. [평균]
            int total = 0;
            foreach (int d in data) total += d;
            Console.WriteLine("평균: " + (total / (float)data.Length));

            // 7. [랜덤치명타]
            int rand = new Random().Next(1, 101);
            if (rand <= 20) Console.WriteLine("치명타!");

            // 8. [가위바위보] (간략화)
            Console.Write("가위(0), 바위(1), 보(2) 입력: ");
            int user = int.Parse(Console.ReadLine());
            int com = new Random().Next(0, 3);
            Console.WriteLine($"컴퓨터: {com}");
            if (user == com) Console.WriteLine("비김");
            else if ((user + 1) % 3 == com) Console.WriteLine("패배");
            else Console.WriteLine("승리");

            // 9. [배열뒤집기]
            int[] arr = { 1, 2, 3, 4, 5 };
            Array.Reverse(arr);
            foreach (int i in arr) Console.Write(i + " ");
            Console.WriteLine();

            // 10. [종합미션]
            Monster[] monsters = new Monster[5];
            string[] names = { "슬라임", "고블린", "오크", "드래곤", "해골" };
            for (int i = 0; i < monsters.Length; i++)
            {
                monsters[i] = new Monster(names[i]);
            }
            foreach (var m in monsters) m.Attack();
        }
    }
}
