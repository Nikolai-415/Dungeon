using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Предмет экипировки (может содержать эффекты, влияющие на характеристики)</summary>
    [Serializable]
    public class DungeonItemEquipment : DungeonItem
    {
        /// <summary>Список эффектов</summary>
        private List<DungeonEffect> m_effects;

        /// <summary>Список эффектов</summary>
        public List<DungeonEffect> Effects
        {
            get
            {
                return m_effects;
            }
        }

        /// <summary>Текущее количество эффектов</summary>
        private int TotalEffectsNumber
        {
            get
            {
                return m_effects.Count;
            }
        }

        /// <summary>Надет ли ли (для шлема, брони, артефакта и меча)</summary>
        private bool m_is_equip;

        /// <summary>Надет ли ли (для шлема, брони, артефакта и меча)</summary>
        public bool IsEquip
        {
            get
            {
                return m_is_equip;
            }
        }

        /// <summary>Создаёт предмет экипировки</summary>
        /// <param name="image">Изображение предмета экипировки (на земле)</param>
        /// <param name="can_use">Можно ли использовать</param>
        /// <param name="can_drop">Можно ли выбросить</param>
        /// <param name="can_destroy">Можно ли уничтожить</param>
        /// <param name="name">Название предмета экипировки</param>
        /// <param name="description">Описание предмета экипировки</param>
        protected DungeonItemEquipment(Bitmap image, bool can_use, bool can_drop, bool can_destroy, string name, string description)
            : base(image, can_use, can_drop, can_destroy, name, description)
        {
            m_effects = new List<DungeonEffect>();
            m_is_equip = false;
        }

        /// <summary>Изменяет специальный цвет (красный, зелёный или синий) области текстуры на указанный, для указанного id специального цвета (для зелий и артефактов)</summary>
        /// <param name="id">Id специального цвета (0-2)</param>
        /// <param name="color">Цвет, на который необходимо изменить специальный цвет</param>
        private void SetShowingEffectColor(int id, Color color)
        {
            if (id >= 0 && id <= 2)
            {
                Bitmap image = Image;
                for (int i = 0; i < image.Height; i++)
                {
                    for (int i2 = 0; i2 < image.Height; i2++)
                    {
                        Color pixel = image.GetPixel(i2, i);
                        bool check = false;
                        if (id == 0)
                        {
                            if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0) check = true;
                        }
                        else if (id == 1)
                        {
                            if (pixel.R == 0 && pixel.G == 255 && pixel.B == 0) check = true;
                        }
                        else if (id == 2)
                        {
                            if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255) check = true;
                        }
                        if (check)
                        {
                            image.SetPixel(i2, i, color);
                        }
                    }
                }
                Image = image;
            }
        }

        /// <summary>Обновляет описание предмета экипировки, изменяя текст про значения, на которые меняются характеристики</summary>
        private void UpdateFullDescription()
        {
            int effects_number = 0;
            for (int i = 0; i < 9; i++)
            {
                double result_effect_value = GetEffectValue(i);
                if (result_effect_value > 0.005 || result_effect_value < -0.005)
                {
                    effects_number++;
                }
            }
            if (effects_number > 0)
            {
                if (this is DungeonItemPotion)
                {
                    if (effects_number == 1)
                    {
                        m_description = "Обычное зелье. Встречается очень часто. Увеличивает или уменьшает характеристику на некоторое время.";
                    }
                    else if (effects_number == 2)
                    {
                        m_description = "Зелье двойного действия. Встречается часто. Увеличивает или уменьшает две характеристики на некоторое время.";
                    }
                    else if (effects_number == 3)
                    {
                        m_description = "Зелье тройного действия. Встречается редко. Увеличивает или уменьшает три характеристики на некоторое время.";
                    }
                }

                m_full_description = m_description + "\n\nИзменяет характеристики персонажа:";
                for (int i = 0; i < 9; i++)
                {
                    double result_effect_value = GetEffectValue(i);
                    if (result_effect_value > 0.005)
                    {
                        m_full_description += "\n+ " + DungeonStatsInfo.Name((DungeonStats)i) + ": +" + result_effect_value;
                    }
                    else if (result_effect_value < -0.005)
                    {
                        m_full_description += "\n- " + DungeonStatsInfo.Name((DungeonStats)i) + ": " + result_effect_value;
                    }
                }

                if (this is DungeonItemPotion)
                {
                    m_full_description += "\n\nДлительность: " + m_effects[0].Duration + " секунд";
                }
            }
            else
            {
                m_full_description = m_description;
            }
        }

        /// <summary>Добавляет эффект к предмету экипировки</summary>
        /// <param name="stat">Характеристика</param>
        /// <param name="value">Значение изменения</param>
        /// <param name="duration">Длительность эффекта (если бесконечно, то -1)</param>
        public bool AddEffect(DungeonStats stat, double value, int duration = -1)
        {
            for (int i = 0; i < m_effects.Count; i++)
            {
                if (m_effects[i].Stat == stat) // если эффект для этой характеристики уже есть
                {
                    return false;
                }
            }
            if (this is DungeonItemArtifact || this is DungeonItemPotion)
            {
                if (this is DungeonItemPotion && duration == -1) return false;
                if (TotalEffectsNumber < 3 || (this is DungeonItemArtifact && (this as DungeonItemArtifact).IsSpecial))
                {
                    m_effects.Add(new DungeonEffect(stat, value, duration));
                    UpdateFullDescription();
                    if (!(this is DungeonItemArtifact && (this as DungeonItemArtifact).IsSpecial))
                    {
                        SetShowingEffectColor(TotalEffectsNumber - 1, DungeonStatsInfo.GetColor(stat));
                    }
                    return true;
                }
            }
            else
            {
                m_effects.Add(new DungeonEffect(stat, value, duration));
                UpdateFullDescription();
                return true;
            }
            return false;
        }

        /// <summary>Получает значение, на которое изменяется характеристика</summary>
        /// <param name="id_stat">Id характеристики</param>
        /// <returns>Значение изменения</returns>
        public double GetEffectValue(int id_stat)
        {
            for (int i = 0; i < m_effects.Count; i++)
            {
                if ((int)m_effects[i].Stat == id_stat)
                {
                    return m_effects[i].Value;
                }
            }
            return 0;
        }

        /// <summary>Активирует эффекты предмета экипировки на существе</summary>
        /// <param name="creature">Существо</param>
        public void StartEffects(DungeonCreature creature)
        {
            for (int i = 0; i < m_effects.Count; i++)
            {
                m_effects[i].AddToCreature(creature);
                m_effects[i].TurnEffectOn();
            }
            if (this is DungeonItemPotion)
            {
                Destroy();
            }
        }

        /// <summary>Деактивирует эффекты предмета экипировки</summary>
        private void StopEffects()
        {
            for (int i = 0; i < m_effects.Count; i++)
            {
                m_effects[i].TurnEffectOff();
            }
        }

        /// <summary>"Надевает" предмет экипировки на существо</summary>
        /// <param name="creature">Существо</param>
        public void Equip(DungeonCreature creature)
        {
            if (!(this is DungeonItemPotion)) StartEffects(creature);
            m_is_equip = true;
        }

        /// <summary>"Снимает" предмет экипировки</summary>
        public void UnEquip()
        {
            if (!(this is DungeonItemPotion)) StopEffects();
            m_is_equip = false;
        }
    }
}
