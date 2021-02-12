using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Контейнер</summary>
    [Serializable]
    public class DungeonContainer
    {
        /// <summary>Радиус разброса предметов</summary>
        private static readonly int spread_radius = 50;

        /// <summary>Владелец контейнера</summary>
        private DungeonOwner m_owner;

        /// <summary>Лимит предметов в контейнере (равен -1 в случае бесконечного объёма)</summary>
        private int m_limit;

        /// <summary>Cписок предметов контейнера</summary>
        private List<DungeonItem> m_items;

        /// <summary>Cписок предметов в контейнере</summary>
        public List<DungeonItem> Items
        {
            get
            {
                return m_items;
            }
        }

        /// <summary>Создаёт контейнер</summary>
        /// <param name="owner">Владелец контейнера</param>
        /// <param name="limit">Лимит предметов в контейнере (равен -1 в случае бесконечного объёма)</param>
        public DungeonContainer(DungeonOwner owner, int limit = -1)
        {
            m_owner = owner;
            m_items = new List<DungeonItem>();
            m_items.Clear();
            m_limit = limit;
            if (m_limit != -1)
            {
                for (int i = 0; i < limit; i++)
                {
                    m_items.Add(null);
                }
            }
        }

        /// <summary>Помещает указанный предмет в контейнер (из старого контейнера (если есть))</summary>
        /// <param name="item">Предмет</param>
        /// <returns>1 - успешно, 0 - нет места в контейнере, -1 - ошибка</returns>
        public int Add(DungeonItem item)
        {
            if (item != null)
            {
                if (m_limit == -1)
                {
                    DungeonContainer old_container = item.Container;
                    if (old_container != null)
                    {
                        old_container.Remove(item);
                    }

                    if (item is DungeonItemSword)
                    {
                        (item as DungeonItemSword).TotalFrame = 0;
                    }

                    item.Container = this;

                    m_items.Add(item); // добавление предмета в контейнер

                    item.ObjectStatus = DungeonObjectStatus.AddedNotDestroyed;

                    if (m_owner is DungeonLevel)
                    {
                        (m_owner as DungeonLevel).MoveObjectIfInBlocks(item);
                    }

                    return 1;
                }
                else
                {
                    bool can_add = false;
                    int i = 0;
                    for (i = 0; i < 30; i++)
                    {
                        if (m_items[i] == null)
                        {
                            can_add = true;
                            break;
                        }
                    }
                    if (can_add)
                    {
                        Set(item, i);
                        return 1;
                    }
                }
                return 0;
            }
            return -1;
        }

        /// <summary>Помещает указанный предмет в указанную позицию контейнера (из старого контейнера (если есть))</summary>
        /// <param name="item">Предмет</param>
        /// <param name="pos_id">ID позиции в контейнере</param>
        /// <returns>True - в случае успешного добавления, false - в случае ошибки</returns>
        public bool Set(DungeonItem item, int pos_id)
        {
            if (item != null)
            {
                if (pos_id >= 0 && (pos_id < m_limit && m_limit != -1))
                {
                    if (m_items[pos_id] == null)
                    {
                        DungeonContainer old_container = item.Container;
                        if (old_container != null)
                        {
                            old_container.Remove(item);
                        }

                        if (item is DungeonItemSword)
                        {
                            (item as DungeonItemSword).TotalFrame = 0;
                        }

                        item.Container = this;

                        m_items[pos_id] = item;

                        item.ObjectStatus = DungeonObjectStatus.AddedNotDestroyed;

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>Удаляет предмет из контейнера</summary>
        /// <param name="item">Предмет</param>
        public void Remove(DungeonItem item)
        {
            if (item != null)
            {
                if (item.Container == this)
                {
                    item.Container = null;

                    if (m_limit == -1)
                    {
                        m_items.Remove(item);
                    }
                    else
                    {
                        int i = 0;
                        for (i = 0; i < m_limit; i++)
                        {
                            if (m_items[i] == item) break;
                        }
                        m_items[i] = null;
                    }

                    item.ObjectStatus = DungeonObjectStatus.CreatedNotAdded;

                    // экипировка (снятие) предметов - шлема, брони, артефакта, меча
                    if (m_owner is DungeonCreature)
                    {
                        if (item is DungeonItemEquipment)
                        {
                            if ((item as DungeonItemEquipment).IsEquip)
                            {
                                (item as DungeonItemEquipment).UnEquip();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Выбрасывает все предметы из контейнера на уровень</summary>
        /// <param name="drop_point_location">Координаты сброса</param>
        /// <param name="procent_not_to_destroy">Процент того, что очередной предмет будет уничтожен при выбрасывании (если может быть уничтожен)</param>
        public void DropAllItems(Point drop_point_location, double procent_not_to_destroy = 50)
        {
            if (m_owner is DungeonObject)
            {
                Random random = new Random();
                bool is_one_item_dropped = false;
                for (int i = 0; i < m_items.Count; i++)
                {
                    if (m_items[i] != null)
                    {
                        m_items[i].Location = new DungeonPoint(drop_point_location.X + random.Next(-spread_radius, spread_radius + 1), drop_point_location.Y + random.Next(-spread_radius, spread_radius + 1));
                        if (random.Next(0, 101) <= procent_not_to_destroy || !(m_items[i].CanDestroy) || !is_one_item_dropped)
                        {
                            (m_owner as DungeonObject).DungeonLevel.Container.Add(m_items[i]);
                            is_one_item_dropped = true;
                        }
                        else
                        {
                            m_items[i].Destroy();
                        }
                    }
                }
            }
        }
    }
}