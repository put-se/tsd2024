namespace ProblemChallenge;

class Program
{
    static void Main(string[] args)
    {
       
        Console.WriteLine($"Here is the result of your Ameba task:");
        Ameba.Run();

        Console.WriteLine($"Here is the result of your Alarms task:");
        Alarms.Run();
        // Console.WriteLine($"Here is the result of your RobotOnTheMoon task:");
        // RobotOnMoon.Run();
        
        Console.Write($"{Environment.NewLine}Press any key to exit...");
        Console.ReadKey(true);
    }
}
