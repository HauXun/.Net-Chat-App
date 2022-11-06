namespace dotNet_Chat_App
{
    partial class frmChatDialog
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.tbMessage = new System.Windows.Forms.TextBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.tbSend = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 362);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.Controls.Add(this.tbMessage);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(2, 2);
			this.panel4.Margin = new System.Windows.Forms.Padding(2);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(374, 303);
			this.panel4.TabIndex = 0;
			// 
			// tbMessage
			// 
			this.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.tbMessage.Location = new System.Drawing.Point(0, 0);
			this.tbMessage.Margin = new System.Windows.Forms.Padding(2);
			this.tbMessage.Multiline = true;
			this.tbMessage.Name = "tbMessage";
			this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbMessage.Size = new System.Drawing.Size(374, 303);
			this.tbMessage.TabIndex = 4;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.tbSend);
			this.panel5.Controls.Add(this.btnSend);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel5.Location = new System.Drawing.Point(2, 309);
			this.panel5.Margin = new System.Windows.Forms.Padding(2);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(374, 51);
			this.panel5.TabIndex = 1;
			// 
			// tbSend
			// 
			this.tbSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.tbSend.Location = new System.Drawing.Point(2, 2);
			this.tbSend.Margin = new System.Windows.Forms.Padding(2);
			this.tbSend.Multiline = true;
			this.tbSend.Name = "tbSend";
			this.tbSend.Size = new System.Drawing.Size(300, 48);
			this.tbSend.TabIndex = 1;
			// 
			// btnSend
			// 
			this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.btnSend.Location = new System.Drawing.Point(306, 2);
			this.btnSend.Margin = new System.Windows.Forms.Padding(2);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(64, 47);
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "Send";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// frmChatDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(386, 370);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximumSize = new System.Drawing.Size(402, 409);
			this.MinimumSize = new System.Drawing.Size(402, 409);
			this.Name = "frmChatDialog";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chat Dialog - Hau Xun";
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox tbSend;
        private System.Windows.Forms.Button btnSend;
		public System.Windows.Forms.TextBox tbMessage;
	}
}