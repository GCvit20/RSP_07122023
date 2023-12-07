namespace Entidades.Interfaces
{
    /// <summary>
    /// Interfaz que define las propiedades y métodos comunes para los objetos comestibles.
    /// </summary>
    public interface IComestible
    {
        bool Estado { get; }

        string Imagen { get; }

        string Ticket { get; }

        void FinalizarPreparacion(string cocinero);

        void IniciarPreparacion();
    }
}
