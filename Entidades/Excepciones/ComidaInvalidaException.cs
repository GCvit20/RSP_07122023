using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Excepciones
{
    /// <summary>
    /// Excepción lanzada cuando se detecta que el tipo de comida no es válida.
    /// </summary>
    public class ComidaInvalidaException : Exception
    {
        public ComidaInvalidaException(string? message) : base(message)
        {
        }
    }
}