using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Entidades.Files
{
    
    public static class FileManager
    {
        private static string path; 

        static FileManager()
        {
            
            string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            path = Path.Combine(rutaEscritorio, "20231207_Alumn");

            ValidarExitenciaDeDirectorio();
        }

        /// <summary>
        /// Este metodo valida la existencia de un directorio
        /// </summary>
        /// <exception cref="FileManagerException">En el caso de que no se pueda crear el directorio, lanza una excepcion</exception>
        private static void ValidarExitenciaDeDirectorio()
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch(Exception ex)
                {
                    FileManager.Guardar(ex.Message, "Errores.txt", true);
                    throw new FileManagerException("Error al crear el directorio");
                }
                
            }
        }

        /// <summary>
        /// Este método genera un archivo de texto
        /// </summary>
        /// <param name="data">Recibe el mensaje de error</param>
        /// <param name="nombreArchivo">Recibe el nombre del archivo</param>
        /// <param name="append">Recibe un booleano en caso de que se quiera sobreescribir el archivo o no</param>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            string filePath = Path.Combine(path, nombreArchivo);

            using (StreamWriter writer = new StreamWriter(filePath, append))
            {
                writer.WriteLine(data);
            }
        }

        /// <summary>
        /// Este metodo se encarga de serializar un objeto del tipo generico a formato JSON y guardarlo en un archivo.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elemento">Recibe una referencia a una class</param>
        /// <param name="nombreArchivo">Recibe el nombre del archivo</param>
        /// <returns>Retorna true si la operacion se realizo con exito de lo contrario retorna false</returns>
        public static bool Serializar<T>(T elemento, string nombreArchivo) where T : class
        {
            string filePath = Path.Combine(path, nombreArchivo);

            try
            {
                string json = JsonConvert.SerializeObject(elemento, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);

                return true;
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "Errores.txt", true);
                return false;
            }
        }

    }
}
