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
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string consulta = "SELECT * FROM comidas WHERE tipo_comida = @tipo_comida";

                    SqlCommand command = new SqlCommand(consulta, DataBaseManager.connection);
                    command.Parameters.AddWithValue("@tipo_comida", tipo);

                    DataBaseManager.connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        return reader.GetString(2);
                    }
                    else
                    {
                        throw new ComidaInvalidaExeption($"Tipo de comida '{tipo}' no encontrado.");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "Errores.txt", true);
                throw new DataBaseManagerException("Error al leer la base de dato");
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

            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string consulta = "INSERT INTO tickets (empleado, ticket) VALUES (@empleado, @ticket)";

                    SqlCommand command = new SqlCommand(consulta, DataBaseManager.connection);

                    command.Parameters.AddWithValue("@empleado", nombreEmpleado);
                    command.Parameters.AddWithValue("@ticket", comida.ToString());

                    DataBaseManager.connection.Open();

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
