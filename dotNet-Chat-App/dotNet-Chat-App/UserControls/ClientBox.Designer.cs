namespace dotNet_Chat_App.UserControls
{
    partial class ClientBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.lbName = new System.Windows.Forms.Label();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlContainer.Controls.Add(this.lbName);
            this.pnlContainer.Controls.Add(this.pbStatus);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(321, 50);
            this.pnlContainer.TabIndex = 0;
            this.pnlContainer.MouseEnter += new System.EventHandler(this.pnlContainer_MouseEnter);
            this.pnlContainer.MouseLeave += new System.EventHandler(this.pnlContainer_MouseLeave);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbName.Location = new System.Drawing.Point(60, 16);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(74, 20);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "Hau Xun";
            this.lbName.MouseEnter += new System.EventHandler(this.pnlContainer_MouseEnter);
            this.lbName.MouseLeave += new System.EventHandler(this.pnlContainer_MouseLeave);
            // 
            // pbStatus
            // 
            this.pbStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbStatus.Location = new System.Drawing.Point(3, 3);
            this.pbStatus.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(44, 44);
            this.pbStatus.TabIndex = 0;
            this.pbStatus.TabStop = false;
            this.pbStatus.MouseEnter += new System.EventHandler(this.pnlContainer_MouseEnter);
            this.pbStatus.MouseLeave += new System.EventHandler(this.pnlContainer_MouseLeave);
            // 
            // ClientBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlContainer);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.Name = "ClientBox";
            this.Size = new System.Drawing.Size(321, 50);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.PictureBox pbStatus;
    }
}
