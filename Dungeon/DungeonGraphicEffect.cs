using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Графический эффект</summary>
    [Serializable]
    public class DungeonGraphicEffect : DungeonObject
    {
        /// <summary>GIF</summary>
        private GifImage m_gif_image;

        /// <summary>Запущен ли таймер анимации</summary>
        private bool m_timer_play_is_working;

        /// <summary>Таймер анимации</summary>
        [NonSerialized]
        private Timer m_timer_play;

        /// <summary>Создаёт графический эффект на уровне</summary>
        /// <param name="dungeon_level">Уровень подземелья</param>
        /// <param name="gif_image">GIF</param>
        /// <param name="x">Координата X (центр)</param>
        /// <param name="y">Координата Y (центр)</param>
        public DungeonGraphicEffect(DungeonLevel dungeon_level, GifImage gif_image, int x = 0, int y = 0)
            : base(new Bitmap(gif_image.GetFrame(0)), DungeonObjectCollision.NoCollision)
        {
            DungeonLevel = dungeon_level;
            m_gif_image = gif_image;
            SetLocation(x, y);

            m_timer_play_is_working = true;
            InitializeTimers();
        }

        /// <summary>Инициализирует таймеры</summary>
        public void InitializeTimers()
        {
            m_timer_play = new Timer();
            m_timer_play.Tick += timer_play_Tick;
            m_timer_play.Interval = 10;
            if (m_timer_play_is_working)
            {
                Play();
            }
        }

        /// <summary>Запускает анимацию графического эффекта</summary>
        private void Play()
        {
            m_timer_play.Start();
            m_timer_play_is_working = true;
            ObjectStatus = DungeonObjectStatus.AddedNotDestroyed;
            DungeonLevel.Add(this);
        }

        /// <summary>Останавливает анимацию графического эффекта</summary>
        private void Stop()
        {
            m_timer_play.Stop();
            m_timer_play_is_working = false;
            ObjectStatus = DungeonObjectStatus.Destroyed;
            Destroy();
        }

        /// <summary>Событие таймера анимации</summary>
        private void timer_play_Tick(object sender, EventArgs e)
        {
            if (m_gif_image.CurrentFrame + 1 >= m_gif_image.FramesCount) // если достигнут конец GIF
            {
                Stop();
            }
            else
            {
                Image = new Bitmap(m_gif_image.GetNextFrame());
            }
        }
    }
}
