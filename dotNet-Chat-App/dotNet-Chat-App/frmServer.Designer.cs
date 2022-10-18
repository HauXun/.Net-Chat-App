namespace dotNet_Chat_App
{
    partial class frmServer
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.flpClientContainer = new System.Windows.Forms.FlowLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.lbTitle = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.lvMessageData = new System.Windows.Forms.ListView();
			this.panel5 = new System.Windows.Forms.Panel();
			this.tbSend = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.flpClientContainer.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
			this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(8);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
			this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(6, 8, 6, 6);
			this.splitContainer1.Size = new System.Drawing.Size(663, 372);
			this.splitContainer1.SplitterDistance = 283;
			this.splitContainer1.SplitterWidth = 3;
			this.splitContainer1.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 356F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(267, 356);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.White;
			this.panel2.Controls.Add(this.flpClientContainer);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(2, 2);
			this.panel2.Margin = new System.Windows.Forms.Padding(2);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(263, 352);
			this.panel2.TabIndex = 1;
			// 
			// flpClientContainer
			// 
			this.flpClientContainer.AutoScroll = true;
			this.flpClientContainer.Controls.Add(this.panel1);
			this.flpClientContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpClientContainer.Location = new System.Drawing.Point(0, 0);
			this.flpClientContainer.Margin = new System.Windows.Forms.Padding(2);
			this.flpClientContainer.Name = "flpClientContainer";
			this.flpClientContainer.Size = new System.Drawing.Size(263, 352);
			this.flpClientContainer.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.lbTitle);
			this.panel1.Location = new System.Drawing.Point(2, 2);
			this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 8);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(242, 53);
			this.panel1.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.label1.Location = new System.Drawing.Point(0, 37);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(242, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Online total: ";
			// 
			// lbTitle
			// 
			this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.lbTitle.Location = new System.Drawing.Point(0, 0);
			this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lbTitle.Name = "lbTitle";
			this.lbTitle.Size = new System.Drawing.Size(242, 31);
			this.lbTitle.TabIndex = 1;
			this.lbTitle.Text = "Server Remote Manager";
			this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 8);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(365, 358);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.SystemColors.Control;
			this.panel4.Controls.Add(this.lvMessageData);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(2, 2);
			this.panel4.Margin = new System.Windows.Forms.Padding(2);
			this.panel4.Name = "panel4";
			this.panel4.Padding = new System.Windows.Forms.Padding(2);
			this.panel4.Size = new System.Drawing.Size(361, 300);
			this.panel4.TabIndex = 0;
			// 
			// lvMessageData
			// 
			this.lvMessageData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvMessageData.HideSelection = false;
			this.lvMessageData.Location = new System.Drawing.Point(2, 2);
			this.lvMessageData.Margin = new System.Windows.Forms.Padding(2);
			this.lvMessageData.Name = "lvMessageData";
			this.lvMessageData.Size = new System.Drawing.Size(357, 296);
			this.lvMessageData.TabIndex = 0;
			this.lvMessageData.UseCompatibleStateImageBehavior = false;
			this.lvMessageData.View = System.Windows.Forms.View.Details;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.tbSend);
			this.panel5.Controls.Add(this.btnSend);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel5.Location = new System.Drawing.Point(2, 306);
			this.panel5.Margin = new System.Windows.Forms.Padding(2);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(361, 50);
			this.panel5.TabIndex = 1;
			// 
			// tbSend
			// 
			this.tbSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.tbSend.Location = new System.Drawing.Point(2, 2);
			this.tbSend.Margin = new System.Windows.Forms.Padding(2);
			this.tbSend.Multiline = true;
			this.tbSend.Name = "tbSend";
			this.tbSend.Size = new System.Drawing.Size(289, 46);
			this.tbSend.TabIndex = 1;
			// 
			// btnSend
			// 
			this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.btnSend.Location = new System.Drawing.Point(295, 2);
			this.btnSend.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(64, 46);
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "Send";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// frmServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(663, 372);
			this.Controls.Add(this.splitContainer1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximumSize = new System.Drawing.Size(679, 411);
			this.MinimumSize = new System.Drawing.Size(679, 411);
			this.Name = "frmServer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Server Remote";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServer_FormClosing);
			this.Load += new System.EventHandler(this.frmServer_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.flpClientContainer.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox tbSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.FlowLayoutPanel flpClientContainer;
        private System.Windows.Forms.ListView lvMessageData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label label1;
    }
}