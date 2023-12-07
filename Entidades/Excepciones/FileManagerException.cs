using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entidades.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se produce un error durante la gestión de archivos.
    /// </summary>
    public class FileManagerException : Exception
    {
        public FileManagerException(string? message) : base(message)
        {
        }

        public FileManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}