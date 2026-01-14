using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TricorpMigraFacturasTask
{
    class Program
    {
        static void Main(string[] args) //{sdkUser},{sdkPass},{rutaEmpresaOrigen},{rutaEmpresaDestino},{tbArchivo.Text},{stringconnectionOrigen},{stringconnectionDestino}
        {
            bool lSdkAbierto = false;
            bool lEmpresaAbierta = false;
            int lError = 0;
            bool lErrorRegistros = false;
            string lMensaje = "";

            var parameters = System.Convert.FromBase64String(args[0]);
            string[] valores = System.Text.Encoding.UTF8.GetString(parameters).Split(',');

            try
            {
                List<Interfaces.registro> lRegistros = new List<Interfaces.registro>();
                Dictionary<string, string> lConceptos = new Dictionary<string, string>();
                lError = mProcesaExcel(valores[4], ref lRegistros, ref lConceptos);

                if (lError != 0)
                {
                    return;
                }

                if (lConceptos.Count <= 0)
                {
                    Log.LogMessage("No se obtuvieron conceptos del archivo de Excel ");
                    return;
                }

                if (lRegistros.Count <= 0)
                {
                    Log.LogMessage("No se obtuvieron registros del archivo de Excel ");
                    return;
                }
                //int lRenglon = 1;
                //foreach (Interfaces.registro lRegistro in lRegistros)
                //{
                //    lRenglon = lRenglon + 1;
                //    //Producto
                //    if (string.IsNullOrWhiteSpace(lRegistro.producto))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : Código de producto vacío");
                //        lErrorRegistros = true;
                //    }
                //    else if (lRegistro.producto.Length > 30)
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : Longitud de código de producto mayor a 30");
                //        lErrorRegistros = true;
                //    }
                //    else
                //    {
                //        lMensaje = "";
                //        if (SqlConnections.BuscarProducto(valores[4], ref lMensaje, lRegistro.producto) == false)
                //        {
                //            Log.LogMessage("Renglon " + lRenglon + " : Error al buscar producto : " + lMensaje);
                //            lErrorRegistros = true;
                //        }
                //    }

                //    //Almacen
                //    if (string.IsNullOrWhiteSpace(lRegistro.almacen))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : Código de almacén vacío");
                //        lErrorRegistros = true;
                //    }
                //    else if (lRegistro.almacen.Length > 30)
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : Longitud de código de almacén mayor a 30");
                //        lErrorRegistros = true;
                //    }
                //    else
                //    {
                //        lMensaje = "";
                //        if (SqlConnections.BuscarAlmacen(valores[4], ref lMensaje, lRegistro.almacen) == false)
                //        {
                //            Log.LogMessage("Renglon " + lRenglon + " : Error al buscar almacen : " + lMensaje);
                //            lErrorRegistros = true;
                //        }
                //    }

                //    double cantidad = 0.00;

                //    //Cantidad
                //    if (string.IsNullOrWhiteSpace(lRegistro.cantidad))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : La cantidad está vacía");
                //        lErrorRegistros = true;
                //    }
                //    else if (!double.TryParse(lRegistro.cantidad, out cantidad))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : El valor " + lRegistro.cantidad + " para la cantidad no es un valor numérico ");
                //        lErrorRegistros = true;
                //    }
                //    // Costo
                //    double costo = 0.00;
                //    if (string.IsNullOrWhiteSpace(lRegistro.costo))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : El costo está vacío");
                //        lErrorRegistros = true;
                //    }
                //    else if (!double.TryParse(lRegistro.costo, out costo))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : El valor " + lRegistro.costo + " para el costo no es un valor numérico ");
                //        lErrorRegistros = true;
                //    }

                //    // Lote
                //    if (string.IsNullOrWhiteSpace(lRegistro.lote))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : El lote está vacío");
                //        lErrorRegistros = true;
                //    }
                //    else if (lRegistro.lote.Length > 30)
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : Longitud del lote mayor a 30");
                //        lErrorRegistros = true;
                //    }

                //    // Fabricacion
                //    DateTime fabricacion = new DateTime();
                //    if (string.IsNullOrWhiteSpace(lRegistro.fabricacion))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : La fecha de fabricación está vacía");
                //        lErrorRegistros = true;
                //    }
                //    else if (!DateTime.TryParse(lRegistro.fabricacion, out fabricacion))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : El valor " + lRegistro.fabricacion + " para la fecha de fabricacion no es un valor de fecha ");
                //        lErrorRegistros = true;
                //    }

                //    // Vencimiento
                //    DateTime caducidad = new DateTime();
                //    if (string.IsNullOrWhiteSpace(lRegistro.caducidad))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : La fecha de vencimiento está vacía");
                //        lErrorRegistros = true;
                //    }
                //    else if (!DateTime.TryParse(lRegistro.caducidad, out caducidad))
                //    {
                //        Log.LogMessage("Renglon " + lRenglon + " : El valor " + lRegistro.caducidad + " para la fecha de vencimiento no es un valor de fecha ");
                //        lErrorRegistros = true;
                //    }
                //}

                //if (lErrorRegistros)
                //{
                //    return;
                //}

                //lError = fsdk.fAbreSDK(valores[0], valores[1]);
                //if (lError != 0)
                //{
                //    Log.LogMessage("Error al abrir el SDK " + Interfaces.RError(lError));
                //    return;
                //}
                //lSdkAbierto = true;

                //lError = fsdk.fAbreEmpresa(valores[2]);
                //if (lError != 0)
                //{
                //    Log.LogMessage("Error al abrir la empresa " + Interfaces.RError(lError));
                //    return;
                //}

                //lEmpresaAbierta = true;

                ////Crear documento entrada
                //lError = fsdk.fCreaEntrada(lRegistros, valores[5]);

            }
            catch (Exception ex)
            {
                Log.LogMessage("Excepcion al realizar el proceso :" + ex.Message.ToString());
            }
            finally
            {
                if (lEmpresaAbierta) fsdk.fCierraEmpresa();
                if (lSdkAbierto) fsdk.fTerminaSDK();
            }
        }

        static int mProcesaExcel(string aArchivo, ref List<Interfaces.registro> aListaRegistros, ref Dictionary<string,string> aConceptos)
        {
            int lResult = 0;
            try
            {
                Log.LogMessage("Cargando archivo " + aArchivo);
                var workBook = new XLWorkbook(aArchivo);

                if (!workBook.Worksheets.Contains("MIGRACION"))
                {
                    Log.LogMessage("El archivo de Excel no contiene la hoja MIGRACION");
                    lResult = -1;
                }
                else if (!workBook.Worksheets.Contains("CONCEPTOS"))
                {
                    Log.LogMessage("El archivo de Excel no contiene la hoja CONCEPTOS");
                    lResult = -1;
                }

                if (lResult == 0)
                {
                    var ws1 = workBook.Worksheet("MIGRACION");
                    var rows = ws1.RangeUsed().RowsUsed().Skip(1);
                    var cols = ws1.RangeUsed().ColumnsUsed();

                    if (cols.Count() > 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene mas de 3 columnas");
                        lResult = -1;
                    }
                    else if (cols.Count() < 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene menos de 3 columnas");
                        lResult = -1;
                    }

                    if (lResult == 0)
                    {
                        foreach (var row in rows)
                        {
                            Interfaces.registro lRegistro = new Interfaces.registro();

                            lRegistro.conceptoorigen = row.Cell(1).GetValue<string>().Trim();
                            lRegistro.serie = row.Cell(2).GetValue<string>().Trim();
                            lRegistro.folio = row.Cell(3).GetValue<string>();
                            
                            aListaRegistros.Add(lRegistro);

                        }
                    }
                }

                if (lResult == 0)
                {
                    var ws2 = workBook.Worksheet("CONCEPTOS");
                    var rows2 = ws2.RangeUsed().RowsUsed().Skip(1);
                    var cols2 = ws2.RangeUsed().ColumnsUsed();

                    if (cols2.Count() > 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene mas de 3 columnas");
                        lResult = -1;
                    }
                    else if (cols2.Count() < 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene menos de 3 columnas");
                        lResult = -1;
                    }

                    if (lResult == 0)
                    {
                        foreach (var row in rows2)
                        {
                            string lConceptoOrigen = row.Cell(1).GetValue<string>().Trim();
                            string lConceptoDestino = row.Cell(2).GetValue<string>().Trim();

                            if (!aConceptos.ContainsKey(lConceptoOrigen))
                            {
                                aConceptos.Add(lConceptoOrigen, lConceptoDestino);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Log.LogMessage("Excepcion al procesar archivo de excel : " + ex.Message.ToString());
                lResult = -1;
                return lResult;
            }
            return lResult;
        }
    }
}
