using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    class Program
    {
        static Operators<char> op = new Operators<char>();
        static void Main(string[] args)
        {
            List<string> test = new List<string>();
            op.Add('+');
            op.Add('-');
            op.Add('*');
            op.Add('/');
            /* test.Add("X");
             test.Add("Z");
             ReadVals(test);*/
            ReadVals(ParseExpression(ReadExpression()));
            Console.ReadKey();
        }
        public static string ReadExpression()
        {
            string input = Console.ReadLine();
            return input;
        }
        public static List<string> ReadVals(List<string> variables)        
        {
            List<string> result = new List<string>();
            foreach(string variable in variables)
            {
                string request = "Значение " + variable + " = ";
                Console.Write(request);
                result.Add(Console.ReadLine());
            }
            return result;
        }
        public static List<string> ParseExpression(string expression)
        {
            string someChars = "";
            List<string> result = new List<string>();
            char last = '1';
            foreach (char c in expression)
            {            
                if (!op.Contains(c))
                {
                    someChars += c;
                }
                else
                {
                    result.Add(someChars);
                    someChars = "";
                }
                last = c;
            }
            if (op.Contains(last)) throw new FormatException("Last symbol was an operator!");
            else result.Add(someChars);
            return result;
        }
    }
}
