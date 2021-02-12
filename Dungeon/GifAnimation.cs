using System.Collections.Generic;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Класс для создания GIF</summary>
    public class GifAnimation
    {
        /// <summary>Список кадров</summary>
        private List<Bitmap> m_images;

        /// <summary>Создаёт GIF</summary>
        public GifAnimation()
        {
            m_images = new List<Bitmap>();
        }

        /// <summary>Добавляет изображение как кадр в GIF</summary>
        /// <param name="image">Добавляемое изображение</param>
        public void AddFrame(Bitmap image)
        {
            m_images.Add(image);
        }

        /// <summary>Возвращает изображение указанного кадра GIF</summary>
        /// <param name="frame_id">ID кадра</param>
        /// <returns>Изображение</returns>
        public Bitmap GetFrame(int frame_id)
        {
            if (frame_id < 0 || frame_id >= m_images.Count)
            {
                frame_id = 0;
            }
            return m_images[frame_id];
        }
    }
}
