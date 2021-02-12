using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Меч</summary>
    [Serializable]
    public class DungeonItemSword : DungeonItemEquipment
    {
        /// <summary>ID качества (0-29)</summary>
        private int m_quality_id;

        /// <summary>Текущий кадр анимации удара</summary>
        public int TotalFrame;

        /// <summary>Создаёт меч</summary>
        /// <param name="quality_id">ID качества (0-29)</param>
        /// <param name="name">Название меча</param>
        /// <param name="description">Описание меча</param>
        public DungeonItemSword(int quality_id = 0, string name = "Меч", string description = "Холодное оружие. Встречается очень часто. Увеличивает силу, может уменьшает ловкость.")
            : base(Properties.Resources.error, false, true, true, name, description)
        {
            if (quality_id < 0) quality_id = 0;
            else if (quality_id > 29) quality_id = 29;

            m_quality_id = quality_id;

            Image = MainForm.ImagesSwords[quality_id];

            TotalFrame = 0;
        }

        /// <summary>Возвращает изображение меча в заданном направлении по текущему кадру анимации</summary>
        /// <param name="direction">Направление, в которое направлен меч</param>
        /// <returns>Изображение</returns>
        public Bitmap GetImageOnDirection(DungeonCreatureMoveDirection direction)
        {
            return MainForm.ImagesSwordsGif[m_quality_id, (int)direction].GetFrame(TotalFrame);
        }
    }
}
