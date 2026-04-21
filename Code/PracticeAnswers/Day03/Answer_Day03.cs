using System;

namespace PracticeAnswers.Day03
{
    class Player
    {
        public string name;
        private int hp = 50;

        public void Heal() { hp += 10; }
        public int GetHP() { return hp; }
        public void TakeDamage(int damage) { hp -= damage; }
    }

    class Monster
    {
        public int GetAtk() { return 15; }
    }

    class Program
    {
        // 1. [메소드]
        static int Add(int a, int b) { return a + b; }

        // 2. [출력함수]
        static void Greet(string name) { Console.WriteLine($"안녕하세요, {name}님!"); }

        // 3. [랜덤]
        static int RollDice() { return new Random().Next(1, 7); }

        static void Main(string[] args)
        {
            Console.WriteLine(Add(5, 3));
            Greet("Gemini");
            Console.WriteLine("주사위: " + RollDice());

            // 5. [인스턴스]
            Player p1 = new Player();
            p1.name = "용사";

            // 6. [기능추가]
            p1.Heal();

            // 7. [접근제한]
            Console.WriteLine($"{p1.name}의 HP: {p1.GetHP()}");

            // 9. [여러 객체]
            Player p2 = new Player();
            p2.name = "궁수";

            // 10. [종합]
            Monster slime = new Monster();
            p1.TakeDamage(slime.GetAtk());
            Console.WriteLine($"슬라임의 공격! {p1.name}의 남은 HP: {p1.GetHP()}");
        }
    }
}
