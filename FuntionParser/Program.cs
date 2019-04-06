using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    class Program
    {        
        
        static double MakeOperation(Tree opers, Dictionary<string, double> vals, Tree[] full)
        {
            if (op.ContainsKey(opers.value))
            {
                if (opers.left_child > 0 && opers.right_child > 0)
                {
                    switch (opers.value)
                    {
                        case "+":
                            return MakeOperation(full[opers.left_child], vals, full) + MakeOperation(full[opers.right_child],  vals, full);
                        case "-":
                            return MakeOperation(full[opers.left_child], vals, full) - MakeOperation(full[opers.right_child],  vals, full);
                        case "*":
                            return MakeOperation(full[opers.left_child],  vals, full) * MakeOperation(full[opers.right_child],  vals, full);
                        case "/":
                            return MakeOperation(full[opers.left_child], vals, full) / MakeOperation(full[opers.right_child], vals, full);
                        case "^":
                            return Math.Pow(MakeOperation(full[opers.left_child], vals, full), MakeOperation(full[opers.right_child], vals, full));
                        default:
                            throw new ArgumentException("Undefined operator");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Operation has no argument(s)");
                }
            }
            else if(opers.value!="")
            {
                return vals[opers.value];
            }
            else
            {
                throw new ArgumentNullException("Empty value");
            }
        }    
                
        static Dictionary<string, int> op = new Dictionary<string, int>(); //соответствия операций и приоритетов
        
        public static string ReadExpression()
        {
            Console.WriteLine("Waiting for your expression...\nFor exit insert 0");
            string input = Console.ReadLine();
            return input;
        }

        public static Dictionary<string, double> ReadVals(List<string> variables)        
        {
            Dictionary<string, double> result = new Dictionary<string, double>(); //установим соответствие между именами переменных и их значениями
            foreach(string variable in variables)
            {
                bool isNumber = Double.TryParse(variable, out double number);
                if (!op.ContainsKey(variable) && !result.ContainsKey(variable) && !isNumber) //если не оператор, не считано ранее и не число
                {
                    string request = "Значение " + variable + " = ";
                    Console.Write(request);
                    result.Add(variable, Double.Parse(Console.ReadLine())); //добавим
                }
                else if(isNumber) // если число
                {
                    result.Add(variable, number); //добавим
                }
            }
            return result;
        }

        public static List<int> Priorities = new List<int>(); // список приоритетов внесенных операций

        public static List<string> ParseExpression(string expression)
        {
            string someChars = "";
            List<string> result = new List<string>();
            string last = "1";
            foreach (char c in expression) //смотрим посимвольно, так как операторы односимвольные
            {                     
                if (!op.ContainsKey(c.ToString())) //пока не оператор, будем накапливать имя переменной
                {
                    someChars += c;
                }
                else
                {
                    if (someChars == "") throw new FormatException("Two operators in row"); // если прошлое считывание было оператором, то неверный ввод              
                    result.Add(someChars); // добавим в список переменную
                    someChars = "";
                    someChars += c; // оператор
                    result.Add(someChars); //добавим оператор
                    Priorities.Add(op[someChars]); // приоритет операции занесем в выделенный для этого список
                    someChars = ""; //подготовили для следующего ввода
                }
                last = c.ToString(); // резерв для конца строки
            }
            if (op.ContainsKey(last)) throw new FormatException("Last symbol was an operator!"); // если в конце стоял оператор, нас ПОКА ЧТО не устраивает
            else result.Add(someChars); // если была переменная, добавим            
            return result;
        }

        public static List<string> SortOperators(List<string> parsed, List<int> priorities, Dictionary<string, int> op)
        {
            bool sorted = false;
            while (!sorted)
            {
                int curOperator = 0;
                int i = 0;
                while(i<parsed.Count)
                {
                    string s = parsed[i];
                    if (op.ContainsKey(s) && curOperator > 0)
                    {
                        if (priorities[curOperator] > priorities[curOperator - 1])
                        {
                            string tempStr = s;
                            parsed[i] = parsed[i - 2];
                            parsed[i - 2] = tempStr;
                            tempStr = parsed[i + 1];
                            parsed[i + 1] = parsed[i - 1];
                            parsed[i - 1] = tempStr;
                            i = -1;
                            int changePriority = priorities[curOperator];
                            priorities[curOperator] = priorities[curOperator - 1];
                            sorted = false;
                            curOperator = 0;
                        }
                    }
                    else if (op.ContainsKey(s)) curOperator++;
                    i++;
                    if (i == parsed.Count) sorted = true;
                }
            }
            return parsed;
        }

        static void Main(string[] args)
        {
            Tree[] OperationTree = new Tree[100];
            op.Clear();
            Priorities.Clear();
            op.Add("+", 6);
            op.Add("-", 5);
            op.Add("*", 3);
            op.Add("/", 4);
            op.Add("^", 2);
            op.Add("!", 1);
            try
            {                
                List<string> parsed = ParseExpression(ReadExpression());
                parsed = SortOperators(parsed, Priorities, op);
                if (parsed.First() != "0")
                {
                    int i = 0;
                    bool rightChildExpected = false;
                    string leftOperand = "";
                    foreach (string oper in parsed)
                    {
                        if (!op.ContainsKey(oper)) // если не оператор
                        {
                            if (i == 0 || !rightChildExpected) // в начале всегда должна быть переменная, здесь же все левые ветви
                            {
                                leftOperand = oper; // левая ветвь
                                if (i == 0) OperationTree[1].Init(-1, -1, oper, 0); //если первый элемент
                                rightChildExpected = true;
                            }
                        }
                        else
                        {
                            if (i == 1) OperationTree[0].Init(1, 2, oper, -1);
                            else
                            {
                                OperationTree[i - 1].Init(i, i + 1, oper, i - 3);
                                OperationTree[i].Init(-1, -1, leftOperand, i - 1);
                            }
                            rightChildExpected = false;
                        }
                        i++;
                    }
                    OperationTree[i - 1].Init(-1, -1, parsed.Last(), i - 3); // последний
                    Dictionary<string, double> vals = ReadVals(parsed); // считываем значения переменных
                    double result = MakeOperation(OperationTree[0], vals, OperationTree); // запускаем обход дерева от корня
                    Console.WriteLine(result.ToString()); //вывод результата                    
                }                
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);                
            }
            Console.ReadKey();
        }
    }
}
