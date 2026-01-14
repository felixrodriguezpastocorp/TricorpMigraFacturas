using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TricorpMigraFacturas
{
    public class SqlConnections
    {
        public static bool BuscarEmpresas(string aStringConnection, ref string aMensaje, ref List<Interfaces.empresa> aEmpresas)
        {
            bool lResult = false;

            aMensaje = "No se encontraron empresas";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            "select " +
                            "cNombreEmpresa as nombre," +
                            "cRutaDatos as ruta " +
                            "from " +
                            "Empresas " +
                            "where " +
                            "cidempresa <> 1";

                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        Interfaces.empresa lEmpresa = new Interfaces.empresa();

                        lEmpresa.nombreempresa = registros["nombre"].ToString();
                        lEmpresa.rutaempresa = registros["ruta"].ToString();

                        aEmpresas.Add(lEmpresa);

                        lResult = true;
                    };
                    conexion.Close();
                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar empresas " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }
    }
}
