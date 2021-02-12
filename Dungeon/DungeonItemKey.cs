using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Ключ от двери</summary>
    [Serializable]
    public class DungeonItemKey : DungeonItem
    {
        /// <summary>Счётчик ключей</summary>
        private static int colors_count = -1;

        /// <summary>Цвет ключа</summary>
        private Color m_color;

        /// <summary>Цвет ключа</summary>
        public Color Color
        {
            get
            {
                return m_color;
            }
        }

        /// <summary>Создаёт ключ от двери</summary>
        /// <param name="level_id">Id уровня подземелья</param>
        /// <param name="color">Цвет ключа</param>
        /// <param name="name">Название ключа</param>
        /// <param name="description">Описание ключа</param>
        public DungeonItemKey(int level_id, Color color, string name = "Ключ", string description = "Открывает дверь того же цвета, что и сам ключ. Ранее хранился у монстра.")
            : base(Properties.Resources.item_key_75px, false, true, false, name, description)
        {
            m_color = color;
            SetImages();
            m_description = "Открывает одну из дверей " + (level_id + 1) + "-го этажа, замок которой того же цвета, что и сам ключ. Ранее хранился у монстра.";
            UpdateFullDescription();
        }

        /// <summary>Устанавливает изображение ключа из текстуры, заменяя зелёный цвет на установленный</summary>
        private void SetImages()
        {
            Bitmap image = new Bitmap(Properties.Resources.item_key_75px);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel_color = image.GetPixel(x, y);
                    if (pixel_color == Color.FromArgb(0, 255, 0))
                    {
                        image.SetPixel(x, y, m_color);
                    }
                }
            }
            Image = image;
        }

        /// <summary>Обновляет полное описание предмета</summary>
        private void UpdateFullDescription()
        {
            m_full_description = m_description;
        }

        /// <summary>Сбрасывает счётчик ключей</summary>
        public static void ResetColorsCount()
        {
            colors_count = -1;
        }

        /// <summary>Возвращает следующий цвет по счётчику цветов</summary>
        /// <param name="world_key">Ключ генерации мира</param>
        /// <returns>Цвет</returns>
        public static Color GetNextColor(ref Random world_key)
        {
            colors_count++;
            if (colors_count == 0) return Color.Green;
            else if (colors_count == 1) return Color.Blue;
            else if (colors_count == 2) return Color.White;
            else if (colors_count == 3) return Color.Aqua;
            else if (colors_count == 4) return Color.Purple;
            else if (colors_count == 5) return Color.Maroon;
            else if (colors_count == 6) return Color.Lime;
            else if (colors_count == 7) return Color.Teal;
            else if (colors_count == 8) return Color.Olive;
            else if (colors_count == 9) return Color.Gray;
            else return Color.FromArgb(world_key.Next(0, 256), world_key.Next(0, 256), world_key.Next(0, 256));
        }
    }
}