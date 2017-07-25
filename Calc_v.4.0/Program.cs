using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Calc_v._4
{

    class Program
    {
        static Stack<string> StackConvStk = new Stack<string>();
        static Stack<double> StackFinal = new Stack<double>();
        static Queue<string> QueueStg = new Queue<string>();

        static void Main(string[] args)
        {
            string[] check = ChkInput();

            if (check != null)
            {
                ConvertToOPN(check); //если после проверки вернулась непустая строка, конвертируем её в ОПН
                CountOPN(); //расчёт ОПН
                Console.WriteLine("Должно быть похоже на результат вычислений:");
                if (StackFinal.Count > 1) Console.WriteLine("Что-то пошло не так. Похоже, вы где-то пропустили пробел.");
                else while (StackFinal.Count > 0) Console.WriteLine(StackFinal.Pop());  //вывод стека Final      
            }
            Console.ReadLine();
        }

        static void ConvertToOPN(string[] input)
        {
            for (int i = 0; i <= input.Length - 1; i++)
            {
                //curch = input[i];
                int temp; //временная переменная для TryParse
                if (int.TryParse(input[i], out temp))
                    PushToStack(input[i]); // c null почему-то всегда срабатывает это условие, на любой строке 

                else if ((input[i] == "+") || (input[i] == "-") || (input[i] == "*") || (input[i] == "/") || (input[i] == "^"))
                    PushToStack(input[i]);
                else if ((input[i] == "(") || (input[i] == ")"))
                {
                    if ((input[i] == "("))
                    {
                        while (input[i] != ")")
                        {
                            PushToStack(input[i]);
                            i = i + 1;
                        }

                        do
                        {
                            QueueStg.Enqueue(StackConvStk.Pop());
                        } while (StackConvStk.Peek() != "(");  // мы тут лишний раз Pop'om выгрызаем символ, когда проверяем условие, изменить на Peek. обработать кейс "(1)+(2)"                      
                        StackConvStk.Pop(); // выпинываем из стека уже ненужную нам '('
                    }
                    else if ((input[i] == ")"))
                    {
                        do
                        {
                            QueueStg.Enqueue(StackConvStk.Pop());

                        } while (StackConvStk.Peek() != "(");
                        StackConvStk.Pop(); // выгрызаем из стека уже ненужную нам '('
                    }
                }
            }
            while (StackConvStk.Count > 0) // после того, как входная строка закончилась, перекладываем значения из stk в stg. формируем законченную ОПН
            {
                QueueStg.Enqueue(StackConvStk.Pop());
            }
        }

        static string[] ChkInput() //здесь будем писать всякие ограничения на входную строку
        {
            Console.WriteLine("Введите выражение для расчёта:");

            string inputrow = Console.ReadLine();
            string[] inputSplit = Regex.Split(inputrow, "(\\+|-|\\*|/|\\^|\\)|\\()");

            for (int i = 0; i < inputSplit.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(inputSplit[i]))
                {
                    inputSplit[i].Replace(" ", string.Empty);
                    //return inputrow.Replace(" ", string.Empty); //авотхуй. у нас возвращаемый тип - массив. думай дальше
                    return inputSplit;
                }

                for (int j = 0; j <= inputSplit[i].Length - 1; j++) //проверяем каждую лексему
                {
                    if (char.IsLetter(inputSplit[i][j]))
                    {
                        Console.WriteLine("Ошибка ввода: Буквы в выражении");
                        Console.ReadLine();
                        return null;
                    }

                    if ((inputSplit[i][j] == '(') || (inputSplit[i][j] == ')')) //проверяем парность скобок. как-то учесть перевёрнутые пары!
                    {
                        int bracketOpen = 0;
                        int bracketClose = 0;
                        int l; //местный счётчик цикла
                        int k = 0; //вспомогательный счётчик
                        string inputBrHunt = inputrow; //сделаем себе копию слова, чтоб основные счётчики не дёргать лишний раз

                        for (l = 0; l <= inputBrHunt.Length - 1; l++)
                        {
                            if (inputBrHunt[l] == '(') bracketOpen++;
                            if (inputBrHunt[l] == ')') bracketClose++;
                        }

                        if (bracketOpen != bracketClose)
                        {
                            Console.WriteLine("Ошибка ввода: В выражении есть неполные пары скобок.");
                            Console.ReadLine();
                            return null;
                        }

                        for (l = 0; l <= inputBrHunt.Length - 1; l++)
                        {
                            if (inputBrHunt[l] == '(') k = k + 1;
                            if (inputBrHunt[l] == ')') k = k - 1;
                            if (k < 0)
                            {
                                Console.WriteLine("Ошибка ввода: В выражении есть перевёрнутые пары скобок.");
                                Console.ReadLine();
                                return null;
                            }
                        }
                    }
                    //int.TryParse(inputSplit[i], out temp);
                    //if ((temp == null) && ((inputSplit[i] != "+") || (inputSplit[i] != "-") || (inputSplit[i] != "*") || (inputSplit[i] != "/") || (inputSplit[i] != "^")))
                    //{
                    //    Console.WriteLine("Что-то пошло не так. Похоже, вы пропустили пробел.");
                    //    return null;
                    //}
                }
            }
            return inputSplit; // возвращаем массив строк. если проверки не прошли - возвращаем null
        }

        //static char ImprovedInput(string InputString)
        //{
        //    string impCurStr = "";
        //    char impOutput = '0';

        //    // проверять, что если следующий символ - char.IsDigit, добавлять к результ. строке символ 
        //    for (int i = 0; i <= InputString.Length - 1; i++)
        //    {
        //        if (!char.IsDigit(InputString[i])) break;
        //        impCurStr += InputString[i];
        //    }
        //    //return impOutput = (char)int.Parse(impCurStr);
        //    return impOutput = Convert.ToChar(int.Parse(impCurStr));
        //}

        static void PushToStack(string chpush)
        {
            int temp; //временная переменная для TryParse

            if ((chpush == "+") || (chpush == "-") || (chpush == "*") || (chpush == "/") || (chpush == "^"))
            {
                if (StackConvStk.Count > 0) //блок учёта приоритетов
                {
                    int curprior = ChkPriority(chpush);
                    int stkprior = ChkPriority(StackConvStk.Peek());

                    if (curprior <= stkprior)
                    {
                        QueueStg.Enqueue(StackConvStk.Pop());
                        StackConvStk.Push(chpush);
                        PushToStack(StackConvStk.Pop());
                    }
                    else StackConvStk.Push(chpush);
                }

                else StackConvStk.Push(chpush);
            }
            else if ((chpush == "(") || (chpush == ")"))
            {
                StackConvStk.Push(chpush);
            }
            else if (int.TryParse(chpush, out temp))
            {
                QueueStg.Enqueue(chpush);
            }
        }

        static int ChkPriority(string input)
        {
            int priority = 0;
            if ((input == "+") || (input == "-")) priority = 1; // приоритеты оператора из входной строки
            if ((input == "*") || (input == "/")) priority = 2;
            if (input == "^") priority = 3;

            return priority;
        }


        static void CountOPN()
        {
            int temp;
            while (QueueStg.Count() > 0)
            {
                if (int.TryParse(QueueStg.Peek(), out temp))
                {
                    StackFinal.Push(int.Parse(QueueStg.Dequeue()));
                }
                else if (QueueStg.Peek() == "+")
                {
                    QueueStg.Dequeue();
                    StackFinal.Push(StackFinal.Pop() + StackFinal.Pop());

                }
                else if (QueueStg.Peek() == "-")
                {
                    QueueStg.Dequeue();
                    StackFinal.Push(-StackFinal.Pop() + StackFinal.Pop());
                }
                else if (QueueStg.Peek() == "*")
                {
                    QueueStg.Dequeue();
                    StackFinal.Push(StackFinal.Pop() * StackFinal.Pop());
                }
                else if (QueueStg.Peek() == "/")
                {
                    QueueStg.Dequeue();
                    StackFinal.Push((1 / StackFinal.Pop()) * StackFinal.Pop());
                }

                else if (QueueStg.Peek() == "^")
                {
                    double tmp = 0;
                    QueueStg.Dequeue();
                    tmp = StackFinal.Pop();
                    StackFinal.Push(Math.Pow(StackFinal.Pop(), tmp));
                }

            }

        }
    }
}