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
    public static class UserRepository
    {
        private static readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\users.json");

        public static List<User> LoadUsers()
        {
            if (!File.Exists(UsersFile))
                return new List<User>();

            try
            {
                string json = File.ReadAllText(UsersFile);
                var list = JsonSerializer.Deserialize<List<User>>(json);
                return list ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public static void SaveUsers(List<User> users)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(UsersFile, json);
        }

        public static User? FindByPersonalId(string personalId)
        {
            return LoadUsers()
                .FirstOrDefault(u => u.PersonalId == personalId);
        }

        public static void AddUser(User user)
        {
            var users = LoadUsers();
            users.Add(user);
            SaveUsers(users);
        }

        public static void UpdateUser(User user)
        {
            var users = LoadUsers();
            var existing = users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                existing.PersonalId = user.PersonalId;
                existing.Password = user.Password;
                existing.Balance = user.Balance;
                SaveUsers(users);
            }
        }

        public static int GetNextId()
        {
            var users = LoadUsers();
            return users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;
        }
    }
}
