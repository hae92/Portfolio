using System.Text;

namespace BMS.Core.DB.Common
{
    public class DBData
    {
        private readonly List<Dictionary<string, string>> data;

        public DBData(List<Dictionary<string, string>> data)
        {
            this.data = data;
        }

        public Dictionary<string, string> this[int index]
        {
            get
            {
                if (index < 0 || index >= data.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return data[index];
            }
        }

        public string this[int index, string key]
        {
            get
            {
                if (index < 0 || index >= data.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (!data[index].ContainsKey(key))
                {
                    throw new KeyNotFoundException($"Key '{key}' not found.");
                }
                return data[index][key];
            }
        }

        public List<Dictionary<string, string>> GetData()
        {
            return data;
        }

        public long GetSize()
        {
            return data.Count;
        }

        public string ConvertToCsv(bool isTrim)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("Data cannot be null or empty.");
            }

            StringBuilder csvBuilder = new StringBuilder();

            // 헤더 작성
            var padRight = 20;
            var headers = data[0].Keys;

            if (isTrim) csvBuilder.AppendLine(string.Join(",", headers));
            else csvBuilder.AppendLine(string.Join(",", headers.Select(header => header.PadRight(padRight))));

            // 데이터 작성
            foreach (var row in data)
            {
                var values = new List<string>();
                foreach (var header in headers)
                {
                    values.Add(row.ContainsKey(header) ? row[header] : "");
                }

                if (isTrim) csvBuilder.AppendLine(string.Join(",", values));
                else csvBuilder.AppendLine(string.Join(",", values.Select(value => value.PadRight(padRight))));
            }

            return csvBuilder.ToString();
        }
    }
}
