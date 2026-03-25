using System.Collections;
using System.Globalization;

namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix).ToArray().Reverse();
        return EvaluatePostfix(postfix);
    }

    private static double EvaluatePostfix(IEnumerable<string> postfix)
    {
        var stack = new Stack<double>();
        foreach (string item in postfix)
        {
            if (IsOperator(item))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(item switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(item, CultureInfo.InvariantCulture));
            }
        }
        return stack.Pop();
    }

    private static Stack<string> InfixToPostfix(string infix)
    {
        var temp = string.Empty;
        var stackPostfix = new Stack<string>();
        var stack = new Stack<char>();
        foreach (var item in infix)
        {
            if (IsOperator(item))
            {
                if (ValidateLenght(temp))
                {
                    stackPostfix.Push(temp);
                    temp = string.Empty;
                }
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')

                    {
                        do
                        {
                            stackPostfix.Push(stack.Pop().ToString());
                        } while (stack.Peek() != '(');
                        stack.Pop();
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            stackPostfix.Push(stack.Pop().ToString());
                            stack.Push(item);
                        }
                    }
                }
            }
            else
            {
                temp += item;
            }
        }
        if (ValidateLenght(temp))
        {
            stackPostfix.Push(temp);
        }
        while (stack.Count > 0)
        {
            stackPostfix.Push(stack.Pop().ToString());
        }
        return stackPostfix;
    }

    private static bool IsOperator(string item) => "+-*/^()".Contains(item);

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static bool ValidateLenght(string temporal) => temporal.Length > 0;
}