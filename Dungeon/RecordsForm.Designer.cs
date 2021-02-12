namespace Dungeon
{
    partial class RecordsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordsForm));
            this.dataGridView_records = new System.Windows.Forms.DataGridView();
            this.button_back = new System.Windows.Forms.Button();
            this.radioButton_records_player = new System.Windows.Forms.RadioButton();
            this.radioButton_records_all = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_records)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_records
            // 
            this.dataGridView_records.AllowUserToAddRows = false;
            this.dataGridView_records.AllowUserToDeleteRows = false;
            this.dataGridView_records.AllowUserToResizeColumns = false;
            this.dataGridView_records.AllowUserToResizeRows = false;
            this.dataGridView_records.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_records.BackgroundColor = System.Drawing.Color.Black;
            this.dataGridView_records.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_records.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_records.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_records.GridColor = System.Drawing.Color.DimGray;
            this.dataGridView_records.Location = new System.Drawing.Point(8, 62);
            this.dataGridView_records.Name = "dataGridView_records";
            this.dataGridView_records.ReadOnly = true;
            this.dataGridView_records.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_records.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_records.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_records.Size = new System.Drawing.Size(1160, 320);
            this.dataGridView_records.TabIndex = 8;
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_back.Font = new System.Drawing.Font("Segoe WP Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(455, 395);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(280, 50);
            this.button_back.TabIndex = 9;
            this.button_back.Text = "Назад";
            this.button_back.UseVisualStyleBackColor = false;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // radioButton_records_player
            // 
            this.radioButton_records_player.AutoSize = true;
            this.radioButton_records_player.Checked = true;
            this.radioButton_records_player.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButton_records_player.ForeColor = System.Drawing.Color.White;
            this.radioButton_records_player.Location = new System.Drawing.Point(299, 12);
            this.radioButton_records_player.Name = "radioButton_records_player";
            this.radioButton_records_player.Size = new System.Drawing.Size(259, 35);
            this.radioButton_records_player.TabIndex = 10;
            this.radioButton_records_player.TabStop = true;
            this.radioButton_records_player.Text = "Личные рекорды";
            this.radioButton_records_player.UseVisualStyleBackColor = true;
            this.radioButton_records_player.CheckedChanged += new System.EventHandler(this.radioButton_records_CheckedChanged);
            // 
            // radioButton_records_all
            // 
            this.radioButton_records_all.AutoSize = true;
            this.radioButton_records_all.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButton_records_all.ForeColor = System.Drawing.Color.White;
            this.radioButton_records_all.Location = new System.Drawing.Point(632, 12);
            this.radioButton_records_all.Name = "radioButton_records_all";
            this.radioButton_records_all.Size = new System.Drawing.Size(249, 35);
            this.radioButton_records_all.TabIndex = 11;
            this.radioButton_records_all.Text = "Общие рекорды";
            this.radioButton_records_all.UseVisualStyleBackColor = true;
            this.radioButton_records_all.CheckedChanged += new System.EventHandler(this.radioButton_records_CheckedChanged);
            // 
            // RecordsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1180, 457);
            this.Controls.Add(this.radioButton_records_all);
            this.Controls.Add(this.radioButton_records_player);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.dataGridView_records);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecordsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подземелье - Рекорды";
            this.Load += new System.EventHandler(this.RecordsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_records)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_records;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.RadioButton radioButton_records_player;
        private System.Windows.Forms.RadioButton radioButton_records_all;
    }
}