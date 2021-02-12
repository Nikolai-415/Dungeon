using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Зелье</summary>
    [Serializable]
    public class DungeonItemPotion : DungeonItemEquipment
    {
        /// <summary>Создаёт зелье</summary>
        /// <param name="quality_id">ID качества (0-2)</param>
        /// <param name="style_id">ID стиля (0-2)</param>
        /// <param name="name">Название зелья</param>
        /// <param name="description">Описание зелья</param>
        public DungeonItemPotion(int quality_id = 0, int style_id = 0, string name = "Зелье", string description = "Обычное зелье. Увеличивает характеристики на некоторое время.")
            : base(Properties.Resources.error, true, true, true, name, description)
        {
            SetImagesFromTexture(quality_id, style_id);
        }

        /// <summary>Задаёт изображение зелья из текстуры</summary>
        /// <param name="quality_id">ID качества (0-2)</param>
        /// <param name="style_id">ID стиля (0-2)</param>
        private void SetImagesFromTexture(int quality_id = 0, int style_id = 0)
        {
            const int potion_width = 20;
            const int potion_height = 20;

            if (quality_id < 0) quality_id = 0;
            else if (quality_id >= 3) quality_id = 2;

            if (style_id < 0) style_id = 0;
            else if (style_id >= 3) style_id = 2;

            Bitmap texture = Properties.Resources.TEXTURE_item_potions;

            Bitmap new_image = new Bitmap(potion_width, potion_height);

            for (int i = potion_height * style_id; i < potion_height * style_id + potion_height; i++)
            {
                for (int i2 = potion_width * quality_id; i2 < potion_width * quality_id + potion_width; i2++)
                {
                    Color pixel_color = texture.GetPixel(i2, i);
                    new_image.SetPixel(i2 - potion_width * quality_id, i - potion_height * style_id, pixel_color);
                }
            }

            Image = new_image;
        }
    }
}