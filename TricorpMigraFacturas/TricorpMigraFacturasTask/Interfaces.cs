using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TricorpMigraFacturasTask
{
    public class Interfaces
    {
        public class registro
        {
            public string conceptoorigen { get; set; }
            public string serie { get; set; }
            public string folio { get; set; }
        }

        public class documento
        {
            public string iddocumento { get; set; }
            public string concepto { get; set; }
            public string serie { get; set; }
            public string moneda { get; set; }
            public string tc { get; set; }
            public string descuentodoc1 { get; set; }
            public string descuentodoc2 { get; set; }
            public string cliente { get; set; }
            public string referencia { get; set; }
            public string gasto1 { get; set; }
            public string gasto2 { get; set; }
            public string gasto3 { get; set; }
            public string observaciones { get; set; }
            public string uuid { get; set; }
            public List<Interfaces.movimiento> movimientos { get; set; }
        }

        public class movimiento
        {
            public string unidades { get; set; }
            public string unidad { get; set; }
            public string precio { get; set; }
            public string porcimp1 { get; set; }
            public string porcimp2 { get; set; }
            public string porcimp3 { get; set; }
            public string porcret1 { get; set; }
            public string porcret2 { get; set; }
            public string porcdes1 { get; set; }
            public string porcdes2 { get; set; }
            public string porcdes3 { get; set; }
            public string porcdes4 { get; set; }
            public string porcdes5 { get; set; }
            public string referencia { get; set; }
            public string observacioens { get; set; }
        }


        public class constantes
        {
            public const int kLongitudNomBanExtranjero = 255;
            public const int kLongId = 12;
            public const int kLongFecha = 24;
            public const int kLongSerie = 12;
            public const int kLongCodigo = 31;
            public const int kLongNombre = 61;
            public const int kLongReferencia = 21;
            public const int kLongDescripcion = 61;
            public const int kLongCuenta = 101;
            public const int kLongMensaje = 3001;
            public const int kLongNombreProducto = 256;
            public const int kLongAbreviatura = 4;
            public const int kLongCodValorClasif = 4;
            public const int kLongDenComercial = 51;
            public const int kLongRepLegal = 51;
            public const int kLongTextoExtra = 51;
            public const int kLongRFC = 21;
            public const int kLongCURP = 21;
            public const int kLongDesCorta = 21;
            public const int kLongNumeroExtInt = 7;
            public const int kLongNumeroExpandido = 31;
            public const int kLongCodigoPostal = 7;
            public const int kLongTelefono = 16;
            public const int kLongEmailWeb = 51;
            public const int kLongSelloSat = 176;
            public const int kLonSerieCertSAT = 21;
            public const int kLongFechaHora = 36;
            public const int kLongSelloCFDI = 176;
            public const int kLongCadOrigComplSAT = 501;
            public const int kLongUUID = 37;
            public const int kLongRegimen = 101;
            public const int kLongMoneda = 61;
            public const int kLongFolio = 17;
            public const int kLongMonto = 31;
            public const int kLogLugarExpedicion = 401;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tDocumento
        {
            public double aFolio;
            public int aNumMoneda;
            public double aTipoCambio;
            public double aImporte;
            public double aDescuentoDoc1;
            public double aDescuentoDoc2;
            public int aSistemaOrigen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodConcepto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongSerie)]
            public string aSerie;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string aFecha;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodigoCteProv;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodigoAgente;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public string aReferencia;
            public int aAfecta;
            public double aGasto1;
            public double aGasto2;
            public double aGasto3;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimiento
        {
            public int aConsecutivo;
            public double aUnidades;
            public double aPrecio;
            public double aCosto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodProdSer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public string aReferencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aCodClasificacion;
        }

        //Funciones operación de registro:
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int RegOpenKeyEx(
        UIntPtr hKey,
        string subKey,
        int ulOptions,
        int samDesired,
        out UIntPtr hkResult);

        //Funciones de Windows: 
        [DllImport("advapi32")]
        public static extern int RegCloseKey(UIntPtr hKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
        public static extern int RegQueryValueEx(
        UIntPtr hKey,
        string lpValueName,
        int lpReserved,
        out uint lpType,
        StringBuilder lpData,
        ref uint lpcbData);

        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string pPtrDirActual);

        //Funciones SDK ContPAQi Comercial Premuium®:
        [DllImport("MGWServicios.dll")]
        public static extern int fInicializaSDK();

        [DllImport("MGWSERVICIOS.DLL")]
        public static extern void fInicioSesionSDK(string aUsuario, string aContrasenia);

        [DllImport("MGWSERVICIOS.DLL")]
        public static extern void fInicioSesionSDKCONTPAQi(string aUsuario, string aContrasenia);

        [DllImport("MGWServicios.dll")]
        public static extern void fTerminaSDK();

        [DllImport("MGWServicios.dll", EntryPoint = "fSetNombrePAQ")]
        public static extern int fSetNombrePAQ(String aNombrePAQ);

        [DllImport("MGWServicios.dll")]
        public static extern int fAbreEmpresa(string Directorio);

        [DllImport("MGWServicios.dll")]
        public static extern void fCierraEmpresa();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fAltaDocumento(ref Int32 aIdDocumento, ref tDocumento atDocumento);

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fBorraDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fEditarDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fGuardaDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fBuscarIdDocumento(int aIdDocumento);

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fDesbloqueaDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fSetDatoDocumento([MarshalAs(UnmanagedType.LPStr)] string aCampo, [MarshalAs(UnmanagedType.LPStr)] string aValor);

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fAltaMovimiento(Int32 aIdDocumento, ref Int32 aIdMovimiento, ref tMovimiento astMovimiento);

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fEditarMovimiento();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fGuardaMovimiento();

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fSetDatoMovimiento([MarshalAs(UnmanagedType.LPStr)] string aCampo, [MarshalAs(UnmanagedType.LPStr)] string aValor);

        [DllImport("MGWServicios.dll")]
        public static extern Int32 fSiguienteFolio([MarshalAs(UnmanagedType.LPStr)] string aCodigoConcepto, [MarshalAs(UnmanagedType.LPStr)] string aSerie, ref double aFolio);
                
        [DllImport("MGWSERVICIOS.DLL", EntryPoint = "fError")]
        public static extern void FError(int NumeroError, StringBuilder Mensaje, int Longitud);

        public static string RError(int iError)
        {
            StringBuilder sMensaje = new StringBuilder(512);
            string msg = string.Empty;
            if (iError != 0)
            {
                Interfaces.FError(iError, sMensaje, 512);
                msg = "Error: " + sMensaje;
            }

            return msg;
        }
    }
}
