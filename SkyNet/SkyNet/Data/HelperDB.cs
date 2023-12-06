using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using SkyNet.Entidades.Operadores;


namespace SkyNet.Data
{
    public class HelperDB
    {
        //Data Source=MARTIN\SQLEXPRESS;Initial Catalog=SKYNET;Integrated Security=True;Trust Server Certificate=True
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
        public int ObtenerProximaPartidaId()
        {
            int nextGameId = 0;

            try
            {
                OpenConnection();

                using (SqlCommand command = new SqlCommand("GetNextGame", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        nextGameId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el próximo juego: {ex.Message}");
            }
            finally
            {

                CloseConnection();
            }

            return nextGameId;
        }

        public void InsertPartida(int number)
        {
            number = ObtenerProximaPartidaId();
            OpenConnection();
            SqlCommand command = new SqlCommand("InsertPartida");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PartidaID", number);
        }
        public void InsertOperator(MechanicalOperator oper)
        {
            try
            {
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

                   /* SqlParameter outputParameter = new SqlParameter("@OperatorID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParameter);*/

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
