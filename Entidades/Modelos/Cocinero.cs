using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Serializacion;

namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoIngreso(IComestible menu);
    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;
        public event DelegadoNuevoIngreso OnIngreso;
        public event DelegadoDemoraAtencion OnDemora;

        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        /// <summary>
        /// Este metodo inicia un ingreso notificando un nuevo ingreso, espera un proximo ingreso y guarda un ticket en la base de datos.
        /// </summary>
        /// <exception cref="DataBaseManagerException">lanzara una excepcion en caso de que no pueda guardar el ticket en la base de datos</exception>

        private void IniciarIngreso()
        {

            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    this.NotificarNuevoIngreso();
                    this.EsperarProximoIngreso();
                    this.cantPedidosFinalizados++;

                    try
                    {
                        DataBaseManager.GuardarTicket(this.Nombre, this.menu);
                    }
                    catch (DataBaseManagerException ex)
                    {
                        FileManager.Guardar(ex.Message, "Logs.txt", true);
                        throw new DataBaseManagerException("Error al guardar el ticket en la Base de datos.", ex.InnerException);
                    }
                }
            }, this.cancellation.Token);
        }

        /// <summary>
        /// Este metodo inciia la preparacion del menu y notifica el menu.
        /// </summary>
        private void NotificarNuevoIngreso()
        {
            if (this.OnIngreso is not null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.OnIngreso.Invoke(this.menu);
            }
        }

        /// <summary>
        /// Este metodo espera hasta que se solicite la cancelación o hasta que se cumpla alguna condición acumulando el tiempo de espera.
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            while (this.OnDemora is not null && !this.menu.Estado && !this.cancellation.IsCancellationRequested)
            {
                tiempoEspera++;
                this.OnDemora.Invoke(tiempoEspera);
                Thread.Sleep(1000);
            }

            this.demoraPreparacionTotal += tiempoEspera;
        }
    }
}
