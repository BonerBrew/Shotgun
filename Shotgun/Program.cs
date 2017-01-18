using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shotgun
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length <= 1)
            {
                Console.WriteLine("Nope!");
                Console.WriteLine();
                Console.WriteLine("Shotgun# usage:");
                Console.WriteLine("Pass 2 parameters.");
                Console.WriteLine("Output of the program at parameter 1 will be passed as an argument to the program at parameter 2.");
                Console.WriteLine("If you want the output as a variable, perhaps try something like this: http://stackoverflow.com/a/6362922");
                return;
            }

            // spawns process 1
            //Console.Write(args[0]);
            var split = args[0].Split(' ');
            var l = split.ToList();
            l.RemoveAt(0);
            var asi = new ProcessStartInfo(split[0], string.Join(" ", l)) {
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            var aproc = Process.Start(asi);
            Debug.Assert(aproc != null, "could not start process " + args[0]);
            //Console.Write(aproc.StandardOutput.ReadToEnd());

            // reads stdin
            var sb = new List<char>();
            int s;
            while ((s = aproc.StandardOutput.Read()) != -1)
            {
                sb.Add((char)s);//TODO doesn't support code points
            }

            // pipes to process 2
            var index0 = args.Length-1;
            //Console.Write(args[index0] + " " + new string(sb.ToArray()));
            var si = new ProcessStartInfo(args[index0], new string(sb.ToArray())) {
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            var proc = Process.Start(si);
            Debug.Assert(proc != null, "could not start process " + args[index0]);
            Console.Write(proc.StandardOutput.ReadToEnd());
        }
    }
}