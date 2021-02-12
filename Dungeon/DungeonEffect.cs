using System;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Эффект (меняет характеристику существа)</summary>
    [Serializable]
    public class DungeonEffect
    {
        /// <summary>Характеристика, которую изменяет эффект</summary>
        private DungeonStats m_stat;

        /// <summary>Характеристика, которую изменяет эффект</summary>
        public DungeonStats Stat
        {
            get
            {
                return m_stat;
            }
        }

        /// <summary>Значение, на которое эффект изменяет характеристику</summary>
        private double m_value;

        /// <summary>Значение, на которое эффект изменяет характеристику</summary>
        public double Value
        {
            get
            {
                return m_value;
            }
        }

        /// <summary>Длительность действия эффекта (в секундах) (равно -1 в случае бесконечной длительности)</summary>
        private int m_duration;

        /// <summary>Длительность действия эффекта (в секундах) (равно -1 в случае бесконечной длительности)</summary>
        public int Duration
        {
            get
            {
                return m_duration;
            }
        }

        /// <summary>Существо, на которое накладывается эффект</summary>
        private DungeonCreature m_creature;

        /// <summary>Активен ли эффект</summary>
        private bool m_is_active;

        /// <summary>Запущен ли таймер действия эффекта</summary>
        private bool m_timer_usage_is_working;

        /// <summary>Таймер действия эффекта</summary>
        [NonSerialized]
        private Timer m_timer_usage;

        /// <summary>Время, которое эффект уже действует (в миллисекундах)</summary>
        private int m_usage_time;

        /// <summary>Время, которое эффект уже действует (в миллисекундах)</summary>
        public int UsageTime
        {
            get
            {
                return m_usage_time;
            }
        }

        /// <summary>Создаёт эффект</summary>
        /// <param name="stat">Характеристика, которую изменяет эффект</param>
        /// <param name="value">Значение, на которое эффект изменяет характеристику</param>
        /// <param name="duration">Длительность действия эффекта (в секундах) (равно -1 в случае бесконечной длительности)</param>
        public DungeonEffect(DungeonStats stat, double value = 0, int duration = -1)
        {
            m_stat = stat;
            m_value = value;
            m_duration = duration;

            m_creature = null;

            m_is_active = false;
            m_usage_time = m_duration * 1000;

            m_timer_usage_is_working = false;
            InitializeTimers();
        }

        /// <summary>Инициализирует таймеры</summary>
        public void InitializeTimers()
        {
            m_timer_usage = new Timer();
            m_timer_usage.Tick += timer_usage_Tick;
            m_timer_usage.Interval = 100;
            if (m_timer_usage_is_working)
            {
                m_timer_usage.Start();
            }
        }

        /// <summary>Устанавливает владельца эффекта</summary>
        /// <param name="creature">Существо</param>
        public void AddToCreature(DungeonCreature creature)
        {
            m_creature = creature;
        }

        /// <summary>Активирует эффект</summary>
        public void TurnEffectOn()
        {
            if (!m_is_active)
            {
                if (m_creature != null)
                {
                    m_creature.AddEffect(this);
                    if (m_duration != -1)
                    {
                        m_usage_time = m_duration * 1000; // ms
                        m_timer_usage.Start();
                        m_timer_usage_is_working = true;
                    }
                    m_is_active = true;

                    m_creature.DungeonLevel.Form.CalculateInterfaceHeroActivePotions();
                }
                else throw new Exception("Владелец эффекта не задан");
            }
        }

        /// <summary>Деактивирует эффект</summary>
        public void TurnEffectOff()
        {
            if (m_is_active)
            {
                if (m_creature != null)
                {
                    m_creature.RemoveEffect(this);
                    if (m_duration != -1)
                    {
                        m_timer_usage.Stop();
                        m_timer_usage_is_working = false;
                    }
                    m_is_active = false;

                    m_creature.DungeonLevel.Form.CalculateInterfaceHeroActivePotions();

                    m_creature = null;
                }
                else throw new Exception("Владелец эффекта не задан");
            }
        }

        /// <summary>Событие для таймера действия эффекта</summary>
        private void timer_usage_Tick(object sender, EventArgs e)
        {
            if (m_creature != null && m_creature.DungeonLevel != null)
            {
                if (!m_creature.DungeonLevel.Form.IsGamePaused) // если игра не на паузе
                {
                    m_usage_time -= m_timer_usage.Interval;
                    m_creature.DungeonLevel.Form.CalculateInterfaceHeroActivePotions();
                    if (m_usage_time <= 0)
                    {
                        TurnEffectOff();
                    }
                }
            }
        }
    }
}
