using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    public static class operacje
    {
        public static double Dodaj(double value1, double value2)
        {
            double result;
            result = value1 + value2;
            return result;
        }
        public static double odejmij(double value1, double value2)
        {
            double result;
            result = value1 - value2;
            return result;
        }
        public static double Pomnoz(double value1, double value2)
        {
            double result;
            result = value1 * value2;
            return result;
        }
        public static double Podziel(double value1, double value2)
        {
            double result;
            result = value1 / value2;
            return result;
        }
    }
}
