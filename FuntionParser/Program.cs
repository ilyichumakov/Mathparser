using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    class Program
    {   
        static void Main(string[] args)
        {
            bool end = false;
            Dictionary<string, int> op = new Dictionary<string, int>();            
            op.Add("+", 6);
            op.Add("-", 5);
            op.Add("*", 3);
            op.Add("/", 4);
            op.Add("^", 2);
            op.Add("!", 1);
            Parser Prs = new Parser(op);
            while (!end)
            {
                try
                {                   
                    string input = Parser.ReadExpression();
                    List<string> parsed = Prs.ParseExpression(input);
                    if (parsed.First() != "0")
                    {
                        Dictionary<string, double> vals = Prs.ReadVals(parsed); // считываем значения переменных
                        double result = Prs.ProceedParse(input, vals); // запускаем обход дерева от корня
                        Console.WriteLine(result.ToString()); //вывод результата    
                    }
                    else end = true; 
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
