using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TricorpMigraFacturasTask
{
    public class SqlConnections
    {
        public static bool BuscarDocumento(string aStringConnection, ref string aMensaje, ref Interfaces.documento aDocumento, string aConcepto, string aSerie, string aFolio)
        {
            bool lResult = false;

            aMensaje = "No se encontró documento";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            "select " +
                            "d.ciddocumento as iddocumento, " +
                            "d.cidmoneda as moneda, " +
                            "d.ctipocambio as tc, " +
                            "d.cdescuentodoc1 as des1, " +
                            "d.cdescuentodoc2 as des2, " +
                            "cl.ccodigocliente as cliente, " +
                            "d.creferencia as referencia, " +
                            "d.cgasto1 as gasto1, " +
                            "d.cgasto2 as gasto2, " +
                            "d.cgasto3 as gasto3, " +
                            "isnull(d.cobservaciones, '') as observaciones, " +
                            "fd.cuuid as uuid " +
                            "from " +
                            "admDocumentos d " +
                            "join admConceptos c on c.cidconceptodocumento = d.cidconceptodocumento " +
                            "join admClientes cl on cl.cidclienteproveedor = d.cidclienteproveedor " +
                            "join admFoliosDigitales fd on fd.ciddocto = d.ciddocumento " +
                            "where " +
                            "d.ccancelado = 0 " +
                            "and d.ciddocumentode = 4 " +
                            "and d.cseriedocumento = '" + aSerie + "' " +
                            "and d.cfolio = " + aFolio + " " +
                            "and c.ccodigoconcepto = '" + aConcepto + "' ";

                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        aDocumento.iddocumento = registros["iddocumento"].ToString();
                        aDocumento.moneda = registros["moneda"].ToString();
                        aDocumento.tc = registros["tc"].ToString();
                        aDocumento.descuentodoc1 = registros["des1"].ToString();
                        aDocumento.descuentodoc2 = registros["des2"].ToString();
                        aDocumento.cliente = registros["cliente"].ToString();
                        aDocumento.referencia = registros["referencia"].ToString();
                        aDocumento.gasto1 = registros["gasto1"].ToString();
                        aDocumento.gasto2 = registros["gasto2"].ToString();
                        aDocumento.gasto3 = registros["gasto3"].ToString();
                        aDocumento.observaciones = registros["observaciones"].ToString();
                        aDocumento.uuid = registros["uuid"].ToString();

                        lResult = true;
                    };
                    conexion.Close();
                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar documento " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }

        public static bool BuscarMovimientos(string aStringConnection, ref string aMensaje, ref List<Interfaces.movimiento> aMovimientos, string aIdDocumento)
        {
            bool lResult = false;

            aMensaje = "No se encontró documento";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            "select " +
                            "m.cunidadescapturadas as unidades, " +
                            "case " +
                            "when m.cidunidad = 0 then '' " +
                            "else ump.cabreviatura end as unidad, " +
                            "m.cprecio as precio, " +
                            "m.cporcentajeimpuesto1 as porcimp1, " +
                            "m.cporcentajeimpuesto2 as porcimp2, " +
                            "m.cporcentajeimpuesto3 as porcimp3, " +
                            "m.cporcentajeretencion1 as porret1, " +
                            "m.cporcentajeretencion2 as porret2, " +
                            "m.cporcentajedescuento1 as pordes1, " +
                            "m.cporcentajedescuento2 as pordes2, " +
                            "m.cporcentajedescuento3 as pordes3, " +
                            "m.cporcentajedescuento4 as pordes4, " +
                            "m.cporcentajedescuento5 as pordes5, " +
                            "m.creferencia as referencia, " +
                            "isnull(m.cobservamov, '') as observaciones " +
                            "from " +
                            "admMovimientos m " +
                            "join admUnidadesMedidaPeso ump on ump.cidunidad = m.cidunidad " +
                            "where m.ciddocumento = " + aIdDocumento + " ";

                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        Interfaces.movimiento lMovimiento = new Interfaces.movimiento();

                        lMovimiento.unidades = registros["unidades"].ToString();
                        lMovimiento.unidad = registros["unidad"].ToString();
                        lMovimiento.precio = registros["precio"].ToString();
                        lMovimiento.porcimp1 = registros["porcimp1"].ToString();
                        lMovimiento.porcimp2 = registros["porcimp2"].ToString();
                        lMovimiento.porcimp3 = registros["porcimp3"].ToString();
                        lMovimiento.porcret1 = registros["porret1"].ToString();
                        lMovimiento.porcret2 = registros["porret2"].ToString();
                        lMovimiento.porcdes1 = registros["pordes1"].ToString();
                        lMovimiento.porcdes2 = registros["pordes2"].ToString();
                        lMovimiento.porcdes3 = registros["pordes3"].ToString();
                        lMovimiento.porcdes4 = registros["pordes4"].ToString();
                        lMovimiento.porcdes5 = registros["pordes5"].ToString();
                        lMovimiento.referencia = registros["referencia"].ToString();
                        lMovimiento.observacioens = registros["observaciones"].ToString();

                        aMovimientos.Add(lMovimiento);

                        lResult = true;
                    };
                    conexion.Close();
                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar movimientos " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }

        public static bool BuscarIdUnidad(string aStringConnection, ref string aMensaje, string aUnidad, ref string aIdUnidad)
        {
            bool lResult = false;

            aMensaje = "No se encontró unidad";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            "select cidunidad as id from admUnidadesMedidaPeso where cabreviatura = '" + aUnidad + "' ";

                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        aIdUnidad = registros["id"].ToString();

                        lResult = true;
                    };
                    conexion.Close();
                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar unidad de medida " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }

        public static bool ActualizarDocumentoComercial(string aStringConnection, ref string aMensaje, Interfaces.documento aDocumento, string aIdDocumento, string aGuidDSL)
        {
            bool lResult = false;

            SqlConnection conexion = new SqlConnection(aStringConnection);
            try
            {
                //string[] IdsPagos = aPagosCompras.Select(x => x.idpago).Distinct().ToArray();

                SqlCommand comando = conexion.CreateCommand();
                string lquery = "";

                if (!string.IsNullOrWhiteSpace(aGuidDSL))
                {
                    lquery = "set transaction isolation level read uncommitted; " +
                   "update admFoliosDigitales " +
                   "set cestado = 2, choraemi = '" + "12:01:01" + "', cuuid = '" + aDocumento.uuid + "', ciddoctodsl = '" + aGuidDSL + "' " +
                   //"where ciddocumento in ( " + string.Join(",", IdsPagos) + ")";
                   "where ciddocto = " + aIdDocumento + " ";
                }
                else
                {
                    lquery = "set transaction isolation level read uncommitted; " +
                   "update admFoliosDigitales " +
                   "set cestado = 2, choraemi = '" + "12:01:01" + "', cuuid = '" + aDocumento.uuid + "' " +
                   //"where ciddocumento in ( " + string.Join(",", IdsPagos) + ")";
                   "where ciddocto = " + aIdDocumento + " ";
                }   

                comando.CommandText = lquery;

                conexion.Open();
                int rowAffected = comando.ExecuteNonQuery();
                if (rowAffected <= 0)
                {
                    lResult = false;
                }
                else
                {
                    lResult = true;
                }
                //conexion.Close();

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al actualizar documento Comercial : " + ex.Message.ToString();
                return false;
            }
            finally
            {
                conexion.Close();
            }
            return lResult;
        }

        public static bool ObtenerGUIDDSL(string aStringConnection, ref string aMensaje, ref string aGuidDSL)
        {
            bool lResult = false;
            int lActivo = 0;

            aMensaje = "No se encontraron registros";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            " select cguiddsl from admParametros";
                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        aGuidDSL = registros["cguiddsl"].ToString();
                        lResult = true;
                    };
                    conexion.Close();

                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar base de datos ADD " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }

        public static bool buscaUUID(string aStringConnection, string aUUID)
        {
            string stringconnection = aStringConnection;
            bool lResult = false;

            try
            {
                SqlConnection conexion = new SqlConnection(stringconnection);
                conexion.Open();

                string lquery = "set transaction isolation level read uncommitted;Select Top 1 cuuid from admFoliosDigitales where ciddocto <> 0 and cuuid = '" + aUUID + "'";
                SqlCommand comando = new SqlCommand(lquery, conexion);
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    lResult = true;
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepcion al buscar UUID " + aUUID + ": " + ex.Message.ToString());
                return false;
            }
            return lResult;
        }

        public static bool ObtenerGuidDocumentoComercial(string aStringConnection, ref string aMensaje, ref string aGUIDDocumento, ref string aIdDocumento, string aConcepto, string aSerie, string aFolio)
        {
            bool lResult = false;

            aMensaje = "No se encontró registro";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            " select " +
                            " d.cguiddocumento as guiddocumento, " +
                            " d.ciddocumento as iddocumento " +
                            " from " +
                            " admDocumentos d " +
                            " join admConceptos c on c.cidconceptodocumento = d.cidconceptodocumento " +
                            " where c.ccodigoconcepto = '" + aConcepto + "' " +
                            " and d.cseriedocumento = '" + aSerie + "' " +
                            " and d.cfolio = '" + aFolio + "' ";

                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        aGUIDDocumento = registros["guiddocumento"].ToString();
                        aIdDocumento = registros["iddocumento"].ToString();
                        lResult = true;
                    };
                    conexion.Close();

                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar GUID Documento " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }

        public static bool ObtenerGuidDocumentoDSL(string aStringConnection, ref string aMensaje, string aUUID, ref string aGuidDocumentDSL)
        {
            bool lResult = false;

            aMensaje = "No se encontó registro";

            try
            {
                using (SqlConnection conexion = new SqlConnection(aStringConnection))
                {
                    conexion.Open();
                    string lquery = "";
                    lquery = "set transaction isolation level read uncommitted; " +
                            " select " +
                            " guiddocument " +
                            " from " +
                            " Comprobante " +
                            " where uuid = \'" + aUUID + "\' ";

                    SqlCommand comando = new SqlCommand(lquery, conexion);
                    SqlDataReader registros = comando.ExecuteReader();
                    while (registros.Read())
                    {
                        aGuidDocumentDSL = registros["guiddocument"].ToString();
                        lResult = true;
                    };
                    conexion.Close();

                }

            }
            catch (Exception ex)
            {
                aMensaje = "Excepcion sql al buscar guiddocument del ADD " + ex.Message.ToString();
                return false;
            }

            return lResult;
        }
    }
}
