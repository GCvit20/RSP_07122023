using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;
using System.Globalization;

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
        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";


        public Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        /// <summary>
        /// Este meotdo agrega los ingredientes de forma aleatoria 
        /// </summary>

        private void AgregarIngredientes()
        {
            Random rand = new Random();
            
            List<EIngrediente> ingredientesAleatorios = rand.IngredientesAleatorios();
        }

        /// <summary>
        /// Este metodo mostrara los datos del pedido
        /// </summary>
        /// <returns>Retorna un string con los datos</returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }



        public override string ToString() => this.MostrarDatos();

        
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo =  IngredientesExtension.CalcularCostoIngredientes(this.ingredientes, Hamburguesa.costoBase);
            this.estado = false;
        }


        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                Random random = new Random();
                int numeroRandom = random.Next(1,9);

                string img = DataBaseManager.GetImagenComida($"Hamburguesa_{numeroRandom}");

                AgregarIngredientes();
            }
        }
    }
}