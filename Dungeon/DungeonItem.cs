using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Предмет</summary>
    [Serializable]
    public class DungeonItem : DungeonObject
    {
        /// <summary>Можно ли использовать предмет</summary>
        private bool m_can_use;

        /// <summary>Можно ли использовать предмет</summary>
        public bool CanUse
        {
            get
            {
                return m_can_use;
            }
        }

        /// <summary>Можно ли выбросить предмет</summary>
        private bool m_can_drop;

        /// <summary>Можно ли выбросить предмет</summary>
        public bool CanDrop
        {
            get
            {
                return m_can_drop;
            }
        }

        /// <summary>Можно ли уничтожить предмет</summary>
        protected bool m_can_destroy;

        /// <summary>Можно ли уничтожить предмет</summary>
        public bool CanDestroy
        {
            get
            {
                return m_can_destroy;
            }
        }

        /// <summary>Название предмета</summary>
        private string m_name;

        /// <summary>Название предмета</summary>
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>Описание предмета</summary>
        protected string m_description;

        /// <summary>"Полное" предмета (включает дополнительную информацию о предмете)</summary>
        protected string m_full_description;

        /// <summary>"Полное" предмета (включает дополнительную информацию о предмете)</summary>
        public string FullDescription
        {
            get
            {
                return m_full_description;
            }
        }

        /// <summary>Контейнер, в котором хранится предмет</summary>
        public DungeonContainer Container;

        /// <summary>Создаёт предмет</summary>
        /// <param name="image">Изображение предмета</param>
        /// <param name="can_use">Можно ли использовать предмет</param>
        /// <param name="can_drop">Можно ли выбросить предмет</param>
        /// <param name="can_destroy">Можно ли уничтожить предмет</param>
        /// <param name="name">Название предмета</param>
        /// <param name="description">Описание предмета</param>
        protected DungeonItem(Bitmap image, bool can_use, bool can_drop, bool can_destroy, string name, string description)
            : base(image, DungeonObjectCollision.NoCollision)
        {
            m_can_use = can_use;
            m_can_drop = can_drop;
            m_can_destroy = can_destroy;
            m_name = name;
            m_description = description;
            m_full_description = description;

            DungeonLevel = null;
        }
    }
}