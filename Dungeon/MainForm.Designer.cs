namespace Dungeon
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timer_game = new System.Windows.Forms.Timer(this.components);
            this.timer_stats_update = new System.Windows.Forms.Timer(this.components);
            this.timer_loading_wait = new System.Windows.Forms.Timer(this.components);
            this.timer_monsters_move = new System.Windows.Forms.Timer(this.components);
            this.timer_walkthrow = new System.Windows.Forms.Timer(this.components);
            this.timer_change_music = new System.Windows.Forms.Timer(this.components);
            this.timer_main_loading_wait = new System.Windows.Forms.Timer(this.components);
            this.timer_saving_wait = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer_game
            // 
            this.timer_game.Interval = 1;
            this.timer_game.Tick += new System.EventHandler(this.timer_game_Tick);
            // 
            // timer_stats_update
            // 
            this.timer_stats_update.Tick += new System.EventHandler(this.timer_stats_update_Tick);
            // 
            // timer_loading_wait
            // 
            this.timer_loading_wait.Tick += new System.EventHandler(this.timer_loading_wait_Tick);
            // 
            // timer_monsters_move
            // 
            this.timer_monsters_move.Interval = 10;
            this.timer_monsters_move.Tick += new System.EventHandler(this.timer_monsters_move_Tick);
            // 
            // timer_walkthrow
            // 
            this.timer_walkthrow.Interval = 1000;
            this.timer_walkthrow.Tick += new System.EventHandler(this.timer_walkthrow_Tick);
            // 
            // timer_change_music
            // 
            this.timer_change_music.Interval = 500;
            this.timer_change_music.Tick += new System.EventHandler(this.timer_change_music_Tick);
            // 
            // timer_main_loading_wait
            // 
            this.timer_main_loading_wait.Tick += new System.EventHandler(this.timer_main_loading_wait_Tick);
            // 
            // timer_saving_wait
            // 
            this.timer_saving_wait.Tick += new System.EventHandler(this.timer_saving_wait_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(780, 557);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подземелье";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameForm_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_game;
        private System.Windows.Forms.Timer timer_stats_update;
        private System.Windows.Forms.Timer timer_loading_wait;
        private System.Windows.Forms.Timer timer_monsters_move;
        private System.Windows.Forms.Timer timer_walkthrow;
        private System.Windows.Forms.Timer timer_change_music;
        private System.Windows.Forms.Timer timer_main_loading_wait;
        private System.Windows.Forms.Timer timer_saving_wait;
    }
}

