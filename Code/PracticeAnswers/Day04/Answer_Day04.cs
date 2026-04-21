using System;

namespace PracticeAnswers.Day04
{
    class Animal
    {
        public void Eat() { Console.WriteLine("냠냠 먹습니다."); }
        public virtual void MakeSound() { Console.WriteLine("소리를 냅니다."); }
    }

    class Dog : Animal
    {
        // 5. [가상함수/오버라이드]
        public override void MakeSound() { Console.WriteLine("멍멍!"); }
        
        // 6. [고유기능]
        public void WagTail() { Console.WriteLine("꼬리를 흔듭니다."); }
    }

    class Player
    {
        public string name;
        public int level;

        // 1. [생성자]
        public Player(string name) { this.name = name; }
        
        // 2. [초기화/오버로딩]
        public Player() { this.level = 1; }
    }

    abstract class Monster
    {
        public virtual void Attack() { Console.WriteLine("기본 공격!"); }
    }

    class Slime : Monster
    {
        public override void Attack() { Console.WriteLine("끈적끈적 몸통 박치기!"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Player p1 = new Player("지우");
            Player p2 = new Player();
            
            Dog myDog = new Dog();
            myDog.Eat(); // 4. [재사용]
            myDog.MakeSound();
            myDog.WagTail();

            // 9. [다형성]
            Monster m = new Slime();
            m.Attack();
        }
    }
}
