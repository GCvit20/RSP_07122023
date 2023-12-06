using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection; 

        static DataBaseManager()
        {
            DataBaseManager.stringConnection = "Server=DESKTOP-JJ7VCTI;Database=20230622SP;Trusted_Connection=True;";
        }


        /// <summary>
        /// Este metodo se encarga de buscar la URL de la imagen almacenada en la BD dependiendo el tipo que se le pase por parametros.
        /// </summary>
        /// <param name="tipo">Recibe el tipo de comida</param>
        /// <returns>Retorna un string con la URL de la imagen encontrada</returns>
        /// <exception cref="ComidaInvalidaExeption">Lanza esta excepcion en caso de que no exista el tipo de comida solicitado</exception>
        /// <exception cref="DataBaseManagerException">Lanza esta excepcion en caso de que se produzca otra excepcion</exception>
        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string consulta = "SELECT imagen FROM comidas WHERE tipo_comida = @tipo_comida";

                    SqlCommand command = new SqlCommand(consulta, connection);
                    command.Parameters.AddWithValue("@tipo_comida", tipo);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        else
                        {
                            throw new ComidaInvalidaExeption($"Tipo de comida '{tipo}' no encontrado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "Errores.txt", true);
                throw new DataBaseManagerException("Error al intentar conectar en la DB o en realizar la consulta solicitada"); ; // Lanza la excepción para que pueda ser manejada en el código que llama a este método
            }
        }

        /// <summary>
        /// Este metodo sirve para generar un ticket de la comida
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nombreEmpleado">Recibe el nombre del empleado</param>
        /// <param name="comida">Recibe la comida</param>
        /// <returns>Retornara true si la cantidad de columnas afectadas es mayor a 0. De lo contrario retorna false</returns>
        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            string query = "INSERT INTO tickets (empleado, ticket) VALUES (@empleado, @ticket)";

            try
            {
                using (SqlCommand command = new SqlCommand(query, DataBaseManager.connection))
                {
                    connection.Open();


                    command.Parameters.AddWithValue("@empleado", nombreEmpleado);
                    command.Parameters.AddWithValue("@ticket", comida.ToString()); 

                    int columnasAfectadas = command.ExecuteNonQuery();

                    return columnasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "Errores.txt", true);
                return false;
            }
        }

    }
}
