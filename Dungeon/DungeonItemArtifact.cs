using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Артефакт</summary>
    [Serializable]
    public class DungeonItemArtifact : DungeonItemEquipment
    {
        /// <summary>Является ли особым артефактом</summary>
        private bool m_is_special;

        /// <summary>Является ли особым артефактом</summary>
        public bool IsSpecial
        {
            get
            {
                return m_is_special;
            }
        }

        /// <summary>Создаёт артефакт</summary>
        /// <param name="quality_id">ID качества (0-30)</param>
        /// <param name="style_id">ID стиля (0-6)</param>
        /// <param name="name">Название артефакта</param>
        /// <param name="description">Описание артефакта</param>
        public DungeonItemArtifact(int quality_id = 0, int style_id = 0, string name = "Артефакт", string description = "Сгусток магической энергии, материализовавшийся в необычной форме. Встречается крайне редко. Может изменять любые характеристики.")
            : base(Properties.Resources.error, false, true, true, name, description)
        {
            m_is_special = false;
            SetImagesFromTexture(quality_id, style_id);
        }

        /// <summary>Задаёт изображение артефакта из текстуры</summary>
        /// <param name="quality_id">ID качества (0-30)</param>
        /// <param name="style_id">ID стиля (0-6)</param>
        private void SetImagesFromTexture(int quality_id = 0, int style_id = 0)
        {
            const int artifact_width = 16;
            const int artifact_height = 16;

            if (quality_id < 0) quality_id = 0;
            else if (quality_id >= 31) quality_id = 30;

            if (style_id < 0) style_id = 0;
            else if (style_id >= 6) style_id = 5;

            if (quality_id == 30) // особый артефакт - хранится у последнего босса
            {
                style_id = 0;
                m_can_destroy = false;
                m_is_special = true;
            }

            Bitmap texture = Properties.Resources.TEXTURE_item_artifacts;

            Bitmap new_image = new Bitmap(artifact_width, artifact_height);

            for (int i = artifact_height * quality_id; i < artifact_height * quality_id + artifact_height; i++)
            {
                for (int i2 = artifact_width * style_id; i2 < artifact_width * style_id + artifact_width; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image.SetPixel(i2 - artifact_width * style_id, i - artifact_height * quality_id, pixel);
                }
            }

            Image = new_image;
        }
    }
}
