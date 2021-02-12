using System;
using System.IO;
using System.Windows.Media;

namespace Dungeon
{
    /// <summary>Аудио-эффект</summary>
    public class AudioEffect
    {
        /// <summary>Текущее количество созданных аудио-эффектов</summary>
        private static int audio_effects_number = 0;

        /// <summary>Проигрыватель аудио-эффекта</summary>
        private MediaPlayer m_media_player;

        /// <summary>Путь к временному файлу аудио-эффекта</summary>
        private string m_temp_file_path;

        /// <summary>Повторяется ли аудио-эффект</summary>
        private bool m_is_repeat;

        /// <summary>Закончил ли проигрывание аудио-эффект</summary>
        private bool m_is_ended;

        /// <summary>Закончил ли проигрывание аудио-эффект</summary>
        public bool IsEnded
        {
            get
            {
                return m_is_ended;
            }
        }

        /// <summary>Приостановлено ли проигрывание аудио-эффекта</summary>
        private bool m_is_paused;

        /// <summary>Приостановлено ли проигрывание аудио-эффекта</summary>
        public bool IsPaused
        {
            get
            {
                return m_is_paused;
            }
        }

        /// <summary>Создаёт аудио-эффект</summary>
        /// <param name="resource">Ресурс (WAV файл)</param>
        /// <param name="is_music">Является ли аудио-эффект музыкой</param>
        /// <param name="is_repeat">Повторяется ли аудио-эффект по окончанию</param>
        public AudioEffect(UnmanagedMemoryStream resource, bool is_music, bool is_repeat = false)
        {
            m_is_repeat = is_repeat;
            m_media_player = new MediaPlayer();
            m_media_player.MediaOpened += media_player_MediaOpened;
            m_media_player.MediaEnded += media_player_MediaEnded;
            ExtractAudioResource(resource);
            m_media_player.Open(new Uri(m_temp_file_path));
            if (is_music)
            {
                SetVolume(MainForm.VolumeMusic);
            }
            else
            {
                SetVolume(MainForm.VolumeSound);
            }
            m_is_ended = false;
            m_is_paused = false;
            audio_effects_number++;
        }

        /// <summary>Создаёт аудио-эффект из уже существующего</summary>
        /// <param name="audio_effect">Аудио-эффект</param>
        public AudioEffect(AudioEffect audio_effect)
        {
            m_is_repeat = audio_effect.m_is_repeat;
            m_media_player = audio_effect.m_media_player;
            m_media_player.Position = TimeSpan.FromSeconds(0);
            SetVolume(MainForm.VolumeSound);
            m_is_ended = false;
            m_is_paused = false;
            audio_effects_number++;
        }

        /// <summary>Событие после загрузки аудио-эффекта</summary>
        private void media_player_MediaOpened(object sender, EventArgs e)
        {
            File.Delete(m_temp_file_path); // удаляем временный файл
        }

        /// <summary>Событие окончания проигрывания аудио-эффекта</summary>
        private void media_player_MediaEnded(object sender, EventArgs e)
        {
            if (m_is_repeat)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }

        /// <summary>Создаёт копию ресурса во временной папке компьютера и присваивает значение переменной m_temp_file_path</summary>
        /// <param name="resource">Ресурс (WAV файл)</param>
        private void ExtractAudioResource(UnmanagedMemoryStream resource)
        {
            Random random = new Random();
            m_temp_file_path = Path.GetTempPath() + "DungeonTemp_" + audio_effects_number+ "_" + random.Next(0, 1000000) + ".mp3";
            Stream ReadResource = resource;
            byte[] buffer = new byte[ReadResource.Length];
            using (ReadResource)
            {
                ReadResource.Read(buffer, 0, (int)ReadResource.Length);
            }
            File.WriteAllBytes(m_temp_file_path, buffer);
        }

        /// <summary>Устанавливает громкость аудио-эффекта</summary>
        /// <param name="volume">Громкость (0-100)</param>
        public void SetVolume(byte volume)
        {
            m_media_player.Volume = (double)volume / 100;
        }

        /// <summary>Начинает / продолжает проигрывание аудио-эффекта</summary>
        public void Play()
        {
            m_is_paused = false;
            m_media_player.Play();
        }

        /// <summary>Заканчивает проигрывание аудио-эффекта</summary>
        public void Stop()
        {
            m_media_player.Stop();
            m_is_ended = true;
        }

        /// <summary>Приостанавливает проигрывание аудио-эффекта</summary>
        public void Pause()
        {
            m_is_paused = true;
            m_media_player.Pause();
        }
    }
}
