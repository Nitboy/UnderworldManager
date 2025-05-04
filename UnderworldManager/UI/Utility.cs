namespace UnderworldManager
{
  public static class Utility
  {
    public static int GetNumber(int minNum = -1, int maxNum = -1, string errorMessage = "Must Choose a number between ")
    {
      do
      {
        int choice;
        Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.Out.WriteLine("║                      INPUT REQUIRED                       ║");
        Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
        Console.Out.Write($"║ Input {minNum}-{maxNum} > ");
        var input = Console.In.ReadLine();
        Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
        
        if (int.TryParse(input, out choice))
        {
          if ((minNum == -1 && maxNum == -1) || (choice >= minNum && choice <= maxNum))
          {
            return choice;
          }
        }
        
        Console.Out.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.Out.WriteLine("║                      ERROR                               ║");
        Console.Out.WriteLine("╠════════════════════════════════════════════════════════════╣");
        Console.Out.WriteLine($"║ {errorMessage} {minNum}-{maxNum}                        ║");
        Console.Out.WriteLine("╚════════════════════════════════════════════════════════════╝");
      }
      while (true);
    }
  }
}