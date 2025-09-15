using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class EvaluarFuncion
    {
        public static string ConvertirPotencias(string funcion)
        {
            // Expresión regular básica para detectar a^b
            string pattern = @"(\w+)\s*\^\s*(\w+)";
            return Regex.Replace(funcion, pattern, "Pow($1,$2)");
        }


        public static string InsertarMultiplicacion(string funcion)
        {
            //4x → 4*x
            funcion = Regex.Replace(funcion, @"(\d)(x)", "$1*$2", RegexOptions.IgnoreCase);

            // x2 → x*2
            funcion = Regex.Replace(funcion, @"(x)(\d)", "$1*$2", RegexOptions.IgnoreCase);

            // x(2+3) → x*(2+3)
            funcion = Regex.Replace(funcion, @"(x)\(", "$1*(", RegexOptions.IgnoreCase);

            //3(2+1) → 3*(2+1)
            funcion = Regex.Replace(funcion, @"(\d)\(", "$1*(");

            return funcion;
        }


        public static string ConvertirAbs(string funcion)
        {
            return Regex.Replace(funcion, @"\|([^|]+)\|", "Abs($1)");
        }


        public static string NormalizarFunciones(string funcion)
        {
            funcion = Regex.Replace(funcion, @"\babs\b", "Abs", RegexOptions.IgnoreCase);
            funcion = Regex.Replace(funcion, @"\bsin\b", "Sin", RegexOptions.IgnoreCase);
            funcion = Regex.Replace(funcion, @"\bcos\b", "Cos", RegexOptions.IgnoreCase);
            funcion = Regex.Replace(funcion, @"\btan\b", "Tan", RegexOptions.IgnoreCase);
            funcion = Regex.Replace(funcion, @"\blog\b", "Log", RegexOptions.IgnoreCase);
            funcion = Regex.Replace(funcion, @"\bsqrt\b", "Sqrt", RegexOptions.IgnoreCase);
            return funcion;
        }

        public static double Evaluar(string funcion, double x)
        {
            funcion = InsertarMultiplicacion(funcion);
            funcion = ConvertirPotencias(funcion);
            funcion = NormalizarFunciones(funcion);
            funcion = ConvertirAbs(funcion);
            var expresion = new NCalc.Expression(funcion);
            expresion.Parameters["x"] = x;
            return Convert.ToDouble(expresion.Evaluate());
        }
    }

}
