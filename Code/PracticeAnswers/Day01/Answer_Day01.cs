using System;

namespace PracticeAnswers.Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. [출력]
            Console.WriteLine("홍길동");

            // 2. [정수]
            int age = 20;
            Console.WriteLine(age);

            // 3. [실수]
            float moveSpeed = 5.5f;
            Console.WriteLine(moveSpeed);

            // 4. [문자열]
            string gameTitle = "League of Legends";
            Console.WriteLine(gameTitle);

            // 5. [논리]
            bool isGaming = true;
            Console.WriteLine(isGaming);

            // 6. [덧셈]
            int a = 10;
            int b = 20;
            Console.WriteLine(a + b);

            // 7. [교환]
            int x = 10;
            int y = 20;
            int temp = x;
            x = y;
            y = temp;
            Console.WriteLine($"x: {x}, y: {y}");

            // 8. [연결]
            int level = 1;
            Console.WriteLine("나의 레벨은 " + level);

            // 9. [오류찾기]
            // 오류: int는 정수만 담는데 소수점(95.5)을 넣으려 함.
            // 수정: float score = 95.5f; 또는 int score = 95;
            float score = 95.5f;
            Console.WriteLine(score);

            // 10. [종합]
            string charName = "용사";
            int charLevel = 10;
            bool charAlive = true;
            Console.WriteLine($"이름: {charName}\n레벨: {charLevel}\n생존: {charAlive}");
        }
    }
}
