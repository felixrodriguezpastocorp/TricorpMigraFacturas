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

        /*
        public static int fCreaEntrada(List<Interfaces.registro> aRegistros, string aConcepto)
        {
            int lError = 0;
            double lSiguienteFolio = 0.00;
            int lIdDocumento = 0;
            int lIdMovimiento = 0;
            string lMensaje = "";

            try
            {
                lError = Interfaces.fSiguienteFolio(aConcepto, "SYNC", ref lSiguienteFolio);
                if (lError != 0)
                {
                    Log.LogMessage("Error al obtener siguiente folio: " + Interfaces.RError(lError));
                    return lError;
                }

                Interfaces.tDocumento ltDocumento = new Interfaces.tDocumento();

                ltDocumento.aFolio = lSiguienteFolio;
                ltDocumento.aSistemaOrigen = 205;
                ltDocumento.aCodConcepto = aConcepto;
                ltDocumento.aSerie = "SYNC";
                ltDocumento.aFecha = DateTime.Now.ToString("MM/dd/yyyy");
                ltDocumento.aAfecta = 1;

                lError = Interfaces.fAltaDocumento(ref lIdDocumento, ref ltDocumento);

                if (lError != 0)
                {
                    Log.LogMessage("Error al crear documento: " + Interfaces.RError(lError));
                    return lError;
                }


                int lNumMovimiento = 0;

                foreach (Interfaces.registro lMovimiento in aRegistros)
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
                    ltMovimiento.aUnidades = Convert.ToDouble(lMovimiento.cantidad);
                    ltMovimiento.aCosto = Convert.ToDouble(lMovimiento.costo);
                    ltMovimiento.aCodProdSer = lMovimiento.producto;
                    ltMovimiento.aCodAlmacen = lMovimiento.almacen;

                    lError = Interfaces.fAltaMovimiento(lIdDocumento, ref lIdMovimiento, ref ltMovimiento);

                    if (lError != 0)
                    {
                        Log.LogMessage("Error al agregar movimiento Producto:" + lMovimiento.producto + " Cantidad:" + lMovimiento.cantidad + " : " + Interfaces.RError(lError));
                        Interfaces.fDesbloqueaDocumento();
                        fBorrarDocumento(lIdDocumento);
                        return lError;
                    }

                    bool lUnidades = false;
                    bool lSerie = false;
                    bool lLote = false;
                    bool lPedimento = false;
                    bool lCaracteristicas = false;

                    lError = Interfaces.fRecuperaTipoProducto(ref lUnidades, ref lSerie, ref lLote, ref lPedimento, ref lCaracteristicas);

                    if (lError != 0)
                    {
                        Log.LogMessage("Error al buscar tipo de producto Producto:" + lMovimiento.producto + " : " + Interfaces.RError(lError));
                        Interfaces.fDesbloqueaDocumento();
                        fBorrarDocumento(lIdDocumento);
                        return lError;
                    }

                    if (!lLote)
                    {
                        Log.LogMessage("El Producto:" + lMovimiento.producto + " no maneja lotes : " + Interfaces.RError(lError));
                        Interfaces.fDesbloqueaDocumento();
                        fBorrarDocumento(lIdDocumento);
                        return lError;
                    }
                    else
                    {
                        Interfaces.tSeriesCapas ltSeriesCapas = new Interfaces.tSeriesCapas();

                        ltSeriesCapas.aUnidades = Convert.ToDouble(lMovimiento.cantidad);
                        ltSeriesCapas.aTipoCambio = 1;
                        ltSeriesCapas.aSeries = "";
                        ltSeriesCapas.aPedimento = "";
                        ltSeriesCapas.aAgencia = "";
                        ltSeriesCapas.aFechaPedimento = "";
                        ltSeriesCapas.aNumeroLote = lMovimiento.lote;
                        ltSeriesCapas.aFechaFabricacion = Convert.ToDateTime(lMovimiento.fabricacion).ToString("MM/dd/yyyy");
                        ltSeriesCapas.aFechaCaducidad = Convert.ToDateTime(lMovimiento.caducidad).ToString("MM/dd/yyyy");

                        lError = Interfaces.fAltaMovimientoSeriesCapas(lIdMovimiento, ref ltSeriesCapas);

                        if (lError != 0)
                        {
                            Log.LogMessage("Error al agregar lote al movimiento Producto :" + lMovimiento.producto + " Cantidad:" + lMovimiento.cantidad + " : " + Interfaces.RError(lError));
                            Interfaces.fDesbloqueaDocumento();
                            fBorrarDocumento(lIdDocumento);
                            return lError;
                        }

                    }
                }

                Interfaces.fDesbloqueaDocumento();

            }
            catch (Exception ex)
            {
                Log.LogMessage("Excepcion al crear entrada de inventario: " + ex.Message.ToString());
                lError = -1;
                return lError;
            }

            return lError;
        }
        */
    }
}
