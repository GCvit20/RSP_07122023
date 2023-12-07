using System;
using System.Collections.Generic;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;
using System.Data.SqlClient;
using Entidades.Serializacion;

public static class DataBaseManager
{
    private static string connectionString;
    private static SqlConnection connection;

    /// <summary>
    /// Inicializa la cadena de conexión estática.
    /// </summary>
    static DataBaseManager()
    {
        DataBaseManager.connectionString = "Server=DESKTOP-JJ7VCTI;Database=20230622SP;Trusted_Connection=True;"; //Server=.;
    }

    /// <summary>
    /// Este metodo se encarga de buscar la URL de la imagen almacenada en la BD dependiendo el tipo que se le pase por parametros.
    /// </summary>
    /// <param name="tipo">Recibe el tipo de comida</param>
    /// <returns>Retorna un string con la URL de la imagen encontrada</returns>
    /// <exception cref="ComidaInvalidaException">Lanza esta excepcion en caso de que no exista el tipo de comida solicitado</exception>
    /// <exception cref="DataBaseManagerException">Lanza esta excepcion en caso de que se produzca otra excepcion</exception>
    public static string GetImagenComida(string tipo)
    {
        try
        {
            using (DataBaseManager.connection = new SqlConnection(DataBaseManager.connectionString))
            {
                string query = "SELECT * FROM comidas WHERE Tipo_comida = @Tipo";

                SqlCommand command = new SqlCommand(query, DataBaseManager.connection);
                command.Parameters.AddWithValue("@Tipo", tipo);

                DataBaseManager.connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return reader.GetString(2);
                }

                throw new ComidaInvalidaException($"Tipo de comida '{tipo}' no encontrado.");
            }
        }
        catch (Exception ex)
        {
            FileManager.Guardar(ex.Message, "Logs.txt", true);
            throw new DataBaseManagerException("Error al leer", ex);
        }
    }

    /// <summary>
    /// Este metodo sirve para generar un ticket de la comida
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nombreEmpleado">Recibe el nombre del empleado</param>
    /// <param name="comida">Recibe un generico</param>
    /// <returns>Retorna un booleano</returns>
    public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
    {
        try
        {
            using (DataBaseManager.connection = new SqlConnection(DataBaseManager.connectionString))
            {
                string consulta = @"INSERT INTO tickets (empleado, ticket) VALUES (@empleado, @ticket)";

                SqlCommand command = new SqlCommand(consulta, connection);

                command.Parameters.AddWithValue("@empleado", nombreEmpleado);
                command.Parameters.AddWithValue("@ticket", comida.Ticket);

                DataBaseManager.connection.Open();

                command.ExecuteNonQuery();
                return true;
            }
        }
        catch (Exception ex)
        {
            FileManager.Guardar(ex.Message, "Logs.txt", true);
            throw new DataBaseManagerException("Error al escribir", ex);
        }
    }
}
