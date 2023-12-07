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
    public delegate void DelegadoPedidoEnCurso(IComestible pedido);
    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidosEnPreparacion;
        private Task tarea;
        public event DelegadoPedidoEnCurso OnPedido;
        public event DelegadoDemoraAtencion OnDemora;
        private Mozo<T> mozo;
        private Queue<T> pedidos;


        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();
            this.mozo.OnPedido += this.TomarNuevoPedido;
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
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation.Cancel();

                    if (this.mozo.EmpezarATrabajar)
                    {
                        this.mozo.EmpezarATrabajar = false;
                    }
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => this.nombre; }
        public int CantPedidosFinalizados { get => this.cantPedidosFinalizados; }
        public Queue<T> Pedidos { get => this.pedidos; }

        /// <summary>
        /// Este metodo inicia un ingreso notificando un nuevo ingreso, espera un proximo ingreso y guarda un ticket en la base de datos.
        /// </summary>
        /// <exception cref="DataBaseManagerException">lanzara una excepcion en caso de que no pueda guardar el ticket en la base de datos</exception>

        private void EmpezarACocinar()
        {

            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    if(this.Pedidos.Count > 0 && this.OnPedido is not null)
                    {
                        this.pedidosEnPreparacion = this.Pedidos.Dequeue();
                        this.OnPedido?.Invoke(this.pedidosEnPreparacion);
                        this.EsperarProximoIngreso();
                        this.cantPedidosFinalizados++;
                    

                        try
                        {
                            DataBaseManager.GuardarTicket(this.Nombre, this.pedidosEnPreparacion);
                        }
                        catch (DataBaseManagerException ex)
                        {
                            FileManager.Guardar(ex.Message, "Logs.txt", true);
                            throw new DataBaseManagerException("Error al guardar el ticket en la Base de datos.", ex.InnerException);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }, this.cancellation.Token);
        }


        /// <summary>
        /// Este metodo espera hasta que se solicite la cancelación o hasta que se cumpla alguna condición acumulando el tiempo de espera.
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            while (this.OnDemora is not null && !this.pedidosEnPreparacion.Estado && !this.cancellation.IsCancellationRequested)
            {
                tiempoEspera++;
                this.OnDemora.Invoke(tiempoEspera);
                Thread.Sleep(1000);
            }

            this.demoraPreparacionTotal += tiempoEspera;
        }

        private void TomarNuevoPedido(T menu)
        {
            if(this.OnPedido is not null)
            {
                this.pedidos.Enqueue(menu);
            }
        }
    }
}
