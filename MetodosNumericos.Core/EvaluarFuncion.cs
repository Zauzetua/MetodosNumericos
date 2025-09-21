using System.Text.RegularExpressions;

namespace MetodosNumericos.Core
{
    public class EvaluarFuncion
    {
        /// <summary>
        /// Funcion tipo regex que convierte las potencias de la forma a^b en Pow(a,b). Es de apoyo para NCalc
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public static string ConvertirPotencias(string funcion)
        {
            // Expresión regular básica para detectar a^b
            string pattern = @"(\w+)\s*\^\s*(\w+)";
            return Regex.Replace(funcion, pattern, "Pow($1,$2)");
        }


        /// <summary>
        /// Funcion tipo regex que inserta los signos de multiplicacion donde se omiten. Es de apoyo para NCalc
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Funcion tipo regex que convierte abs(...) en Abs(x). Es de apoyo para NCalc. Medio inutil
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public static string ConvertirAbs(string funcion)
        {
            return Regex.Replace(funcion, @"\|([^|]+)\|", "Abs($1)");
        }

        /// <summary>
        /// Funcion que normaliza las funciones matematicas a la notacion de NCalc
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Funcion que normaliza una funcion para que pueda ser evaluada por NCalc
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public static string NormalizarFuncion(string funcion)
        {
            funcion = InsertarMultiplicacion(funcion);
            funcion = ConvertirPotencias(funcion);
            funcion = NormalizarFunciones(funcion);
            funcion = ConvertirAbs(funcion);
            return funcion;
        }

        /// <summary>
        /// Funcion que evalua una funcion en un punto x
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Evaluar(string funcion, double x)
        {
            funcion = NormalizarFuncion(funcion);
            var expresion = new NCalc.Expression(funcion);
            expresion.Parameters["x"] = x;
            return Convert.ToDouble(expresion.Evaluate());
        }
    }

}
