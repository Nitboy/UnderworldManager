namespace UnderworldManager
{
  public static class Utility
  {
    public static int GetNumber(int min, int max)
    {
      while (true)
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine("INPUT REQUIRED");
        Console.Out.WriteLine($"Please enter a number between {min} and {max}:");
        Console.Out.WriteLine();

        var input = Console.ReadLine();
        if (int.TryParse(input, out var number) && number >= min && number <= max)
        {
          return number;
        }

        Console.Out.WriteLine();
        Console.Out.WriteLine("INVALID INPUT");
        Console.Out.WriteLine($"Please enter a number between {min} and {max}");
      }
    }
  }
}