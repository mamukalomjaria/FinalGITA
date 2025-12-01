using System.Globalization;

public static class CalculatorApp
{
    public static void Run()
    {
        Console.WriteLine("Calculator ---> Supported operations: + - * /");
        while (true)
        {
            try
            {
                Console.Write("Enter first number ('q' to quit): ");
                var firstNumber = Console.ReadLine();
                if (firstNumber == null) return;
                if (firstNumber.Trim().ToLower() == "q") return;
                //check if valid double
                if (!double.TryParse(firstNumber, out double a))
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }


                Console.Write("Enter second number: ");
                var secondNumber = Console.ReadLine();
                if (secondNumber == null) return;
                //check if valid double
                if (!double.TryParse(firstNumber, out double b))
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }


                Console.Write("Enter operation (+, -, *, /): ");
                var op = Console.ReadLine()?.Trim();
                double result;
                switch (op)
                {
                    case "+":
                        result = a + b; break;
                    case "-":
                        result = a - b; break;
                    case "*":
                        result = a * b; break;
                    case "/":
                        if (b == 0) { Console.WriteLine("Division by zero is not allowed."); continue; }
                        result = a / b; break;
                    default:
                        Console.WriteLine("Unknown operation."); continue;
                }
                Console.WriteLine($"Result: {result}");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}