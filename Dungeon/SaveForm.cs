using System;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Форма создания сохранения</summary>
    public partial class SaveForm : Form
    {
        /// <summary>Начать ли сохранение</summary>
        private bool m_go_save;

        /// <summary>Начать ли сохранение</summary>
        public bool GoSave
        {
            get
            {
                return m_go_save;
            }
        }

        /// <summary>Создаёт форму сохранения игры</summary>
        public SaveForm()
        {
            InitializeComponent();
        }

        /// <summary>Событие загрузки формы</summary>
        private void SaveForm_Load(object sender, EventArgs e)
        {
            m_go_save = false;
            textBox_save_name.Text = (Owner as MainForm).SaveName;
        }

        /// <summary>Событие нажатия клавиши в текстовом поле</summary>
        private void textBox_save_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
               (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
               (e.KeyChar >= 'А' && e.KeyChar <= 'Я') ||
               (e.KeyChar >= 'а' && e.KeyChar <= 'я') ||
               (e.KeyChar >= '0' && e.KeyChar <= '9') ||
                e.KeyChar == ' ' ||
                e.KeyChar == '_' ||
                e.KeyChar == '-' ||
                e.KeyChar == 8)
            {
                return;
            }
            else
            {
                e.KeyChar = '\0';
            }
        }

        /// <summary>Событие изменения текста в текстовом поле</summary>
        private void textBox_save_name_TextChanged(object sender, EventArgs e)
        {
            if (textBox_save_name.Text != "")
            {
                button_save.Enabled = true;
            }
            else
            {
                button_save.Enabled = false;
            }
        }

        /// <summary>Событие нажатия на кнопку "Назад"</summary>
        private void button_back_Click(object sender, EventArgs e)
        {
            m_go_save = false;
            Close();
        }

        /// <summary>Событие нажатия на кнопку "Сохранить"</summary>
        private void button_save_Click(object sender, EventArgs e)
        {
            m_go_save = true;
            (Owner as MainForm).SaveName = textBox_save_name.Text;
            Close();
        }
    }
}
