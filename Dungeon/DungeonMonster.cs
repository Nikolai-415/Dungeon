using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Монстр подземелья</summary>
    [Serializable]
    public class DungeonMonster : DungeonCreature
    {
        /// <summary>Время размышления монстра</summary>
        private static readonly int thinking_time = 1500;

        /// <summary>Является ли монстр боссом</summary>
        private bool m_is_boss;

        /// <summary>Является ли монстр боссом</summary>
        public bool IsBoss
        {
            get
            {
                return m_is_boss;
            }
        }

        /// <summary>Текущее действие монстра</summary>
        private DungeoMonsterActionStatus m_action_status;

        /// <summary>Текущее действие монстра</summary>
        public DungeoMonsterActionStatus ActionStatus
        {
            get
            {
                return m_action_status;
            }
        }

        /// <summary>Привязан ли монстр к комнате</summary>
        private bool m_is_attached_to_room;

        /// <summary>Привязан ли монстр к комнате</summary>
        public bool IsAttachedToRoom
        {
            get
            {
                return m_is_attached_to_room;
            }
        }

        /// <summary>Точка соединения карты, к которой привязан монстр</summary>
        private Point m_connection_point_attached;

        /// <summary>Точка соединения карты, к которой привязан монстр</summary>
        public Point ConnectionPointAttached
        {
            get
            {
                return m_connection_point_attached;
            }
        }

        /// <summary>Целевые координаты (координаты, к которым монстр будет двигаться)</summary>
        private Point m_target_location;

        /// <summary>Запущен ли таймер размышления (интервала между движениями) монстра</summary>
        private bool m_thinking_timer_is_working;

        /// <summary>Таймер размышления (интервала между движениями) монстра</summary>
        [NonSerialized]
        private Timer m_thinking_timer;

        /// <summary>Запущен ли таймер движения монстра</summary>
        private bool m_moving_timer_is_working;

        /// <summary>Таймер движения монстра</summary>
        [NonSerialized]
        private Timer m_moving_timer;

        /// <summary>Создаёт монстра</summary>
        /// <param name="texture">Текстура монстра</param>
        /// <param name="name">Имя монстра</param>
        /// <param name="is_boss">Является ли монстр боссом</param>
        /// <param name="max_health">Здоровье монстра</param>
        /// <param name="regeneration">Регенерация (восстановление здоровья) монстра</param>
        /// <param name="max_energy">Энергия монстра</param>
        /// <param name="restore">Восстановление (возобновление энергии) монстра</param>
        /// <param name="power">Сила монстра</param>
        /// <param name="mobility">Ловкость монстра</param>
        /// <param name="speed">Скорость монстра</param>
        public DungeonMonster(Bitmap texture, string name = "Монстр", bool is_boss = false, double max_health = 50, double regeneration = 0, double max_energy = 25, double restore = 0.5, double power = 5, double mobility = 0, double speed = 1)
            : base(texture, name, max_health, regeneration, max_energy, restore, power, mobility, speed, 0, 0)
        {
            m_is_boss = is_boss;
            m_is_attached_to_room = false;

            m_action_status = DungeoMonsterActionStatus.AFK;

            m_thinking_timer_is_working = false;
            m_moving_timer_is_working = false;
            InitializeTimers();
        }

        /// <summary>Инициализирует таймеры</summary>
        new public void InitializeTimers()
        {
            m_thinking_timer = new Timer();
            m_thinking_timer.Interval = thinking_time;
            m_thinking_timer.Tick += m_thinking_timer_Tick;
            if (m_thinking_timer_is_working)
            {
                m_thinking_timer.Start();
                m_thinking_timer_is_working = true;
            }

            m_moving_timer = new Timer();
            Random random = new Random();
            m_moving_timer.Interval = 100 * random.Next(10, 51);
            m_moving_timer.Tick += m_moving_timer_Tick;
            if (m_moving_timer_is_working)
            {
                m_moving_timer.Start();
                m_moving_timer_is_working = true;
            }
        }

        /// <summary>Распределяет все очки скилла монстра</summary>
        /// <param name="world_key">Ключ генерации мира</param>
        public void DistributeAllSkillPoints(ref Random world_key)
        {
            while (SkillPoints > 0)
            {
                DungeonStats stat = (DungeonStats)(world_key.Next(0, 10));
                while (stat == DungeonStats.Intelligence || stat == DungeonStats.Luck) // монстры не имеют этих характеристик
                {
                    stat = (DungeonStats)(world_key.Next(0, 10));
                }
                UpStatValue(stat);
            }
        }

        /// <summary>Выдаёт монстру ключ от двери</summary>
        /// <param name="key">Ключ от двери</param>
        public void GiveKey(DungeonItemKey key)
        {
            Container.Add(key);
            if (!m_is_boss)
            {
                m_name = "Хранитель ключа";
            }
        }

        /// <summary>Привязывает монстра к точке соединения карты</summary>
        /// <param name="connection_point_location"></param>
        public void AttachToConnectionPoint(Point connection_point_location)
        {
            m_connection_point_attached = connection_point_location;
            m_is_attached_to_room = true;
        }

        /// <summary>Устанавливает целевые координаты</summary>
        /// <param name="target_location">Координаты</param>
        public void SetTargetLocation(Point target_location)
        {
            m_target_location = target_location;
        }

        /// <summary>Передвигает монстра на один шаг к целевым координатам</summary>
        public void MakeStepToTargetLocation()
        {
            if (DungeonLevel.GetCellLocationFromGlobalLocation(Location.Point) == DungeonLevel.GetCellLocationFromGlobalLocation(m_target_location) &&
                m_action_status != DungeoMonsterActionStatus.Fighting)
            {
                StopThinking();
                StartThinking();
            }

            DungeonCreatureMoveDirection move_direction = DungeonCreatureMoveDirection.Right;
            double length_y = Math.Abs(Location.Y - m_target_location.Y);
            double length_x = Math.Abs(Location.X - m_target_location.X);
            bool is_move = false;
            if (length_y > length_x)
            {
                if (Location.Y > m_target_location.Y)
                {
                    move_direction = DungeonCreatureMoveDirection.Up;
                    is_move = true;
                }
                else if (Location.Y < m_target_location.Y)
                {
                    move_direction = DungeonCreatureMoveDirection.Down;
                    is_move = true;
                }
                else if (Location.X > m_target_location.X)
                {
                    move_direction = DungeonCreatureMoveDirection.Left;
                    is_move = true;
                }
                else if (Location.X < m_target_location.X)
                {
                    move_direction = DungeonCreatureMoveDirection.Right;
                    is_move = true;
                }
            }
            else
            {
                if (Location.X > m_target_location.X)
                {
                    move_direction = DungeonCreatureMoveDirection.Left;
                    is_move = true;
                }
                else if (Location.X < m_target_location.X)
                {
                    move_direction = DungeonCreatureMoveDirection.Right;
                    is_move = true;
                }
                else if (Location.Y > m_target_location.Y)
                {
                    move_direction = DungeonCreatureMoveDirection.Up;
                    is_move = true;
                }
                else if (Location.Y < m_target_location.Y)
                {
                    move_direction = DungeonCreatureMoveDirection.Down;
                    is_move = true;
                }
            }
            if (is_move)
            {
                if (Math.Abs(length_y - length_x) < GetStatValueWithEffectValue(DungeonStats.Speed) * 2) // если движение по диагонали
                {
                    Move(move_direction, true);
                }
                else
                {
                    Move(move_direction);
                }
            }
        }

        /// <summary>Монстр переходит в состояние размышления</summary>
        public void StartThinking()
        {
            if (m_action_status != DungeoMonsterActionStatus.Thinking)
            {
                m_action_status = DungeoMonsterActionStatus.Thinking;
                m_thinking_timer.Start();
                m_thinking_timer_is_working = true;
            }
        }

        /// <summary>Монстр переходит в нулевое состояние</summary>
        public void StopThinking()
        {
            if (m_action_status != DungeoMonsterActionStatus.AFK)
            {
                m_action_status = DungeoMonsterActionStatus.AFK;
                m_thinking_timer.Stop();
                m_thinking_timer_is_working = false;
                m_moving_timer.Stop();
                m_moving_timer_is_working = false;
            }
        }

        /// <summary>Монстр переходит в состояние сражения с героем</summary>
        public void StartFighting()
        {
            m_action_status = DungeoMonsterActionStatus.Fighting;
        }

        /// <summary>Событие таймера размышления</summary>
        private void m_thinking_timer_Tick(object sender, EventArgs e)
        {
            if (DungeonLevel != DungeonLevel.Form.Hero.DungeonLevel)
            {
                return;
            }

            Point total_connection_point_where_is_monster;
            if (m_is_attached_to_room)
            {
                total_connection_point_where_is_monster = m_connection_point_attached;
            }
            else
            {
                total_connection_point_where_is_monster = DungeonLevel.GetConnectionPointLocationFromGlobalLocation(Location.Point);
            }

            m_target_location = DungeonLevel.GetGlobalLocationRandomCellInConnectionPointNotWall(total_connection_point_where_is_monster);

            m_action_status = DungeoMonsterActionStatus.MovingToCell;

            m_thinking_timer.Stop();
            m_thinking_timer_is_working = false;

            Random random = new Random();
            m_moving_timer.Interval = 100 * random.Next(10, 51);
            m_moving_timer.Start();
            m_moving_timer_is_working = true;
        }

        /// <summary>Событие таймера движения</summary>
        private void m_moving_timer_Tick(object sender, EventArgs e)
        {
            m_moving_timer.Stop();
            m_moving_timer_is_working = false;
            StartThinking();
        }

        /// <summary>Находится ли монстр в области вокруг героя</summary>
        /// <param name="hero">Герой</param>
        /// <param name="showing_size">Размер области</param>
        /// <returns>True - если монстр попадает в область, false - если монстр вне области</returns>
        public bool IsInPlayerView(DungeonHero hero, Size showing_size)
        {
            if (DungeonLevel == hero.DungeonLevel || m_action_status == DungeoMonsterActionStatus.Fighting)
            {
                Point calculating_location = new Point(hero.Location.Point.X - showing_size.Width / 2, hero.Location.Point.Y - showing_size.Height / 2);
                Rectangle calculating_size_rectangle = new Rectangle(calculating_location, showing_size);
                if (calculating_size_rectangle.Contains(Location.Point))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
