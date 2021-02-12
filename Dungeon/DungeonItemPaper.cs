using System;

namespace Dungeon
{
    /// <summary>Записка</summary>
    [Serializable]
    public class DungeonItemPaper : DungeonItem
    {
        /// <summary>Текст записки</summary>
        private string m_text;

        /// <summary>Создаёт записку</summary>
        /// <param name="level_id">Id уровня подземелья</param>
        /// <param name="text">Текст записки</param>
        /// <param name="name">Название записки</param>
        /// <param name="description">Описание записки</param>
        public DungeonItemPaper(int level_id, string text = "", string name = "Записка", string description = "")
            : base(Properties.Resources.paper, false, true, false, name, description)
        {
            m_text = text;
            m_description = "Кусочек старой бумаги, найденный в одном из сундуков " + (level_id + 1) + "-го этажа.\n\nВ записке указано: ";
            UpdateFullDescription();
        }

        /// <summary>Обновляет полное описание предмета</summary>
        private void UpdateFullDescription()
        {
            m_full_description = m_description + m_text;
        }

        /// <summary>Записывает число как часть кода в записку</summary>
        /// <param name="world_key">Ключ генерации мира</param>
        /// <param name="number_char">Число</param>
        /// <param name="number_position">Позиция среди других чисел</param>
        public void GenerateTextWithNumber(ref Random world_key, byte number_char, byte number_position = 0)
        {
            m_text = "";
            for (int i = 0; i < number_position; i++)
            {
                m_text += "*";
            }
            m_text += number_char.ToString();
            for (int i = number_position + 1; i < 4; i++)
            {
                m_text += "*";
            }
            m_text += ".";
            UpdateFullDescription();
        }
    }
}
