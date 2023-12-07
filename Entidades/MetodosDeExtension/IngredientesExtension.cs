using Entidades.Enumerados;


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
            
            if (ingredientes.Count == 0)
            {
                throw new InvalidOperationException("La lista de ingredientes está vacía.");
            }


            double costoTotal = costoInicial;

            foreach (EIngrediente ingrediente in ingredientes)
            {
                costoTotal += costoTotal * ((double)ingrediente / 100);
            }

            return costoTotal;
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

            // Genera un número aleatorio entre 1 y el tamaño de la lista + 1
            int numeroAleatorio = rand.Next(1, ingredientes.Count + 1);

            // Retorna una lista de ingredientes aleatorios en base al número obtenido aleatoriamente

            return ingredientes.Take(numeroAleatorio).ToList();

        }

    }
}
