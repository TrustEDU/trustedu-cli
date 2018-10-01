using System;
using System.Security;
using System.Text;

namespace TrustEDU.CLI.Base.Helpers
{
    public static class ConsoleHelper
    {
        public static string ReadPassword(string prompt)
        {
            const string t = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo key;
            Console.Write(prompt);
            Console.Write(": ");

            Console.ForegroundColor = ConsoleColor.Yellow;

            do
            {
                key = Console.ReadKey(true);
                if (t.IndexOf(key.KeyChar) != -1)
                {
                    sb.Append(key.KeyChar);
                    Console.Write('*');
                }
                else if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Length--;
                    Console.Write(key.KeyChar);
                    Console.Write(' ');
                    Console.Write(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            return sb.ToString();
        }

        public static SecureString ReadSecureString(string prompt)
        {
            const string t = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            SecureString securePwd = new SecureString();
            ConsoleKeyInfo key;
            Console.Write(prompt);
            Console.Write(": ");

            Console.ForegroundColor = ConsoleColor.Yellow;

            do
            {
                key = Console.ReadKey(true);
                if (t.IndexOf(key.KeyChar) != -1)
                {
                    securePwd.AppendChar(key.KeyChar);
                    Console.Write('*');
                }
                else if (key.Key == ConsoleKey.Backspace && securePwd.Length > 0)
                {
                    securePwd.RemoveAt(securePwd.Length - 1);
                    Console.Write(key.KeyChar);
                    Console.Write(' ');
                    Console.Write(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            securePwd.MakeReadOnly();
            return securePwd;
        }
    }
}
