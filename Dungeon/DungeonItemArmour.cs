using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Броня</summary>
    [Serializable]
    public class DungeonItemArmour : DungeonItemEquipment
    {
        /// <summary>ID качества (0-9)</summary>
        private int m_quality_id;

        /// <summary>ID стиля (0-2)</summary>
        private int m_style_id;

        /// <summary>Создаёт броню</summary>
        /// <param name="quality_id">ID качества (0-9)</param>
        /// <param name="style_id">ID стиля (0-2)</param>
        /// <param name="name">Название брони</param>
        /// <param name="description">Описание брони</param>
        public DungeonItemArmour(int quality_id = 0, int style_id = 0, string name = "Броня", string description = "Элемент защиты. Встречается часто.  Увеличивает максимальное здоровье, снижает скорость передвижения, может увеличивать или уменьшать регенерацию здоровья.")
            : base(Properties.Resources.error, false, true, true, name, description)
        {
            if (quality_id < 0) quality_id = 0;
            else if (quality_id > 9) quality_id = 9;

            if (style_id < 0) style_id = 0;
            else if (style_id > 2) style_id = 2;

            m_quality_id = quality_id;
            m_style_id = style_id;

            Image = MainForm.ImagesArmours[quality_id, style_id, 1];
        }

        /// <summary>Возвращает изображение брони в заданном направлении</summary>
        /// <param name="direction">Направление, в которое направлено существо</param>
        /// <returns>Изображение</returns>
        public Bitmap GetImageOnDirection(DungeonCreatureMoveDirection direction)
        {
            return MainForm.ImagesArmours[m_quality_id, m_style_id, (int)direction];
        }
    }
}