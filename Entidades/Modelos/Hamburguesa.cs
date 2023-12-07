using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Serializacion;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {
        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;
        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        public bool Estado { get => this.estado; }
        public string Imagen { get => this.imagen; }
        public string Ticket => $"{this}\nTotal a pagar: ${this.costo}";

        public Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
            this.ingredientes = new List<EIngrediente>();
        }

        /// <summary>
        /// Este meotdo agrega los ingredientes de forma aleatoria.
        /// </summary>

        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }


        /// <summary>
        /// Este metodo mostrara los datos del pedido.
        /// </summary>
        /// <returns>Retorna un string que representa los datos de la hamburguesa.</returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));

            return stringBuilder.ToString();
        }

        public override string ToString() => this.MostrarDatos();

        /// <summary>
        /// Finaliza la preparación del objeto, calcula el costo total de los ingredientes y cambia el estado del objeto.
        /// </summary>
        /// <param name="cocinero">El nombre del cocinero responsable de la preparación.</param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngredientes(Hamburguesa.costoBase);
            this.estado = !this.Estado;
        }


        /// <summary>
        /// Inicia la preparación de la comida si no ha sido iniciada previamente.
        /// </summary>
        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int indice = this.random.Next(1, 9);

                try
                {
                    this.imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{indice}");
                    this.AgregarIngredientes();
                }
                catch (DataBaseManagerException ex)
                {
                    FileManager.Guardar(ex.Message, "Logs.txt", true);
                    throw new DataBaseManagerException("No se pudo inicar la preparacion", ex);
                }
            }
        }

    }
}
