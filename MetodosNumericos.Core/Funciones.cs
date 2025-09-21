using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class Funciones
    {
        public Func<double, double> ConvertirAFunc(string funcion)
        {
            funcion = EvaluarFuncion.InsertarMultiplicacion(funcion);
            funcion = EvaluarFuncion.ConvertirPotencias(funcion);
            funcion = EvaluarFuncion.NormalizarFunciones(funcion);
            funcion = EvaluarFuncion.ConvertirAbs(funcion);

            // Parsear la función usando NCalc
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
