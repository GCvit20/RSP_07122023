using Entidades.Enumerados;
using Entidades.Serializacion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        /// <summary>
        /// Este metodo toma el costo inicial e incrementar su valor porcentualmente en base a los valores de la lista de Eingredientes. 
        /// </summary>
        /// <param name="ingredientes">Recibe la lista de ingredientes</param>
        /// <param name="costoInicial">Recibe un numero entero que representa el costo inicial</param>
        /// <returns>Retorna el costo total</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            try
            {
                ingredientes.ForEach(ingrediente => costoInicial += (costoInicial * (int)ingrediente / 100));

                return costoInicial;
            }
            catch (Exception ex) 
            {
                FileManager.Guardar(ex.Message, "Logs.txt", true);
                throw new InvalidOperationException("La lista de ingredientes está vacía.");
            }

        }

        /// <summary>
        /// Este metodo genera una lista de ingredientes aleatoria
        /// </summary>
        /// <param name="rand">extiende la clase Random</param>
        /// <returns>Lista de ingredisentes</returns>
        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON
            };

            int numeroAleatorio = rand.Next(1, ingredientes.Count + 1);

            return ingredientes.Take(numeroAleatorio).ToList();

        }
    }
}
