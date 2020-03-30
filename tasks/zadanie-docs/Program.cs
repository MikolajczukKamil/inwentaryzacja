using System;

namespace DoxygenTest
{
	class Program
	{
		static void Main(string[] args)
		{
			// Powitanie użytkownika
			Console.WriteLine("Jak się nazywasz?");
			WelcomeName(GetString());

			// Test metod matematycznych
			Console.WriteLine(MoreMath.Factorial(5));
			Console.WriteLine(MoreMath.Fibonacci(10));
			Console.WriteLine(MoreMath.Lerp(1f, 6f, 0.5f));

			Console.ReadKey();
		}

		/// <summary>
		/// Pobiera tekst od użytkownika i zwraca go jako string.
		/// </summary>
		/// <returns>(string) wartość wpisana do konsoli przez użytkownika</returns>
		static string GetString()
		{
			return Console.ReadLine();
		}

		/// <summary>
		/// Przymuje imię użytkownika jako parametr string i wyświetla wiadomość powitalną w konsoli.
		/// </summary>
		/// <param name="name">(string) imię użytkownika</param>
		static void WelcomeName(string name)
		{
			Console.WriteLine("Witaj " + name + "!");
		}

		/// <summary>
		/// Zwraca sumę dwóch liczb całkowitych.
		/// </summary>
		/// <param name="a">(int) pierwszy składnik dodwania</param>
		/// <param name="b">(int) drugi składnik dodwania</param>
		/// <returns>(int) suma podanych składników</returns>
		static int Add( int a, int b )
		{
			return a + b;
		}
	}

	static class MoreMath
	{
		public static int Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentException( "Parameter must be >= 0!" );
			}

			if (n <= 1)
			{
				return 1;
			}

			return n * Factorial(n - 1);
		}

		public static int Fibonacci(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentException( "Parameter must be >= 0!" );
			}

			if (n <= 2)
			{
				return 1;
			}

			return Fibonacci(n - 1) + Fibonacci(n - 2);
		}

		public static float Lerp(float v0, float v1, float t)
		{
			return (1 - t) * v0 + t * v1;
		}
	}
}
