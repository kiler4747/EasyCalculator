using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
{
	public enum Angle
	{
		Deg,
		Rad
	}
	public class Calculator
	{
		private string expression;
		private int currentChar;
		private Angle angle;

		enum Funcs
		{
			Plus, Minus, Multiplicate, Devision,
			Pow, Koren,
			Sin, Cos, Tan,
			Asin, Acos, Atan,
			Ln, Lg,
			Fact, Dot
		}

		Dictionary<Funcs, string> funcs = new Dictionary<Funcs, string>()
			{
				{Funcs.Plus, "+"},
				{Funcs.Minus, "-"},
				{Funcs.Multiplicate, "*"}
			};

		public string Expression
		{
			get { return expression; }
			set
			{
				expression = value.Replace(" ", "");
				expression = expression.Replace('.', ',');
			}
		}

		public char CurrentChar
		{
			get { return Expression[currentChar]; }
		}

		public Calculator()
		{
			Expression = "";
			currentChar = 0;
		}

		double SetAngle(double x)
		{
			if (angle == Angle.Deg)
				return x * Math.PI / 180;
			return x;
		}


		public double Calculate(string str)
		{
			currentChar = 0;
			Expression = str;
			return Plus();
		}

		private double Plus()
		{
			double result = Multiplicate();
			if (currentChar == Expression.Length)
				return result;
			while (Match("+") || Match("-"))
			{
				switch (CurrentChar)
				{
					case '+':
						currentChar++;
						result += Multiplicate();
						break;
					case '-':
						currentChar++;
						result -= Multiplicate();
						break;
				}
			}
			return result;
		}

		private bool Match(string patt)
		{
			if (patt.Length <= Expression.Length - currentChar)
				if (patt.ToLower() == Expression.Substring(currentChar, patt.Length).ToLower())
					return true;
			return false;
		}

		private double Multiplicate()
		{
			double result = Pow();
			if (currentChar == Expression.Length)
				return result;
			while (Match("*") || Match("/") || Match("√") ||
					Match("(") || Match(")"))
			{
				switch (CurrentChar)
				{
					case '*':
						currentChar++;
						result *= Multiplicate();
						break;
					case '/':
						currentChar++;
						result /= Multiplicate();
						break;
					case '√':
						result *= Pow();
						break;
					case '(':
						result *= Brackets();
						break;
					case ')':
						currentChar++;
						break;
				}
			}
			return result;
		}

		private double Pow()
		{
			double result = MathArifm();
			if (currentChar == Expression.Length)
				return result;
			while (Match("^") || Match("√") || Match("!") || (Match("%")))
			{
				switch (CurrentChar)
				{
					case '^':
						currentChar++;
						result = Math.Pow(result, Pow());
						break;
					case '√':
						currentChar++;
						double step = 1;
						if (result <= 1)
						{
							//Console.WriteLine("Степень корня должна быть больше 1");
							return 0;
						}
						step /= result;
						result = Pow();
						result = Math.Pow(result, step);
						break;
					case '!':
						currentChar++;
						result = Factorial(result);
						break;
					case '%':
						currentChar++;
						result = result * 0.01;
						break;
					default:

						break;
				}
			}
			return result;
		}

		private double MathArifm()
		{
			double result = Brackets();
			if (currentChar == Expression.Length)
				return result;
			while (Match("sin") || Match("cos") ||
					Match("tan") || Match("asin") ||
					Match("acos") || Match("atan") ||
					Match("ln") || Match("lg"))
			{
				switch (Expression.Substring(currentChar, 2))
				{
					case "ln":
						currentChar += 2;
						result = Math.Log(MathArifm());
						break;
					case "lg":
						currentChar += 2;
						result = Math.Log10(MathArifm());
						break;
					default:
						switch (Expression.Substring(currentChar, 3))
						{
							case "sin":
								currentChar += 3;
								result = Math.Sin(SetAngle(MathArifm()));
								break;
							case "cos":
								currentChar += 3;
								result = Math.Cos(SetAngle(MathArifm()));
								break;
							case "tan":
								currentChar += 3;
								result = Math.Tan(SetAngle(MathArifm()));
								break;
							default:
								switch (Expression.Substring(currentChar, 4))
								{
									case "asin":
										currentChar += 3;
										result = Math.Asin(SetAngle(MathArifm()));
										break;
									case "acos":
										currentChar += 3;
										result = Math.Acos(SetAngle(MathArifm()));
										break;
									case "atan":
										currentChar += 3;
										result = Math.Atan(SetAngle(MathArifm()));
										break;
								}
								break;
						}
						break;
				}
			}
			return result;
		}

		private double Factorial(double x)
		{
			if (x <= 1)
				return 1;
			return x * Factorial(x - 1);
		}

		private double Brackets()
		{
			double result = 0;
			if (currentChar == Expression.Length)
				return result;
			while (Match("(") || Match(")"))
			{
				if (CurrentChar == '(')
				{
					currentChar++;
					if (Expression.LastIndexOf(')') == -1 || Expression.LastIndexOf(')') < currentChar - 1)
					{
						//Console.WriteLine("Нету закрывающей скобки");
						return result;
					}
					Calculator calc = new Calculator();
					result = calc.Calculate(Expression.Substring(currentChar, Expression.LastIndexOf(')') - currentChar));
					currentChar = Expression.LastIndexOf(')') + 1;
					return result;
				}
				if (CurrentChar == ')')
				{
					currentChar++;
					//Console.WriteLine("Нету открывающей скобки");
					return result;
				}
			}
			return Digits();
		}

		private double Digits()
		{
			string number = "0";
			while ((Expression.Length != currentChar) && (
				                                             (CurrentChar >= '0') &&
				                                             (CurrentChar <= '9') ||
				                                             (CurrentChar == '.') ||
				                                             (CurrentChar == ',') ||
				                                             (CurrentChar == 'e') ||
															 (CurrentChar == 'π'))) 
			{
				if (CurrentChar == 'e')
				{
					currentChar++;
					return Math.E;
				}
				if (CurrentChar == 'π')
				{
					currentChar++;
					return Math.PI;
				}
				number += Expression[currentChar++];
			}
			return double.Parse(number);
		}
	}
}
