using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static ValueTask CalculateAndShowResult(int data)
        {
            if (data > 0)
            {
                return new ValueTask(Task.Run(()=> Calculate(data)));
            }
            else
            {
                return new ValueTask(); //задача закрыта или невалидны аргументы
            }
        }
        static void Calculate (int data)
        {
            Console.WriteLine($" Сложная задача: {Task.CurrentId}, началась в потоке: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Сложная задача запущена.");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.WriteLine("Сложная задача завершена.");
            Random random = new Random();
            Console.WriteLine($" Результат: {random.Next(0, data)}, Задача № {Task.CurrentId}, началась в потоке: {Thread.CurrentThread.ManagedThreadId}");
        }

        static ValueTask<int> CalculateSum(int a, int b)
        {
            Thread.Sleep(1500);
            if (a == 0)
            {
                return new ValueTask<int>(b);
            }
            else if (b == 0)
            {
                return new ValueTask<int>(a);
            } 
            else if (a == 0 && b == 0)
            {
                return new ValueTask<int>(0);
            }
            else {
                return new ValueTask<int>(Task.Run(() => { return a + b; }));
            }
        }
        static void Main(string[] args)
        {
            CalculateAndShowResult(-5).GetAwaiter().GetResult();
            CalculateAndShowResult(15000).GetAwaiter().GetResult();

            Console.WriteLine($"Результат сложения: {CalculateSum(15, 17).Result}");

            ValueTask<int> valTask = CalculateSum(15, 17);

            while (!valTask.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(250);
            }
            
            
            Task<int> task = valTask.AsTask();
            task.ContinueWith((t) => { Console.WriteLine($"Задача была завершена.... {t.Result}"); });
            
        }
    }
}
