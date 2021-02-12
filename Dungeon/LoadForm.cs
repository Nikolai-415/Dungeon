using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Форма загрузки сохранения</summary>
    public partial class LoadForm : Form
    {
        /// <summary>Начать ли загрузку</summary>
        private bool m_go_load;

        /// <summary>Начать ли загрузку</summary>
        public bool GoLoad
        {
            get
            {
                return m_go_load;
            }
        }

        /// <summary>Создаёт форму загрузки сохранения</summary>
        public LoadForm()
        {
            InitializeComponent();
        }

        /// <summary>Событие загрузки формы</summary>
        private void LoadForm_Load(object sender, EventArgs e)
        {

            dataGridView_saves.ColumnCount = 7; // количество столбцов таблицы
            dataGridView_saves.RowCount = 0; // количество строк таблицы

            // задаём названия столбцов
            dataGridView_saves.Columns[0].Name = "Название сохранения";
            dataGridView_saves.Columns[1].Name = "Сложность";
            dataGridView_saves.Columns[2].Name = "Персонаж";
            dataGridView_saves.Columns[3].Name = "Уровень персонажа";
            dataGridView_saves.Columns[4].Name = "Ключ генерации мира";
            dataGridView_saves.Columns[5].Name = "Текущий уровень подземелья";
            dataGridView_saves.Columns[6].Name = "Дата и время сохранения";


            string folder_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\data";
            if (!Directory.Exists(folder_path))
            {
                Directory.CreateDirectory(folder_path);
            }
            folder_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\data\\saves";
            if (!Directory.Exists(folder_path))
            {
                Directory.CreateDirectory(folder_path);
            }
            folder_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\data\\saves\\" + (Owner as MainForm).Nick;
            if (!Directory.Exists(folder_path))
            {
                Directory.CreateDirectory(folder_path);
            }
            string[] files = Directory.GetFiles("data\\saves\\" + (Owner as MainForm).Nick + "\\", "*.bin");

            dataGridView_saves.RowCount = files.Length;
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter(); // создаем объект BinaryFormatter
                    using (FileStream fs = new FileStream(files[i], FileMode.Open)) // получаем поток, откуда будем считывать сериализованные объекты
                    {
                        string filename = (string)formatter.Deserialize(fs);
                        DungeonDifficulty total_difficulty_id = (DungeonDifficulty)formatter.Deserialize(fs);
                        string hero_name = (string)formatter.Deserialize(fs);
                        int hero_level = (int)formatter.Deserialize(fs);
                        string total_level_generation_number = (string)formatter.Deserialize(fs);
                        int hero_dungeon_level_id = (int)formatter.Deserialize(fs);
                        string datetime = (string)formatter.Deserialize(fs);
                        formatter.Deserialize(fs); // герой
                        DungeonInventoryStatus m_total_inventory_mode = (DungeonInventoryStatus)formatter.Deserialize(fs);
                        int m_id_clicked_cell = (int)formatter.Deserialize(fs);
                        string difficulty;
                        if (total_difficulty_id == DungeonDifficulty.Easy)
                        {
                            difficulty = "Легко";
                        }
                        else if (total_difficulty_id == DungeonDifficulty.Normal)
                        {
                            difficulty = "Нормально";
                        }
                        else if (total_difficulty_id == DungeonDifficulty.Hard)
                        {
                            difficulty = "Сложно";
                        }
                        else
                        {
                            difficulty = "Хардкор";
                        }
                        dataGridView_saves.Rows[i].Cells[0].Value = filename;
                        dataGridView_saves.Rows[i].Cells[1].Value = difficulty;
                        dataGridView_saves.Rows[i].Cells[2].Value = hero_name;
                        dataGridView_saves.Rows[i].Cells[3].Value = hero_level;
                        dataGridView_saves.Rows[i].Cells[4].Value = total_level_generation_number;
                        dataGridView_saves.Rows[i].Cells[5].Value = (hero_dungeon_level_id + 1).ToString();
                        dataGridView_saves.Rows[i].Cells[6].Value = datetime;
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при чтении файла!", "Рекорды", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (dataGridView_saves.RowCount == 0)
            {
                button_load.Enabled = false;
            }
            else
            {
                button_load.Enabled = true;
            }
        }

        /// <summary>Событие нажатия на кнопку "Назад"</summary>
        private void button_back_Click(object sender, EventArgs e)
        {
            m_go_load = false;
            Close();
        }

        /// <summary>Событие нажатия на кнопку "Загрузить"</summary>
        private void button_load_Click(object sender, EventArgs e)
        {
            m_go_load = true;
            (Owner as MainForm).SaveName = dataGridView_saves.SelectedRows[0].Cells[0].Value.ToString();
            Close();
        }
    }
}
