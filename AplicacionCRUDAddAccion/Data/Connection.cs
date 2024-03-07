using System.Data.SqlClient;

namespace AplicacionCRUDAddAccion.Data
{
    public class Connection
    {
        private string cadenaSQL = string.Empty;

        //Constructor
        public Connection()
        {   
            //La cadena de conexion se guarda en el archivo appsettings.json, de ahi la obtenemos.
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            cadenaSQL = builder.GetSection("ConnectionStrings:CadenaSQL").Value;
        }

        //Metodo para obtener la cadena de conexion a la base de datos
        public string getCadenaSQL() {  return cadenaSQL; } 
    }
}
