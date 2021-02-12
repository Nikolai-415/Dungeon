namespace Dungeon
{
    partial class SaveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveForm));
            this.label_nick = new System.Windows.Forms.Label();
            this.textBox_save_name = new System.Windows.Forms.TextBox();
            this.button_save = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_nick
            // 
            this.label_nick.Font = new System.Drawing.Font("Segoe WP Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_nick.ForeColor = System.Drawing.Color.White;
            this.label_nick.Location = new System.Drawing.Point(14, 13);
            this.label_nick.Name = "label_nick";
            this.label_nick.Size = new System.Drawing.Size(591, 34);
            this.label_nick.TabIndex = 6;
            this.label_nick.Text = "Название сохранения:";
            this.label_nick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_save_name
            // 
            this.textBox_save_name.Font = new System.Drawing.Font("Segoe WP Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_save_name.Location = new System.Drawing.Point(14, 50);
            this.textBox_save_name.MaxLength = 128;
            this.textBox_save_name.Name = "textBox_save_name";
            this.textBox_save_name.Size = new System.Drawing.Size(591, 35);
            this.textBox_save_name.TabIndex = 5;
            this.textBox_save_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_save_name.TextChanged += new System.EventHandler(this.textBox_save_name_TextChanged);
            this.textBox_save_name.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_save_name_KeyPress);
            // 
            // button_save
            // 
            this.button_save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_save.Enabled = false;
            this.button_save.Font = new System.Drawing.Font("Segoe WP Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_save.ForeColor = System.Drawing.Color.Black;
            this.button_save.Location = new System.Drawing.Point(328, 105);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(280, 50);
            this.button_save.TabIndex = 9;
            this.button_save.Text = "Сохранить";
            this.button_save.UseVisualStyleBackColor = false;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_back.Font = new System.Drawing.Font("Segoe WP Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(12, 105);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(280, 50);
            this.button_back.TabIndex = 8;
            this.button_back.Text = "Назад";
            this.button_back.UseVisualStyleBackColor = false;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // SaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(620, 167);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.label_nick);
            this.Controls.Add(this.textBox_save_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подземелье - Сохранение игры";
            this.Load += new System.EventHandler(this.SaveForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_nick;
        private System.Windows.Forms.TextBox textBox_save_name;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_back;
    }
}