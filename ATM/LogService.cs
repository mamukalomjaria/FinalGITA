using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ATM
{
    public static class LogService
    {
        private static readonly string LogsFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\atm_logs.json");

        public static void AddLog(TransactionLog log)
        {
            var logs = LoadLogs();
            logs.Add(log);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(logs, options);
            File.WriteAllText(LogsFile, json);
        }

        public static List<TransactionLog> LoadLogs()
        {
            if (!File.Exists(LogsFile))
                return new List<TransactionLog>();

            try
            {
                string json = File.ReadAllText(LogsFile);
                var list = JsonSerializer.Deserialize<List<TransactionLog>>(json);
                return list ?? new List<TransactionLog>();
            }
            catch
            {
                return new List<TransactionLog>();
            }
        }

        public static List<TransactionLog> LoadLogsForUser(string personalId)
        {
            return LoadLogs()
                .Where(l => l.PersonalId == personalId)
                .OrderByDescending(l => l.Date)
                .ToList();
        }
    }
}
