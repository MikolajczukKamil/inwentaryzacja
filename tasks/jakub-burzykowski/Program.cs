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
	{   /// <summary>
    /// Zwraca silnie liczby całkowitej.
    /// </summary>
    /// <param name="n">(int) liczba z której będzie policzona silnia</param>
    /// <returns>(int)wynik silni dla podanej liczby</returns>
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

        /// <summary>
        /// Zwraca wartość n-tego wyrazu ciągu Fibonacciego (sumy jego dwóch poprzednich wyrazów).
        /// </summary>
        /// <param name="n">(int) wyraz, którego suma będzie liczona</param>
        /// <returns>(int) policzona wartość podanego wyrazu</returns>
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

        /// <summary>
        /// Zwraca przybliżoną wartość funkcji interpolacyjnej (przybliżającej), w podanym przedziale, która przyjmuje znane wartości w określonych punktach.
        /// </summary>
        /// <param name="v0">(float) wartość znanego punktu 1</param>
        /// <param name="v1">(float) wartość znanego punktu 2</param>
        /// <param name="t">(float) odległość (znormalizowana) pomiędzy jednym z punktów, a nieznanym punktem</param>
        /// <returns>przybliżona wartość funkcji interpolacyjnej dla podanych punktów</returns>
		public static float Lerp(float v0, float v1, float t)
		{
			return (1 - t) * v0 + t * v1;
		}
	}
}
