
namespace TricorpMigraFacturas
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbEmpresasOrigen = new System.Windows.Forms.ComboBox();
            this.lblEmpresaOrigen = new System.Windows.Forms.Label();
            this.cbEmpresasDestino = new System.Windows.Forms.ComboBox();
            this.lblEmpresasDestino = new System.Windows.Forms.Label();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.tbArchivo = new System.Windows.Forms.TextBox();
            this.btnArchivo = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.tbHistorial = new System.Windows.Forms.TextBox();
            this.lblHistorial = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbEmpresasOrigen
            // 
            this.cbEmpresasOrigen.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbEmpresasOrigen.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmpresasOrigen.FormattingEnabled = true;
            this.cbEmpresasOrigen.Location = new System.Drawing.Point(12, 28);
            this.cbEmpresasOrigen.Name = "cbEmpresasOrigen";
            this.cbEmpresasOrigen.Size = new System.Drawing.Size(570, 21);
            this.cbEmpresasOrigen.TabIndex = 3;
            // 
            // lblEmpresaOrigen
            // 
            this.lblEmpresaOrigen.AutoSize = true;
            this.lblEmpresaOrigen.Location = new System.Drawing.Point(9, 12);
            this.lblEmpresaOrigen.Name = "lblEmpresaOrigen";
            this.lblEmpresaOrigen.Size = new System.Drawing.Size(85, 13);
            this.lblEmpresaOrigen.TabIndex = 2;
            this.lblEmpresaOrigen.Text = "Empresa Origen:";
            // 
            // cbEmpresasDestino
            // 
            this.cbEmpresasDestino.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbEmpresasDestino.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmpresasDestino.FormattingEnabled = true;
            this.cbEmpresasDestino.Location = new System.Drawing.Point(12, 76);
            this.cbEmpresasDestino.Name = "cbEmpresasDestino";
            this.cbEmpresasDestino.Size = new System.Drawing.Size(570, 21);
            this.cbEmpresasDestino.TabIndex = 5;
            // 
            // lblEmpresasDestino
            // 
            this.lblEmpresasDestino.AutoSize = true;
            this.lblEmpresasDestino.Location = new System.Drawing.Point(9, 60);
            this.lblEmpresasDestino.Name = "lblEmpresasDestino";
            this.lblEmpresasDestino.Size = new System.Drawing.Size(90, 13);
            this.lblEmpresasDestino.TabIndex = 4;
            this.lblEmpresasDestino.Text = "Empresa Destino:";
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(9, 109);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(46, 13);
            this.lblArchivo.TabIndex = 6;
            this.lblArchivo.Text = "Archivo:";
            // 
            // tbArchivo
            // 
            this.tbArchivo.Location = new System.Drawing.Point(12, 125);
            this.tbArchivo.Name = "tbArchivo";
            this.tbArchivo.ReadOnly = true;
            this.tbArchivo.Size = new System.Drawing.Size(523, 20);
            this.tbArchivo.TabIndex = 7;
            // 
            // btnArchivo
            // 
            this.btnArchivo.Location = new System.Drawing.Point(541, 123);
            this.btnArchivo.Name = "btnArchivo";
            this.btnArchivo.Size = new System.Drawing.Size(41, 23);
            this.btnArchivo.TabIndex = 8;
            this.btnArchivo.Text = "...";
            this.btnArchivo.UseVisualStyleBackColor = true;
            this.btnArchivo.Click += new System.EventHandler(this.btnArchivo_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 343);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(570, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(12, 379);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 12;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(93, 379);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 13;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // tbHistorial
            // 
            this.tbHistorial.Location = new System.Drawing.Point(12, 169);
            this.tbHistorial.Multiline = true;
            this.tbHistorial.Name = "tbHistorial";
            this.tbHistorial.ReadOnly = true;
            this.tbHistorial.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbHistorial.Size = new System.Drawing.Size(570, 168);
            this.tbHistorial.TabIndex = 10;
            this.tbHistorial.TabStop = false;
            // 
            // lblHistorial
            // 
            this.lblHistorial.AutoSize = true;
            this.lblHistorial.Location = new System.Drawing.Point(9, 153);
            this.lblHistorial.Name = "lblHistorial";
            this.lblHistorial.Size = new System.Drawing.Size(47, 13);
            this.lblHistorial.TabIndex = 9;
            this.lblHistorial.Text = "Historial:";
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 408);
            this.Controls.Add(this.tbHistorial);
            this.Controls.Add(this.lblHistorial);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnArchivo);
            this.Controls.Add(this.tbArchivo);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.cbEmpresasDestino);
            this.Controls.Add(this.lblEmpresasDestino);
            this.Controls.Add(this.cbEmpresasOrigen);
            this.Controls.Add(this.lblEmpresaOrigen);
            this.Name = "frmPrincipal";
            this.ShowIcon = false;
            this.Text = "Tricorp Migra Facturas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbEmpresasOrigen;
        private System.Windows.Forms.Label lblEmpresaOrigen;
        private System.Windows.Forms.ComboBox cbEmpresasDestino;
        private System.Windows.Forms.Label lblEmpresasDestino;
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.TextBox tbArchivo;
        private System.Windows.Forms.Button btnArchivo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.TextBox tbHistorial;
        private System.Windows.Forms.Label lblHistorial;
    }
}

