using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Дверь</summary>
    [Serializable]
    public class DungeonDoor : DungeonObject
    {
        /// <summary>Изображение открытой двери</summary>
        private Bitmap m_image_opened;

        /// <summary>Изображение закрытой двери</summary>
        private Bitmap m_image_closed;

        /// <summary>Код от двери (если дверь с кодовым замком)</summary>
        private string m_code;

        /// <summary>Код от двери (если дверь с кодовым замком)</summary>
        public string Code
        {
            get
            {
                return m_code;
            }
        }

        /// <summary>Является ли дверь дверью с кодовым замком</summary>
        public bool IsCode
        {
            get
            {
                if (m_code == null) return false;
                else return true;
            }
        }

        /// <summary>Записки, содержащие подсказки к коду (если дверь с кодовым замком)</summary>
        private List<DungeonItemPaper> m_papers;

        /// <summary>Ключ от двери (если дверь обычная)</summary>
        public DungeonItemKey Key;

        /// <summary>Открыта ли дверь</summary>
        private bool m_is_opened;

        /// <summary>Создаёт дверь</summary>
        /// <param name="door_image_type">Тип изображения двери</param>
        private DungeonDoor(DungeonDoorImageType door_image_type)
            : base(Properties.Resources.error, DungeonObjectCollision.StaticCollision)
        {
            m_is_opened = false;
            m_collision_type = DungeonObjectCollision.StaticCollision;
        }

        /// <summary>Создаёт дверь с кодовым замком</summary>
        /// <param name="door_image_type">Тип изображения двери</param>
        /// <param name="code">Код от двери</param>
        /// <param name="papers">Записки, содержащие подсказки к коду</param>
        public DungeonDoor(DungeonDoorImageType door_image_type, string code, List<DungeonItemPaper> papers)
            : this(door_image_type)
        {
            m_code = code;
            Key = null;
            m_papers = new List<DungeonItemPaper>();
            for (int i = 0; i < 4; i++)
            {
                m_papers.Add(papers[i]);
            }
            SetImages(door_image_type, Color.Yellow);
        }

        /// <summary>Создаёт обычную дверь</summary>
        /// <param name="door_image_type">Тип изображения двери</param>
        /// <param name="key">Ключ от двери</param>
        public DungeonDoor(DungeonDoorImageType door_image_type, DungeonItemKey key)
            : this(door_image_type)
        {
            m_code = null;
            Key = key;
            SetImages(door_image_type, Key.Color);
        }

        /// <summary>Устанавливает изображение двери из текстуры, заменяя зелёный цвет на указанный</summary>
        /// <param name="door_image_type">Тип изображения двери</param>
        /// <param name="color">Цвет</param>
        private void SetImages(DungeonDoorImageType door_image_type, Color color)
        {
            // изображение закрытой двери
            Bitmap image_closed;
            if (door_image_type == DungeonDoorImageType.Horizontal)
            {
                if (IsCode) image_closed = new Bitmap(Properties.Resources.door_code_h_closed_100px);
                else image_closed = new Bitmap(Properties.Resources.door_h_closed_100px);
            }
            else
            {
                if (IsCode) image_closed = new Bitmap(Properties.Resources.door_code_v_closed_100px);
                else image_closed = new Bitmap(Properties.Resources.door_v_closed_100px);
            }
            for (int y = 0; y < image_closed.Height; y++)
            {
                for (int x = 0; x < image_closed.Width; x++)
                {
                    Color pixel_color = image_closed.GetPixel(x, y);
                    if (pixel_color.R == 0 && pixel_color.G == 255 && pixel_color.B == 0)
                    {
                        image_closed.SetPixel(x, y, color);
                    }
                }
            }
            m_image_closed = image_closed;
            Image = m_image_closed;

            // изображение открытой двери
            Color m_color_alpha = Color.FromArgb(128, color);
            Bitmap image_opened;
            if (door_image_type == DungeonDoorImageType.Horizontal)
            {
                if (IsCode) image_opened = new Bitmap(Properties.Resources.door_code_h_opened_100px);
                else image_opened = new Bitmap(Properties.Resources.door_h_opened_100px);
            }
            else
            {
                if (IsCode) image_opened = new Bitmap(Properties.Resources.door_code_v_opened_100px);
                else image_opened = new Bitmap(Properties.Resources.door_v_opened_100px);
            }
            for (int y = 0; y < image_opened.Height; y++)
            {
                for (int x = 0; x < image_opened.Width; x++)
                {
                    Color pixel_color = image_opened.GetPixel(x, y);
                    if (pixel_color.R == 0 && pixel_color.G == 255 && pixel_color.B == 0)
                    {
                        image_opened.SetPixel(x, y, m_color_alpha);
                    }
                }
            }
            m_image_opened = image_opened;
        }

        /// <summary>Открывает дверь</summary>
        public void Open()
        {
            if (!m_is_opened)
            {
                m_is_opened = true;
                m_collision_type = DungeonObjectCollision.NoCollision;
                Image = m_image_opened;
                if (IsCode)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        m_papers[i].Destroy();
                    }
                    m_papers = null;
                }
                else
                {
                    Key.Destroy();
                    Key = null;
                }
            }
        }
    }
}
