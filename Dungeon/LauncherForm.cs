using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Форма авторизации / регистрации пользователей</summary>
    public partial class LauncherForm : Form
    {
        /// <summary>Идёт ли регистрация пользователя (если нет, то идёт авторизация)</summary>
        private bool m_is_registration = false;

        /// <summary>Главная форма</summary>
        private MainForm m_main_form;

        /// <summary>Создаёт форму авторизации / регистрации пользователей</summary>
        public LauncherForm()
        {
            InitializeComponent();
        }

        /// <summary>Событие перед закрытием формы</summary>
        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>Событие нажатия клавиши в текстовом поле</summary>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
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
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (textBox_nick.Text != "" && textBox_password.Text != "")
            {
                button_right.Enabled = true;
            }
            else
            {
                button_right.Enabled = false;
            }
        }

        /// <summary>Событие нажатия на левую кнопку ("Регистрация" или "Авторизация")</summary>
        private void button_left_Click(object sender, EventArgs e)
        {
            if (m_is_registration)
            {
                m_is_registration = false;
                Text = "Подземелье - Авторизация";
                label_main.Text = "Вход в существующий аккаунт";
                button_left.Text = "Регистрация";
                button_right.Text = "Войти";
                textBox_password.PasswordChar = '*';
            }
            else
            {
                m_is_registration = true;
                Text = "Подземелье - Регистрация";
                label_main.Text = "Создание нового аккаунта";
                button_left.Text = "Авторизация";
                button_right.Text = "Создать";
                textBox_password.PasswordChar = '\0';
            }
        }

        /// <summary>Событие нажатия на правую кнопку ("Создать" или "Войти")</summary>
        private void button_right_Click(object sender, EventArgs e)
        {
            if (m_is_registration)
            {
                string nick = textBox_nick.Text; // ник игрока

                int users_number = 0; // количество пользователей
                try
                {
                    string folder_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\data";
                    if (!Directory.Exists(folder_path))
                    {
                        Directory.CreateDirectory(folder_path);
                    }
                    BinaryFormatter formatter = new BinaryFormatter(); // создаем объект BinaryFormatter
                    using (FileStream fs = new FileStream("data\\users.bin", FileMode.OpenOrCreate)) // получаем поток, куда будем записывать сериализованные объекты
                    {
                        if (fs.Length != 0) // если длина файла не равна нулю - пользователи есть
                        {
                            fs.Position = 0; // перемещаем указатель в начало файла
                            users_number = (int)formatter.Deserialize(fs);
                            for (int i = 0; i < users_number; i++) // проходим циклом по всем пользователям
                            {
                                string finded_nick = (string)formatter.Deserialize(fs); // найденный ник
                                string finded_password_md5 = (string)formatter.Deserialize(fs); // найденный зашифрованный пароль
                                if (finded_nick == nick) // если введённый ник уже есть в файле
                                {
                                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при чтении файла!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                users_number++;

                string password = textBox_password.Text; // пароль
                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                string password_md5 = sBuilder.ToString(); // зашифрованный при помощи MD5 пароль

                try
                {
                    BinaryFormatter formatter = new BinaryFormatter(); // создаем объект BinaryFormatter
                    using (FileStream fs = new FileStream("data\\users.bin", FileMode.OpenOrCreate)) // получаем поток, куда будем записывать сериализованные объекты
                    {
                        if (fs.Length != 0) // если длина файла не равна нулю - пользователи есть
                        {
                            fs.Position = 0; // перемещаем указатель в начало файла
                            formatter.Serialize(fs, users_number); // перезаписываем число пользователей
                            fs.Position = fs.Length; // перемещаем указатель в конец файла
                        }
                        else // если длина файла равна нулю - пользователей нет
                        {
                            formatter.Serialize(fs, 1); // 1 пользователь
                        }
                        formatter.Serialize(fs, nick); // записываем ник в файл
                        formatter.Serialize(fs, password_md5); // записываем зашифрованный пароль в файл
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при записи в файл!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                textBox_password.Clear(); // очищаем поле с паролем

                Hide(); // скрываем текущую форму

                MessageBox.Show("Вы успешно зарегистрировались!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                if (m_main_form == null || m_main_form.IsDisposed)
                {
                    m_main_form = new MainForm(); // создаём форму с главным меню
                    m_main_form.Owner = this;
                }
                else
                {
                    m_main_form.LoadForm();
                }

                m_main_form.IsRegistration = true;
                m_main_form.Nick = nick;

                m_main_form.Show();
            }
            else
            {
                string nick = textBox_nick.Text;

                string password = textBox_password.Text; // пароль
                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                string password_md5 = sBuilder.ToString(); // зашифрованный при помощи MD5 пароль

                try
                {
                    string folder_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\data";
                    if (!Directory.Exists(folder_path))
                    {
                        Directory.CreateDirectory(folder_path);
                    }
                    BinaryFormatter formatter = new BinaryFormatter(); // создаем объект BinaryFormatter
                    using (FileStream fs = new FileStream("data\\users.bin", FileMode.OpenOrCreate)) // получаем поток, куда будем записывать сериализованные объекты
                    {
                        if (fs.Length != 0) // если длина файла не равна нулю - пользователи есть
                        {
                            fs.Position = 0; // перемещаем указатель в начало файла
                            int users_number = (int)formatter.Deserialize(fs); // количество пользователей
                            bool is_finded = false; // найден ли введённый ник
                            for (int i = 0; i < users_number; i++) // проходим циклом по всем пользователям
                            {
                                string finded_nick = (string)formatter.Deserialize(fs); // найденный ник
                                string finded_password_md5 = (string)formatter.Deserialize(fs); // найденный зашифрованный пароль
                                if (finded_nick == nick) // если введённый ник найден
                                {
                                    if (finded_password_md5 == password_md5) // если введённый зашифрованный пароль совпадает с найденным в файле
                                    {
                                        is_finded = true;
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Неверный пароль!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                            if (!is_finded)
                            {
                                MessageBox.Show("Введённый ник не найден!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            formatter.Serialize(fs, 0); // перезаписываем число пользователей
                            MessageBox.Show("Введённый ник не найден!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при чтении файла!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                textBox_password.Clear(); // очищаем поле с паролем

                Hide(); // скрываем текущую форму

                MessageBox.Show("Вы успешно авторизовались!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                if (m_main_form == null || m_main_form.IsDisposed)
                {
                    m_main_form = new MainForm(); // создаём форму с главным меню
                    m_main_form.Owner = this;
                }
                else
                {
                    m_main_form.LoadForm();
                }

                m_main_form.IsRegistration = false;
                m_main_form.Nick = nick;

                m_main_form.Show();
            }
        }
    }
}
