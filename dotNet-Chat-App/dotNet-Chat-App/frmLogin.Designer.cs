namespace dotNet_Chat_App
{
    partial class frmLogin
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
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbPass = new System.Windows.Forms.TextBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txbUserName
            // 
            this.txbUserName.Location = new System.Drawing.Point(87, 40);
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(180, 23);
            this.txbUserName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tài khoản";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 87);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mật khẩu";
            // 
            // txbPass
            // 
            this.txbPass.Location = new System.Drawing.Point(87, 84);
            this.txbPass.Name = "txbPass";
            this.txbPass.Size = new System.Drawing.Size(180, 23);
            this.txbPass.TabIndex = 2;
            // 
            // btnServer
            // 
            this.btnServer.AutoSize = true;
            this.btnServer.Location = new System.Drawing.Point(24, 129);
            this.btnServer.Margin = new System.Windows.Forms.Padding(2);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(134, 27);
            this.btnServer.TabIndex = 4;
            this.btnServer.Text = "Đăng nhập Server";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnClient
            // 
            this.btnClient.AutoSize = true;
            this.btnClient.Location = new System.Drawing.Point(158, 129);
            this.btnClient.Margin = new System.Windows.Forms.Padding(2);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(125, 27);
            this.btnClient.TabIndex = 5;
            this.btnClient.Text = "Đăng nhập client";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(94, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Đăng nhập";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 175);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClient);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbUserName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximumSize = new System.Drawing.Size(311, 222);
            this.MinimumSize = new System.Drawing.Size(311, 222);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App Chat";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbPass;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Label label3;
    }
}