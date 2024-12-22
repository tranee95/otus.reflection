using System.Globalization;
using System.Text.Json;
using CsvHelper;

namespace otus.reflection;

public class ReflectionExample
{
    public ReflectionExample()
    {
        A = GenerateRandomNumber();
        B = GenerateRandomNumber();
        C = GenerateRandomNumber();
        D = GenerateRandomNumber();
        E = GenerateRandomNumber();
    }

    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public int D { get; set; }
    public int E { get; set; }

    public override string ToString() => 
        string.Concat(GetType().GetProperties().Select(s => $"{s.Name} =  {s.GetValue(this)}\n"));

    public string ToStringJson() => JsonSerializer.Serialize(this);

    public ReflectionExample DeserializeFromCsvFile()
    {
        using var reader = new StreamReader("file.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecords<ReflectionExample>().First();
    }

    private int GenerateRandomNumber() => new Random().Next(1, 100);
}