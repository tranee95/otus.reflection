using System.Diagnostics;

public class Program
{
    static void Main(string[] args)
    {
        var reflectionExample = new ReflectionExample();

        var csvContent = UniversalSerializer.SerializeToCsv(reflectionExample, true);
        File.WriteAllText("file.csv", csvContent);

        var timer1 = new Stopwatch();
        timer1.Start();
        for (var i = 0; i < 1000; i++)
        {
            Console.WriteLine(reflectionExample.ToStringText());
        }

        timer1.Stop();

        var timer2 = new Stopwatch();
        timer2.Start();
        for (var i = 0; i < 1000; i++)
        {
            Console.WriteLine(reflectionExample.ToStringJson());
        }

        timer2.Stop();

        var timer4 = new Stopwatch();
        timer4.Start();
        for (var i = 0; i < 1000; i++)
        {
            Console.WriteLine(reflectionExample.ToStringCsv());
        }

        timer4.Stop();

        var timer3 = new Stopwatch();
        timer3.Start();
        for (var i = 0; i < 1000; i++)
        {
            var obj = ReflectionExample.DeserializeFromCsvFile();
        }

        timer3.Stop();

        Console.WriteLine($"Время выполнения сериализации в текст: {timer1.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Время выполнения сериализации в JSON: {timer2.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Время выполнения сериализации в CSV: {timer4.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Время десериализации из CSV: {timer3.ElapsedMilliseconds} ms.");

        var textData = reflectionExample.ToStringText();
        var deserializedFromText = UniversalSerializer.DeserializeFromText<ReflectionExample>(textData);
        Console.WriteLine("Десериализовано из текста:");
        Console.WriteLine(deserializedFromText.ToStringText());

        var jsonData = reflectionExample.ToStringJson();
        var deserializedFromJson = UniversalSerializer.DeserializeFromJson<ReflectionExample>(jsonData);
        Console.WriteLine("Десериализовано из JSON:");
        Console.WriteLine(deserializedFromJson.ToStringJson());
    }
}