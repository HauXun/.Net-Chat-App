namespace dotNet_Chat_App
{
    partial class AddToGroup
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
            this.tbGroupName = new System.Windows.Forms.TextBox();
            this.lbToAdd = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbAdded = new System.Windows.Forms.ListBox();
            this.btnFoward = new System.Windows.Forms.Button();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnBackwardAll = new System.Windows.Forms.Button();
            this.btnFowardAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group Name";
            // 
            // tbGroupName
            // 
            this.tbGroupName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tbGroupName.Location = new System.Drawing.Point(122, 12);
            this.tbGroupName.Name = "tbGroupName";
            this.tbGroupName.Size = new System.Drawing.Size(223, 30);
            this.tbGroupName.TabIndex = 1;
            // 
            // lbToAdd
            // 
            this.lbToAdd.FormattingEnabled = true;
            this.lbToAdd.ItemHeight = 20;
            this.lbToAdd.Location = new System.Drawing.Point(6, 27);
            this.lbToAdd.Name = "lbToAdd";
            this.lbToAdd.Size = new System.Drawing.Size(260, 344);
            this.lbToAdd.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBackwardAll);
            this.groupBox1.Controls.Add(this.btnFowardAll);
            this.groupBox1.Controls.Add(this.btnBackward);
            this.groupBox1.Controls.Add(this.btnFoward);
            this.groupBox1.Controls.Add(this.lbAdded);
            this.groupBox1.Controls.Add(this.lbToAdd);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox1.Location = new System.Drawing.Point(12, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 377);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clients";
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnAdd.Location = new System.Drawing.Point(351, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 35);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // lbAdded
            // 
            this.lbAdded.FormattingEnabled = true;
            this.lbAdded.ItemHeight = 20;
            this.lbAdded.Location = new System.Drawing.Point(353, 27);
            this.lbAdded.Name = "lbAdded";
            this.lbAdded.Size = new System.Drawing.Size(260, 344);
            this.lbAdded.TabIndex = 3;
            // 
            // btnFoward
            // 
            this.btnFoward.AutoSize = true;
            this.btnFoward.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnFoward.Location = new System.Drawing.Point(272, 100);
            this.btnFoward.Name = "btnFoward";
            this.btnFoward.Size = new System.Drawing.Size(75, 35);
            this.btnFoward.TabIndex = 5;
            this.btnFoward.Text = ">";
            this.btnFoward.UseVisualStyleBackColor = true;
            // 
            // btnBackward
            // 
            this.btnBackward.AutoSize = true;
            this.btnBackward.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnBackward.Location = new System.Drawing.Point(272, 141);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(75, 35);
            this.btnBackward.TabIndex = 6;
            this.btnBackward.Text = "<";
            this.btnBackward.UseVisualStyleBackColor = true;
            // 
            // btnBackwardAll
            // 
            this.btnBackwardAll.AutoSize = true;
            this.btnBackwardAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnBackwardAll.Location = new System.Drawing.Point(272, 223);
            this.btnBackwardAll.Name = "btnBackwardAll";
            this.btnBackwardAll.Size = new System.Drawing.Size(75, 35);
            this.btnBackwardAll.TabIndex = 8;
            this.btnBackwardAll.Text = "<<";
            this.btnBackwardAll.UseVisualStyleBackColor = true;
            // 
            // btnFowardAll
            // 
            this.btnFowardAll.AutoSize = true;
            this.btnFowardAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnFowardAll.Location = new System.Drawing.Point(272, 182);
            this.btnFowardAll.Name = "btnFowardAll";
            this.btnFowardAll.Size = new System.Drawing.Size(75, 35);
            this.btnFowardAll.TabIndex = 7;
            this.btnFowardAll.Text = ">>";
            this.btnFowardAll.UseVisualStyleBackColor = true;
            // 
            // AddToGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 453);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbGroupName);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(660, 500);
            this.MinimumSize = new System.Drawing.Size(660, 500);
            this.Name = "AddToGroup";
            this.Text = "AddToGroup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbGroupName;
        private System.Windows.Forms.ListBox lbToAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnFoward;
        private System.Windows.Forms.ListBox lbAdded;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnBackwardAll;
        private System.Windows.Forms.Button btnFowardAll;
    }
}