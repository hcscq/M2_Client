namespace MirDataTools
{
    partial class UpdateName
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
            this.dgv_files = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_selDir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_files)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_files
            // 
            this.dgv_files.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_files.Location = new System.Drawing.Point(13, 43);
            this.dgv_files.Name = "dgv_files";
            this.dgv_files.RowTemplate.Height = 23;
            this.dgv_files.Size = new System.Drawing.Size(504, 350);
            this.dgv_files.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(22, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(275, 21);
            this.textBox1.TabIndex = 1;
            // 
            // btn_selDir
            // 
            this.btn_selDir.Location = new System.Drawing.Point(303, 13);
            this.btn_selDir.Name = "btn_selDir";
            this.btn_selDir.Size = new System.Drawing.Size(75, 23);
            this.btn_selDir.TabIndex = 2;
            this.btn_selDir.Text = "选择目录";
            this.btn_selDir.UseVisualStyleBackColor = true;
            this.btn_selDir.Click += new System.EventHandler(this.btn_selDir_Click);
            // 
            // UpdateName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 405);
            this.Controls.Add(this.btn_selDir);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dgv_files);
            this.Name = "UpdateName";
            this.Text = "UpdateName";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_files)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_files;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_selDir;
    }
}

