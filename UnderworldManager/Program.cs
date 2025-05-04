// See https://aka.ms/new-console-template for more information
using UnderworldManager.Business;
using UnderworldManager;


var e = Console.OutputEncoding;
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
{
  AutoFlush = true,
  NewLine = "\n",

});
Console.ForegroundColor = ConsoleColor.Yellow;
Console.BackgroundColor = ConsoleColor.Black;
Console.Title = "Underworld Manager 0.1";
Console.Clear();
Console.Out.WriteLine();
Console.Out.WriteLine();

CharGen cg = new CharGen();
var c = cg.CreateCharacter(1,null, true);
Console.WriteLine("Your character has been generated:");
Console.WriteLine(c);

GangGen gg = new GangGen();
var g = gg.CreateGang();
g.Members.Add(c);
g.ResetRoster();
Console.WriteLine("Your gang has been generated:");
Console.WriteLine(g);

Game game = new Game(c, g);
var mainloop = new MainLoop(game);
mainloop.Run();