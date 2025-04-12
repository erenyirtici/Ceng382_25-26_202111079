using System.Text.Json;

namespace Week5Lab.Helpers
{
    public class Utils
    {
        private static Utils? _instance;
        private static readonly object _lock = new();

        private Utils() { }

        public static Utils Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new Utils();
                }
            }
        }

        public string ExportToJson<T>(List<T> data, List<string>? selectedColumns = null)
        {
            if (selectedColumns == null || !selectedColumns.Any())
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var filtered = data.Select(item =>
            {
                var dict = new Dictionary<string, object?>();
                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                {
                    if (selectedColumns.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }
                return dict;
            });

            return JsonSerializer.Serialize(filtered, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
