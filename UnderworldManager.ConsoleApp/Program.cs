// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Text;
using UnderworldManager.Core.Business;
using UnderworldManager.Core.Models;
using UnderworldManager.UI;

namespace UnderworldManager.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var e = System.Console.OutputEncoding;
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = System.Text.Encoding.UTF8;

            System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput())
            {
                AutoFlush = true,
                NewLine = "\n",
            });
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.Title = "Underworld Manager 0.1";
            System.Console.Clear();
            System.Console.Out.WriteLine();
            System.Console.Out.WriteLine();

            var cg = new CharGen();
            var c = cg.CreateCharacter(1, null, true);
            System.Console.WriteLine("Your character has been generated:");
            System.Console.WriteLine(c);

            var gg = new GangGen();
            var g = gg.CreateGang();
            g.Members.Add(c);
            g.ResetRoster();
            System.Console.WriteLine("Your gang has been generated:");
            System.Console.WriteLine(g);

            var game = new UnderworldManager.Core.Business.Game(c, g);
            var mainloop = new MainLoop(game);
            mainloop.Run();
        }
    }
}