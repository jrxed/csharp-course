public class ConsoleCalculator()
{
    static void Main()
    {
        Console.WriteLine("=== C# Console Calculator ===");
        Console.WriteLine("Supported operations: +, -, *, /");
        Console.WriteLine("To exit enter 'q'\n");

        while (true)
        {
            Console.Write("Enter first num: ");
            string? input1 = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input1))
                continue;

            if (input1 == "q")
            {
                break;
            }

            if (!double.TryParse(input1, out double num1))
            {
                Console.WriteLine("Error: incorrect num\n");
                continue;
            }

            Console.Write("Enter second num: ");
            string? input2 = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input2))
                continue;

            if (input2 == "q")
            {
                break;
            }

            if (!double.TryParse(input2, out double num2))
            {
                Console.WriteLine("Error: incorrect num\n");
                continue;
            }

            Console.Write("Enter operation (+, -, *, /): ");
            string? operation = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(operation))
                continue;

            if (operation == "q")
            {
                break;
            }

            double result;

            switch (operation)
            {
                case "+":
                    result = num1 + num2;
                    break;

                case "-":
                    result = num1 - num2;
                    break;

                case "*":
                    result = num1 * num2;
                    break;

                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Error: division by zero\n");
                        continue;
                    }
                    result = num1 / num2;
                    break;

                default:
                    Console.WriteLine("Error: unknown operation\n");
                    continue;
            }
            
            Console.WriteLine($"Result: {num1} {operation} {num2} = {result}\n");
        }
    }
}
