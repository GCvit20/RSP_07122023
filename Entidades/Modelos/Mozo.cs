using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoPedido<T>(T menu);
    public class Mozo<T> where T : IComestible
    {
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;
        public event DelegadoNuevoPedido<T> OnPedido;

        public bool EmpezarATrabajar
        {
            get
            {
                return this.tarea != null &&
                       (this.tarea.Status == TaskStatus.Running ||
                        this.tarea.Status == TaskStatus.WaitingToRun ||
                        this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value)
                {
                    if (this.tarea == null ||
                        this.tarea.Status != TaskStatus.Running &&
                        this.tarea.Status != TaskStatus.WaitingToRun &&
                        this.tarea.Status != TaskStatus.WaitingForActivation)
                    {
                        this.cancellation = new CancellationTokenSource();
                        this.tarea = Task.Run(() => TomarPedidos());
                    }
                }
                else
                {
                    this.cancellation?.Cancel();
                }
            }
        }




        private void NotificarNuevoPedido()
        {
            if (this.OnPedido is not null)
            {
                //this.menu = new T(); //arreglar
                this.menu.IniciarPreparacion();
                this.OnPedido.Invoke(this.menu);
            }
        }

        private void TomarPedidos()
        {
            this.tarea = Task.Run(() => 
            {
                if(this.cancellation.IsCancellationRequested)
                {
                    
                    this.NotificarNuevoPedido();
                    Thread.Sleep(5000);
                }
            });
        }

    }
}
