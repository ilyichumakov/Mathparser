using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    class Program
    {
        public struct Tree
        {
            public int left_child;
            public int right_child;
            public string value;
            public int parent;
            public void Init(int left_child, int right_child, string value, int parent)
            {
                this.left_child = left_child;
                this.right_child = right_child;
                this.value = value;
                this.parent = parent;
            }
        }
        static void Create(int n, int i, Tree[] a)
        {
            int k = 0;
            for (int j = 0; j < 100; j++)
            {
                if (a[j].left_child == 0 && a[j].right_child == 0 && a[j].parent == 0) { k = j; break; }
            }
            for (int j = k; j < k + i; j++)
            {
                a[j].parent = n;
                if (j != k) a[j - 1].right_child = j;
                if (j == k) a[n].left_child = j;
                a[j].value = "#";
            }
        }



        //static Operators<char> op = new Operators<char>();
        static Dictionary<string, int> op = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            Tree[] OperationTree = new Tree[100];
            op.Add("+", 1);
            op.Add("-", 1);
            op.Add("*", 2);
            op.Add("/", 2);
            op.Add("^", 3);
            op.Add("!", 4);
            try
            {
                //ReadVals(ParseExpression(ReadExpression()));
                List<string> parsed = ParseExpression(ReadExpression());
                int i = 0;
                //bool operatorExpected = false;
                bool rightChildExpected = false;                
                string leftOperand = "";
                //string rightOperand = "";
                foreach(string oper in parsed)
                {                    
                    if(!op.ContainsKey(oper)) // если не оператор
                    {
                        if (i == 0 || !rightChildExpected) // в начале всегда должна быть переменная, здесь же все левые ветви
                        {
                            leftOperand = oper; // левая ветвь
                            if (i == 0) OperationTree[1].Init(-1, -1, oper, 0); //если первый элемент
                            /*else //не первый, но левая ветвь
                            {
                                OperationTree[i].Init(-1, -1, oper, i-1);
                            }*/
                            rightChildExpected = true;

                        }
                        /*else if (rightChildExpected) // если правая ветвь
                        {
                            rightOperand = oper; //правая ветвь
                            OperationTree[i].Init(-1, -1, oper, i + 1);
                        }*/
                    }
                    else
                    {
                        if (i == 1) OperationTree[0].Init(1, 2, oper, -1);
                        else
                        {
                            OperationTree[i-1].Init(i, i + 1, oper, i - 3);
                            OperationTree[i].Init(-1, -1, leftOperand, i - 1);
                        }
                        rightChildExpected = false;
                    }
                    i++;
                }
                OperationTree[i-1].Init(-1, -1, parsed.Last(), i-3);
            }
            catch (FormatException ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
        public static string ReadExpression()
        {
            Console.WriteLine("Waiting for your expression...");
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
            string last = "1";
            foreach (char c in expression)
            {                     
                if (!op.ContainsKey(c.ToString()))
                {
                    someChars += c;
                }
                else
                {
                    if (someChars == "") throw new FormatException("Two operators in row");                    
                    result.Add(someChars);
                    someChars = "";
                    someChars += c;
                    result.Add(someChars);
                    someChars = "";
                }
                last = c.ToString();
            }
            if (op.ContainsKey(last)) throw new FormatException("Last symbol was an operator!");
            else result.Add(someChars);
            return result;
        }
    }
}
