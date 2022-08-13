using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp5
{



    class Program
    {

        static void ShowInfo()
        {
            Console.WriteLine($" Простая задача: {Task.CurrentId}, началась в потоке: {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Важная работа, этап {i}");
                Thread.Sleep(200);
            }
        } 

        static int ShowResult()
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
            return random.Next(0, Int32.MaxValue);
        }

        static int ComplexCalculation(int a, int b)
        {
            Console.WriteLine($" Очередная задача: {Task.CurrentId}, выполняется в потоке: {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(".");
            }
            Console.WriteLine();
            return a + b;
        }
        static void ShowComplexCalculationResult(Task<int> task)
        {
            Console.WriteLine($"Результатом очередной задачи: {task.Result}");
        }

        static void OneStage(object argum)
        {
            Console.WriteLine($" СверхПакетная задача: {Task.CurrentId}, началась в потоке: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(Convert.ToInt32(argum));
            Console.WriteLine($" СверхПакетная задача: {Task.CurrentId}, завершилась в потоке: {Thread.CurrentThread.ManagedThreadId}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Идентификатор основного потока: {Thread.CurrentThread.ManagedThreadId}");
            Action simpleAction = new Action(ShowInfo);
            Task simpleTask = new Task(simpleAction);

            simpleTask.Start();
            simpleTask.Wait(); //команда основному потоку водождать результатов


            Func<int> returnedAction = new Func<int>(ShowResult);
            Task<int> returnedTask = new Task<int>(returnedAction);
            returnedTask.Start();
            returnedTask.Wait();
            Console.WriteLine(returnedTask.Result); //обращение к результату



            List<Task> tasks = new List<Task>();
            tasks.Add(new Task(OneStage, 1500));
            tasks.Add(new Task(OneStage, 1300));
            tasks.Add(new Task(OneStage, 3500));
            tasks.Add(new Task(OneStage, 7500));
            tasks.Add(new Task(OneStage, 6700));
            tasks.Add(new Task(OneStage, 3000));

            foreach (var item in tasks)
            {
                item.Start();
            }
            //Task.WaitAny(tasks.ToArray());
            Task.WaitAll(tasks.ToArray());

            //Console.ReadKey();


            Task<int> taskComplexCalculation = new Task<int>(()=> ComplexCalculation(159, 317));
            Task continuationCallBack = taskComplexCalculation.ContinueWith(new Action<Task<int>>(ShowComplexCalculationResult));

            taskComplexCalculation.Start();
            continuationCallBack.Wait();

        }
    }
}
