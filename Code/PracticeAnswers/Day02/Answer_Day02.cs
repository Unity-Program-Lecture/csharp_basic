using System;

namespace PracticeAnswers.Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. [조건]
            int score = 75;
            if (score >= 60) Console.WriteLine("합격");
            else Console.WriteLine("불합격");

            // 2. [홀짝]
            Console.Write("숫자 입력: ");
            int num = int.Parse(Console.ReadLine());
            if (num % 2 == 0) Console.WriteLine("짝");
            else Console.WriteLine("홀");

            // 3. [반복]
            for (int i = 1; i <= 10; i++) Console.WriteLine(i);

            // 4. [구구단]
            for (int i = 1; i <= 9; i++) Console.WriteLine($"2 * {i} = {2 * i}");

            // 5. [누적]
            int sum = 0;
            for (int i = 1; i <= 100; i++) sum += i;
            Console.WriteLine("합계: " + sum);

            // 6. [무한루프]
            while (true)
            {
                Console.Write("숫자 입력(0이면 종료): ");
                if (int.Parse(Console.ReadLine()) == 0) break;
            }

            // 7. [스위치]
            int weaponType = 1;
            switch (weaponType)
            {
                case 1: Console.WriteLine("검"); break;
                case 2: Console.WriteLine("활"); break;
                case 3: Console.WriteLine("지팡이"); break;
            }

            // 8. [다중조건]
            int age = 15;
            if (age >= 10 && age < 20) Console.WriteLine("청소년");

            // 9. [역순]
            for (int i = 10; i >= 1; i--) Console.WriteLine(i);

            // 10. [미션]
            int monsterHp = 100;
            while (monsterHp > 0)
            {
                monsterHp -= 15;
                Console.WriteLine($"공격! 남은 HP: {monsterHp}");
            }
            Console.WriteLine("처치 완료");
        }
    }
}
