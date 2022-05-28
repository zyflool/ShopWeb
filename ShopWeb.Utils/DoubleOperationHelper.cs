using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Utils
{
    public class DoubleOperationHelper
    {
        public static double Add(double a, double b)
        {
            double A = a * 100;
            double B = b * 100;
            double C = A + B;
            double c = C / 100.0;
           return ( a * 100 + b * 100) / 100.0;
        }

        public static double Subtract(double a, double b)
        {
            return (a * 100 - b * 100) / 100.0;
        }
        public static double Multiply(double a, double b)
        {
            return (a * 100 * b * 100) / 10000.0;
        }
        public static double Divide(double a, double b)
        {
            return (a * 100.0) / (b * 100.0);
        }
    }
}
