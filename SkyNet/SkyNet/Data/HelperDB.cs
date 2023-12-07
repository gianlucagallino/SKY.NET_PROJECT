using Microsoft.Data.SqlClient;
using SkyNet.Entidades.Operadores;
using System.Data;

/*
    La clase HelperDB es el puente de conexión entre la aplicación y la base de datos.
    Proporciona métodos para realizar operaciones como obtener el siguiente juego, insertar información de partida y operador.
    Utiliza procedimientos almacenados y patrones de diseño, como Singleton, para gestionar eficientemente la conexión
    y ejecución de comandos en la base de datos.
    
    Pasos a tener en cuenta: 
    *Primero ejecute el Script de la Base de Datos en SQL Server.
    *Luego realice la conexión en visual studio ingresando en ver-->Explorador de servicios
    *Presione en el icono q posee una Base de Datos con un enchufe seleccione Microsoft SQL Server
    *En nombre de servidor rellene con el nombre del servidor con el que se conecta su SQL Server
    *En el campo de Seleccion de Base de Datos seleccione la base de SKYNET 
    *Pruebe Conexión, y si funciona dirijase hacia AVANZADAS.
    *Allí encontrara la cadena de conexión similar a la siguiente pero con los datos de su ordenador:
    Data Source=MARTIN\SQLEXPRESS;Initial Catalog=SKYNET;Integrated Security=True;Trust Server Certificate=True

    *Cambie la cadena de conexión ubicada en el constructor por la de su ordenador para que funcione.
    *Recuerde que en los parámetros del SqlConnection primero debe iniciar con @ y luego "cadena de conexión"
    *IMPORTANTE: SOLO SE GUARDARA EN BASE DE DATOS SI EL JUEGO DESEA SER GUARDADO (SAVEGAME).
 */

namespace SkyNet.Data
{
    public class HelperDB
    {

        private static HelperDB instance;
        private SqlConnection connection;
        public HelperDB()
        {
            connection = new SqlConnection(@"Data Source=MARTIN\SQLEXPRESS;Initial Catalog=SKYNET;Integrated Security=True;Trust Server Certificate=True");
        }
        public static HelperDB ObtenerInstancia()
        {
            if (instance == null)
                instance = new HelperDB();
            return instance;
        }
        public int GetNextGame()
        {
            string sp_nombre = "GetGame";
            string nombreOutput = "@next";
            return GetNext(sp_nombre, nombreOutput);
        }

        public int GetNext(string sp_nombre, string nombreOutPut)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = sp_nombre;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter OutPut = new SqlParameter();
            OutPut.ParameterName = nombreOutPut;
            OutPut.DbType = DbType.Int32;
            OutPut.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutPut);
            cmd.ExecuteNonQuery();
            connection.Close();
            return (int)OutPut.Value;
        }
        public void InsertPartida(int partidaID)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("InsertPartida", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar partida: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }
        public void InsertOperator(MechanicalOperator oper, int nextGame)
        {
            try
            {
                // Insertar en la tabla Operators
                OpenConnection();
                using (SqlCommand command = new SqlCommand("InsertOperator", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Nombre", oper.Id);
                    command.Parameters.AddWithValue("@KilometrosRecorridos", oper.KilometersTraveled);
                    command.Parameters.AddWithValue("@EnergiaConsumida", oper.EnergyConsumed);
                    command.Parameters.AddWithValue("@CargaTransportada", oper.TotalCarriedLoad);
                    command.Parameters.AddWithValue("@InstruccionesEjecutadas", oper.ExecutedInstructions);
                    command.Parameters.AddWithValue("@DaniosRecibidos", oper.DamagesReceived);
                    command.Parameters.AddWithValue("@UltimoLugar1", 0);
                    command.Parameters.AddWithValue("@UltimoLugar2", 0);
                    command.Parameters.AddWithValue("@UltimoLugar3", 0);
                    command.Parameters.AddWithValue("@PartidaID", nextGame);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción (por ejemplo, registrarla o lanzarla nuevamente si es necesario)
                Console.WriteLine($"Error al insertar operador: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        private void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }


    }
}
