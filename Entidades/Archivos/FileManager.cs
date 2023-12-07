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

            FileManager.path = Path.Combine(rutaEscritorio, "20231207_Gaston.Cvitanich.C");

            FileManager.ValidarExitenciaDeDirectorio();
        }

        /// <summary>
        /// Este metodo valida la existencia de un directorio
        /// </summary>
        /// <exception cref="FileManagerException">En el caso de que no se pueda crear el directorio, lanza una excepcion</exception>
        private static void ValidarExitenciaDeDirectorio()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (!Directory.Exists(FileManager.path))
            {
                try
                {
                    Directory.CreateDirectory(FileManager.path);
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
            string path = Path.Combine(FileManager.path, nombreArchivo);

            using (StreamWriter writer = new StreamWriter(path, append))
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
            string path = Path.Combine(FileManager.path, nombreArchivo);

            try
            {         
                //string json = JsonConvert.SerializeObject(elemento, Newtonsoft.Json.Formatting.Indented);
                //File.WriteAllText(FileManager.path, json);

                FileManager.Guardar(System.Text.Json.JsonSerializer.Serialize(elemento, typeof(T)), nombreArchivo, false);

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
