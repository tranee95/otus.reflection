using System.Reflection;
using System.Text;

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

    public string ToStringText() => UniversalSerializer.SerializeToText(this);

    public string ToStringJson() => UniversalSerializer.SerializeToJson(this);

    public string ToStringCsv() => UniversalSerializer.SerializeToCsv(this);

    public static ReflectionExample DeserializeFromCsvFile() =>
        UniversalSerializer.DeserializeFromCsvFile<ReflectionExample>("file.csv", true);

    private int GenerateRandomNumber() => new Random().Next(1, 100);
}

public static class UniversalSerializer
{
    public static string SerializeToText(object obj)
    {
        var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return string.Join("\n", props.Select(p => $"{p.Name} = {p.GetValue(obj)}"));
    }

    public static string SerializeToJson(object obj)
    {
        var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sb = new StringBuilder("{");
        foreach (var p in props)
        {
            var value = p.GetValue(obj);
            string jsonValue = (p.PropertyType == typeof(int)) ? value.ToString() : $"\"{value}\"";
            sb.Append($"\"{p.Name}\":{jsonValue},");
        }

        if (props.Length > 0) sb.Remove(sb.Length - 1, 1);
        sb.Append("}");
        return sb.ToString();
    }

    public static string SerializeToCsv(object obj, bool includeHeader = false)
    {
        var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var values = string.Join(",", props.Select(p => p.GetValue(obj).ToString()));
        if (includeHeader)
        {
            var header = string.Join(",", props.Select(p => p.Name));
            return header + "\n" + values;
        }

        return values;
    }

    public static T DeserializeFromText<T>(string text) where T : new()
    {
        var instance = new T();
        var lines = text.Split('\n');
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split('=');
            if (parts.Length != 2) continue;
            var name = parts[0].Trim();
            var valueStr = parts[1].Trim();
            var prop = props.FirstOrDefault(p => p.Name == name);
            if (prop != null && prop.PropertyType == typeof(int))
            {
                if (int.TryParse(valueStr, out int val))
                {
                    prop.SetValue(instance, val);
                }
            }
        }

        return instance;
    }

    public static T DeserializeFromJson<T>(string json) where T : new()
    {
        var instance = new T();
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        json = json.Trim('{', '}');
        var pairs = json.Split(',');
        foreach (var pair in pairs)
        {
            var parts = pair.Split(':');
            if (parts.Length != 2) continue;
            var name = parts[0].Trim('"');
            var valueStr = parts[1].Trim();
            var prop = props.FirstOrDefault(p => p.Name == name);
            if (prop != null && prop.PropertyType == typeof(int))
            {
                if (int.TryParse(valueStr, out int val))
                {
                    prop.SetValue(instance, val);
                }
            }
        }

        return instance;
    }

    public static T DeserializeFromCsv<T>(string csv, bool hasHeader = false) where T : new()
    {
        var instance = new T();
        var lines = csv.Split('\n');
        string dataLine = hasHeader && lines.Length > 1 ? lines[1] : lines[0];
        var values = dataLine.Split(',');
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < Math.Min(values.Length, props.Length); i++)
        {
            if (int.TryParse(values[i].Trim(), out int val))
            {
                props[i].SetValue(instance, val);
            }
        }

        return instance;
    }

    public static T DeserializeFromCsvFile<T>(string filePath, bool hasHeader = false) where T : new()
    {
        var csv = File.ReadAllText(filePath);
        return DeserializeFromCsv<T>(csv, hasHeader);
    }
}