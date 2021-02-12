using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dungeon
{
    /// <summary>Класс для работы с GIF</summary>
    [Serializable]
    public class GifImage
    {
        /// <summary>Изображение</summary>
        private Bitmap m_image;

        /// <summary>Настройки границ кадров изображения</summary>
        private FrameDimension m_dimension;

        /// <summary>Количество кадров</summary>
        private int m_frames_count;

        /// <summary>Количество кадров</summary>
        public int FramesCount
        {
            get
            {
                return m_frames_count;
            }
        }

        /// <summary>ID текущего кадра</summary>
        private int m_current_frame;

        /// <summary>ID текущего кадра</summary>
        public int CurrentFrame
        {
            get
            {
                return m_current_frame;
            }
        }

        /// <summary>Создаёт GIF</summary>
        /// <param name="image">Изображение</param>
        public GifImage(Bitmap image)
        {
            m_image = image;
            m_dimension = new FrameDimension(m_image.FrameDimensionsList[0]); // получение GUID
            m_frames_count = m_image.GetFrameCount(m_dimension);
            m_current_frame = -1;
        }

        /// <summary>Возвращает изображение следующего кадра GIF</summary>
        /// <returns>Изображение</returns>
        public Bitmap GetNextFrame()
        {
            m_current_frame++;

            if (m_current_frame >= m_frames_count)
            {
                m_current_frame = 0;
            }
            return GetFrame(m_current_frame);
        }

        /// <summary>Возвращает изображение указанного кадра GIF</summary>
        /// <param name="frame_id">ID кадра</param>
        /// <returns>Изображение</returns>
        public Bitmap GetFrame(int frame_id)
        {
            m_image.SelectActiveFrame(m_dimension, frame_id); // нахождение кадра
            return m_image; // возвращение копии кадра
        }
    }
}
