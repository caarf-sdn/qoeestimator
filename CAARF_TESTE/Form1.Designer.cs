namespace CAARF_TESTE
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAgentIp = new System.Windows.Forms.TextBox();
            this.textBoxRip = new System.Windows.Forms.TextBox();
            this.textBoxAgentPort = new System.Windows.Forms.TextBox();
            this.textBoxRport = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxTlat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxRsize = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.textBoxTjit = new System.Windows.Forms.TextBox();
            this.textBoxTloss = new System.Windows.Forms.TextBox();
            this.textBoxRqt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "AGENTE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(374, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "REQUISIÇÃO_QOE: IP, porta, tamanho do payload e quantidade de pacotes.";
            // 
            // textBoxAgentIp
            // 
            this.textBoxAgentIp.Location = new System.Drawing.Point(95, 21);
            this.textBoxAgentIp.Name = "textBoxAgentIp";
            this.textBoxAgentIp.Size = new System.Drawing.Size(131, 20);
            this.textBoxAgentIp.TabIndex = 2;
            // 
            // textBoxRip
            // 
            this.textBoxRip.Location = new System.Drawing.Point(26, 142);
            this.textBoxRip.Name = "textBoxRip";
            this.textBoxRip.Size = new System.Drawing.Size(97, 20);
            this.textBoxRip.TabIndex = 3;
            // 
            // textBoxAgentPort
            // 
            this.textBoxAgentPort.Location = new System.Drawing.Point(232, 21);
            this.textBoxAgentPort.Name = "textBoxAgentPort";
            this.textBoxAgentPort.Size = new System.Drawing.Size(75, 20);
            this.textBoxAgentPort.TabIndex = 4;
            // 
            // textBoxRport
            // 
            this.textBoxRport.Location = new System.Drawing.Point(129, 142);
            this.textBoxRport.Name = "textBoxRport";
            this.textBoxRport.Size = new System.Drawing.Size(56, 20);
            this.textBoxRport.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(352, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "ENVIAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxTlat
            // 
            this.textBoxTlat.Location = new System.Drawing.Point(26, 217);
            this.textBoxTlat.Name = "textBoxTlat";
            this.textBoxTlat.Size = new System.Drawing.Size(78, 20);
            this.textBoxTlat.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(267, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "TESTE: Latência média, jitter e porcentagem de perda.";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(322, 215);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "ESTIMAR QOE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxRsize
            // 
            this.textBoxRsize.Location = new System.Drawing.Point(191, 143);
            this.textBoxRsize.Name = "textBoxRsize";
            this.textBoxRsize.Size = new System.Drawing.Size(56, 20);
            this.textBoxRsize.TabIndex = 10;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(26, 260);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(411, 113);
            this.textBoxOutput.TabIndex = 11;
            // 
            // textBoxTjit
            // 
            this.textBoxTjit.Location = new System.Drawing.Point(116, 217);
            this.textBoxTjit.Name = "textBoxTjit";
            this.textBoxTjit.Size = new System.Drawing.Size(84, 20);
            this.textBoxTjit.TabIndex = 12;
            // 
            // textBoxTloss
            // 
            this.textBoxTloss.Location = new System.Drawing.Point(206, 217);
            this.textBoxTloss.Name = "textBoxTloss";
            this.textBoxTloss.Size = new System.Drawing.Size(85, 20);
            this.textBoxTloss.TabIndex = 13;
            // 
            // textBoxRqt
            // 
            this.textBoxRqt.Location = new System.Drawing.Point(253, 143);
            this.textBoxRqt.Name = "textBoxRqt";
            this.textBoxRqt.Size = new System.Drawing.Size(73, 20);
            this.textBoxRqt.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(459, 411);
            this.Controls.Add(this.textBoxRqt);
            this.Controls.Add(this.textBoxTloss);
            this.Controls.Add(this.textBoxTjit);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.textBoxRsize);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTlat);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxRport);
            this.Controls.Add(this.textBoxAgentPort);
            this.Controls.Add(this.textBoxRip);
            this.Controls.Add(this.textBoxAgentIp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "DEBUG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAgentIp;
        private System.Windows.Forms.TextBox textBoxRip;
        private System.Windows.Forms.TextBox textBoxAgentPort;
        private System.Windows.Forms.TextBox textBoxRport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxTlat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxRsize;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.TextBox textBoxTjit;
        private System.Windows.Forms.TextBox textBoxTloss;
        private System.Windows.Forms.TextBox textBoxRqt;
    }
}

