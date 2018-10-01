using System;
using System.Reflection;
using System.Text;

namespace TrustEDU.CLI.Base
{
    public abstract class ConsoleBasedService
    {
        protected virtual string Prompt => "service";

        public abstract string ServiceName { get; }

        protected bool ShowPrompt { get; set; } = true;

        protected virtual bool OnCommand(string[] args)
        {
            switch (args[0].ToLower())
            {
                case "clear":
                    Console.Clear();
                    return true;
                case "exit":
                    return false;
                case "version":
                    Console.WriteLine(Assembly.GetEntryAssembly().GetName().Version);
                    return true;
                default:
                    Console.WriteLine("Error");
                    return true;
            }
        }

        protected internal abstract void OnStart(string[] args);

        protected internal abstract void OnStop();

        public void Run(string[] args)
        {
            OnStart(args);
            RunConsole();
            OnStop();
        }

        private void RunConsole()
        {
            bool running = true;
            Console.Title = ServiceName;
            Console.OutputEncoding = Encoding.Unicode;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Version ver = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine($"{ServiceName} Version: {ver}");
            Console.WriteLine();

            while (running)
            {
                if (ShowPrompt)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{Prompt}> ");
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                string line = Console.ReadLine()?.Trim();
                if (line == null) break;
                Console.ForegroundColor = ConsoleColor.White;
                string[] args = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (args.Length == 0)
                    continue;
                try
                {
                    running = OnCommand(args);
                }
                catch (Exception ex)
                {
                    #if DEBUG
                        Console.WriteLine($"Error: {ex.Message}");
                    #else
                        Console.WriteLine("Error");
                    #endif
                }
            }

            Console.ResetColor();
        }
    }
}
