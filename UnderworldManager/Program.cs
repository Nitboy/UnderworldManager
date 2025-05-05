// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Text;
using UnderworldManager.Core.Business;
using UnderworldManager.Core.Models;
using UnderworldManager.UI;

namespace UnderworldManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

            var cg = new CharGen();
            var c = cg.CreateCharacter(1, null, true);
            Console.WriteLine("Your character has been generated:");
            Console.WriteLine(c);

            var gg = new GangGen();
            var g = gg.CreateGang();
            g.Members.Add(c);
            g.ResetRoster();
            Console.WriteLine("Your gang has been generated:");
            Console.WriteLine(g);

            var game = new UnderworldManager.Core.Business.Game(c, g);
            var mainloop = new MainLoop(game);
            mainloop.Run();
        }
    }
}