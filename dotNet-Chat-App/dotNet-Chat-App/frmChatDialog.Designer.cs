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
            this.lvMessageData = new System.Windows.Forms.ListView();
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(503, 440);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.lvMessageData);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(497, 368);
            this.panel4.TabIndex = 0;
            // 
            // lvMessageData
            // 
            this.lvMessageData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMessageData.HideSelection = false;
            this.lvMessageData.Location = new System.Drawing.Point(0, 0);
            this.lvMessageData.Name = "lvMessageData";
            this.lvMessageData.Size = new System.Drawing.Size(497, 368);
            this.lvMessageData.TabIndex = 0;
            this.lvMessageData.UseCompatibleStateImageBehavior = false;
            this.lvMessageData.View = System.Windows.Forms.View.Details;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tbSend);
            this.panel5.Controls.Add(this.btnSend);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 377);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(497, 60);
            this.panel5.TabIndex = 1;
            // 
            // tbSend
            // 
            this.tbSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbSend.Location = new System.Drawing.Point(3, 3);
            this.tbSend.Multiline = true;
            this.tbSend.Name = "tbSend";
            this.tbSend.Size = new System.Drawing.Size(399, 56);
            this.tbSend.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSend.Location = new System.Drawing.Point(408, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(86, 54);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // frmChatDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 450);
            this.Controls.Add(this.tableLayoutPanel2);
            this.MaximumSize = new System.Drawing.Size(531, 497);
            this.MinimumSize = new System.Drawing.Size(531, 497);
            this.Name = "frmChatDialog";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Dialog - Hau Xun";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView lvMessageData;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox tbSend;
        private System.Windows.Forms.Button btnSend;
    }
}