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
        static void Main(string[] args) //{sdkUser},{sdkPass},{rutaEmpresaOrigen},{rutaEmpresaDestino},{tbArchivo.Text},{stringconnectionOrigen},{stringconnectionDestino},{relacionaADD}
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
                Dictionary<string, string> lUnidades = new Dictionary<string, string>();
                lError = mProcesaExcel(valores[4], ref lRegistros, ref lConceptos, ref lUnidades);

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
                
                lError = fsdk.fAbreSDK(valores[0], valores[1]);
                if (lError != 0)
                {
                    Log.LogMessage("Error al abrir el SDK " + Interfaces.RError(lError));
                    return;
                }
                lSdkAbierto = true;

                lError = fsdk.fAbreEmpresa(valores[3]);
                if (lError != 0)
                {
                    Log.LogMessage("Error al abrir la empresa " + Interfaces.RError(lError));
                    return;
                }

                lEmpresaAbierta = true;

                
                int lRenglon = 0;
                lMensaje = "";

                foreach (Interfaces.registro lRegistro in lRegistros)
                {
                    lRenglon = lRenglon + 1;
                 
                    Log.LogMessage("Procesando renglón " + lRenglon + " Concepto: " + lRegistro.conceptoorigen + " Serie: " + lRegistro.serie + " Folio: " + lRegistro.folio);
                    Interfaces.documento lDocumento = new Interfaces.documento();
                    if (SqlConnections.BuscarDocumento(valores[5], ref lMensaje, ref lDocumento, lRegistro.conceptoorigen, lRegistro.serie, lRegistro.folio) == false  )
                    {
                        Log.LogMessage("Error al buscar Documento " + lMensaje);
                        continue;
                    }

                    if (lConceptos.ContainsKey(lRegistro.conceptoorigen))
                    {
                        lDocumento.concepto = lConceptos[lRegistro.conceptoorigen];
                        lDocumento.serie = lRegistro.serie;
                    }
                    else
                    {
                        Log.LogMessage("El concepto " + lRegistro.conceptoorigen + " no existe en la lista de conceptos en el Excel");
                        continue;
                    }

                    List<Interfaces.movimiento> lMovimientos = new List<Interfaces.movimiento>();
                    lMensaje = "";
                    if (SqlConnections.BuscarMovimientos(valores[5], ref lMensaje, ref lMovimientos, lDocumento.iddocumento) == false)
                    {
                        Log.LogMessage("Error al buscar Movimientos " + lMensaje);
                        continue;
                    }

                    if (lMovimientos.Count > 0)
                    {
                        lDocumento.movimientos = lMovimientos;
                    }

                    lError = fsdk.fCreaDocumento(lDocumento, lUnidades, valores[6], valores[7], valores[8], valores[9], valores[10]);

                }               
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

        static int mProcesaExcel(string aArchivo, ref List<Interfaces.registro> aListaRegistros, ref Dictionary<string,string> aConceptos, ref Dictionary<string, string> aUnidades)
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
                else if (!workBook.Worksheets.Contains("UNIDADES"))
                {
                    Log.LogMessage("El archivo de Excel no contiene la hoja UNIDADES");
                    lResult = -1;
                }

                if (lResult == 0)
                {
                    var ws1 = workBook.Worksheet("MIGRACION");
                    var rows = ws1.RangeUsed().RowsUsed().Skip(1);
                    var cols = ws1.RangeUsed().ColumnsUsed();

                    if (cols.Count() > 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene mas de 3 columnas en la hoja MIGRACION");
                        lResult = -1;
                    }
                    else if (cols.Count() < 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene menos de 3 columnas en la hoja MIGRACION");
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
                        Log.LogMessage("El archivo de Excel tiene mas de 3 columnas en la hoja CONCEPTOS");
                        lResult = -1;
                    }
                    else if (cols2.Count() < 3)
                    {
                        Log.LogMessage("El archivo de Excel tiene menos de 3 columnas en la hoja CONCEPTOS");
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

                    if (lResult == 0)
                    {
                        var ws3 = workBook.Worksheet("UNIDADES");
                        var rows3 = ws3.RangeUsed().RowsUsed().Skip(1);
                        var cols3 = ws3.RangeUsed().ColumnsUsed();

                        if (cols3.Count() > 2)
                        {
                            Log.LogMessage("El archivo de Excel tiene mas de 2 columnas en la hoja UNIDADES");
                            lResult = -1;
                        }
                        else if (cols3.Count() < 2)
                        {
                            Log.LogMessage("El archivo de Excel tiene menos de 2 columnas en la hoja UNIDADES");
                            lResult = -1;
                        }

                        if (lResult == 0)
                        {
                            foreach (var row in rows3)
                            {
                                string lUnidadOrigen = row.Cell(1).GetValue<string>().Trim();
                                string lUnidadDestino = row.Cell(2).GetValue<string>().Trim();

                                if (!aUnidades.ContainsKey(lUnidadOrigen))
                                {
                                    aUnidades.Add(lUnidadOrigen, lUnidadDestino);
                                }
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
