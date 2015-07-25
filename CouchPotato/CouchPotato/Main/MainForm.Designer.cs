namespace com.CouchPotato.Main
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.axsopocx1 = new AxsopocxLib.Axsopocx();
            this.osdChannelList1 = new com.CouchPotato.UserControls.OSDChannelList();
            ((System.ComponentModel.ISupportInitialize)(this.axsopocx1)).BeginInit();
            this.SuspendLayout();
            // 
            // axsopocx1
            // 
            this.axsopocx1.Enabled = true;
            this.axsopocx1.Location = new System.Drawing.Point(12, 12);
            this.axsopocx1.Name = "axsopocx1";
            this.axsopocx1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axsopocx1.OcxState")));
            this.axsopocx1.Size = new System.Drawing.Size(484, 356);
            this.axsopocx1.TabIndex = 1;
            this.axsopocx1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.axsopocx1_PreviewKeyDown);
            // 
            // osdChannelList1
            // 
            this.osdChannelList1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.osdChannelList1.Location = new System.Drawing.Point(20, 20);
            this.osdChannelList1.Name = "osdChannelList1";
            this.osdChannelList1.Size = new System.Drawing.Size(300, 38);
            this.osdChannelList1.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 398);
            this.Controls.Add(this.osdChannelList1);
            this.Controls.Add(this.axsopocx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CouchPotato";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axsopocx1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxsopocxLib.Axsopocx axsopocx1;
        private com.CouchPotato.UserControls.OSDChannelList osdChannelList1;
        //private AlphaLabel alphaLabel1;
    }
}

