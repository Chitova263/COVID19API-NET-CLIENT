using System.Text.Json;

namespace Examples
{
    public static class Extensions
    {
        public static void Dump(this object obj) => System.Console.WriteLine(JsonSerializer.Serialize(obj));
    }
}
