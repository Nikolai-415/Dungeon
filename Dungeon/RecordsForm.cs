using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Форма рекордов</summary>
    public partial class RecordsForm : Form
    {
        /// <summary>Создаёт форму рекордов</summary>
        public RecordsForm()
        {
            InitializeComponent();
        }

        /// <summary>Событие загрузки формы</summary>
        private void RecordsForm_Load(object sender, EventArgs e)
        {
            radioButton_records_player.Checked = true;
            UpdateData();
        }

        /// <summary>Событие изменения выбранной радио-кнопки</summary>
        private void radioButton_records_CheckedChanged(object sender, EventArgs e)
        {
            if ((radioButton_records_player.Checked && !radioButton_records_all.Checked) ||
               (!radioButton_records_player.Checked && radioButton_records_all.Checked))
            {
                UpdateData();
            }
        }

        /// <summary>Событие нажатия на кнопку "Назад"</summary>
        private void button_back_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Обновляет таблицу рекордов</summary>
        private void UpdateData()
        {
            dataGridView_records.ColumnCount = 11; // количество столбцов таблицы
            dataGridView_records.RowCount = 0; // количество строк таблицы

            // задаём названия столбцов
            dataGridView_records.Columns[0].Name = "Номер рекорда";
            dataGridView_records.Columns[1].Name = "Состояние";
            dataGridView_records.Columns[2].Name = "Время прохождения";
            dataGridView_records.Columns[3].Name = "Ник игрока";
            dataGridView_records.Columns[4].Name = "Сложность";
            dataGridView_records.Columns[5].Name = "Персонаж";
            dataGridView_records.Columns[6].Name = "Достигнутый уровень персонажа";
            dataGridView_records.Columns[7].Name = "Ключ генерации мира";
            dataGridView_records.Columns[8].Name = "Пройденное расстояние";
            dataGridView_records.Columns[9].Name = "Монстров убито";
            dataGridView_records.Columns[10].Name = "Боссов убито";

            try
            {
                string folder_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\data";
                if (!Directory.Exists(folder_path))
                {
                    Directory.CreateDirectory(folder_path);
                }
                BinaryFormatter formatter = new BinaryFormatter(); // создаем объект BinaryFormatter
                using (FileStream fs = new FileStream("data\\records.bin", FileMode.OpenOrCreate)) // получаем поток, куда будем записывать сериализованные объекты
                {
                    if (fs.Length != 0) // если длина файла не равна нулю - рекорды есть
                    {
                        fs.Position = 0; // перемещаем указатель в начало файла
                        int records_number = (int)formatter.Deserialize(fs); // количество рекордов
                        for (int i = 0; i < records_number; i++) // проходим циклом по всем рекордам
                        {
                            int record_number = (int)formatter.Deserialize(fs); // номер рекорда
                            bool is_win = (bool)formatter.Deserialize(fs); // пройдена ли игра
                            int time = (int)formatter.Deserialize(fs); // время
                            string nick = (string)formatter.Deserialize(fs); // ник игрока
                            DungeonDifficulty difficulty_id = (DungeonDifficulty)formatter.Deserialize(fs); // сложность
                            string character_name = (string)formatter.Deserialize(fs); // имя персонажа
                            int character_level = (int)formatter.Deserialize(fs); // достигнутый уровень персонажа
                            string world_key = (string)formatter.Deserialize(fs); // ключ генерации мира
                            double distance_walked = (double)formatter.Deserialize(fs); // пройденное расстояние
                            int monsters_killed = (int)formatter.Deserialize(fs); // монстров убито
                            int bosses_killed = (int)formatter.Deserialize(fs); // боссов убито

                            if (radioButton_records_all.Checked || (radioButton_records_player.Checked && nick == (Owner as MainForm).Nick))
                            {
                                dataGridView_records.Rows.Add();
                                int id = dataGridView_records.RowCount - 1;

                                // занесение прочитанных из файла данных в таблицу
                                dataGridView_records.Rows[id].Cells[0].Value = record_number;
                                string is_win_text;
                                if (is_win)
                                {
                                    is_win_text = "Победа";
                                }
                                else
                                {
                                    is_win_text = "Поражение";
                                }
                                dataGridView_records.Rows[id].Cells[1].Value = is_win_text;
                                string time_text = "";
                                if (time / 60 / 60 < 10)
                                {
                                    time_text += "0";
                                }
                                time_text += (time / 60 / 60).ToString();
                                time_text += ":";
                                if ((time - time / 60 / 60 * 60) / 60 < 10) time_text += "0";
                                time_text += ((time - time / 60 / 60 * 60) / 60).ToString();
                                time_text += ":";
                                if (time - time / 60 / 60 * 60 - (time - time / 60 / 60 * 60) / 60 * 60 < 10) time_text += "0";
                                time_text += (time - time / 60 / 60 * 60 - (time - time / 60 / 60 * 60) / 60 * 60).ToString();
                                dataGridView_records.Rows[id].Cells[2].Value = time_text;
                                dataGridView_records.Rows[id].Cells[3].Value = nick;
                                string difficulty;
                                if (difficulty_id == DungeonDifficulty.Easy)
                                {
                                    difficulty = "Легко";
                                }
                                else if (difficulty_id == DungeonDifficulty.Normal)
                                {
                                    difficulty = "Нормально";
                                }
                                else if (difficulty_id == DungeonDifficulty.Hard)
                                {
                                    difficulty = "Сложно";
                                }
                                else
                                {
                                    difficulty = "Хардкор";
                                }
                                dataGridView_records.Rows[id].Cells[4].Value = difficulty;
                                dataGridView_records.Rows[id].Cells[5].Value = character_name;
                                dataGridView_records.Rows[id].Cells[6].Value = character_level;
                                dataGridView_records.Rows[id].Cells[7].Value = world_key;
                                dataGridView_records.Rows[id].Cells[8].Value = distance_walked;
                                dataGridView_records.Rows[id].Cells[9].Value = monsters_killed;
                                dataGridView_records.Rows[id].Cells[10].Value = bosses_killed;
                            }
                        }
                    }
                    else // если нет рекордов
                    {
                        dataGridView_records.RowCount = 0; // количество строк = 0
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла!", "Рекорды", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
