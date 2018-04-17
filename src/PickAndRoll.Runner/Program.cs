using System;
using System.IO;
using PickAndRoll.Models;

namespace PickAndRoll.Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new PickAndRoll(Console.WriteLine)
                .Go(new PickAndRollSettings
                {
                    Pwd = Path.GetFullPath(args[0])
                });
        }
    }
}