using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Объект подземелья</summary>
    [Serializable]
    public class DungeonObject : DungeonOwner
    {
        /// <summary>Текущее изображение объекта</summary>
        protected Bitmap m_image;

        /// <summary>Текущее изображение объекта</summary>
        public Bitmap Image
        {
            get
            {
                return m_image;
            }
            set
            {
                m_image = value;
                m_size = m_image.Size;
                m_collision_size = m_size;
                m_collision_offset = new Point(0, 0);
            }
        }

        /// <summary>Размер области, в которой отображается объект</summary>
        protected Size m_size;

        /// <summary>Возвращает размер области, в которой находится объект</summary>
        public Size Size
        {
            get
            {
                return m_size;
            }
        }

        /// <summary>Тип столкновения с данным объектом</summary>
        protected DungeonObjectCollision m_collision_type;

        /// <summary>Тип столкновения с данным объектом</summary>
        public DungeonObjectCollision CollisionType
        {
            get
            {
                return m_collision_type;
            }
        }

        /// <summary>Размер области столкновения с данным объектом</summary>
        protected Size m_collision_size;

        /// <summary>Размер области столкновения с данным объектом</summary>
        public Size CollisionSize
        {
            get
            {
                return m_collision_size;
            }
        }

        /// <summary>Смещение области столкновения с данным объектом</summary>
        protected Point m_collision_offset;

        /// <summary>Смещение области столкновения с данным объектом</summary>
        public Point CollisionOffset
        {
            get
            {
                return m_collision_offset;
            }
        }

        /// <summary>Уровень подземелья, на котором расположен объект</summary>
        public DungeonLevel DungeonLevel;

        /// <summary>Координаты (центр) объекта</summary>
        private DungeonPoint m_location;

        /// <summary>Координаты (центр) объекта</summary>
        public DungeonPoint Location
        {
            get
            {
                return m_location;
            }
            set
            {
                m_location = value;
                if (this is DungeonHero)
                {
                    if (DungeonLevel != null)
                    {
                        DungeonLevel.FindCells(this as DungeonHero, MainForm.ShowingSize);
                    }
                }
            }
        }

        /// <summary>Статус объекта</summary>
        public DungeonObjectStatus ObjectStatus;

        /// <summary>Создаёт объект</summary>
        /// <param name="image">Изображение объекта</param>
        /// <param name="collision_type">Тип столкновения с данным объектом</param>
        protected DungeonObject(Bitmap image, DungeonObjectCollision collision_type)
            : base()
        {
            m_image = image;
            m_size = image.Size;
            m_collision_type = collision_type;
            m_collision_size = m_size;
            m_collision_offset = new Point(0, 0);

            DungeonLevel = null;
            m_location = new DungeonPoint(0, 0);
            ObjectStatus = DungeonObjectStatus.CreatedNotAdded;
        }

        /// <summary>Задаёт координаты (центр) объекта</summary>
        /// <param name="x">X-координата</param>
        /// <param name="y">Y-координата</param>
        public void SetLocation(double x, double y = 0)
        {
            Location = new DungeonPoint(x, y);
        }

        /// <summary>Уничтожает объект (принудительно - даже если нельзя уничтожить)</summary>
        public void Destroy()
        {
            if (ObjectStatus == DungeonObjectStatus.AddedNotDestroyed)
            {
                ObjectStatus = DungeonObjectStatus.Destroyed;
            }
            m_collision_type = DungeonObjectCollision.NoCollision;
            if (this is DungeonItem)
            {
                if ((this as DungeonItem).Container != null)
                {
                    (this as DungeonItem).Container.Remove(this as DungeonItem);
                }
            }
            if (DungeonLevel != null)
            {
                DungeonLevel.Remove(this);
            }
        }

        /// <summary>Рисует объект, проверяя, что он создан, но не уничтожен</summary>
        /// <param name="player_location">Координаты игрока (персонаж игрока находится в центре экрана)</param>
        public bool Paint(object sender, PaintEventArgs e, Point player_location, Size showing_size)
        {
            if (ObjectStatus == DungeonObjectStatus.AddedNotDestroyed)
            {
                Size form_size = ((MainForm)sender).Size;
                Point center_screen = new Point(((MainForm)sender).ClientSize.Width / 2, ((MainForm)sender).ClientSize.Height / 2);
                Point screen_location = new DungeonPoint(Location.X - player_location.X + form_size.Width / 2 - Size.Width / 2, Location.Y - player_location.Y + form_size.Height / 2 - Size.Height / 2).Point;
                if (IsInView(screen_location, center_screen, showing_size))
                {
                    e.Graphics.DrawImage(m_image, screen_location);
                    return true;
                }
            }
            return false;
        }

        /// <summary>Находится ли объект в поле зрения</summary>
        /// <param name="center_screen">Центр экрана (центр поля зрения)</param>
        /// <param name="screen_location">Позиция объекта на экране</param>
        /// <param name="showing_size">Размер поля зрения</param>
        /// <returns>True - если объект в поле зрения, false - если нет</returns>
        protected bool IsInView(Point screen_location, Point center_screen, Size showing_size)
        {
            if ((screen_location.X - Size.Width >= center_screen.X - showing_size.Width / 2) &&
                (screen_location.X + Size.Width <= center_screen.X + showing_size.Width / 2) &&
                (screen_location.Y - Size.Height >= center_screen.Y - showing_size.Height / 2) &&
                (screen_location.Y + Size.Height <= center_screen.Y + showing_size.Height / 2))
            {
                return true;
            }
            return false;
        }
    }
}