namespace FrmView
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicializa la configuración de la aplicación, como la configuración de DPI o la fuente predeterminada.
            // Consulta https://aka.ms/applicationconfiguration para personalizar la configuración de la aplicación.
            ApplicationConfiguration.Initialize();

            // Ejecuta la aplicación, mostrando el formulario principal (FrmView).
            Application.Run(new FrmView());
        }
    }
}
