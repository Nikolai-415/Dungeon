using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Шлем</summary>
    [Serializable]
    public class DungeonItemHelmet : DungeonItemEquipment
    {
        /// <summary>ID качества (0-9)</summary>
        private int m_quality_id;

        /// <summary>ID стиля (0-2)</summary>
        private int m_style_id;

        /// <summary>Создаёт шлем</summary>
        /// <param name="quality_id">ID качества (0-9)</param>
        /// <param name="style_id">ID стиля (0-2)</param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public DungeonItemHelmet(int quality_id = 0, int style_id = 0, string name = "Шлем", string description = "Элемент защиты. Встречается часто. Увеличивает максимальный запас энергии, снижает скорость передвижения, может увеличивать или уменьшать восстановление энергии.")
            : base(Properties.Resources.error, false, true, true, name, description)
        {
            if (quality_id < 0) quality_id = 0;
            else if (quality_id > 9) quality_id = 9;

            if (style_id < 0) style_id = 0;
            else if (style_id > 2) style_id = 2;

            m_quality_id = quality_id;
            m_style_id = style_id;

            Image = MainForm.ImagesHelmets[quality_id, style_id, 4];
        }

        /// <summary>Возвращает изображение шлема в заданном направлении</summary>
        /// <param name="direction">Направление, в которое направлено существо</param>
        /// <returns>Изображение</returns>
        public Bitmap GetImageOnDirection(DungeonCreatureMoveDirection direction)
        {
            return MainForm.ImagesHelmets[m_quality_id, m_style_id, (int)direction];
        }
    }
}
