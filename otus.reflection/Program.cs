using System.Diagnostics;

namespace otus.reflection;

class Program
{
    static void Main(string[] args)
    {
        var reflectionExample = new ReflectionExample();

        var timer1 = new Stopwatch();
        timer1.Start();
        for (var i = 0; i < 1000; i++)
        {
             Console.WriteLine(reflectionExample.ToString());
        }
        timer1.Stop();
       
        
        var timer2 = new Stopwatch();
        timer2.Start();
        for (var i = 0; i < 1000; i++)
        {
             Console.WriteLine(reflectionExample.ToStringJson());
        }
        timer2.Stop();

        var timer3 = new Stopwatch();
        timer3.Start();
        for (var i = 0; i < 1000; i++)
        {
             var file = reflectionExample.DeserializeFromCsvFile();
        }
        timer3.Stop();

        Console.WriteLine($"Время выполнения сериализации: {timer1.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Время выполнения сериализации Json: {timer2.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Время десеарилизации: {timer3.ElapsedMilliseconds} ms.");
    }
}