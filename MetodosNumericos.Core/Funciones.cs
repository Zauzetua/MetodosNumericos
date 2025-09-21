using NCalc;

namespace MetodosNumericos.Core
{
    public class Funciones
    {
        /// <summary>
        /// Metodo que convierte una funcion en string a un delegado Func<double, double>
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public Func<double, double> ConvertirAFunc(string funcion)
        {
            funcion = EvaluarFuncion.InsertarMultiplicacion(funcion);
            funcion = EvaluarFuncion.ConvertirPotencias(funcion);
            funcion = EvaluarFuncion.NormalizarFunciones(funcion);
            funcion = EvaluarFuncion.ConvertirAbs(funcion);

            // Parsear la funcion usando NCalc
            var expr = new Expression(funcion);
            

            // Devolver un delegado que evalua la funcion para un valor dado de x
            return x =>
            {
                expr.Parameters["x"] = x;          // Asignamos valor de x
                return Convert.ToDouble(expr.Evaluate()); // Evaluamos y convertimos a double
            };
        }
    }
}
