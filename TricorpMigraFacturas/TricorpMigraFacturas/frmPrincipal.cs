using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TricorpMigraFacturas
{
    public partial class frmPrincipal : Form
    {
        Dictionary<int, Interfaces.empresa> gEmpresasOrigen = new Dictionary<int, Interfaces.empresa>();
        Dictionary<int, Interfaces.empresa> gEmpresasDestino = new Dictionary<int, Interfaces.empresa>();
        FileSystemWatcher watcherLog = new FileSystemWatcher();
        private string logFlag = "";
        public frmPrincipal()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            btnAceptar.Enabled = true;
            btnSalir.Enabled = true;

            LlenaEmpresas();
        }

        private void LlenaEmpresas()
        {
            try
            {
                string server = ConfigurationManager.AppSettings["sqlServer"];
                string user = ConfigurationManager.AppSettings["sqlUser"];
                string pass = ConfigurationManager.AppSettings["sqlPass"];

                string stringconnection = "server=" + server + "; database=CompacWAdmin; User Id =" + user + "; Password = " + pass + ";";

                string lMensaje = "";

                List<Interfaces.empresa> lEmpresas = new List<Interfaces.empresa>();

                if (SqlConnections.BuscarEmpresas(stringconnection, ref lMensaje, ref lEmpresas) == false)
                {
                    MessageBox.Show("Error al buscar empresas " + lMensaje);
                    return;
                }

                foreach (Interfaces.empresa lEmpresa in lEmpresas)
                {
                    cbEmpresasOrigen.Items.Add(lEmpresa.nombreempresa + " <" + Path.GetFileName(lEmpresa.rutaempresa) + ">");
                    gEmpresasOrigen.Add(cbEmpresasOrigen.Items.Count, lEmpresa);

                    cbEmpresasDestino.Items.Add(lEmpresa.nombreempresa + " <" + Path.GetFileName(lEmpresa.rutaempresa) + ">");
                    gEmpresasDestino.Add(cbEmpresasDestino.Items.Count, lEmpresa);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Excepcion al llenar lista de empresas " + ex.Message.ToString());
            }

        }

        private void OnChangedLog(object source, FileSystemEventArgs e)
        {
            if (logFlag != "")
                return;
            try
            {
                lock (logFlag)
                {
                    logFlag = "true";
                }

                string FileName = Path.GetFileName(e.FullPath);

                if (FileName == "Log" + DateTime.Now.ToString("ddMMyy") + ".txt")
                {
                    Task.Run(() => {
                        tbHistorial.Text = "";
                        tbHistorial.AppendText(File.ReadAllText(e.FullPath));
                        Application.DoEvents();
                    }).Wait();
                }
            }
            finally
            {
                lock (logFlag)
                {
                    logFlag = "";
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnSalir.Enabled)
            {
                MessageBox.Show("El migrador está en ejecucion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdArchivo = new OpenFileDialog();
            ofdArchivo.Filter = "EXCEL|*.xlsx";
            ofdArchivo.ShowDialog();
            tbArchivo.Text = ofdArchivo.FileName.ToString();
        }

        private async Task EjecutaTarea()
        {
            string server = ConfigurationManager.AppSettings["sqlServer"];
            string user = ConfigurationManager.AppSettings["sqlUser"];
            string pass = ConfigurationManager.AppSettings["sqlPass"];
            string sdkUser = ConfigurationManager.AppSettings["usuarioSDK"];
            string sdkPass = ConfigurationManager.AppSettings["passSDK"];
            string relacionaADD = ConfigurationManager.AppSettings["relacionaADD"];
            string rutaEmpresaOrigen = gEmpresasOrigen[cbEmpresasOrigen.SelectedIndex + 1].rutaempresa;
            string rutaEmpresaDestino = gEmpresasDestino[cbEmpresasDestino.SelectedIndex + 1].rutaempresa;

            string bddOrigen = Path.GetFileName(rutaEmpresaOrigen);
            string stringconnectionOrigen;
            stringconnectionOrigen = "server=" + server + "; database=" + bddOrigen + "; User Id =" + user + "; Password = " + pass + ";";

            string bddDestino = Path.GetFileName(rutaEmpresaDestino);
            string stringconnectionDestino;
            stringconnectionDestino = "server=" + server + "; database=" + bddDestino + "; User Id =" + user + "; Password = " + pass + ";";

            string valores = $"{sdkUser},{sdkPass},{rutaEmpresaOrigen},{rutaEmpresaDestino},{tbArchivo.Text},{stringconnectionOrigen},{stringconnectionDestino},{relacionaADD},{server},{user},{pass}";
            var bytesText = System.Text.Encoding.UTF8.GetBytes(valores);
            string parameters = System.Convert.ToBase64String(bytesText);

            var task = new Task(() =>
            {
                ProcessStartInfo process = new ProcessStartInfo();
                process.CreateNoWindow = true;
                process.UseShellExecute = false;
                process.FileName = Application.StartupPath + "\\TricorpMigraFacturasTask.exe";
                process.WindowStyle = ProcessWindowStyle.Hidden;
                process.Arguments = parameters;

                try
                {
                    Log.LogMessage("Inicia proceso archivo " + tbArchivo.Text);
                    using (Process exeProcess = Process.Start(process))
                    {
                        exeProcess.WaitForExit();
                    }

                }
                catch (Exception ex)
                {
                    Log.LogMessage(ex.Message.ToString());
                    return;
                }
                finally
                {
                    Log.LogMessage("Termina proceso archivo " + tbArchivo.Text);
                }
            });

            btnAceptar.Enabled = false;
            btnSalir.Enabled = false;

            task.Start();
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;

            await task;

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;

            progressBar1.Visible = false;

            btnAceptar.Enabled = true;
            btnSalir.Enabled = true;

        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cbEmpresasOrigen.Text))
                {
                    MessageBox.Show("Seleccione una empresa origen ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    bool lEncontro = false;
                    string[] lDatosEmrpesaOrigen = cbEmpresasOrigen.Text.Split('<');
                    string lEmpresaOrigen = lDatosEmrpesaOrigen[0].Substring(0, lDatosEmrpesaOrigen[0].Length - 1);

                    foreach (KeyValuePair<int, Interfaces.empresa> entry in gEmpresasOrigen)
                    {
                        if (entry.Value.nombreempresa == lEmpresaOrigen)
                        {
                            lEncontro = true;
                            break;
                        }
                    }

                    if (!lEncontro)
                    {
                        MessageBox.Show("Seleccione una empresa origen del listado ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(cbEmpresasDestino.Text))
                {
                    MessageBox.Show("Seleccione una empresa destino ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    bool lEncontro = false;
                    string[] lDatosEmrpesaDestino = cbEmpresasDestino.Text.Split('<');
                    string lEmpresaDestino = lDatosEmrpesaDestino[0].Substring(0, lDatosEmrpesaDestino[0].Length - 1);

                    foreach (KeyValuePair<int, Interfaces.empresa> entry in gEmpresasDestino)
                    {
                        if (entry.Value.nombreempresa == lEmpresaDestino)
                        {
                            lEncontro = true;
                            break;
                        }
                    }

                    if (!lEncontro)
                    {
                        MessageBox.Show("Seleccione una empresa destino del listado ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(tbArchivo.Text.ToString()))
                {
                    MessageBox.Show("Es necesario seleccionar un archivo", "Advertencia");
                    return;
                }

                EjecutaTarea();

            }
            catch (Exception ex)
            {
                Log.LogMessage("Excepcion al ejecutar el proceso : " + ex.Message.ToString());
            }

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            watcherLog.Path = AppDomain.CurrentDomain.BaseDirectory;
            watcherLog.NotifyFilter = NotifyFilters.LastWrite;
            watcherLog.Filter = "*.txt";
            watcherLog.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            watcherLog.Changed += new FileSystemEventHandler(OnChangedLog);
            watcherLog.EnableRaisingEvents = true;
        }
    }
}
