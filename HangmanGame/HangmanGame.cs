using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace StrangleGame
{
    public static class HangmanGame
    {
        private static List<string> words = new List<string>
        {
            "apple", "banana", "orange", "grape", "kiwi",
            "strawberry", "pineapple", "blueberry", "peach", "watermelon"
        };

        private static readonly string ScoresFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\hangman_scores.xml");

        public static void Run()
        {
            Console.Write("Enter your name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            var random = new Random();
            string secret = words[random.Next(words.Count)];
            char[] progress = new string('_', secret.Length).ToCharArray();
            HashSet<char> usedLetters = new HashSet<char>();

            int wrong = 0;
            const int maxWrong = 6;

            Console.WriteLine("Hangman game started!");
            Console.WriteLine($"Word: {new string(progress)}");
            Console.WriteLine($"You can try {maxWrong} letters.\n");

            while (wrong < maxWrong && progress.Contains('_'))
            {
                try
                {
                    Console.Write("Enter a letter: ");
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
                    {
                        Console.WriteLine("Please enter exactly one letter.");
                        continue;
                    }

                    char c = char.ToLower(input[0]);

                    if (usedLetters.Contains(c))
                    {
                        Console.WriteLine("You already tried this letter.");
                        continue;
                    }

                    usedLetters.Add(c);

                    if (secret.Contains(c))
                    {
                        for (int i = 0; i < secret.Length; i++)
                        {
                            if (secret[i] == c)
                                progress[i] = c;
                        }
                        Console.WriteLine($"Good! {new string(progress)}");

                        if (!progress.Contains('_'))
                        {
                            Console.WriteLine("You revealed all letters!");
                            break;
                        }
                    }
                    else
                    {
                        wrong++;
                        Console.WriteLine($"Wrong letter. Wrong attempts: {wrong}/{maxWrong}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            bool won = false;

            if (!progress.Contains('_') && wrong <= maxWrong)
            {
                won = true;
            }
            else if (wrong >= maxWrong)
            {
                Console.WriteLine("You used all wrong letter attempts.");
            }

            if (!won)
            {
                Console.Write("\nNow guess the whole word: ");
                string? guessWord = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(guessWord) &&
                    guessWord.Trim().ToLower() == secret)
                {
                    won = true;
                }
            }

            if (won)
            {
                int score = maxWrong - wrong;
                Console.WriteLine($"You won! The word was '{secret}'. Score: {score}");
                UpdateScores(name!, score);
            }
            else
            {
                Console.WriteLine($"You lost. The word was '{secret}'.");
            }
        }

        private static List<HangmanScore> LoadScores()
        {
            if (!File.Exists(ScoresFile))
                return new List<HangmanScore>();

            try
            {
                using var fs = File.OpenRead(ScoresFile);
                var serializer = new XmlSerializer(typeof(List<HangmanScore>));
                return (List<HangmanScore>)serializer.Deserialize(fs)!;
            }
            catch
            {
                return new List<HangmanScore>();
            }
        }

        private static void SaveScores(List<HangmanScore> scores)
        {
            using var fs = File.Create(ScoresFile);
            var serializer = new XmlSerializer(typeof(List<HangmanScore>));
            serializer.Serialize(fs, scores);
        }

        private static void UpdateScores(string name, int newScore)
        {
            var scores = LoadScores();
            var existing = scores.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                scores.Add(new HangmanScore { Name = name, BestScore = newScore });
            }
            else if (newScore > existing.BestScore)
            {
                existing.BestScore = newScore;
            }
            SaveScores(scores);
        }

    }
}
