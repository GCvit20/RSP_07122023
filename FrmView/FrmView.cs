using Entidades.Exceptions;
using Entidades.Serializacion;
using Entidades.Interfaces;
using Entidades.Modelos;

namespace FrmView
{
    public partial class FrmView : Form
    {
        private Queue<IComestible> comidas;
        private Cocinero<Hamburguesa> hamburguesero;

        public FrmView()
        {
            InitializeComponent();
            this.comidas = new Queue<IComestible>();
            this.hamburguesero = new Cocinero<Hamburguesa>("Ramon");

            // Alumno - agregar manejadores al cocinero
            this.hamburguesero.OnDemora += this.MostrarConteo;
            this.hamburguesero.OnIngreso += this.MostrarComida;
        }

        // Alumno: Realizar los cambios necesarios sobre MostrarComida de manera que se refleje
        // en el formulario los datos de la comida
        private void MostrarComida(IComestible comida)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarComida(comida));
            }
            else
            {
                this.comidas.Enqueue(comida);
                this.pcbComida.Load(comida.Imagen);
                this.rchElaborando.Text = comida.ToString();
            }
        }

        // Alumno: Realizar los cambios necesarios sobre MostrarConteo de manera que se refleje
        // en el formulario el tiempo transcurrido
        private void MostrarConteo(double tiempo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarConteo(tiempo));
            }
            else
            {
                this.lblTiempo.Text = $"{tiempo} segundos";
                this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion.ToString("00.0")} segundos";
            }
        }

        private void ActualizarAtendidos(IComestible comida)
        {
            this.rchFinalizados.Text += "\n" + comida.Ticket;
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                this.hamburguesero.HabilitarCocina = true;
                this.btnAbrir.Image = Properties.Resources.close_icon;
            }
            else
            {
                this.hamburguesero.HabilitarCocina = false;
                this.btnAbrir.Image = Properties.Resources.open_icon;
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            if (this.comidas.Count > 0)
            {
                IComestible comida = this.comidas.Dequeue();
                comida.FinalizarPreparacion(this.hamburguesero.Nombre);
                this.ActualizarAtendidos(comida);
            }
            else
            {
                MessageBox.Show("El Cocinero no posee comidas", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void FrmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FileManager.Serializar(this.hamburguesero, "Cocinero.json");
            }
            catch (FileManagerException ex)
            {
                FileManager.Guardar(ex.Message, "Logs.txt", true);
            }
        }


        private void FrmView_Load(object sender, EventArgs e)
        {

        }

        private void lblTmp_Click(object sender, EventArgs e)
        {

        }

        private void FrmView_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
