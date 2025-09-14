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
            funcion = ConvertirPotencias(funcion);
            funcion = NormalizarFunciones(funcion);
            var expresion = new NCalc.Expression(funcion);
            expresion.Parameters["x"] = x;
            return Convert.ToDouble(expresion.Evaluate());
        }
    }

}
