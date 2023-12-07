using Entidades.Exceptions;
using Entidades.Serializacion;
using Entidades.Modelos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {

            string nombreArchivoInvalido = "";
            string data = "Prueba";

            FileManager.Guardar(data, nombreArchivoInvalido, false);

        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Ramon");

            //act
            int valorEsperado = 0;

            //assert
            Assert.AreEqual(cocinero.CantPedidosFinalizados, valorEsperado);
        }
    }
}