using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGame
{
    public static class GuessNumberGame
    {
        private static readonly string ScoresFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\scores.csv");

        public static void Run()
        {

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            int max = (int)AskDifficulty();

            Random rnd = new Random();
            int secret = rnd.Next(1, max + 1);

            Console.WriteLine($"Guess the number between 1 and {max}. You have 10 attempts.");

            bool won = false;
            int attemptsUsed = 0;

            for (int attempt = 1; attempt <= 10; attempt++)
            {
                try
                {
                    Console.Write($"Attempt {attempt}/10 - enter your guess: ");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out int guess))
                    {
                        Console.WriteLine("Invalid number, try again.");
                        continue;
                    }

                    attemptsUsed = attempt;

                    if (guess == secret)
                    {
                        Console.WriteLine($"Correct! You won in {attempt} attempts.");
                        won = true;
                        break;
                    }
                    else if (guess < secret)
                    {
                        Console.WriteLine("Too low.");
                    }
                    else
                    {
                        Console.WriteLine("Too high.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            if (!won)
            {
                Console.WriteLine($"You lost. The number was: {secret}");
                return;
            }

            int score = 11 - attemptsUsed;

            UpdateScores(name, score);

        }

        private static int AskDifficulty()
        {
            while (true)
            {
                Console.WriteLine("Choose difficulty:");
                Console.WriteLine("1) Easy   (1-15)");
                Console.WriteLine("2) Medium (1-25)");
                Console.WriteLine("3) Hard   (1-50)");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": return 15;
                    case "2": return 25;
                    case "3": return 50;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        private static void UpdateScores(string name, int newScore)
        {
            var scores = LoadScores();

            var existing = scores.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                scores.Add(new PlayerScore { Name = name, BestScore = newScore });
            }
            else if (newScore > existing.BestScore)
            {
                existing.BestScore = newScore;
            }

            SaveScores(scores);
        }

        private static List<PlayerScore> LoadScores()
        {
            var list = new List<PlayerScore>();
            if (!File.Exists(ScoresFile)) return list;

            foreach (var line in File.ReadAllLines(ScoresFile))
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                {
                    list.Add(new PlayerScore { Name = parts[0], BestScore = score });
                }
            }
            return list;
        }

        private static void SaveScores(List<PlayerScore> scores)
        {
            var lines = scores.Select(s => $"{s.Name},{s.BestScore}");
            File.WriteAllLines(ScoresFile, lines);
        }


    }
}

