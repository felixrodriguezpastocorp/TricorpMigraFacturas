using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TricorpMigraFacturasTask
{
    public class fsdk
    {
        private static StringBuilder gRutaBinarios = new System.Text.StringBuilder(1024);

        public static int fAbreSDK(string aUser, string aPass)
        {
            UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
            long lResult;
            UIntPtr hRegKey;
            StringBuilder sNombreEmpresa = new StringBuilder(255);
            StringBuilder sDirectorioEmpresa = new StringBuilder(255);
            uint pvSize = 1024;
            StringBuilder lEntrada = new System.Text.StringBuilder(1024);
            uint pdwType = 0;
            int lError = 0;

            string szRegKeySistema = "SOFTWARE\\Computación en Acción, SA CV\\CONTPAQ I COMERCIAL";
            string sNombrePAQ = "CONTPAQ I COMERCIAL";

            lResult = Interfaces.RegOpenKeyEx(HKEY_LOCAL_MACHINE, szRegKeySistema, 0, 1, out hRegKey);
            if (lResult != 0)
            {
                //CustomException ce = new CustomException();
                //ce.codigo = 3001;
                //ce.mensaje = "Error al abrir el Registry";
                //throw ce; ;
            }

            lResult = Interfaces.RegQueryValueEx(hRegKey, "DirectorioBase", 0, out pdwType, lEntrada, ref pvSize);
            if (lResult != 0)
            {
                lResult = Interfaces.RegCloseKey(hRegKey);
            }
            gRutaBinarios = lEntrada;
            lResult = Interfaces.SetCurrentDirectory(lEntrada.ToString());
            //Interfaces.fInicializaSDK();
            Interfaces.fInicioSesionSDK(aUser, aPass);
            Interfaces.fInicioSesionSDKCONTPAQi(aUser, aPass);

            lError = Interfaces.fSetNombrePAQ(sNombrePAQ);

            return lError;
        }

        public static void fTerminaSDK()
        {
            Interfaces.fTerminaSDK();
        }

        public static void fCierraEmpresa()
        {
            Interfaces.fCierraEmpresa();
        }

        public static int fAbreEmpresa(string aRutaEmpresa)
        {
            int lError = 0;

            lError = Interfaces.fAbreEmpresa(aRutaEmpresa);

            return lError;
        }
        public static void fBorrarDocumento(int aIdDocumento)
        {
            int lError = 0;

            lError = Interfaces.fBuscarIdDocumento(aIdDocumento);

            if (lError == 0)
            {
                lError = Interfaces.fBorraDocumento();
            }
        }

        public static int fCreaDocumento(Interfaces.documento aDocumento, Dictionary<string,string> aUnidades, string aStringConnection, string aRelacionarADD, string sqlServer, string sqlUser, string sqlPass)
        {
            int lError = 0;
            double lSiguienteFolio = 0.00;
            int lIdDocumento = 0;
            int lIdMovimiento = 0;
            string lMensaje = "";

            try
            {
                lError = Interfaces.fSiguienteFolio(aDocumento.concepto, aDocumento.serie, ref lSiguienteFolio);
                if (lError != 0)
                {
                    Log.LogMessage("Error al obtener siguiente folio: " + Interfaces.RError(lError));
                    return lError;
                }

                Interfaces.tDocumento ltDocumento = new Interfaces.tDocumento();

                ltDocumento.aFolio = lSiguienteFolio;
                ltDocumento.aNumMoneda = Convert.ToInt32(aDocumento.moneda);
                ltDocumento.aTipoCambio = Convert.ToDouble(aDocumento.tc);
                if (Convert.ToDouble(aDocumento.descuentodoc1) > 0)
                {
                    ltDocumento.aDescuentoDoc1 = Convert.ToDouble(aDocumento.descuentodoc1);
                }
                if (Convert.ToDouble(aDocumento.descuentodoc2) > 0)
                {
                    ltDocumento.aDescuentoDoc2 = Convert.ToDouble(aDocumento.descuentodoc2);
                }
                ltDocumento.aSistemaOrigen = 205;
                ltDocumento.aCodConcepto = aDocumento.concepto;
                ltDocumento.aSerie = aDocumento.serie;
                ltDocumento.aFecha = DateTime.Now.ToString("MM/dd/yyyy");
                ltDocumento.aCodigoCteProv = aDocumento.cliente;
                if (!string.IsNullOrWhiteSpace(aDocumento.referencia))
                {
                    ltDocumento.aReferencia = aDocumento.referencia;
                }
                ltDocumento.aAfecta = 1;
                if (Convert.ToDouble(aDocumento.gasto1) > 0)
                {
                    ltDocumento.aGasto1 = Convert.ToDouble(aDocumento.gasto1);
                }
                if (Convert.ToDouble(aDocumento.gasto2) > 0)
                {
                    ltDocumento.aGasto2 = Convert.ToDouble(aDocumento.gasto2);
                }
                if (Convert.ToDouble(aDocumento.gasto3) > 0)
                {
                    ltDocumento.aGasto3 = Convert.ToDouble(aDocumento.gasto3);
                }

                lError = Interfaces.fAltaDocumento(ref lIdDocumento, ref ltDocumento);

                if (lError != 0)
                {
                    Log.LogMessage("Error al crear documento: " + Interfaces.RError(lError));
                    return lError;
                }

                if (!string.IsNullOrWhiteSpace(aDocumento.observaciones))
                {
                    lError = Interfaces.fEditarDocumento();
                    if (lError == 0) lError = Interfaces.fSetDatoDocumento("cObservaciones", aDocumento.observaciones);
                    if (lError == 0) lError = Interfaces.fGuardaDocumento();
                }

                if (lError != 0)
                {
                    Log.LogMessage("Error al actualizar documento: " + Interfaces.RError(lError));
                    Interfaces.fDesbloqueaDocumento();
                    fBorrarDocumento(lIdDocumento);
                    return lError;
                }

                int lNumMovimiento = 0;

                foreach (Interfaces.movimiento lMovimiento in aDocumento.movimientos)
                {
                    //lError = Interfaces.fBuscaProducto(lMovimiento.producto);

                    //if (lError != 0)
                    //{
                    //    Log.LogMessage("Error al buscar producto movimiento Producto:" + lMovimiento.producto + " : " + Interfaces.RError(lError));
                    //    Interfaces.fDesbloqueaDocumento();
                    //    fBorrarDocumento(lIdDocumento);
                    //    return lError;
                    //}

                    Interfaces.tMovimiento ltMovimiento = new Interfaces.tMovimiento();

                    lMensaje = "";
                    lNumMovimiento = lNumMovimiento + 1;

                    ltMovimiento.aConsecutivo = lNumMovimiento;
                    ltMovimiento.aUnidades = Convert.ToDouble(lMovimiento.unidades);
                    ltMovimiento.aPrecio = Convert.ToDouble(lMovimiento.precio);
                    ltMovimiento.aCodProdSer = "SER001";
                    ltMovimiento.aCodAlmacen = "MKLOG";
                    if (!string.IsNullOrWhiteSpace(lMovimiento.referencia))
                    {
                        ltMovimiento.aReferencia = lMovimiento.referencia;
                    }

                    lError = Interfaces.fAltaMovimiento(lIdDocumento, ref lIdMovimiento, ref ltMovimiento);

                    if (lError != 0)
                    {
                        Log.LogMessage("Error al agregar movimiento : " + Interfaces.RError(lError));
                        Interfaces.fDesbloqueaDocumento();
                        fBorrarDocumento(lIdDocumento);
                        return lError;
                    }
                    string lUnidad = "";
                    string lIdUnidad = "";
                    if (!string.IsNullOrWhiteSpace(lMovimiento.unidad))
                    {
                        lUnidad = aUnidades[lMovimiento.unidad];
                        lMensaje = "";
                        if (SqlConnections.BuscarIdUnidad(aStringConnection, ref lMensaje, lUnidad, ref lIdUnidad) == false)
                        {
                            Log.LogMessage("Error al buscar unidad de medida " + lUnidad + " : " + Interfaces.RError(lError));
                            Interfaces.fDesbloqueaDocumento();
                            fBorrarDocumento(lIdDocumento);
                            return lError;
                        }
                    }

                    lError = Interfaces.fEditarMovimiento();
                    if (lError == 0 && !string.IsNullOrWhiteSpace(lIdUnidad)) lError = Interfaces.fSetDatoMovimiento("cIdUnidad", lIdUnidad);
                    if (lError == 0 && !string.IsNullOrWhiteSpace(lIdUnidad)) lError = Interfaces.fSetDatoMovimiento("cPrecio", lMovimiento.precio);
                    if (lError == 0 && !string.IsNullOrWhiteSpace(lIdUnidad)) lError = Interfaces.fSetDatoMovimiento("cPrecioCapturado", lMovimiento.precio) ;
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcimp1) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeImpuesto1", lMovimiento.porcimp1);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcimp2) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeImpuesto2", lMovimiento.porcimp2);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcimp3) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeImpuesto3", lMovimiento.porcimp3);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcret1) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeRetencion1", lMovimiento.porcret1);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcret2) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeRetencion2", lMovimiento.porcret2);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcdes1) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeDescuento1", lMovimiento.porcdes1);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcdes2) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeDescuento2", lMovimiento.porcdes2);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcdes3) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeDescuento3", lMovimiento.porcdes3);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcdes4) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeDescuento4", lMovimiento.porcdes4);
                    if (lError == 0 && Convert.ToDouble(lMovimiento.porcdes5) > 0) lError = Interfaces.fSetDatoMovimiento("cPorcentajeDescuento5", lMovimiento.porcdes5);
                    if (lError == 0 && !string.IsNullOrWhiteSpace(lMovimiento.observacioens)) lError = Interfaces.fSetDatoMovimiento("cObervaMov", lMovimiento.observacioens);
                    if (lError == 0) lError = Interfaces.fGuardaMovimiento();

                    if (lError != 0)
                    {
                        Log.LogMessage("Error al actualizar movimiento : " + Interfaces.RError(lError));
                        Interfaces.fDesbloqueaDocumento();
                        fBorrarDocumento(lIdDocumento);
                        return lError;
                    }
                }

                Interfaces.fDesbloqueaDocumento();


                if (lError == 0)
                {
                    if (aRelacionarADD == "1")
                    {
                        string lStringConnectionADD = "";
                        string lGuidDSL = "";

                        if (SqlConnections.ObtenerGUIDDSL(aStringConnection, ref lMensaje, ref lGuidDSL) == false)
                        {
                            Log.LogMessage("Error al obtener el ADD  de la empresa: " + lMensaje);
                            return -1;
                        }

                        if (!string.IsNullOrWhiteSpace(lGuidDSL))
                        {
                            lStringConnectionADD = "server=" + sqlServer + "; database=document_" + lGuidDSL + "_metadata" + "; User Id =" + sqlUser + "; Password = " + sqlPass + ";"; ;
                            //foreach (documento lDocumento in gDocumentos)
                            //{
                                string lGuidDocumentoComercial = "";
                                string lIdDocumentoComercial = "";

                            if (SqlConnections.buscaUUID(aStringConnection, aDocumento.uuid))
                            {
                                Log.LogMessage("El UUID " + aDocumento.uuid + " ya fue procesado anteriormente");
                                return -1;
                            }

                            if (SqlConnections.ObtenerGuidDocumentoComercial(aStringConnection, ref lMensaje, ref lGuidDocumentoComercial, ref lIdDocumentoComercial, aDocumento.concepto, aDocumento.serie, lSiguienteFolio.ToString()) == false)
                                {
                                    Log.LogMessage("Error al obtener GUID documento comercial : Concepto: " + aDocumento.concepto + " Serie: " + aDocumento.serie + " Folio" +lSiguienteFolio.ToString() + " " + lMensaje);
                                    return -1;
                                }

                                if (!string.IsNullOrWhiteSpace(lGuidDocumentoComercial))
                                {
                                    string lGUIDDocumentoDSL = "";

                                    if (SqlConnections.ObtenerGuidDocumentoDSL(lStringConnectionADD, ref lMensaje, aDocumento.uuid, ref lGUIDDocumentoDSL) == false)
                                    {
                                        Log.LogMessage("Error al obtener GUID de documento ADD : " + aDocumento.uuid + " " + lMensaje);
                                        return -1;
                                    }

                                    if (!string.IsNullOrWhiteSpace(lGUIDDocumentoDSL))
                                    {
                                        var options = new RestClientOptions("http://127.0.0.1:9080");
                                        var client = new RestClient(options);
                                        var request = new RestRequest("/saci/storageRestServices/addapi/documentos/referencias/" + lGuidDSL, Method.Put);
                                        request.AddHeader("Content-Type", "application/json");
                                        var body = "{" +
                                            "\"data\":[{" +
                                            "\"applicationtype\":\"Comercial\"," +
                                            "\"association\":[\"" + lGUIDDocumentoDSL + "\"]," +
                                            "\"comment\":\"Concepto: " + aDocumento.concepto + " Serie: " + aDocumento.serie + " Folio" + lSiguienteFolio.ToString() + " \"," +
                                            "\"docapp\":{" +
                                            "\"cuenta\":\"\"," +
                                            "\"ejercicio\":0," +
                                            "\"folio\":\"\"," +
                                            "\"numero\":0," +
                                            "\"periodo\":0," +
                                            "\"responsable\":0," +
                                            "\"subtipo\":\"\"," +
                                            "\"subtiponumero\":0," +
                                            "\"tipo\":\"\"" +
                                            "}," +
                                            "\"fecha\":\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"," +
                                            "\"guid\":\"" + lGuidDocumentoComercial + "\"," +
                                            "\"tipodoc\":\"CFDI\"" +
                                            "}]" +
                                            "}";

                                        request.AddStringBody(body, DataFormat.Json);
                                        RestResponse response = client.Execute(request);

                                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                                        {
                                            Console.WriteLine("Error de comunicacion REST " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            SqlConnections.ActualizarDocumentoComercial(aStringConnection, ref lMensaje, aDocumento, lIdDocumentoComercial, lGUIDDocumentoDSL);
                                            Log.LogMessage("UUID asociado " + aDocumento.uuid);
                                        }
                                    }
                                }
                           // }
                        }
                    }
                    else
                    {
                        SqlConnections.ActualizarDocumentoComercial(aStringConnection, ref lMensaje, aDocumento, lIdDocumento.ToString(), "");
                    }
                }

            }
            catch (Exception ex)
            {
                Log.LogMessage("Excepcion al crear factura: " + ex.Message.ToString());
                lError = -1;
                return lError;
            }

            return lError;
        }
    }
}
