using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuntionParser
{
    public class Parser
    {
        private double MakeOperation(Tree opers, Dictionary<string, double> vals, Tree[] full)
        {
            if (op.ContainsKey(opers.value))
            {
                if (opers.left_child > 0 && opers.right_child > 0)
                {
                    switch (opers.value)
                    {
                        case "+":
                            return MakeOperation(full[opers.left_child], vals, full) + MakeOperation(full[opers.right_child], vals, full);
                        case "-":
                            return MakeOperation(full[opers.left_child], vals, full) - MakeOperation(full[opers.right_child], vals, full);
                        case "*":
                            return MakeOperation(full[opers.left_child], vals, full) * MakeOperation(full[opers.right_child], vals, full);
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
            else if (opers.value != "")
            {
                return vals[opers.value];
            }
            else
            {
                throw new ArgumentNullException("Empty value");
            }
        }

        private Dictionary<string, int> op = new Dictionary<string, int>(); //соответствия операций и приоритетов

        public static string ReadExpression()
        {
            Console.WriteLine("Waiting for your expression...\nFor exit insert 'exit'");
            string input = Console.ReadLine();
            return input;
        }

        public Dictionary<string, double> ReadVals(List<string> variables)
        {
            Dictionary<string, double> result = new Dictionary<string, double>(); //установим соответствие между именами переменных и их значениями
            /*foreach (string variable in variables)*/
            while(variables.Count>0)
            {
                string variable = variables.First();
                List<string> check = new List<string>();
                if (!op.ContainsKey(variable))
                {
                    check = ParseExpression(variable);
                }
                else
                {
                    check.Add(variable);
                }
                if (check.Count == 1)
                {
                    bool isNumber = Double.TryParse(variable, out double number);
                    if (!op.ContainsKey(variable) && !result.ContainsKey(variable) && !isNumber) //если не оператор, не считано ранее и не число
                    {
                        string request = "Значение " + variable + " = ";
                        Console.Write(request);
                        result.Add(variable, Double.Parse(Console.ReadLine())); //добавим
                    }
                    else if (isNumber && !result.ContainsKey(variable)) // если число
                    {
                        result.Add(variable, number); //добавим
                    }
                }
                else
                {
                    //result.Concat(ReadVals(check));
                    //result.Union(ReadVals(check));
                    foreach(string elem in check)
                    {
                        variables.Add(elem);
                    }
                }
                variables.RemoveAt(0);
            }
            return result;
        }

        private static List<int> Priorities = new List<int>(); // список приоритетов внесенных операций

        public List<string> ParseExpression(string expression)
        {
            Priorities.Clear();            
            string someChars = "";
            int brackets = 0;
            List<string> result = new List<string>();
            string last = "1";
            foreach (char c in expression) //смотрим посимвольно, так как операторы односимвольные
            {
                if (c == '(')
                {
                    brackets++;//глубина скобок возрастает
                    //if (brackets == 1) continue;
                }
                if (c == ')') brackets--; //глубина убывает
                if (brackets < 0) throw new FormatException("A closing bracket caught when not expected");
                if ((!op.ContainsKey(c.ToString()) || brackets > 0)/* && !(c == ')' && brackets == 0)*/) //пока не оператор, будем накапливать имя переменной
                {
                    someChars += c;
                }
                else if(c == ')' && brackets == 0)
                {
                   
                }
                else /*if (brackets == 0)*/
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
            if (brackets > 0) throw new FormatException("Too much opening brackets");
            else result.Add(someChars); // если была переменная, добавим  
            if(result.Count == 1 && result.First()[0] == '(')
            {
                string correct = result.First();
                result.Clear();
                correct = correct.Substring(1, correct.Length-2);
                result=ParseExpression(correct);
            }
            return result;
        }

        private List<string> SortOperators(List<string> parsed)
        {
            bool sorted = false;
            while (!sorted)
            {
                int curOperator = 0;
                int i = 0;
                while (i < parsed.Count)
                {
                    string s = parsed[i];
                    if (op.ContainsKey(s) && curOperator > 0)
                    {
                        if (Priorities[curOperator] > Priorities[curOperator - 1])
                        {
                            string tempStr = s;
                            parsed[i] = parsed[i - 2];
                            parsed[i - 2] = tempStr;
                            tempStr = parsed[i + 1];
                            parsed[i + 1] = parsed[i - 1];
                            parsed[i - 1] = tempStr;
                            i = -1;
                            int changePriority = Priorities[curOperator];
                            Priorities[curOperator] = Priorities[curOperator - 1];
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

       /* private static string MergeParsed(List<string> parsed) // сливаем компоненту выражения
        {
            string res = "";
            foreach (string s in parsed)
            {
                res += s;
            }
            return res;
        }*/

        private static int FindFreeIndex(Tree[] OperationTree)
        {
            for (int i = 0; i < OperationTree.Length; i++)
            {
                Tree node = OperationTree[i];
                if (node.parent == 0 && node.left_child == 0 && node.right_child == 0) return i;
            }
            return -1;
        }

        public double ProceedParse(string expression, Dictionary<string, double> vals)
        {
            Tree[] OperationTree = new Tree[100];            
            List<string> parsed = ParseExpression(expression);
            parsed = SortOperators(parsed);
            int i = 0;
            bool decomposed = false;
            OperationTree[0].Init(-1, -1, expression, -1);
            while (!decomposed)
            {
                bool foundMax = false;
                string right = "";
                string left = "";
                string maxOp = "";
                if (OperationTree[i].value == null)
                {
                    decomposed = true;
                    continue;
                }
                List<string> node = ParseExpression(OperationTree[i].value);
                if (node.Count > 1)
                {
                    int maxPriority = Priorities.Max();
                    for (int j = 0; j < node.Count; j++)
                    {
                        string oper = node[j];
                        if (op.ContainsKey(oper) && op[oper] == maxPriority && !foundMax)
                        {
                            foundMax = true;
                            maxOp = oper;
                        }
                        else
                        {
                            if (foundMax) right += oper;
                            else left += oper;
                        }
                    }
                    int freeCell = FindFreeIndex(OperationTree);
                    if (freeCell > -1)
                    {
                        OperationTree[i].Init(freeCell, freeCell + 1, maxOp, OperationTree[i].parent);
                        OperationTree[freeCell].Init(-1, -1, left, i);
                        OperationTree[freeCell + 1].Init(-1, -1, right, i);
                    }
                    decomposed = false;
                }
                i++;
            }
            double result = MakeOperation(OperationTree[0], vals, OperationTree); // запускаем обход дерева от корня
            return result;
        }

        public Parser(Dictionary<string, int> opers)
        {
            op = opers;
        }
    }
}
