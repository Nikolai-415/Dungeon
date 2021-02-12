using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Dungeon
{
    /// <summary>Уровень подземелья</summary>
    [Serializable]
    public class DungeonLevel : DungeonOwner
    {
        /// <summary>Длина блока</summary>
        private static readonly int block_length = 100;

        /// <summary>Длина комнаты с лестницой (в ячейках)</summary>
        private static readonly int room_with_ladder_length = 3;

        /// <summary>Минимальная длина комнаты (в ячейках)</summary>
        private static readonly int min_room_length = 5;

        /// <summary>Минимальное количество предметов в сундуке</summary>
        private static readonly int min_items_in_chest = 1;

        /// <summary>Максимальное количество предметов в сундуке</summary>
        private static readonly int max_items_in_chest = 5;

        /// <summary>Вероятность (в процентах) создать сундук в центре комнаты</summary>
        private static readonly double procent_chance_room_create_chest = 50;

        /// <summary>Вероятность (в процентах) не создать одного монстра в комнате (всего 8 попыток создания)</summary>
        private static readonly double procent_chance_room_not_create_one_monster = 90;

        /// <summary>Вероятность (в процентах) не создавать один из предметов экипировки для монстра (шлем, броня, артефакт, 3 зелья; меч всегда есть)</summary>
        private static readonly double procent_chance_monster_not_create_one_item = 95;

        /// <summary>Вероятность (в процентах) не создавать один из предметов экипировки для босса (шлем, броня, артефакт, 3 зелья; меч всегда есть)</summary>
        private static readonly double procent_chance_monster_boss_not_create_one_item = 10;

        /// <summary>Вероятность (в процентах) создать зелье</summary>
        private static readonly double procent_chance_create_item_potion = 60;

        /// <summary>Вероятность (в процентах) создать шлем</summary>
        private static readonly double procent_chance_create_item_helmet = 20;

        /// <summary>Вероятность (в процентах) создать броню</summary>
        private static readonly double procent_chance_create_item_armour = 20;

        /// <summary>Вероятность (в процентах) создать меч</summary>
        private static readonly double procent_chance_create_item_sword = 15;

        /// <summary>Вероятность (в процентах) создать артефакт</summary>
        private static readonly double procent_chance_create_item_artifact = 0.2;

        /// <summary>Вероятность (в процентах) создать положительный эффект для экипировки</summary>
        private static readonly double procent_chance_effect_is_plus = 25;

        /// <summary>Вероятность (в процентах) создать отрицательный эффект для экипировки</summary>
        private static readonly double procent_chance_effect_is_minus = 10;

        /// <summary>Вероятность (в процентах) создать зелье с тремя эффектами</summary>
        private static readonly double procent_chance_potion_set_three_effects = 10;

        /// <summary>Вероятность (в процентах) создать зелье с двумя эффектами</summary>
        private static readonly double procent_chance_potion_set_two_effects = 20;

        /// <summary>Вероятность (в процентах) создать артефакт с тремя эффектами</summary>
        private static readonly double procent_chance_artifact_set_three_effects = 5;

        /// <summary>Вероятность (в процентах) создать артефакт с двумя эффектами</summary>
        private static readonly double procent_chance_artifact_set_two_effects = 30;

        /// <summary>Вероятность (в процентах) создать из пустой точки соединения угловую точку соединения</summary>
        private static readonly double procent_chance_from_none_create_corner = 10;

        /// <summary>Вероятность (в процентах) создать из пустой точки соединения комнату</summary>
        private static readonly double procent_chance_from_none_create_room = 65;

        /// <summary>Вероятность (в процентах) соединить комнату с рядом стоящей (4 проверки; специальные комнаты соединяются отдельно)</summary>
        private static readonly double procent_chance_room_connect_to_another = 30;

        /// <summary>Вероятность (в процентах) не создавать дверь между комнатами</summary>
        private static readonly double procent_chance_not_create_door = 50;

        /// <summary>Максимальный множитель для величины, на которую эффект изменяет характеристику</summary>
        private static readonly int effect_max_value_multiplier = 10; // потом умножается на величину прокачки характеристики

        /// <summary>Максимальный множитель для длительности эффекта (зелья)</summary>
        private static readonly int effect_max_duration_nultiplier = 300 / 5; // потом умножается на 5 секунд

        /// <summary>Интервал (в миллисекундах) таймера респавна монстров</summary>
        private static readonly int timer_monsters_spawn_interval = 10000;

        /// <summary>Интервал (в миллисекундах) таймера смены действия монстров</summary>
        private static readonly int timer_monsters_change_action_interval = 50;

        /// <summary>Количество уровней подземелья</summary>
        public static int LevelsNumber = 0;

        /// <summary>Id уровня подземелья</summary>
        private int m_level_id;

        /// <summary>Id уровня подземелья</summary>
        public int Id
        {
            get
            {
                return m_level_id;
            }
        }

        /// <summary>Форма, в которой отображается игра</summary>
        [NonSerialized]
        public MainForm Form;

        /// <summary>Установленный уровень сложности</summary>
        private DungeonDifficulty m_difficulty;

        /// <summary>Установленный уровень сложности</summary>
        public DungeonDifficulty Difficulty
        {
            get
            {
                return m_difficulty;
            }
        }

        /// <summary>Предыдущий уровень подземелья (если есть)</summary>
        public DungeonLevel PreviousDungeonLevel;

        /// <summary>Следующий уровень подземелья (если есть)</summary>
        public DungeonLevel NextDungeonLevel;

        /// <summary>Существа на уровне</summary>
        private List<DungeonCreature> m_creatures;

        /// <summary>Существа на уровне</summary>
        public List<DungeonCreature> Creatures
        {
            get
            {
                return m_creatures;
            }
        }

        /// <summary>Сундуки на уровне</summary>
        private List<DungeonChest> m_chests;

        /// <summary>Сундуки на уровне</summary>
        public List<DungeonChest> Chests
        {
            get
            {
                return m_chests;
            }
        }

        /// <summary>Блоки на уровне</summary>
        private List<DungeonBlock> m_blocks;

        /// <summary>Двери на уровне</summary>
        private List<DungeonDoor> m_doors;

        /// <summary>Записки, в которых находится подказка к кодовому замку бонусной двери, которая расположена на этом уровне</summary>
        private List<DungeonItemPaper> m_papers;

        /// <summary>Контейнер, хранящий валяющиеся предметы на уровне подземелья</summary>
        private DungeonContainer m_container;

        /// <summary>Контейнер, хранящий валяющиеся предметы на уровне подземелья</summary>
        public DungeonContainer Container
        {
            get
            {
                return m_container;
            }
        }

        /// <summary>Список объектов, находящихся на уровне подземелья, для которых включена статическая коллизия (столкновение)</summary>
        private List<DungeonObject> m_objects_with_static_collision;

        /// <summary>Список объектов, находящихся на уровне подземелья, для которых включена статическая коллизия (столкновение)</summary>
        public List<DungeonObject> ObjectsWithStaticCollision
        {
            get
            {
                return m_objects_with_static_collision;
            }
        }

        /// <summary>Информация о созданных дверях (содержит 4 точки: [0] - координаты точки соединения откуда; [1] - координаты точки соединения куда; [2] - координаты точки соединения с ключом; [3] - координаты ячейки с дверью)</summary>
        private List<Point[]> m_doors_info;

        /// <summary>Ключ генерации мира</summary>
        private Random m_world_key;

        /// <summary>Изображение блока стены</summary>
        private Bitmap m_image_wall;

        /// <summary>Изображение блока тёмной стены</summary>
        private Bitmap m_image_wall_dark;

        /// <summary>Изображение блока очень тёмной стены</summary>
        private Bitmap m_image_wall_dark_dark;

        /// <summary>Изображение блока пола/summary>
        private Bitmap m_image_floor;

        /// <summary>Изображение блока с лестницей, ведущей наверх/summary>
        private Bitmap m_image_ladder_up;

        /// <summary>Изображение блока с лестницей, ведущей вниз/summary>
        private Bitmap m_image_ladder_down;

        /// <summary>Изображение блока с лестницей, ведущей влево/summary>
        private Bitmap m_image_ladder_left;

        /// <summary>Изображение блока с лестницей, ведущей вправо/summary>
        private Bitmap m_image_ladder_right;

        /// <summary>Направление лестницы входа на уровень</summary>
        private DungeonLadderType m_ladder_entrance_direction;

        /// <summary>Направление лестницы входа на уровень</summary>
        public DungeonLadderType LadderEntranceDirection
        {
            get
            {
                return m_ladder_entrance_direction;
            }
        }

        /// <summary>Направление лестницы выхода с уровня</summary>
        private DungeonLadderType m_ladder_exit_direction;

        /// <summary>Направление лестницы выхода с уровня</summary>
        public DungeonLadderType LadderExitDirection
        {
            get
            {
                return m_ladder_exit_direction;
            }
        }

        /// <summary>Количество точек соединения в строке / столбце карты</summary>
        private int m_connection_points_number_in_line;

        /// <summary>Вероятность (в процентах) наилучшего исхода (чем уровень сложности выше, тем ниже данная вероятность)</summary>
        private int m_chance_procent = 80;

        /// <summary>Длина одного коридора (в ячейках)</summary>
        private int m_tunnel_length;

        /// <summary>Точки соединения карты</summary>
        private DungeonMapConnectionPointType[,] m_connection_points;

        /// <summary>Списки монстров в каждой комнате по точке соединения</summary>
        private List<DungeonMonster>[,] m_monsters_in_room;

        /// <summary>Количество ячеек в строке / столбце карты</summary>
        private int m_cells_number_in_line;

        /// <summary>Количество ячеек в строке / столбце карты</summary>
        public int CellsNumberInLine
        {
            get
            {
                return m_cells_number_in_line;
            }
        }

        /// <summary>Ячейки карты (после - преобразуются в объекты)</summary>
        private DungeonMapCell[,] m_cells;

        /// <summary>Координаты комнат (левый верхний угол комнат)</summary>
        private Point[,] m_rooms_locations;

        /// <summary>Длины комнат</summary>
        private int[,] m_rooms_lengths;

        /// <summary>Список соединённых точек соединения</summary>
        private List<Point[]> m_connection_points_connected;

        /// <summary>Блок с лестницей входа на уровень</summary>
        private DungeonBlock m_ladder_entrance_block;

        /// <summary>Блок с лестницей входа на уровень</summary>
        public DungeonBlock LadderEntranceBlock
        {
            get
            {
                return m_ladder_entrance_block;
            }
        }

        /// <summary>Блок с лестницей выхода с уровня</summary>
        private DungeonBlock m_ladder_exit_block;

        /// <summary>Блок с лестницей выхода с уровня</summary>
        public DungeonBlock LadderExitBlock
        {
            get
            {
                return m_ladder_exit_block;
            }
        }

        /// <summary>Позиция входа на уровень</summary>
        private Point m_entrance_location;

        /// <summary>Позиция входа на уровень</summary>
        public Point EntranceLocation
        {
            get
            {
                return m_entrance_location;
            }
        }

        /// <summary>Позиция выхода с уровня</summary>
        private Point m_ladder_exit_location;

        /// <summary>Позиция выхода с уровня</summary>
        public Point LadderExitLocation
        {
            get
            {
                return m_ladder_exit_location;
            }
        }

        /// <summary>Размер карты (длина всех ячеек в строке / столбце)</summary>
        public int MapLengthInLine
        {
            get
            {
                return block_length * CellsNumberInLine;
            }
        }

        /// <summary>Найдена ли ячейка игроком</summary>
        private bool[,] m_is_cell_finded;

        /// <summary>Координаты точки соединения, которая содержит комнату входа на уровень</summary>
        private Point m_room_entrance_location;

        /// <summary>Координаты точки соединения, которая содержит комнату выхода с уровня</summary>
        private Point m_room_exit_location;

        /// <summary>Координаты точки соединения, которая содержит комнату босса</summary>
        private Point m_room_boss_location;

        /// <summary>Координаты точки соединения, которая содержит бонусную комнату</summary>
        private Point m_room_bonus_location;

        /// <summary>Жив ли босс уровня</summary>
        public bool IsBossAlive;

        /// <summary>Список эффектов (анимаций), находящихся на уровне подземелья</summary>
        private List<DungeonGraphicEffect> m_graphic_effects;

        /// <summary>Список эффектов (анимаций), находящихся на уровне подземелья</summary>
        public List<DungeonGraphicEffect> GraphicEffects
        {
            get
            {
                return m_graphic_effects;
            }
        }

        /// <summary>Максимальное число монстров на уровне</summary>
        private int m_max_monsters_on_level;

        /// <summary>Запущен ли таймер спавна монстров (монстры спавнятся, пока жив босс уровня)</summary>
        private bool m_timer_monsters_spawn_is_working;

        /// <summary>Таймер спавна монстров (монстры спавнятся, пока жив босс уровня)</summary>
        [NonSerialized]
        private Timer m_timer_monsters_spawn;

        /// <summary>Запущен ли таймер смены действий монстров</summary>
        private bool m_timer_monsters_change_action_is_working;

        /// <summary>Таймер смены действий монстров</summary>
        [NonSerialized]
        private Timer m_timer_monsters_change_action;

        /// <summary>Создаёт уровень подземелья</summary>
        public DungeonLevel(MainForm main_form, ref Random world_key, DungeonDifficulty difficulty, DungeonLadderType ladder_entrance_direction, Point ladder_entrance_location, DungeonLadderType ladder_exit_direction)
            : base()
        {
            m_level_id = LevelsNumber;
            LevelsNumber++;

            Form = main_form;

            m_difficulty = difficulty;

            m_creatures = new List<DungeonCreature>();
            m_chests = new List<DungeonChest>();
            m_blocks = new List<DungeonBlock>();
            m_doors = new List<DungeonDoor>();
            m_papers = new List<DungeonItemPaper>();
            for (int i = 0; i < 4; i++)
            {
                m_papers.Add(new DungeonItemPaper(m_level_id));
            }
            m_graphic_effects = new List<DungeonGraphicEffect>();
            m_container = new DungeonContainer(this);
            m_objects_with_static_collision = new List<DungeonObject>();
            m_doors_info = new List<Point[]>();

            m_world_key = world_key;
            SetImagesFromTexture();

            m_ladder_entrance_direction = ladder_entrance_direction;
            m_ladder_exit_direction = ladder_exit_direction;

            if (m_difficulty == DungeonDifficulty.Easy)
            {
                m_connection_points_number_in_line = 4;
                m_chance_procent = 80;
            }
            else if (m_difficulty == DungeonDifficulty.Normal)
            {
                m_connection_points_number_in_line = 5;
                m_chance_procent = 55;
            }
            else if (m_difficulty == DungeonDifficulty.Hard)
            {
                m_connection_points_number_in_line = 6;
                m_chance_procent = 30;
            }
            else
            {
                m_connection_points_number_in_line = 7;
                m_chance_procent = 5;
            }

            int max_room_length = m_connection_points_number_in_line * 3 / 2;
            m_tunnel_length = max_room_length * 2 + 1; // длина одного коридора (от одного соединения до другого)

            m_connection_points = new DungeonMapConnectionPointType[m_connection_points_number_in_line, m_connection_points_number_in_line];

            m_monsters_in_room = new List<DungeonMonster>[m_connection_points_number_in_line, m_connection_points_number_in_line];
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    m_monsters_in_room[y, x] = new List<DungeonMonster>();
                }
            }

            m_cells_number_in_line = m_tunnel_length * m_connection_points_number_in_line; // ширина и высота карты (в ячейках)
            m_cells = new DungeonMapCell[CellsNumberInLine, CellsNumberInLine];

            m_rooms_locations = new Point[m_connection_points_number_in_line, m_connection_points_number_in_line];
            m_rooms_lengths = new int[m_connection_points_number_in_line, m_connection_points_number_in_line];

            m_connection_points_connected = new List<Point[]>();

            m_is_cell_finded = new bool[CellsNumberInLine, CellsNumberInLine];
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    m_is_cell_finded[y, x] = false;
                }
            }

        // =========================================================
        TryToGenerateAllAgain:

            // обнуление всех точек соединения
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    m_connection_points[y, x] = DungeonMapConnectionPointType.None;
                }
            }

            // обнуление всех ячеек карты
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    m_cells[y, x] = DungeonMapCell.Nothing;
                }
            }

            // обнуление списка соединённых точек соединения
            m_connection_points_connected.Clear();

            int connection_point_x;
            int connection_point_y;

            // генерация комнаты входа
            do
            {
                if (ladder_entrance_direction == DungeonLadderType.NoLadder)
                {
                    connection_point_x = m_world_key.Next(0, m_connection_points_number_in_line);
                    connection_point_y = m_world_key.Next(0, m_connection_points_number_in_line);
                }
                else
                {
                    connection_point_x = ladder_entrance_location.X;
                    connection_point_y = ladder_entrance_location.Y;
                }
            } while (!TryToCreateRoomEntrance(new Point(connection_point_x, connection_point_y)));

            // генерация комнаты выхода и комнаты босса
            do
            {
                if (ladder_exit_direction == DungeonLadderType.Up)
                {
                    connection_point_x = m_world_key.Next(0, m_connection_points_number_in_line);
                    connection_point_y = m_world_key.Next(1, m_connection_points_number_in_line - 1);
                }
                else if (ladder_exit_direction == DungeonLadderType.Down)
                {
                    connection_point_x = m_world_key.Next(0, m_connection_points_number_in_line);
                    connection_point_y = m_world_key.Next(1, m_connection_points_number_in_line - 1);
                }
                else if (ladder_exit_direction == DungeonLadderType.Left)
                {
                    connection_point_x = m_world_key.Next(1, m_connection_points_number_in_line - 1);
                    connection_point_y = m_world_key.Next(0, m_connection_points_number_in_line);
                }
                else
                {
                    connection_point_x = m_world_key.Next(1, m_connection_points_number_in_line - 1);
                    connection_point_y = m_world_key.Next(0, m_connection_points_number_in_line);
                }

            } while (!TryToCreateRoomExitAndRoomBoss(new Point(connection_point_x, connection_point_y), max_room_length, ladder_exit_direction));

            // генерация бонусной комнаты
            do
            {
                connection_point_x = m_world_key.Next(0, m_connection_points_number_in_line);
                connection_point_y = m_world_key.Next(0, m_connection_points_number_in_line);
            } while (!TryToCreateRoomBonus(new Point(connection_point_x, connection_point_y), max_room_length));

            // преобразование всех пустых точек соединения в комнаты или в угловые соединения
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    if (m_connection_points[y, x] == DungeonMapConnectionPointType.None)
                    {
                        if (IsGlobalProcentWork(procent_chance_from_none_create_corner))
                        {
                            m_connection_points[y, x] = DungeonMapConnectionPointType.Corner;
                        }
                        else if (IsGlobalProcentWork(procent_chance_from_none_create_room))
                        {
                            int room_length = m_world_key.Next(min_room_length, max_room_length + 1);
                            TryToCreateRoom(new Point(x, y), room_length, DungeonRoomType.Usual);
                        }
                    }
                }
            }

            // соединение комнат
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    if (m_connection_points[y, x] != DungeonMapConnectionPointType.None)
                    {
                        bool result_of_connect = ConnectConnectionPointToConnectionPointsAround(x, y);
                        if (!result_of_connect)
                        {
                            goto TryToGenerateAllAgain; // если не получилось правильно соединить все комнаты - генерируем заново
                        }
                    }
                }
            }

            // преобразование угловых точек соединения ведущих в никуда, в комнаты
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    if (m_connection_points[y, x] == DungeonMapConnectionPointType.Corner && GetConnectionPointsConnectedToConnectionPoint(x, y).Count == 1)
                    {
                        int room_length = m_world_key.Next(min_room_length, max_room_length + 1);
                        TryToCreateRoom(new Point(x, y), room_length, DungeonRoomType.Usual);
                    }
                }
            }

            // если нельзя пройти от входа до комнаты босса
            if (!IsTwoConnectionPointsConnectedThrowOther(m_room_entrance_location, m_room_boss_location))
            {
                goto TryToGenerateAllAgain;
            }

            // если нельзя пройти от входа до бонусной комнаты
            if (!IsTwoConnectionPointsConnectedThrowOther(m_room_entrance_location, m_room_bonus_location))
            {
                goto TryToGenerateAllAgain;
            }

            // =========================================================

            // уничтожение комнат, в которые невозможно попасть
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    if (!IsTwoConnectionPointsConnectedThrowOther(m_room_entrance_location, new Point(x, y)))
                    {
                        DestroyRoom(new Point(x, y));
                    }
                }
            }

            // создание тоннелей (заполнение полом) между точками соединения, которые соединены между собой
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    if (m_connection_points[y, x] != DungeonMapConnectionPointType.None)
                    {
                        if (IsTwoConnectionPointsConnected(x, y, x - 1, y))
                        {
                            CreateTunnelBetweenTwoCells(GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x, y)), GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x - 1, y)));
                        }
                        if (IsTwoConnectionPointsConnected(x, y, x + 1, y))
                        {
                            CreateTunnelBetweenTwoCells(GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x, y)), GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x + 1, y)));
                        }
                        if (IsTwoConnectionPointsConnected(x, y, x, y - 1))
                        {
                            CreateTunnelBetweenTwoCells(GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x, y)), GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x, y - 1)));
                        }
                        if (IsTwoConnectionPointsConnected(x, y, x, y + 1))
                        {
                            CreateTunnelBetweenTwoCells(GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x, y)), GetCellLocationRoomCenterFromConnectionPointLocation(new Point(x, y + 1)));
                        }
                    }
                }
            }

            bool[,] was_wall_created = new bool[CellsNumberInLine, CellsNumberInLine];

            // окружение карты стенами (1 слой)
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    was_wall_created[y, x] = false;
                }
            }
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    if (m_cells[y, x] != DungeonMapCell.Nothing &&
                        m_cells[y, x] != DungeonMapCell.Wall)
                    {
                        if (y - 1 >= 0 && m_cells[y - 1, x] == DungeonMapCell.Nothing) SetCell(x, y - 1, DungeonMapCell.Wall);
                        if (y + 1 < CellsNumberInLine - 1 && m_cells[y + 1, x] == DungeonMapCell.Nothing) SetCell(x, y + 1, DungeonMapCell.Wall);
                        if (x - 1 >= 0 && m_cells[y, x - 1] == DungeonMapCell.Nothing) SetCell(x - 1, y, DungeonMapCell.Wall);
                        if (x + 1 < CellsNumberInLine - 1 && m_cells[y, x + 1] == DungeonMapCell.Nothing) SetCell(x + 1, y, DungeonMapCell.Wall);

                        was_wall_created[y, x] = true;
                    }
                }
            }

            // окружение карты стенами (2 слой)
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    was_wall_created[y, x] = false;
                }
            }
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    if (m_cells[y, x] == DungeonMapCell.Wall)
                    {
                        if (!was_wall_created[y, x])
                        {
                            int is_very_dark = m_world_key.Next(0, 4);
                            DungeonMapCell map_cell;
                            if (is_very_dark == 1) map_cell = DungeonMapCell.Wall;
                            else if (is_very_dark == 2) map_cell = DungeonMapCell.WallDark;
                            else map_cell = DungeonMapCell.WallDarkDark;

                            if (y - 1 >= 0 && m_cells[y - 1, x] == DungeonMapCell.Nothing)
                            {
                                SetCell(x, y - 1, map_cell);
                                was_wall_created[y - 1, x] = true;
                            }
                            if (y + 1 < CellsNumberInLine - 1 && m_cells[y + 1, x] == DungeonMapCell.Nothing)
                            {
                                SetCell(x, y + 1, map_cell);
                                was_wall_created[y + 1, x] = true;
                            }
                            if (x - 1 >= 0 && m_cells[y, x - 1] == DungeonMapCell.Nothing)
                            {
                                SetCell(x - 1, y, map_cell);
                                was_wall_created[y, x - 1] = true;
                            }
                            if (x + 1 < CellsNumberInLine - 1 && m_cells[y, x + 1] == DungeonMapCell.Nothing)
                            {
                                SetCell(x + 1, y, map_cell);
                                was_wall_created[y, x + 1] = true;
                            }
                        }
                    }
                }
            }

            // окружение карты стенами (3 слой)
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    was_wall_created[y, x] = false;
                }
            }
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    if (m_cells[y, x] == DungeonMapCell.Wall ||
                        m_cells[y, x] == DungeonMapCell.WallDark)
                    {
                        if (!was_wall_created[y, x])
                        {
                            DungeonMapCell map_cell;
                            if (m_cells[y, x] == DungeonMapCell.Wall) map_cell = DungeonMapCell.WallDark;
                            else map_cell = DungeonMapCell.WallDarkDark;

                            if (y - 1 >= 0 && m_cells[y - 1, x] == DungeonMapCell.Nothing)
                            {
                                SetCell(x, y - 1, map_cell);
                                was_wall_created[y - 1, x] = true;
                            }
                            if (y + 1 < CellsNumberInLine - 1 && m_cells[y + 1, x] == DungeonMapCell.Nothing)
                            {
                                SetCell(x, y + 1, map_cell);
                                was_wall_created[y + 1, x] = true;
                            }
                            if (x - 1 >= 0 && m_cells[y, x - 1] == DungeonMapCell.Nothing)
                            {
                                SetCell(x - 1, y, map_cell);
                                was_wall_created[y, x - 1] = true;
                            }
                            if (x + 1 < CellsNumberInLine - 1 && m_cells[y, x + 1] == DungeonMapCell.Nothing)
                            {
                                SetCell(x + 1, y, map_cell);
                                was_wall_created[y, x + 1] = true;
                            }
                        }
                    }
                }
            }

            // окружение карты стенами (4 слой)
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    was_wall_created[y, x] = false;
                }
            }
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    if (m_cells[y, x] == DungeonMapCell.WallDark)
                    {
                        if (!was_wall_created[y, x])
                        {
                            DungeonMapCell map_cell = DungeonMapCell.WallDarkDark;

                            if (y - 1 >= 0 && m_cells[y - 1, x] == DungeonMapCell.Nothing)
                            {
                                SetCell(x, y - 1, map_cell);
                                was_wall_created[y - 1, x] = true;
                            }
                            if (y + 1 < CellsNumberInLine - 1 && m_cells[y + 1, x] == DungeonMapCell.Nothing)
                            {
                                SetCell(x, y + 1, map_cell);
                                was_wall_created[y + 1, x] = true;
                            }
                            if (x - 1 >= 0 && m_cells[y, x - 1] == DungeonMapCell.Nothing)
                            {
                                SetCell(x - 1, y, map_cell);
                                was_wall_created[y, x - 1] = true;
                            }
                            if (x + 1 < CellsNumberInLine - 1 && m_cells[y, x + 1] == DungeonMapCell.Nothing)
                            {
                                SetCell(x + 1, y, map_cell);
                                was_wall_created[y, x + 1] = true;
                            }
                        }
                    }
                }
            }

            CreateDoors();

            CreateObjectsFromCells();

            m_max_monsters_on_level = Creatures.Count;

            m_timer_monsters_change_action_is_working = false;
            m_timer_monsters_spawn_is_working = false;
            InitializeTimers();
        }

        /// <summary>Инициализирует таймеры</summary>
        public void InitializeTimers()
        {
            // таймер респавна монстров
            m_timer_monsters_spawn = new Timer();
            m_timer_monsters_spawn.Interval = timer_monsters_spawn_interval;
            m_timer_monsters_spawn.Tick += m_timer_monsters_spawn_Tick;
            if (m_timer_monsters_spawn_is_working)
            {
                m_timer_monsters_spawn.Start();
            }

            // таймер смены действий монстров
            m_timer_monsters_change_action = new Timer();
            m_timer_monsters_change_action.Interval = timer_monsters_change_action_interval;
            m_timer_monsters_change_action.Tick += m_timer_monsters_change_action_Tick;
            if (m_timer_monsters_change_action_is_working)
            {
                m_timer_monsters_change_action.Start();
            }
        }

        /// <summary>Устанавливает изображения стен, пола и лестниц из текстуры уровней</summary>
        private void SetImagesFromTexture()
        {
            const int length = 100;

            if (m_level_id < 0 || m_level_id >= 10) m_level_id = 0;
            Bitmap texture = Properties.Resources.TEXTURE_levels;

            Bitmap new_image_floor = new Bitmap(length, length);
            Bitmap new_image_wall = new Bitmap(length, length);
            Bitmap new_image_wall_dark = new Bitmap(length, length);
            Bitmap new_image_wall_dark_dark = new Bitmap(length, length);
            Bitmap new_image_ladder_up = new Bitmap(length, length);
            Bitmap new_image_ladder_down = new Bitmap(length, length);
            Bitmap new_image_ladder_left = new Bitmap(length, length);
            Bitmap new_image_ladder_right = new Bitmap(length, length);

            for (int i = length * (m_level_id); i < length * (m_level_id + 1); i++)
            {
                for (int i2 = 0; i2 < length; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_floor.SetPixel(i2, i - length * (m_level_id), pixel);
                }
                for (int i2 = length; i2 < length * 2; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_wall.SetPixel(i2 - length, i - length * (m_level_id), pixel);
                }
                for (int i2 = length * 2; i2 < length * 3; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_wall_dark.SetPixel(i2 - length * 2, i - length * (m_level_id), pixel);
                }
                for (int i2 = length * 3; i2 < length * 4; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_wall_dark_dark.SetPixel(i2 - length * 3, i - length * (m_level_id), pixel);
                }
                for (int i2 = length * 4; i2 < length * 5; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_ladder_up.SetPixel(i2 - length * 4, i - length * (m_level_id), pixel);
                }
                for (int i2 = length * 5; i2 < length * 6; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_ladder_down.SetPixel(i2 - length * 5, i - length * (m_level_id), pixel);
                }
                for (int i2 = length * 6; i2 < length * 7; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_ladder_left.SetPixel(i2 - length * 6, i - length * (m_level_id), pixel);
                }
                for (int i2 = length * 7; i2 < length * 8; i2++)
                {
                    Color pixel = texture.GetPixel(i2, i);
                    new_image_ladder_right.SetPixel(i2 - length * 7, i - length * (m_level_id), pixel);
                }
            }

            m_image_floor = new_image_floor;
            m_image_wall = new_image_wall;
            m_image_wall_dark = new_image_wall_dark;
            m_image_wall_dark_dark = new_image_wall_dark_dark;
            m_image_ladder_up = new_image_ladder_up;
            m_image_ladder_down = new_image_ladder_down;
            m_image_ladder_left = new_image_ladder_left;
            m_image_ladder_right = new_image_ladder_right;
        }

        /// <summary>Проверяет, пересекается ли объект с другим объектом, у которого статическая коллизия</summary>
        /// <param name="location">Координаты (центр) объекта</param>
        /// <param name="size">Размер объекта</param>
        /// <returns>Объект (равен null, если проверяемый объект ни с чем не пересекается)</returns>
        private DungeonObject IsObjectInBlocks(Point location, Size size)
        {
            Rectangle rectangle_item = new Rectangle(new Point(location.X - size.Width / 2, location.Y - size.Height / 2), size);
            for (int i = 0; i < m_objects_with_static_collision.Count; i++)
            {
                DungeonObject block = m_objects_with_static_collision[i];
                Rectangle rectangle_block = new Rectangle(new Point(block.Location.Point.X - block.CollisionSize.Width / 2 + block.CollisionOffset.X, block.Location.Point.Y - block.CollisionSize.Height / 2 + block.CollisionOffset.Y), block.CollisionSize);
                if (rectangle_item.IntersectsWith(rectangle_block))
                {
                    return block;
                }
            }
            return null;
        }

        /// <summary>Возвращает расстояние, на которое будет сдвинут объект в заданном направлении, если он пересекается с объектом, у которого статическая коллизия</summary>
        /// <param name="obj">Объект</param>
        /// <param name="direction">Направления движения объекта</param>
        /// <returns>Расстояние</returns>
        private double GetLengthToMoveObjectIfInBlocks(DungeonObject obj, DungeonObjectMoveDirection direction)
        {
            double new_x = obj.Location.X;
            double new_y = obj.Location.Y;
            DungeonObject block;
            int iterations = 0;
            while ((block = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) != null)
            {
                Rectangle rectangle_item = new Rectangle(new Point((int)new_x - obj.Image.Width / 2, (int)new_y - obj.Image.Height / 2), obj.Image.Size);
                Rectangle rectangle_block = new Rectangle(new Point(block.Location.Point.X - block.CollisionSize.Width / 2 + block.CollisionOffset.X, block.Location.Point.Y - block.CollisionSize.Height / 2 + block.CollisionOffset.Y), block.CollisionSize);
                if (rectangle_item.IntersectsWith(rectangle_block))
                {
                    if (direction == DungeonObjectMoveDirection.Up)
                    {
                        new_y = block.Location.Y - block.CollisionSize.Height / 2 + block.CollisionOffset.Y - obj.Image.Height / 2 - 1;
                    }
                    else if (direction == DungeonObjectMoveDirection.Down)
                    {
                        new_y = block.Location.Y + block.CollisionSize.Height / 2 + block.CollisionOffset.Y + obj.Image.Height / 2 + 1;
                    }
                    else if (direction == DungeonObjectMoveDirection.Left)
                    {
                        new_x = block.Location.X - block.CollisionSize.Width / 2 + block.CollisionOffset.X - obj.Image.Width / 2 - 1;
                    }
                    else if (direction == DungeonObjectMoveDirection.Right)
                    {
                        new_x = block.Location.X + block.CollisionSize.Width / 2 + block.CollisionOffset.X + obj.Image.Width / 2 + 1;
                    }
                    if (direction == DungeonObjectMoveDirection.UpLeft)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x -= 1;
                            new_y -= 1;
                        }
                    }
                    else if (direction == DungeonObjectMoveDirection.UpRight)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x += 1;
                            new_y -= 1;
                        }
                    }
                    else if (direction == DungeonObjectMoveDirection.DownRight)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x += 1;
                            new_y += 1;
                        }
                    }
                    else if (direction == DungeonObjectMoveDirection.DownLeft)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x -= 1;
                            new_y += 1;
                        }
                    }
                }
                iterations++;
                if (iterations > 3) break; // для производительности
            }

            return Math.Sqrt(Math.Pow(new_x - obj.Location.X, 2) + Math.Pow(new_y - obj.Location.Y, 2));
        }

        /// <summary>Передвигает объект в заданном направлении, если он пересекается с объектом, у которого статическая коллизия</summary>
        /// <param name="obj">Объект</param>
        /// <param name="direction">Направления движения объекта</param>
        private void MoveObjectInDirectionIfInBlocks(DungeonObject obj, DungeonObjectMoveDirection direction)
        {
            DungeonObject block;
            int iterations = 0;
            while ((block = IsObjectInBlocks(obj.Location.Point, obj.Image.Size)) != null)
            {
                Rectangle rectangle_item = new Rectangle(new Point(obj.Location.Point.X - obj.Image.Width / 2, obj.Location.Point.Y - obj.Image.Height / 2), obj.Image.Size);
                Rectangle rectangle_block = new Rectangle(new Point(block.Location.Point.X - block.CollisionSize.Width / 2 + block.CollisionOffset.X, block.Location.Point.Y - block.CollisionSize.Height / 2 + block.CollisionOffset.Y), block.CollisionSize);
                if (rectangle_item.IntersectsWith(rectangle_block))
                {
                    double new_x = obj.Location.X;
                    double new_y = obj.Location.Y;
                    if (direction == DungeonObjectMoveDirection.Up)
                    {
                        new_y = block.Location.Y - block.CollisionSize.Height / 2 + block.CollisionOffset.Y - obj.Image.Height / 2 - 1;
                    }
                    else if (direction == DungeonObjectMoveDirection.Down)
                    {
                        new_y = block.Location.Y + block.CollisionSize.Height / 2 + block.CollisionOffset.Y + obj.Image.Height / 2 + 1;
                    }
                    else if (direction == DungeonObjectMoveDirection.Left)
                    {
                        new_x = block.Location.X - block.CollisionSize.Width / 2 + block.CollisionOffset.X - obj.Image.Width / 2 - 1;
                    }
                    else if (direction == DungeonObjectMoveDirection.Right)
                    {
                        new_x = block.Location.X + block.CollisionSize.Width / 2 + block.CollisionOffset.X + obj.Image.Width / 2 + 1;
                    }
                    if (direction == DungeonObjectMoveDirection.UpLeft)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x -= 1;
                            new_y -= 1;
                        }
                    }
                    else if (direction == DungeonObjectMoveDirection.UpRight)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x += 1;
                            new_y -= 1;
                        }
                    }
                    else if (direction == DungeonObjectMoveDirection.DownRight)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x += 1;
                            new_y += 1;
                        }
                    }
                    else if (direction == DungeonObjectMoveDirection.DownLeft)
                    {
                        DungeonObject block2;
                        while ((block2 = IsObjectInBlocks(new Point((int)new_x, (int)new_y), obj.Image.Size)) == block)
                        {
                            new_x -= 1;
                            new_y += 1;
                        }
                    }

                    obj.Location = new DungeonPoint(new_x, new_y);

                }
                iterations++;
                if (iterations > 3) break; // для производительности
            }

        }

        /// <summary>Передвигает объект, если он пересекается с объектом, у которого статическая коллизия</summary>
        /// <param name="obj">Объект</param>
        public void MoveObjectIfInBlocks(DungeonObject obj)
        {
            if (IsObjectInBlocks(obj.Location.Point, obj.Image.Size) != null) // если предмет попадает на объект с коллизией - необходимо передвинуть
            {
                double min_length = -1;
                DungeonObjectMoveDirection max_length_direction = DungeonObjectMoveDirection.Up;
                for (int i = 0; i < 8; i++)
                {
                    double length = GetLengthToMoveObjectIfInBlocks(obj, (DungeonObjectMoveDirection)i);
                    if (length < min_length || min_length == -1)
                    {
                        min_length = length;
                        max_length_direction = (DungeonObjectMoveDirection)i;
                    }
                }
                MoveObjectInDirectionIfInBlocks(obj, max_length_direction);
            }
        }

        /// <summary>Добавляет объект на уровень</summary>
        /// <param name="obj">Объект</param>
        public void Add(DungeonObject obj)
        {
            if (obj != null)
            {
                DungeonLevel old_dungeon_level = obj.DungeonLevel;
                if (old_dungeon_level != null)
                {
                    old_dungeon_level.Remove(obj);
                }

                obj.DungeonLevel = this;

                if (obj is DungeonHero)
                {
                    Creatures.Add(obj as DungeonHero);
                }
                else if (obj is DungeonMonster)
                {
                    Creatures.Add(obj as DungeonMonster);
                    if ((obj as DungeonMonster).IsBoss)
                    {
                        IsBossAlive = true;
                    }
                    MoveObjectIfInBlocks(obj);
                }
                else if (obj is DungeonChest)
                {
                    m_chests.Add(obj as DungeonChest);
                }
                else if (obj is DungeonBlock)
                {
                    m_blocks.Add(obj as DungeonBlock);
                }
                else if (obj is DungeonDoor)
                {
                    m_doors.Add(obj as DungeonDoor);
                }
                else if (obj is DungeonGraphicEffect)
                {
                    m_graphic_effects.Add(obj as DungeonGraphicEffect);
                }

                if (obj.CollisionType == DungeonObjectCollision.StaticCollision)
                {
                    m_objects_with_static_collision.Add(obj);
                }

                obj.ObjectStatus = DungeonObjectStatus.AddedNotDestroyed;
            }
        }

        /// <summary>Удаляет объект с уровня</summary>
        /// <param name="obj">Объект</param>
        public void Remove(DungeonObject obj)
        {
            if (obj != null)
            {
                if (obj.DungeonLevel == this) // если герой находится на этом уровне
                {
                    obj.DungeonLevel = null;

                    if (obj is DungeonHero)
                    {
                        Creatures.Remove(obj as DungeonHero);
                    }
                    else if (obj is DungeonMonster)
                    {
                        Creatures.Remove(obj as DungeonMonster);
                    }
                    else if (obj is DungeonChest)
                    {
                        m_chests.Remove(obj as DungeonChest);
                    }
                    else if (obj is DungeonBlock)
                    {
                        m_blocks.Remove(obj as DungeonBlock);
                    }
                    else if (obj is DungeonDoor)
                    {
                        m_doors.Remove(obj as DungeonDoor);
                    }
                    else if (obj is DungeonGraphicEffect)
                    {
                        m_graphic_effects.Remove(obj as DungeonGraphicEffect);
                    }

                    if (obj.CollisionType == DungeonObjectCollision.StaticCollision)
                    {
                        m_objects_with_static_collision.Remove(obj);
                    }

                    obj.ObjectStatus = DungeonObjectStatus.CreatedNotAdded;
                }
            }
        }

        /// <summary>Возвращает процент, изменённый в соответствии с установленным уровнем сложности (чем выше сложность, тем ниже будет процент)</summary>
        /// <param name="procent">Исходный процент</param>
        /// <returns>Новый процент</returns>
        private double GetProcentWithDifficulty(double procent = 100)
        {
            return procent * ((double)m_chance_procent / 100);
        }

        /// <summary>Проверяет указанную вероятность (в процентах)</summary>
        /// <param name="procent">Вероятность (в процентах)</param>
        /// <returns>True - если вероятность сработала, false - если нет</returns>
        private bool IsGlobalProcentWork(double procent)
        {
            if (m_world_key.Next(0, 100 * 100 + 1) <= procent * 100) return true;
            else return false;
        }

        /// <summary>Проверяет указанную вероятность (в процентах) в соответствии с установленным уровнем сложности (чем выше сложность, тем ниже будет процент)</summary>
        /// <param name="procent">Вероятность (в процентах)</param>
        /// <returns>True - если вероятность сработала, false - если нет</returns>
        private bool IsProcentWork(double procent)
        {
            return IsGlobalProcentWork(GetProcentWithDifficulty(procent));
        }

        /// <summary>Устанавливает тип ячейки</summary>
        /// <param name="cell_location">Координаты ячейки</param>
        /// <param name="cell_type">Тип ячейки</param>
        private void SetCell(Point cell_location, DungeonMapCell cell_type)
        {
            m_cells[cell_location.Y, cell_location.X] = cell_type;
        }

        /// <summary>Устанавливает тип ячейки</summary>
        /// <param name="cell_location_x">X-координата ячейки</param>
        /// <param name="cell_location_y">Y-координата ячейки</param>
        /// <param name="cell_type">Тип ячейки</param>
        private void SetCell(int cell_location_x, int cell_location_y, DungeonMapCell cell_type)
        {
            SetCell(new Point(cell_location_x, cell_location_y), cell_type);
        }

        /// <summary>Проверяет, содержит ли ячейка пол</summary>
        /// <param name="cell_location_x">X-координата ячейки</param>
        /// <param name="cell_location_y">Y-координата ячейки</param>
        /// <returns>True - если ячейка содержит пол, false - если нет</returns>
        private bool IsCellContainsFloor(int cell_location_x, int cell_location_y)
        {
            if (m_cells[cell_location_y, cell_location_x] == DungeonMapCell.Floor ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndChest ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndChestBonus ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndDoorHorizontal ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndDoorVertical ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndDoorExitHorizontal ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndDoorExitVertical ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndDoorBonusHorizontal ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndDoorBonusVertical ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndMonster ||
                    m_cells[cell_location_y, cell_location_x] == DungeonMapCell.FloorAndMonsterBoss) return true;
            return false;
        }

        /// <summary>Проверяет, находятся ли две ячейки рядом</summary>
        /// <param name="cell_1_location_x">X-координата первой ячейки</param>
        /// <param name="cell_1_location_y">Y-координата первой ячейки</param>
        /// <param name="cell_2_location_x">X-координата второй ячейки</param>
        /// <param name="cell_2_location_y">Y-координата второй ячейки</param>
        /// <returns>True - есля ячейки находятся рядом, false - есл нет</returns>
        private bool IsCellsIsNear(int cell_1_location_x, int cell_1_location_y, int cell_2_location_x, int cell_2_location_y)
        {
            if (cell_1_location_x == cell_2_location_x - 1 && cell_1_location_y == cell_2_location_y) return true;
            if (cell_1_location_x == cell_2_location_x + 1 && cell_1_location_y == cell_2_location_y) return true;
            if (cell_1_location_x == cell_2_location_x && cell_1_location_y == cell_2_location_y - 1) return true;
            if (cell_1_location_x == cell_2_location_x && cell_1_location_y == cell_2_location_y + 1) return true;
            return false;
        }

        /// <summary>Проверяет, можно ли пройти от одной ячейки к другой (по ячейке можно пройти, если она содержит пол)</summary>
        /// <param name="cell_1_location">Координаты первой ячейки</param>
        /// <param name="cell_2_location">Координаты второй ячейки</param>
        /// <returns>True - если пройти от первой ко второй можно, false - если нет</returns>
        private bool IsTwoCellsConnectedThrowOther(Point cell_1_location, Point cell_2_location)
        {
            bool[,] is_cell_checked = new bool[CellsNumberInLine, CellsNumberInLine];
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    is_cell_checked[y, x] = false;
                }
            }
            return IsTwoCellsConnectedThrowOther_Iteration(ref is_cell_checked, cell_1_location.X, cell_1_location.Y, cell_2_location.X, cell_2_location.Y);
        }

        /// <summary>Итерация для метода IsTwoCellsConnectedThrowOther - проверяет, соединена ли ячейка с целевой ячейкой через другие</summary>
        /// <param name="is_cell_checked">Массив, содержащий информацию о том, какие ячейки уже были посещены</param>
        /// <param name="cell_1_location_x">X-координата первой ячейки</param>
        /// <param name="cell_1_location_y">Y-координата первой ячейки</param>
        /// <param name="cell_2_location_x">X-координата второй ячейки</param>
        /// <param name="cell_2_location_y">Y-координата второй ячейки</param>
        /// <returns>True - если получилось пройти от ячейки к целевой, false - если нет</returns>
        private bool IsTwoCellsConnectedThrowOther_Iteration(ref bool[,] is_cell_checked, int cell_1_location_x, int cell_1_location_y, int cell_2_location_x, int cell_2_location_y)
        {
            if (!is_cell_checked[cell_1_location_y, cell_1_location_x])
            {
                if (IsCellsIsNear(cell_1_location_x, cell_1_location_y, cell_2_location_x, cell_2_location_y) &&
                    IsCellContainsFloor(cell_2_location_x, cell_2_location_y))
                {
                    return true;
                }

                is_cell_checked[cell_1_location_y, cell_1_location_x] = true;

                if (cell_1_location_y - 1 >= 0 &&
                    IsCellContainsFloor(cell_1_location_x, cell_1_location_y - 1) &&
                    IsTwoCellsConnectedThrowOther_Iteration(ref is_cell_checked, cell_1_location_x, cell_1_location_y - 1, cell_2_location_x, cell_2_location_y)) return true;

                if (cell_1_location_y + 1 <= CellsNumberInLine - 1 &&
                    IsCellContainsFloor(cell_1_location_x, cell_1_location_y + 1) &&
                    IsTwoCellsConnectedThrowOther_Iteration(ref is_cell_checked, cell_1_location_x, cell_1_location_y + 1, cell_2_location_x, cell_2_location_y)) return true;

                if (cell_1_location_x - 1 >= 0 &&
                    IsCellContainsFloor(cell_1_location_x - 1, cell_1_location_y) &&
                    IsTwoCellsConnectedThrowOther_Iteration(ref is_cell_checked, cell_1_location_x - 1, cell_1_location_y, cell_2_location_x, cell_2_location_y)) return true;

                if (cell_1_location_x + 1 <= CellsNumberInLine - 1 &&
                    IsCellContainsFloor(cell_1_location_x + 1, cell_1_location_y) &&
                    IsTwoCellsConnectedThrowOther_Iteration(ref is_cell_checked, cell_1_location_x + 1, cell_1_location_y, cell_2_location_x, cell_2_location_y)) return true;
            }
            return false;
        }

        /// <summary>Возвращает координаты точки соединения, в которой расположена указанная ячейка</summary>
        /// <param name="cell_location">Координаты ячейки</param>
        /// <returns>Координаты точки соединения</returns>
        private Point GetConnectionPointLocationFromCellLocation(Point cell_location)
        {
            return new Point(cell_location.X / m_tunnel_length, cell_location.Y / m_tunnel_length);
        }

        /// <summary>Возвращает координаты точки соединения, в которой расположены указанные координаты</summary>
        /// <param name="cell_location">Координаты</param>
        /// <returns>Координаты точки соединения</returns>
        public Point GetConnectionPointLocationFromGlobalLocation(Point global_location)
        {
            return GetConnectionPointLocationFromCellLocation(new Point(global_location.X / block_length, global_location.Y / block_length));
        }

        /// <summary>Возвращает координаты ячейки, в которой расположены указанные координаты</summary>
        /// <param name="cell_location">Координаты</param>
        /// <returns>Координаты ячейки</returns>
        public Point GetCellLocationFromGlobalLocation(Point global_location)
        {
            return new Point(global_location.X / block_length, global_location.Y / block_length);
        }

        /// <summary>Возвращает координаты центра ячейки</summary>
        /// <param name="cell_location">Координаты ячейки</param>
        /// <returns>Координаты центра ячейки</returns>
        private Point GetGlobalLocationFromCellLocation(Point cell_location)
        {
            return new Point(cell_location.X * block_length + block_length / 2, cell_location.Y * block_length + block_length / 2);
        }

        /// <summary>Создаёт комнату в точке соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <param name="room_length">Длина комнаты (в ячейках)</param>
        /// <param name="room_type">Тип комнаты</param>
        /// <returns>True - если комната создана, false - если нет</returns>
        private bool TryToCreateRoom(Point connection_point_location, int room_length, DungeonRoomType room_type)
        {
            if (m_connection_points[connection_point_location.Y, connection_point_location.X] != DungeonMapConnectionPointType.None &&
                m_connection_points[connection_point_location.Y, connection_point_location.X] != DungeonMapConnectionPointType.Corner) return false;

            if (room_length % 2 == 0) room_length++; // чтобы длина комнаты была нечётным числом
            room_length += 2;
            Point room_location = GetCellLocationRoomLeftUpCornerFromConnectionPoint(connection_point_location, room_length);
            Point room_center_location = GetCellLocationRoomCenterFromConnectionPointLocation(connection_point_location);

            for (int i = 0; i < room_length; i++)
            {
                for (int i2 = 0; i2 < room_length; i2++)
                {
                    SetCell(room_location.X + i2, room_location.Y + i, DungeonMapCell.Floor);

                }
            }

            // горизонтальные стенки
            for (int i2 = 0; i2 < room_length; i2++)
            {
                SetCell(new Point(room_location.X + i2, room_location.Y), DungeonMapCell.Wall);
                SetCell(new Point(room_location.X + i2, room_location.Y + room_length - 1), DungeonMapCell.Wall);
            }

            // вертикальные стенки
            for (int i = 0; i < room_length; i++)
            {
                SetCell(room_location.X, room_location.Y + i, DungeonMapCell.Wall);
                SetCell(room_location.X + room_length - 1, room_location.Y + i, DungeonMapCell.Wall);
            }

            // генерация дополнительных блоков (если комната без лестницы)
            if (room_type != DungeonRoomType.Ladder)
            {
                int max_aditional_blocks = m_world_key.Next(0, 1 + (int)Math.Pow(2, room_length / 2));
                for (int i = 0; i < max_aditional_blocks; i++)
                {
                    Point[] p = new Point[4];

                    // блок в верхнем левом углу
                    p[0].X = m_world_key.Next(0, room_length / 2 + 1);
                    p[0].Y = m_world_key.Next(0, room_length / 2 + 1);

                    // блок в верхнем правом углу
                    p[1].X = p[0].X + (room_length / 2 - p[0].X) * 2;
                    p[1].Y = p[0].Y;

                    // блок в нижнем левом углу
                    p[2].X = p[0].X;
                    p[2].Y = p[1].Y + (room_length / 2 - p[1].Y) * 2;

                    // блок в нижнем правом углу
                    p[3].X = p[1].X;
                    p[3].Y = p[2].Y;

                    for (int i2 = 0; i2 < 4; i2++)
                    {
                        p[i2] = new Point(p[i2].X + room_location.X, p[i2].Y + room_location.Y);
                        SetCell(p[i2], DungeonMapCell.Wall);
                    }
                }
            }

            if (room_type == DungeonRoomType.Bonus)
            {
                // заполнение полом квадрата 3х3 в центре комнаты
                for (int i = room_center_location.Y - 2; i <= room_center_location.Y + 2; i++)
                {
                    for (int i2 = room_center_location.X - 2; i2 <= room_center_location.X + 2; i2++)
                    {
                        SetCell(i2, i, DungeonMapCell.Floor);
                    }
                }
            }
            else
            {
                // заполнение полом квадрата 3х3 в центре комнаты
                for (int i = room_center_location.Y - 1; i <= room_center_location.Y + 1; i++)
                {
                    for (int i2 = room_center_location.X - 1; i2 <= room_center_location.X + 1; i2++)
                    {
                        SetCell(i2, i, DungeonMapCell.Floor);
                    }
                }
            }

            if (room_type == DungeonRoomType.Usual)
            {
                if (IsProcentWork(procent_chance_room_create_chest) || m_chests.Count == 0)
                {
                    SetCell(room_center_location, DungeonMapCell.FloorAndChest);
                }
                else if (IsGlobalProcentWork(50)) SetCell(room_center_location, DungeonMapCell.Wall);
            }
            else if (room_type == DungeonRoomType.Bonus)
            {
                bool is_vertical;
                if (m_world_key.Next(0, 2) == 0) is_vertical = true;
                else is_vertical = false;
                if (is_vertical)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        SetCell(room_center_location.X - 1, room_center_location.Y - 1 + i, DungeonMapCell.FloorAndChestBonus);
                        SetCell(room_center_location.X + 1, room_center_location.Y - 1 + i, DungeonMapCell.FloorAndChestBonus);
                    }
                }
                else
                {
                    for (int i2 = 0; i2 < 3; i2++)
                    {
                        SetCell(room_center_location.X - 1 + i2, room_center_location.Y - 1, DungeonMapCell.FloorAndChestBonus);
                        SetCell(room_center_location.X - 1 + i2, room_center_location.Y + 1, DungeonMapCell.FloorAndChestBonus);
                    }
                }
            }

            if (room_type == DungeonRoomType.Usual)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int i2 = 0; i2 < 3; i2++)
                    {
                        if (i != 1 && i2 != 1) // в центре комнаты монстр спавниться не должен, так как на этом месте может находится сундук или блок
                        {
                            Point spawn_location = new Point(room_center_location.X - 1 + i2, room_center_location.Y - 1 + i);
                            if (!IsProcentWork(procent_chance_room_not_create_one_monster)) SetCell(spawn_location, DungeonMapCell.FloorAndMonster);
                        }
                    }
                }
            }
            else if (room_type == DungeonRoomType.Boss)
            {
                SetCell(room_center_location, DungeonMapCell.FloorAndMonsterBoss); // в комнате с боссом в центре комнаты всегда есть место под спавн
            }

            // заполнение пустот между ячейками (если таковые найдутся)
            if (room_type != DungeonRoomType.Ladder)
            {
                for (int y = 0; y < room_length; y++)
                {
                    for (int x = 0; x < room_length; x++)
                    {
                        if (m_cells[room_location.Y + y, room_location.X + x] == DungeonMapCell.Floor)
                        {
                            if (!IsTwoCellsConnectedThrowOther(room_center_location, new Point(room_location.X + x, room_location.Y + y)))
                            {
                                m_cells[room_location.Y + y, room_location.X + x] = DungeonMapCell.Wall;
                            }
                        }
                    }
                }
            }

            m_connection_points[connection_point_location.Y, connection_point_location.X] = DungeonMapConnectionPointType.RoomUsusal;
            m_rooms_locations[connection_point_location.Y, connection_point_location.X] = room_location;
            m_rooms_lengths[connection_point_location.Y, connection_point_location.X] = room_length;
            return true;
        }

        /// <summary>Уничтожает комнату и все её блоки в точке соединения</summary>
        /// <param name="connection_point_location">Точка соединения</param>
        private void DestroyRoom(Point connection_point_location)
        {
            for (int i = 0; i < m_connection_points_connected.Count; i++)
            {
                if (m_connection_points_connected[i][0] == connection_point_location || m_connection_points_connected[i][1] == connection_point_location)
                {
                    m_connection_points_connected.RemoveAt(i);
                    i--;
                }
            }
            m_connection_points[connection_point_location.Y, connection_point_location.X] = DungeonMapConnectionPointType.None;
            for (int i = 0; i < m_rooms_lengths[connection_point_location.Y, connection_point_location.X]; i++)
            {
                for (int i2 = 0; i2 < m_rooms_lengths[connection_point_location.Y, connection_point_location.X]; i2++)
                {
                    SetCell(m_rooms_locations[connection_point_location.Y, connection_point_location.X].X + i2, m_rooms_locations[connection_point_location.Y, connection_point_location.X].Y + i, DungeonMapCell.Nothing);
                }
            }
        }

        /// <summary>Создаёт комнату с лестницей в точке соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <param name="ladder_direction">Направление лестницы</param>
        /// <param name="is_entrance">Является ли эта лестница - входом на этот уровень</param>
        /// <returns>True - если комната создана, false - если нет</returns>
        private bool TryToCreateRoomWithLadder(Point connection_point_location, DungeonLadderType ladder_direction, bool is_entrance)
        {
            Point room_location = GetCellLocationRoomLeftUpCornerFromConnectionPoint(connection_point_location, room_with_ladder_length);
            bool is_ok = TryToCreateRoom(connection_point_location, room_with_ladder_length, DungeonRoomType.Ladder);
            if (is_ok)
            {
                if (is_entrance)
                {
                    m_entrance_location = new Point(room_location.X * block_length + room_with_ladder_length * block_length / 2, room_location.Y * block_length + room_with_ladder_length * block_length / 2);
                }
                else m_ladder_exit_location = connection_point_location; // new Point(room_location.X * block_length + room_with_ladder_length * block_length / 2, room_location.Y * block_length + room_with_ladder_length * block_length / 2);
                if (ladder_direction != DungeonLadderType.NoLadder)
                {
                    // горизонтальные стенки
                    for (int i2 = 0; i2 < room_with_ladder_length; i2++)
                    {
                        SetCell(room_location.X + i2, room_location.Y, DungeonMapCell.Wall);
                        SetCell(room_location.X + i2, room_location.Y + room_with_ladder_length - 1, DungeonMapCell.Wall);
                    }

                    // вертикальные стенки
                    for (int i = 0; i < room_with_ladder_length; i++)
                    {
                        SetCell(room_location.X, room_location.Y + i, DungeonMapCell.Wall);
                        SetCell(room_location.X + room_with_ladder_length - 1, room_location.Y + i, DungeonMapCell.Wall);
                    }

                    DungeonMapCell cell_type = DungeonMapCell.Nothing;
                    if (is_entrance)
                    {
                        if (ladder_direction == DungeonLadderType.Up) cell_type = DungeonMapCell.EntranceLadderUp;
                        else if (ladder_direction == DungeonLadderType.Down) cell_type = DungeonMapCell.EntranceLadderDown;
                        else if (ladder_direction == DungeonLadderType.Left) cell_type = DungeonMapCell.EntranceLadderLeft;
                        else if (ladder_direction == DungeonLadderType.Right) cell_type = DungeonMapCell.EntranceLadderRight;
                    }
                    else
                    {
                        if (ladder_direction == DungeonLadderType.Up) cell_type = DungeonMapCell.ExitLadderUp;
                        else if (ladder_direction == DungeonLadderType.Down) cell_type = DungeonMapCell.ExitLadderDown;
                        else if (ladder_direction == DungeonLadderType.Left) cell_type = DungeonMapCell.ExitLadderLeft;
                        else if (ladder_direction == DungeonLadderType.Right) cell_type = DungeonMapCell.ExitLadderRight;
                    }
                    SetCell(room_location.X + 1, room_location.Y + 1, cell_type); // лестница

                    if (ladder_direction == DungeonLadderType.Up) SetCell(room_location.X + 1, room_location.Y + 2, DungeonMapCell.Floor);
                    else if (ladder_direction == DungeonLadderType.Down) SetCell(room_location.X + 1, room_location.Y, DungeonMapCell.Floor);
                    else if (ladder_direction == DungeonLadderType.Left) SetCell(room_location.X + 2, room_location.Y + 1, DungeonMapCell.Floor);
                    else if (ladder_direction == DungeonLadderType.Right) SetCell(room_location.X, room_location.Y + 1, DungeonMapCell.Floor);
                }
            }
            return is_ok;
        }

        /// <summary>Создаёт комнату входа в точке соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <returns>True - если комната создана, false - если нет</returns>
        private bool TryToCreateRoomEntrance(Point connection_point_location)
        {
            bool is_ok = TryToCreateRoomWithLadder(connection_point_location, m_ladder_entrance_direction, true);
            if (is_ok)
            {
                m_connection_points[connection_point_location.Y, connection_point_location.X] = DungeonMapConnectionPointType.RoomEntrance;
                m_room_entrance_location = connection_point_location;
            }
            return is_ok;
        }

        /// <summary>Создаёт комнату выхода (если уровень не последний) в точке соединения и комнату босса (в любом случае) рядом с ней</summary>
        /// <param name="connection_point_exit_location">Координаты точки соединения (где будет находиться комната выхода)</param>
        /// <param name="room_length">Длина комнаты (в ячейках)</param>
        /// <param name="ladder_exit_direction"></param>
        /// <returns>True - если обе комнаты созданы (если последний уровень - только комната босса), false - если нет</returns>
        private bool TryToCreateRoomExitAndRoomBoss(Point connection_point_exit_location, int room_length, DungeonLadderType ladder_exit_direction)
        {
            int connection_point_location_x = connection_point_exit_location.X;
            int connection_point_location_y = connection_point_exit_location.Y;
            if (ladder_exit_direction == DungeonLadderType.Up)
            {
                if (connection_point_exit_location.Y == m_connection_points_number_in_line - 1) return false;
                else connection_point_location_y = connection_point_exit_location.Y + 1;
            }
            else if (ladder_exit_direction == DungeonLadderType.Down)
            {
                if (connection_point_exit_location.Y == 0) return false;
                else connection_point_location_y = connection_point_exit_location.Y - 1;
            }
            else if (ladder_exit_direction == DungeonLadderType.Left)
            {
                if (connection_point_exit_location.X == 0) return false;
                else connection_point_location_x = connection_point_exit_location.X + 1;
            }
            else if (ladder_exit_direction == DungeonLadderType.Right)
            {
                if (connection_point_exit_location.X == m_connection_points_number_in_line - 1) return false;
                else connection_point_location_x = connection_point_exit_location.X - 1;
            }

            if ((m_connection_points[connection_point_exit_location.Y, connection_point_exit_location.X] == DungeonMapConnectionPointType.None) &&
                (m_connection_points[connection_point_location_y, connection_point_location_x] == DungeonMapConnectionPointType.None))
            {
                if (m_ladder_exit_direction != DungeonLadderType.NoLadder)
                {
                    if (TryToCreateRoomWithLadder(connection_point_exit_location, m_ladder_exit_direction, false)) // комната с лестницей
                    {
                        m_connection_points[connection_point_exit_location.Y, connection_point_exit_location.X] = DungeonMapConnectionPointType.RoomExit;
                        m_room_exit_location = connection_point_exit_location;
                    }
                    else return false;
                }
                if (TryToCreateRoom(new Point(connection_point_location_x, connection_point_location_y), room_length, DungeonRoomType.Boss)) // комната босса
                {
                    m_connection_points[connection_point_location_y, connection_point_location_x] = DungeonMapConnectionPointType.RoomBoss;
                    m_room_boss_location = new Point(connection_point_location_x, connection_point_location_y);
                }
                else return false;

                ConnectTwoConnectionPoints(connection_point_exit_location.X, connection_point_exit_location.Y, connection_point_location_x, connection_point_location_y);
                return true;
            }
            else return false;
        }

        /// <summary>Создаёт бонусную комнату в точке соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <param name="room_length">Длина комнаты (в ячейках)</param>
        /// <returns>True - если комната создана, false - если нет</returns>
        private bool TryToCreateRoomBonus(Point connection_point_location, int room_length)
        {
            bool is_ok = TryToCreateRoom(connection_point_location, room_length, DungeonRoomType.Bonus);
            if (is_ok)
            {
                m_connection_points[connection_point_location.Y, connection_point_location.X] = DungeonMapConnectionPointType.RoomBonus;
                m_room_bonus_location = connection_point_location;
            }
            return is_ok;
        }

        /// <summary>Проверяет, соединены ли точки соединения между собой коридором</summary>
        /// <param name="connection_point_1_location_x">X-координата первой точки соединения</param>
        /// <param name="connection_point_1_location_y">Y-координата первой точки соединения</param>
        /// <param name="connection_point_2_location_x">X-координата второй точки соединения</param>
        /// <param name="connection_point_2_location_y">Y-координата второй точки соединения</param>
        /// <returns>True - если точки соединения соединены, false - если нет</returns>
        private bool IsTwoConnectionPointsConnected(int connection_point_1_location_x, int connection_point_1_location_y, int connection_point_2_location_x, int connection_point_2_location_y)
        {
            for (int i = 0; i < m_connection_points_connected.Count; i++)
            {
                if (m_connection_points_connected[i][0].X == connection_point_1_location_x &&
                    m_connection_points_connected[i][0].Y == connection_point_1_location_y &&
                    m_connection_points_connected[i][1].X == connection_point_2_location_x &&
                    m_connection_points_connected[i][1].Y == connection_point_2_location_y) return true;

                if (m_connection_points_connected[i][1].X == connection_point_1_location_x &&
                    m_connection_points_connected[i][1].Y == connection_point_1_location_y &&
                    m_connection_points_connected[i][0].X == connection_point_2_location_x &&
                    m_connection_points_connected[i][0].Y == connection_point_2_location_y) return true;
            }
            return false;
        }

        /// <summary>Проверяет, соединены ли две точки соединения через другие</summary>
        /// <param name="connection_point_1_location">Координаты первой точки соединения</param>
        /// <param name="connection_point_2_location">Координаты второй точки соединения</param>
        /// <param name="connection_points_reserved">Зарезервированные координаты точек соединения (через них нельзя пройти)</param>
        /// <returns>True - если получилось пройти от первой точки соединения ко второй, false - если нет</returns>
        private bool IsTwoConnectionPointsConnectedThrowOther(Point connection_point_1_location, Point connection_point_2_location, List<Point> connection_points_reserved = null)
        {
            bool[,] is_connection_point_checked = new bool[m_connection_points_number_in_line, m_connection_points_number_in_line];
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    is_connection_point_checked[y, x] = false;
                }
            }
            if (connection_points_reserved != null)
            {
                for (int i = 0; i < connection_points_reserved.Count; i++)
                {
                    is_connection_point_checked[connection_points_reserved[i].Y, connection_points_reserved[i].X] = true;
                }
            }
            return IsTwoConnectionPointsConnectedThrowOther_Iteration(ref is_connection_point_checked, connection_point_1_location.X, connection_point_1_location.Y, connection_point_2_location.X, connection_point_2_location.Y);
        }

        /// <summary>Итерация для метода IsTwoConnectionPointsConnectedThrowOther - проверяет, соединена ли точка соединения с целевой точкой соединения через другие</summary>
        /// <param name="is_connection_point_checked">Массив, содержащий информацию о том, какие точки соединения уже были посещены</param>
        /// <param name="connection_point_1_location_x">X-координата проверяемой точки соединения</param>
        /// <param name="connection_point_1_location_y">Y-координата проверяемой точки соединения</param>
        /// <param name="connection_point_2_location_x">X-координата целевой точки соединения</param>
        /// <param name="connection_point_2_location_y">Y-координата целевой точки соединения</param>
        /// <returns>True - если получилось пройти от точки соединения к целевой, false - если нет</returns>
        private bool IsTwoConnectionPointsConnectedThrowOther_Iteration(ref bool[,] is_connection_point_checked, int connection_point_1_location_x, int connection_point_1_location_y, int connection_point_2_location_x, int connection_point_2_location_y)
        {
            if (!is_connection_point_checked[connection_point_1_location_y, connection_point_1_location_x])
            {
                if (IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y))
                {
                    return true;
                }

                is_connection_point_checked[connection_point_1_location_y, connection_point_1_location_x] = true;

                if (connection_point_1_location_y - 1 >= 0 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x, connection_point_1_location_y - 1) &&
                    IsTwoConnectionPointsConnectedThrowOther_Iteration(ref is_connection_point_checked, connection_point_1_location_x, connection_point_1_location_y - 1, connection_point_2_location_x, connection_point_2_location_y)) return true;

                if (connection_point_1_location_y + 1 <= m_connection_points_number_in_line - 1 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x, connection_point_1_location_y + 1) &&
                    IsTwoConnectionPointsConnectedThrowOther_Iteration(ref is_connection_point_checked, connection_point_1_location_x, connection_point_1_location_y + 1, connection_point_2_location_x, connection_point_2_location_y)) return true;

                if (connection_point_1_location_x - 1 >= 0 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x - 1, connection_point_1_location_y) &&
                    IsTwoConnectionPointsConnectedThrowOther_Iteration(ref is_connection_point_checked, connection_point_1_location_x - 1, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y)) return true;

                if (connection_point_1_location_x + 1 <= m_connection_points_number_in_line - 1 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x + 1, connection_point_1_location_y) &&
                    IsTwoConnectionPointsConnectedThrowOther_Iteration(ref is_connection_point_checked, connection_point_1_location_x + 1, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y)) return true;
            }
            return false;
        }

        /// <summary>Проверяет, соединены ли две точки соединения через другие (проверяются и двери - если между точками соединения есть дверь - проход не проверяется)</summary>
        /// <param name="connection_point_1_location">Координаты первой точки соединения</param>
        /// <param name="connection_point_2_location">Координаты второй точки соединения</param>
        /// <param name="connection_points_reserved">Зарезервированные координаты точек соединения (через них нельзя пройти)</param>
        /// <returns>True - если получилось пройти от первой точки соединения ко второй, false - если нет</returns>
        private bool IsTwoConnectionPointsConnectedThrowOther_CheckDoors(Point connection_point_1_location, Point connection_point_2_location, List<Point> connection_points_reserved = null)
        {
            bool[,] is_connection_point_checked = new bool[m_connection_points_number_in_line, m_connection_points_number_in_line];
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    is_connection_point_checked[y, x] = false;
                }
            }
            if (connection_points_reserved != null)
            {
                for (int i = 0; i < connection_points_reserved.Count; i++)
                {
                    is_connection_point_checked[connection_points_reserved[i].Y, connection_points_reserved[i].X] = true;
                }
            }
            return IsTwoConnectionPointsConnectedThrowOther_CheckDoors_Iteration(ref is_connection_point_checked, connection_point_1_location.X, connection_point_1_location.Y, connection_point_2_location.X, connection_point_2_location.Y);
        }

        /// <summary>Итерация для метода IsTwoConnectionPointsConnectedThrowOther_CheckDoors - проверяет, соединена ли точка соединения с целевой точкой соединения через другие (проверяются и двери - если между точками соединения есть дверь - проход не проверяется)</summary>
        /// <param name="is_connection_point_checked">Массив, содержащий информацию о том, какие точки соединения уже были посещены</param>
        /// <param name="connection_point_1_location_x">X-координата проверяемой точки соединения</param>
        /// <param name="connection_point_1_location_y">Y-координата проверяемой точки соединения</param>
        /// <param name="connection_point_2_location_x">X-координата целевой точки соединения</param>
        /// <param name="connection_point_2_location_y">Y-координата целевой точки соединения</param>
        /// <returns>True - если получилось пройти от точки соединения к целевой, false - если нет</returns>
        private bool IsTwoConnectionPointsConnectedThrowOther_CheckDoors_Iteration(ref bool[,] is_connection_point_checked, int connection_point_1_location_x, int connection_point_1_location_y, int connection_point_2_location_x, int connection_point_2_location_y)
        {
            if (!is_connection_point_checked[connection_point_1_location_y, connection_point_1_location_x])
            {
                if (IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y))
                {
                    if (!IsDoorBetweenTwoConnectionPoints(connection_point_1_location_x, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y))
                    {
                        return true;
                    }
                }

                is_connection_point_checked[connection_point_1_location_y, connection_point_1_location_x] = true;

                if (connection_point_1_location_y - 1 >= 0 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x, connection_point_1_location_y - 1) &&
                    !IsDoorBetweenTwoConnectionPoints(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x, connection_point_1_location_y - 1) &&
                    IsTwoConnectionPointsConnectedThrowOther_CheckDoors_Iteration(ref is_connection_point_checked, connection_point_1_location_x, connection_point_1_location_y - 1, connection_point_2_location_x, connection_point_2_location_y)) return true;

                if (connection_point_1_location_y + 1 <= m_connection_points_number_in_line - 1 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x, connection_point_1_location_y + 1) &&
                    !IsDoorBetweenTwoConnectionPoints(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x, connection_point_1_location_y + 1) &&
                    IsTwoConnectionPointsConnectedThrowOther_CheckDoors_Iteration(ref is_connection_point_checked, connection_point_1_location_x, connection_point_1_location_y + 1, connection_point_2_location_x, connection_point_2_location_y)) return true;

                if (connection_point_1_location_x - 1 >= 0 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x - 1, connection_point_1_location_y) &&
                    !IsDoorBetweenTwoConnectionPoints(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x - 1, connection_point_1_location_y) &&
                    IsTwoConnectionPointsConnectedThrowOther_CheckDoors_Iteration(ref is_connection_point_checked, connection_point_1_location_x - 1, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y)) return true;

                if (connection_point_1_location_x + 1 <= m_connection_points_number_in_line - 1 &&
                    IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x + 1, connection_point_1_location_y) &&
                    !IsDoorBetweenTwoConnectionPoints(connection_point_1_location_x, connection_point_1_location_y, connection_point_1_location_x + 1, connection_point_1_location_y) &&
                    IsTwoConnectionPointsConnectedThrowOther_CheckDoors_Iteration(ref is_connection_point_checked, connection_point_1_location_x + 1, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y)) return true;
            }
            return false;
        }

        /// <summary>Возвращает список координат точек соединения, расположенных рядом с указанной и соединённые с ней</summary>
        /// <param name="connection_point_location_x">X-координата точки соединения</param>
        /// <param name="connection_point_location_y">Y-координата точки соединения</param>
        /// <returns>Список координат точек соединения, расположенных рядом с указанной и соединённые с ней</returns>
        private List<Point> GetConnectionPointsConnectedToConnectionPoint(int connection_point_location_x, int connection_point_location_y)
        {
            List<Point> connection_points = new List<Point>();
            if (IsTwoConnectionPointsConnected(connection_point_location_x, connection_point_location_y, connection_point_location_x, connection_point_location_y - 1))
            {
                connection_points.Add(new Point(connection_point_location_x, connection_point_location_y - 1));
            }
            if (IsTwoConnectionPointsConnected(connection_point_location_x, connection_point_location_y, connection_point_location_x, connection_point_location_y + 1))
            {
                connection_points.Add(new Point(connection_point_location_x, connection_point_location_y + 1));
            }
            if (IsTwoConnectionPointsConnected(connection_point_location_x, connection_point_location_y, connection_point_location_x - 1, connection_point_location_y))
            {
                connection_points.Add(new Point(connection_point_location_x - 1, connection_point_location_y));
            }
            if (IsTwoConnectionPointsConnected(connection_point_location_x, connection_point_location_y, connection_point_location_x + 1, connection_point_location_y))
            {
                connection_points.Add(new Point(connection_point_location_x + 1, connection_point_location_y));
            }
            return connection_points;
        }

        /// <summary>Возвращает координаты ячейки в центре точки соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <returns>Координаты ячейки в центре точки соединения</returns>
        private Point GetCellLocationRoomCenterFromConnectionPointLocation(Point connection_point_location)
        {
            return new Point(connection_point_location.X * m_tunnel_length + m_tunnel_length / 2, connection_point_location.Y * m_tunnel_length + m_tunnel_length / 2);
        }

        /// <summary>Возвращает координаты ячейки в левом верхнем углу комнаты, расположенной в указанной точке соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <param name="room_length">Длина комнаты</param>
        /// <returns>Координаты ячейки в левом верхнем углу комнаты, расположенной в указанной точке соединения</returns>
        private Point GetCellLocationRoomLeftUpCornerFromConnectionPoint(Point connection_point_location, int room_length)
        {
            return new Point(connection_point_location.X * m_tunnel_length + m_tunnel_length / 2 - room_length / 2, connection_point_location.Y * m_tunnel_length + m_tunnel_length / 2 - room_length / 2);
        }

        /// <summary>Проверяет, находится ли дверь между двумя точками соединения</summary>
        /// <param name="connection_point_1_location">Координаты первой точки соединения</param>
        /// <param name="connection_point_2_location">Координаты второй точки соединения</param>
        /// <returns>True - если между точками соединения есть дверь, false - если нет</returns>
        private bool IsDoorBetweenTwoConnectionPoints(Point connection_point_1_location, Point connection_point_2_location)
        {
            for (int i = 0; i < m_doors_info.Count; i++)
            {
                if (m_doors_info[i][0] == connection_point_1_location && m_doors_info[i][1] == connection_point_2_location ||
                    m_doors_info[i][1] == connection_point_1_location && m_doors_info[i][0] == connection_point_2_location) return true;
            }
            return false;
        }

        /// <summary>Проверяет, находится ли дверь между двумя точками соединения</summary>
        /// <param name="connection_point_1_location_x">X-координата первой точки соединения</param>
        /// <param name="connection_point_1_location_y">Y-координата первой точки соединения</param>
        /// <param name="connection_point_2_location_x">X-координата второй точки соединения</param>
        /// <param name="connection_point_2_location_y">Y-координата второй точки соединения</param>
        /// <returns>True - если между точками соединения есть дверь, false - если нет</returns>
        private bool IsDoorBetweenTwoConnectionPoints(int connection_point_1_location_x, int connection_point_1_location_y, int connection_point_2_location_x, int connection_point_2_location_y)
        {
            return IsDoorBetweenTwoConnectionPoints(new Point(connection_point_1_location_x, connection_point_1_location_y), new Point(connection_point_2_location_x, connection_point_2_location_y));
        }

        /// <summary>Создаёт дверь (ячейку)</summary>
        /// <param name="connection_point_from_location">Комната, из которой идёт коридор с дверью</param>
        /// <param name="connection_point_to_location">Комната, в которую идёт коридор с дверью</param>
        /// <param name="connection_point_key_location">Комната, в которой будет находится монстр, хранящий ключ от двери</param>
        /// <param name="door_type">Тип двери</param>
        /// <returns>True - если дверь (ячейка) успешно создана, false - если нет</returns>
        private bool CreateDoor(Point connection_point_from_location, Point connection_point_to_location, Point connection_point_key_location, DungeonDoorType door_type)
        {
            if (!IsDoorBetweenTwoConnectionPoints(connection_point_from_location, connection_point_to_location)) // если между комнатами нет двери
            {
                Point connection_point_from_center_location = GetCellLocationRoomCenterFromConnectionPointLocation(connection_point_from_location);
                Point connection_point_to_center_location = GetCellLocationRoomCenterFromConnectionPointLocation(connection_point_to_location);
                Point door_cell_location = new Point(connection_point_from_center_location.X + (connection_point_to_center_location.X - connection_point_from_center_location.X) / 2, connection_point_from_center_location.Y + (connection_point_to_center_location.Y - connection_point_from_center_location.Y) / 2);

                DungeonMapCell dungeon_map_cell;
                if (connection_point_from_location.Y < connection_point_to_location.Y ||
                    connection_point_from_location.Y > connection_point_to_location.Y)
                {
                    if (door_type == DungeonDoorType.Bonus) dungeon_map_cell = DungeonMapCell.FloorAndDoorBonusHorizontal;
                    else if (door_type == DungeonDoorType.Exit) dungeon_map_cell = DungeonMapCell.FloorAndDoorExitHorizontal;
                    else dungeon_map_cell = DungeonMapCell.FloorAndDoorHorizontal;
                }
                else
                {
                    if (door_type == DungeonDoorType.Bonus) dungeon_map_cell = DungeonMapCell.FloorAndDoorBonusVertical;
                    else if (door_type == DungeonDoorType.Exit) dungeon_map_cell = DungeonMapCell.FloorAndDoorExitVertical;
                    else dungeon_map_cell = DungeonMapCell.FloorAndDoorVertical;
                }
                SetCell(door_cell_location, dungeon_map_cell);

                Point[] info = new Point[4];
                info[0] = connection_point_from_location;
                info[1] = connection_point_to_location;
                info[2] = connection_point_key_location;
                info[3] = door_cell_location;
                m_doors_info.Add(info);
                return true;
            }
            return false;
        }

        /// <summary>Создаёт дверь (ячейку): комната, в которую потом поместится ключ, либо находится, либо нет (если комната бонусная)</summary>
        /// <param name="connection_point_from_location">Комната, из которой идёт коридор с дверью</param>
        /// <param name="connection_point_to_location">Комната, в которую идёт коридор с дверью</param>
        /// <param name="is_bonus">Является ли бонусной - тогда ключа не будет, иначе - находится комната, в которой будет находится монстр, хранящий ключ от двери</param>
        /// <returns>True - если дверь (ячейка) успешно создана, false - если нет</returns>
        private bool CreateDoor(Point connection_point_from_location, Point connection_point_to_location, bool is_bonus = false)
        {
            if (is_bonus)
            {
                return CreateDoor(connection_point_from_location, connection_point_to_location, new Point(-1, -1), DungeonDoorType.Bonus);
            }
            else
            {
                int connection_point_key_location_x = m_world_key.Next(0, m_connection_points_number_in_line);
                int connection_point_key_location_y = m_world_key.Next(0, m_connection_points_number_in_line);
                int start_x = connection_point_key_location_x;
                int start_y = connection_point_key_location_y;
                Point connection_point_key_location = new Point(connection_point_key_location_x, connection_point_key_location_y);
                List<Point> points_reserved = new List<Point>();
                points_reserved.Add(connection_point_to_location);
                int iterator = 1;
                if (IsGlobalProcentWork(50)) iterator = -1;
                while (!IsTwoConnectionPointsConnectedThrowOther_CheckDoors(connection_point_from_location, connection_point_key_location, points_reserved) ||
                       !IsTwoConnectionPointsConnectedThrowOther_CheckDoors(m_room_entrance_location, connection_point_key_location, points_reserved) ||
                       m_connection_points[connection_point_key_location.Y, connection_point_key_location.X] != DungeonMapConnectionPointType.RoomUsusal ||
                       connection_point_key_location == connection_point_from_location ||
                       connection_point_key_location == connection_point_to_location)
                {
                    if (connection_point_key_location_x + iterator == m_connection_points_number_in_line)
                    {
                        connection_point_key_location_x = 0;
                        if (connection_point_key_location_y + iterator == m_connection_points_number_in_line)
                        {
                            connection_point_key_location_y = 0;
                        }
                        else connection_point_key_location_y += iterator;
                    }
                    else if (connection_point_key_location_x + iterator == -1)
                    {
                        connection_point_key_location_x = m_connection_points_number_in_line - 1;
                        if (connection_point_key_location_y + iterator == -1)
                        {
                            connection_point_key_location_y = m_connection_points_number_in_line - 1;
                        }
                        else connection_point_key_location_y += iterator;
                    }
                    else connection_point_key_location_x += iterator;

                    if (connection_point_key_location_x == start_x && connection_point_key_location_y == start_y) return false;

                    connection_point_key_location = new Point(connection_point_key_location_x, connection_point_key_location_y);
                }
                return CreateDoor(connection_point_from_location, connection_point_to_location, connection_point_key_location, DungeonDoorType.Usual);
            }
        }

        /// <summary>Создаёт двери (как ячейки)</summary>
        private void CreateDoors()
        {
            for (int y = 0; y < m_connection_points_number_in_line; y++)
            {
                for (int x = 0; x < m_connection_points_number_in_line; x++)
                {
                    Point room_location = new Point(x, y);
                    if (room_location != m_room_entrance_location &&
                        room_location != m_room_exit_location &&
                        room_location != m_room_bonus_location)
                    {
                        List<Point> connection_points_connected = GetConnectionPointsConnectedToConnectionPoint(x, y);
                        Point connection_point_before = new Point(-1, -1);
                        if (connection_points_connected.Count == 2) // если комната "проходная"
                        {
                            List<Point> points_reserved = new List<Point>();
                            points_reserved.Add(room_location);
                            if (IsTwoConnectionPointsConnectedThrowOther(connection_points_connected[0], m_room_entrance_location, points_reserved) &&
                                !IsTwoConnectionPointsConnectedThrowOther(connection_points_connected[1], m_room_entrance_location, points_reserved))
                            {
                                connection_point_before = connection_points_connected[0];
                            }
                            else if (IsTwoConnectionPointsConnectedThrowOther(connection_points_connected[1], m_room_entrance_location, points_reserved) &&
                                !IsTwoConnectionPointsConnectedThrowOther(connection_points_connected[0], m_room_entrance_location, points_reserved))
                            {
                                connection_point_before = connection_points_connected[1];
                            }
                        }
                        else if (connection_points_connected.Count == 1)
                        {
                            connection_point_before = connection_points_connected[0];
                        }

                        if (connection_point_before != new Point(-1, -1))
                        {
                            if (!IsProcentWork(procent_chance_not_create_door))
                            {
                                CreateDoor(connection_point_before, room_location);
                            }
                        }
                    }
                }
            }

            // дверь из комнаты босса к лестнице на следующий уровень
            if (m_ladder_exit_direction != DungeonLadderType.NoLadder)
            {
                CreateDoor(m_room_boss_location, m_room_exit_location, m_room_boss_location, DungeonDoorType.Exit);
            }

            // дверь в бонусную комнату
            CreateDoor(GetConnectionPointsConnectedToConnectionPoint(m_room_bonus_location.X, m_room_bonus_location.Y)[0], m_room_bonus_location, true);
        }

        /// <summary>Переводит ячейки карты в объекты</summary>
        private void CreateObjectsFromCells()
        {
            List<DungeonChest> m_chests_bonus = new List<DungeonChest>();

            // преобразование ячеек карты в объекты
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    double block_x = x * block_length + block_length / 2;
                    double block_y = y * block_length + block_length / 2;
                    if (IsCellContainsFloor(x, y))
                    {
                        DungeonBlock block = new DungeonBlock(m_image_floor, DungeonObjectCollision.NoCollision);
                        block.SetLocation(block_x, block_y);
                        Add(block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.Wall)
                    {
                        DungeonBlock block = new DungeonBlock(m_image_wall, DungeonObjectCollision.StaticCollision);
                        block.SetLocation(block_x, block_y);
                        Add(block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.WallDark)
                    {
                        DungeonBlock block = new DungeonBlock(m_image_wall_dark, DungeonObjectCollision.StaticCollision);
                        block.SetLocation(block_x, block_y);
                        Add(block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.WallDarkDark)
                    {
                        DungeonBlock block = new DungeonBlock(m_image_wall_dark_dark, DungeonObjectCollision.StaticCollision);
                        block.SetLocation(block_x, block_y);
                        Add(block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.EntranceLadderUp)
                    {
                        m_ladder_entrance_block = new DungeonBlock(m_image_ladder_up, DungeonObjectCollision.NoCollision);
                        m_ladder_entrance_block.SetLocation(block_x, block_y);
                        Add(m_ladder_entrance_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.EntranceLadderDown)
                    {
                        m_ladder_entrance_block = new DungeonBlock(m_image_ladder_down, DungeonObjectCollision.NoCollision);
                        m_ladder_entrance_block.SetLocation(block_x, block_y);
                        Add(m_ladder_entrance_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.EntranceLadderLeft)
                    {
                        m_ladder_entrance_block = new DungeonBlock(m_image_ladder_left, DungeonObjectCollision.NoCollision);
                        m_ladder_entrance_block.SetLocation(block_x, block_y);
                        Add(m_ladder_entrance_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.EntranceLadderRight)
                    {
                        m_ladder_entrance_block = new DungeonBlock(m_image_ladder_right, DungeonObjectCollision.NoCollision);
                        m_ladder_entrance_block.SetLocation(block_x, block_y);
                        Add(m_ladder_entrance_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.ExitLadderUp)
                    {
                        m_ladder_exit_block = new DungeonBlock(m_image_ladder_up, DungeonObjectCollision.NoCollision);
                        m_ladder_exit_block.SetLocation(block_x, block_y);
                        Add(m_ladder_exit_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.ExitLadderDown)
                    {
                        m_ladder_exit_block = new DungeonBlock(m_image_ladder_down, DungeonObjectCollision.NoCollision);
                        m_ladder_exit_block.SetLocation(block_x, block_y);
                        Add(m_ladder_exit_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.ExitLadderLeft)
                    {
                        m_ladder_exit_block = new DungeonBlock(m_image_ladder_left, DungeonObjectCollision.NoCollision);
                        m_ladder_exit_block.SetLocation(block_x, block_y);
                        Add(m_ladder_exit_block);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.ExitLadderRight)
                    {
                        m_ladder_exit_block = new DungeonBlock(m_image_ladder_right, DungeonObjectCollision.NoCollision);
                        m_ladder_exit_block.SetLocation(block_x, block_y);
                        Add(m_ladder_exit_block);
                    }

                    if (m_cells[y, x] == DungeonMapCell.FloorAndChest ||
                        m_cells[y, x] == DungeonMapCell.FloorAndChestBonus)
                    {
                        DungeonChest chest = new DungeonChest();
                        chest.SetLocation(block_x - 23 + chest.Image.Width / 2, block_y);
                        if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus) m_chests_bonus.Add(chest);
                        else Add(chest);
                        int generated_items_number = 0;
                        int max_items = m_world_key.Next(1 + min_items_in_chest, 1 + min_items_in_chest + (int)(max_items_in_chest * GetProcentWithDifficulty() / 100));
                        int procent_chance_effect_set_max_value = m_chance_procent * (m_level_id + 1) / 10;
                        for (int i = 0; i < max_items; i++)
                        {
                            DungeonItemEquipment item = null;

                            SetStatType[] stats_types = new SetStatType[9];
                            for (int i2 = 0; i2 < 9; i2++)
                            {
                                stats_types[i2] = SetStatType.NotSet;
                            }

                            int effects_number = 1;

                            int k = 1;
                            if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus) k *= 3;

                            if (IsProcentWork(procent_chance_create_item_potion)) // зелье
                            {
                                if (IsProcentWork(procent_chance_potion_set_three_effects * k))
                                {
                                    effects_number = 3;
                                }
                                else if (IsProcentWork(procent_chance_potion_set_two_effects * k))
                                {
                                    effects_number = 2;
                                }
                                item = new DungeonItemPotion(effects_number - 1, m_world_key.Next(0, 3));
                                if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus)
                                {
                                    for (int i2 = 0; i2 < 9; i2++)
                                    {
                                        stats_types[i2] = SetStatType.CanSetPlus;
                                    }
                                }
                                else
                                {
                                    for (int i2 = 0; i2 < 9; i2++)
                                    {
                                        stats_types[i2] = SetStatType.CanSet;
                                    }
                                }
                            }
                            else if (IsProcentWork(procent_chance_create_item_helmet)) // шлем
                            {
                                effects_number = m_world_key.Next(1, 4);
                                item = new DungeonItemArmour(m_level_id + m_world_key.Next(0, 3) - 1, m_world_key.Next(0, 3));
                                if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus)
                                {
                                    stats_types[(int)(DungeonStats.MaxHealth)] = SetStatType.MustSetPlus;
                                    stats_types[(int)(DungeonStats.Regeneration)] = SetStatType.CanSetPlus;
                                }
                                else
                                {
                                    stats_types[(int)(DungeonStats.MaxHealth)] = SetStatType.MustSetPlus;
                                    stats_types[(int)(DungeonStats.Regeneration)] = SetStatType.CanSet;
                                    stats_types[(int)(DungeonStats.Speed)] = SetStatType.CanSetMinus;
                                }
                            }
                            else if (IsProcentWork(procent_chance_create_item_armour)) // броня
                            {
                                effects_number = m_world_key.Next(1, 4);
                                item = new DungeonItemHelmet(m_level_id + m_world_key.Next(0, 3) - 1, m_world_key.Next(0, 3));
                                if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus)
                                {
                                    stats_types[(int)(DungeonStats.MaxEnergy)] = SetStatType.MustSetPlus;
                                    stats_types[(int)(DungeonStats.Restore)] = SetStatType.CanSetPlus;
                                }
                                else
                                {
                                    stats_types[(int)(DungeonStats.MaxEnergy)] = SetStatType.MustSetPlus;
                                    stats_types[(int)(DungeonStats.Restore)] = SetStatType.CanSet;
                                    stats_types[(int)(DungeonStats.Speed)] = SetStatType.CanSetMinus;
                                }
                            }
                            else if (IsProcentWork(procent_chance_create_item_sword)) // меч
                            {
                                effects_number = m_world_key.Next(1, 3);
                                item = new DungeonItemSword(m_level_id + m_world_key.Next(0, 3) - 1);
                                if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus)
                                {
                                    stats_types[(int)(DungeonStats.Power)] = SetStatType.MustSetPlus;
                                }
                                else
                                {
                                    stats_types[(int)(DungeonStats.Power)] = SetStatType.MustSetPlus;
                                    stats_types[(int)(DungeonStats.Mobility)] = SetStatType.CanSetMinus;
                                }
                            }
                            else if (IsProcentWork(procent_chance_create_item_artifact)) // артефакт
                            {
                                if (IsProcentWork(procent_chance_artifact_set_three_effects * k))
                                {
                                    effects_number = 3;
                                }
                                else if (IsProcentWork(procent_chance_artifact_set_two_effects * k))
                                {
                                    effects_number = 2;
                                }
                                item = new DungeonItemArtifact(m_level_id + (effects_number - 1) * 10, m_world_key.Next(0, 6));
                                if (m_cells[y, x] == DungeonMapCell.FloorAndChestBonus)
                                {
                                    for (int i2 = 0; i2 < 9; i2++)
                                    {
                                        stats_types[i2] = SetStatType.CanSetPlus;
                                    }
                                }
                                else
                                {
                                    for (int i2 = 0; i2 < 9; i2++)
                                    {
                                        stats_types[i2] = SetStatType.CanSet;
                                    }
                                }
                            }
                            if (item != null)
                            {
                                SetEffects(item, stats_types, effects_number, procent_chance_effect_set_max_value);
                                chest.Container.Items.Add(item);
                                generated_items_number++;
                            }

                            if (i == max_items - 1 && generated_items_number <= min_items_in_chest) i--;
                        }
                    }
                    else if (m_cells[y, x] == DungeonMapCell.FloorAndMonster)
                    {
                        DungeonMonster monster = CreateMonsterInConnectionPoint(GetConnectionPointLocationFromCellLocation(new Point(x, y)));
                        m_monsters_in_room[y / m_tunnel_length, x / m_tunnel_length].Add(monster);
                        monster.SetLocation(block_x, block_y);
                        Add(monster);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.FloorAndMonsterBoss)
                    {
                        int texture_id = m_world_key.Next(1, 6);
                        Bitmap texture = Properties.Resources.TEXTURE_monster_1;
                        if (texture_id == 2) texture = Properties.Resources.TEXTURE_monster_2;
                        else if (texture_id == 3) texture = Properties.Resources.TEXTURE_monster_3;
                        else if (texture_id == 4) texture = Properties.Resources.TEXTURE_monster_4;
                        else if (texture_id == 5) texture = Properties.Resources.TEXTURE_monster_5;
                        DungeonMonster monster_boss = new DungeonMonster(texture, "Босс уровня", true);
                        if (m_ladder_exit_direction == DungeonLadderType.NoLadder) // если уровень - последний
                        {
                            monster_boss = new DungeonMonster(texture, "Босс подземелья", true);
                        }
                        monster_boss.SetLocation(block_x, block_y);
                        monster_boss.AttachToConnectionPoint(GetConnectionPointLocationFromCellLocation(new Point(x, y)));
                        monster_boss.PlusExp((m_level_id + m_world_key.Next(0, 11 - m_chance_procent / 10)) * 10 * 3);
                        monster_boss.DistributeAllSkillPoints(ref m_world_key);
                        monster_boss.FullStats();
                        Add(monster_boss);
                        EquipMonster(monster_boss);
                        if (m_ladder_exit_direction == DungeonLadderType.NoLadder) // если уровень - последний
                        {
                            DungeonItemArtifact artifact = new DungeonItemArtifact(31, 0, "Особый артефакт", "Артефакт, который хранил босс подземелья. В природе не встречается. Происхождение неизвестно.");
                            for (int i = 0; i < 9; i++)
                            {
                                if (i != (int)DungeonStats.Speed)
                                {
                                    artifact.AddEffect((DungeonStats)i, DungeonStatsInfo.Plus((DungeonStats)i) * 20);
                                }
                            }
                            monster_boss.Container.Add(artifact);
                        }
                        m_monsters_in_room[y / m_tunnel_length, x / m_tunnel_length].Add(monster_boss);
                    }
                }
            }

            DungeonItemKey.ResetColorsCount();

            // преобразование ячеек карты в двери
            for (int y = 0; y < CellsNumberInLine; y++)
            {
                for (int x = 0; x < CellsNumberInLine; x++)
                {
                    if (m_cells[y, x] == DungeonMapCell.FloorAndDoorVertical ||
                        m_cells[y, x] == DungeonMapCell.FloorAndDoorHorizontal ||
                        m_cells[y, x] == DungeonMapCell.FloorAndDoorExitVertical ||
                        m_cells[y, x] == DungeonMapCell.FloorAndDoorExitHorizontal)
                    {
                        double block_x = x * block_length + block_length / 2;
                        double block_y = y * block_length + block_length / 2;
                        DungeonItemKey key = new DungeonItemKey(Id, DungeonItemKey.GetNextColor(ref m_world_key));
                        if (m_cells[y, x] == DungeonMapCell.FloorAndDoorExitVertical ||
                        m_cells[y, x] == DungeonMapCell.FloorAndDoorExitHorizontal)
                        {
                            key = new DungeonItemKey(Id, Color.Red);
                        }
                        int id_in_list = -1;
                        for (int i = 0; i < m_doors_info.Count; i++)
                        {
                            if (m_doors_info[i][3] == new Point(x, y))
                            {
                                id_in_list = i;
                                break;
                            }
                        }
                        Point room_key_location = m_doors_info[id_in_list][2]; // id точки соединения, в которой должен находиться ключ
                        if (m_monsters_in_room[room_key_location.Y, room_key_location.X].Count == 0) // если в комнате с ключом нет монстра - создаём монстра
                        {
                            DungeonMonster monster = CreateMonsterInConnectionPoint(room_key_location);
                            m_monsters_in_room[room_key_location.Y, room_key_location.X].Add(monster);
                            double block_x2 = (m_rooms_locations[room_key_location.Y, room_key_location.X].X + m_rooms_lengths[room_key_location.Y, room_key_location.X] / 2 - 1) * block_length + block_length / 2;
                            double block_y2 = (m_rooms_locations[room_key_location.Y, room_key_location.X].Y + m_rooms_lengths[room_key_location.Y, room_key_location.X] / 2 - 1) * block_length + block_length / 2;
                            monster.SetLocation(block_x2, block_y2);
                            Add(monster);
                        }
                        int id_monster = m_world_key.Next(0, m_monsters_in_room[room_key_location.Y, room_key_location.X].Count);
                        m_monsters_in_room[room_key_location.Y, room_key_location.X][id_monster].GiveKey(key);
                        m_monsters_in_room[room_key_location.Y, room_key_location.X][id_monster].AttachToConnectionPoint(room_key_location);
                        DungeonDoorImageType door_type = DungeonDoorImageType.Vertical;
                        if (m_cells[y, x] == DungeonMapCell.FloorAndDoorHorizontal || m_cells[y, x] == DungeonMapCell.FloorAndDoorExitHorizontal)
                        {
                            door_type = DungeonDoorImageType.Horizontal;
                        }
                        DungeonDoor door = new DungeonDoor(door_type, key);
                        door.SetLocation(block_x, block_y);
                        Add(door);
                    }
                    else if (m_cells[y, x] == DungeonMapCell.FloorAndDoorBonusVertical ||
                        m_cells[y, x] == DungeonMapCell.FloorAndDoorBonusHorizontal)
                    {
                        double block_x = x * block_length + block_length / 2;
                        double block_y = y * block_length + block_length / 2;

                        byte[] code_numbers = new byte[4];
                        code_numbers[0] = (byte)m_world_key.Next(0, 10);
                        code_numbers[1] = (byte)m_world_key.Next(0, 10);
                        code_numbers[2] = (byte)m_world_key.Next(0, 10);
                        code_numbers[3] = (byte)m_world_key.Next(0, 10);
                        for (byte i = 0; i < 4; i++)
                        {
                            m_papers[i].GenerateTextWithNumber(ref m_world_key, code_numbers[i], i);
                        }

                        string code = code_numbers[0].ToString() + code_numbers[1].ToString() + code_numbers[2].ToString() + code_numbers[3].ToString();

                        DungeonDoorImageType door_type = DungeonDoorImageType.Vertical;
                        if (m_cells[y, x] == DungeonMapCell.FloorAndDoorBonusHorizontal)
                        {
                            door_type = DungeonDoorImageType.Horizontal;
                        }
                        DungeonDoor door = new DungeonDoor(door_type, code, m_papers);
                        door.SetLocation(block_x, block_y);
                        Add(door);
                    }
                }
            }

            if (m_chests.Count == 0) // если вдруг сундуков нет - записки даются рандомному монстру
            {
                int id = 0;
                while (id < Creatures.Count || (Creatures[id] as DungeonMonster).IsBoss)
                {
                    id++;
                }
                if (id != Creatures.Count)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Creatures[id].Container.Items.Add(m_papers[i]);
                    }
                }
            }
            else
            {
                // занесение записок в сундуки (которые не в бонусной комнате)
                for (int i = 0; i < 4; i++)
                {
                    int chest_id = m_world_key.Next(0, m_chests.Count);
                    m_chests[chest_id].Container.Items.Add(m_papers[i]);
                }
            }

            for (int i = 0; i < m_chests_bonus.Count; i++)
            {
                Add(m_chests_bonus[i]);
            }
        }

        /// <summary>Экипирует монстра</summary>
        /// <param name="monster">Монстр</param>
        private void EquipMonster(DungeonMonster monster)
        {
            int procent_chance_effect_set_max_value = (100 - m_chance_procent) * (m_level_id + 1) / 10;
            if (monster.IsBoss)
            {
                procent_chance_effect_set_max_value = (int)(((double)procent_chance_effect_set_max_value / 100) * 3 * 100); // босс в три раза сильнее обычного монстра
            }

            DungeonItemEquipment item;

            SetStatType[] stats_types = new SetStatType[9];

            int effects_number;

            if (!IsProcentWork(procent_chance_monster_not_create_one_item) || (monster.IsBoss && !IsProcentWork(procent_chance_monster_boss_not_create_one_item))) // броня
            {
                effects_number = m_world_key.Next(1, 4);
                item = new DungeonItemArmour(m_level_id + m_world_key.Next(0, 3) - 1, m_world_key.Next(0, 3));
                for (int i2 = 0; i2 < 9; i2++)
                {
                    stats_types[i2] = SetStatType.NotSet;
                }
                stats_types[(int)(DungeonStats.MaxHealth)] = SetStatType.MustSetPlus;
                stats_types[(int)(DungeonStats.Regeneration)] = SetStatType.CanSetPlus;
                stats_types[(int)(DungeonStats.Speed)] = SetStatType.MustSetMinus;
                SetEffects(item, stats_types, effects_number, procent_chance_effect_set_max_value);
                monster.Container.Items.Add(item);
                monster.ContainerSpecialItems.Set(item, 1);
            }
            if (!IsProcentWork(procent_chance_monster_not_create_one_item) || (monster.IsBoss && !IsProcentWork(procent_chance_monster_boss_not_create_one_item))) // шлем
            {
                effects_number = m_world_key.Next(1, 4);
                item = new DungeonItemHelmet(m_level_id + m_world_key.Next(0, 3) - 1, m_world_key.Next(0, 3));
                for (int i2 = 0; i2 < 9; i2++)
                {
                    stats_types[i2] = SetStatType.NotSet;
                }
                stats_types[(int)(DungeonStats.MaxEnergy)] = SetStatType.MustSetPlus;
                stats_types[(int)(DungeonStats.Restore)] = SetStatType.CanSetPlus;
                stats_types[(int)(DungeonStats.Speed)] = SetStatType.MustSetMinus;
                SetEffects(item, stats_types, effects_number, procent_chance_effect_set_max_value);
                monster.Container.Items.Add(item);
                monster.ContainerSpecialItems.Set(item, 0);
            }
            if (true) // меч
            {
                effects_number = m_world_key.Next(1, 3);
                item = new DungeonItemSword(m_level_id + m_world_key.Next(0, 3) - 1);
                for (int i2 = 0; i2 < 9; i2++)
                {
                    stats_types[i2] = SetStatType.NotSet;
                }
                stats_types[(int)(DungeonStats.Power)] = SetStatType.MustSetPlus;
                stats_types[(int)(DungeonStats.Mobility)] = SetStatType.CanSetMinus;
                SetEffects(item, stats_types, effects_number, procent_chance_effect_set_max_value);
                monster.Container.Items.Add(item);
                monster.ContainerSpecialItems.Set(item, 3);
            }
            int max_potions = m_world_key.Next(0, 4);
            for (int i = 0; i < max_potions; i++) // зелья
            {
                if (!IsProcentWork(procent_chance_monster_not_create_one_item) || (monster.IsBoss && !IsProcentWork(procent_chance_monster_boss_not_create_one_item)))
                {
                    if (IsGlobalProcentWork(procent_chance_potion_set_three_effects))
                    {
                        effects_number = 3;
                    }
                    else if (IsGlobalProcentWork(procent_chance_potion_set_two_effects))
                    {
                        effects_number = 2;
                    }
                    else
                    {
                        effects_number = 1;
                    }
                    item = new DungeonItemPotion(effects_number - 1, m_world_key.Next(0, 3));
                    for (int i2 = 0; i2 < 9; i2++)
                    {
                        stats_types[i2] = SetStatType.CanSet;
                    }
                    SetEffects(item, stats_types, effects_number, procent_chance_effect_set_max_value);
                    monster.Container.Items.Add(item);
                    monster.ContainerSpecialItems.Set(item, 4 + i);
                }
            }
            if ((IsGlobalProcentWork(procent_chance_create_item_artifact) && !IsProcentWork(procent_chance_monster_not_create_one_item)) || (monster.IsBoss && !IsProcentWork(procent_chance_monster_boss_not_create_one_item))) // артефакт
            {
                if (IsGlobalProcentWork(procent_chance_artifact_set_three_effects))
                {
                    effects_number = 3;
                }
                else if (IsGlobalProcentWork(procent_chance_artifact_set_two_effects))
                {
                    effects_number = 2;
                }
                else
                {
                    effects_number = 1;
                }
                item = new DungeonItemArtifact(m_level_id + (effects_number - 1) * 10, m_world_key.Next(0, 6));
                for (int i2 = 0; i2 < 9; i2++)
                {
                    stats_types[i2] = SetStatType.CanSetPlus;
                }
                SetEffects(item, stats_types, effects_number, procent_chance_effect_set_max_value);
                monster.ContainerSpecialItems.Set(item, 2);
            }
        }

        /// <summary>Создаёт эффекты для экипировки</summary>
        /// <param name="item">Предмет экипировки</param>
        /// <param name="stats_types">Список типов задания характеристик</param>
        /// <param name="max_effects_number">Максимальное количество эффектов</param>
        /// <param name="procent_chance_effect_set_max_value">Вероятность (в процентах) задать максимальную планку для возможного множителя эффекта</param>
        private void SetEffects(DungeonItemEquipment item, SetStatType[] stats_types, int max_effects_number, int procent_chance_effect_set_max_value)
        {
            int duration = -1;
            if (item is DungeonItemPotion)
            {
                duration = 5 * m_world_key.Next(1, 3 + (int)(effect_max_duration_nultiplier * ((double)procent_chance_effect_set_max_value / 100)));
            }

            bool[] was_effect_created = new bool[9];
            for (int i = 0; i < 9; i++)
            {
                was_effect_created[i] = false;
            }

            int must_set_effects_number = 0;
            for (int i = 0; i < 9; i++)
            {
                if (stats_types[i] == SetStatType.MustSetPlus || stats_types[i] == SetStatType.MustSetMinus)
                {
                    must_set_effects_number++;
                }
            }

            int added_effects_number = 0;
            while (added_effects_number < max_effects_number || must_set_effects_number != 0)
            {
                int i = m_world_key.Next(0, 9);
                int increment = 1;
                if (IsGlobalProcentWork(50)) increment = -1;

                int saved_i = i;
                while (was_effect_created[i] || stats_types[i] == SetStatType.NotSet)
                {
                    i += increment;
                    if (i == 9) i = 0;
                    if (i == -1) i = 8;
                    if (i == saved_i) return;
                }

                DungeonStats stat = (DungeonStats)i;
                double value = 0;
                if ((IsGlobalProcentWork(procent_chance_effect_is_plus) || stats_types[i] == SetStatType.MustSetPlus) &&
                    stats_types[i] != SetStatType.MustSetMinus &&
                    stats_types[i] != SetStatType.CanSetMinus) // вероятность создать положительный эффект
                {
                    int multiplier = m_world_key.Next(1, 3 + (int)(effect_max_value_multiplier * ((double)procent_chance_effect_set_max_value / 100)));
                    value = m_world_key.Next(1, (int)DungeonStatsInfo.Plus(stat) + 1) * multiplier;
                }
                else if ((IsGlobalProcentWork(procent_chance_effect_is_minus) || stats_types[i] == SetStatType.MustSetMinus) &&
                    stats_types[i] != SetStatType.MustSetPlus &&
                    stats_types[i] != SetStatType.CanSetPlus) // вероятность создать отрицательный эффект
                {
                    int multiplier = m_world_key.Next(1, 3 + (int)((effect_max_value_multiplier / 2) * ((double)procent_chance_effect_set_max_value / 100)));
                    value = -m_world_key.Next(1, (int)DungeonStatsInfo.Plus(stat) + 1) * multiplier;
                }
                if (value != 0)
                {
                    item.AddEffect(stat, value, duration);
                    was_effect_created[i] = true;
                    added_effects_number++;
                    if (stats_types[i] == SetStatType.MustSetPlus || stats_types[i] == SetStatType.MustSetMinus)
                    {
                        must_set_effects_number--;
                    }
                }
            }
        }

        /// <summary>Соединяет две точки соединения</summary>
        /// <param name="connection_point_1_location_x">X-координата первой точки соединения</param>
        /// <param name="connection_point_1_location_y">Y-координата первой точки соединения</param>
        /// <param name="connection_point_2_location_x">X-координата второй точки соединения</param>
        /// <param name="connection_point_2_location_y">Y-координата второй точки соединения</param>
        private void ConnectTwoConnectionPoints(int connection_point_1_location_x, int connection_point_1_location_y, int connection_point_2_location_x, int connection_point_2_location_y)
        {
            if (!IsTwoConnectionPointsConnected(connection_point_1_location_x, connection_point_1_location_y, connection_point_2_location_x, connection_point_2_location_y))
            {
                Point[] connection_points_connected = new Point[2];
                connection_points_connected[0] = new Point(connection_point_1_location_x, connection_point_1_location_y);
                connection_points_connected[1] = new Point(connection_point_2_location_x, connection_point_2_location_y);
                m_connection_points_connected.Add(connection_points_connected);
                connection_points_connected[1] = new Point(connection_point_1_location_x, connection_point_1_location_y);
                connection_points_connected[0] = new Point(connection_point_2_location_x, connection_point_2_location_y);
                m_connection_points_connected.Add(connection_points_connected);
            }
        }

        /// <summary>Соединяет точку соединения с некоторыми из находящихся вокруг</summary>
        /// <param name="connection_point_location_x">X-координата точки соединения</param>
        /// <param name="connection_point_location_y">Y-координата точки соединения</param>
        /// <returns></returns>
        private bool ConnectConnectionPointToConnectionPointsAround(int connection_point_location_x, int connection_point_location_y)
        {
            bool was_connected_to_another = false;

            int[] near_connection_point_location_x = new int[4];
            int[] near_connection_point_location_y = new int[4];

            near_connection_point_location_x[0] = connection_point_location_x; near_connection_point_location_y[0] = connection_point_location_y - 1;
            near_connection_point_location_x[1] = connection_point_location_x; near_connection_point_location_y[1] = connection_point_location_y + 1;
            near_connection_point_location_x[2] = connection_point_location_x - 1; near_connection_point_location_y[2] = connection_point_location_y;
            near_connection_point_location_x[3] = connection_point_location_x + 1; near_connection_point_location_y[3] = connection_point_location_y;

            for (int i = 0; i < 4; i++)
            {
                if (was_connected_to_another)
                {
                    if (IsGlobalProcentWork(procent_chance_room_connect_to_another)) break; // с такой вероятностью комната не будет соединяться дальше, если уже была соединена
                }

                if ((near_connection_point_location_x[i] >= 0 &&
                    near_connection_point_location_x[i] < m_connection_points_number_in_line &&
                    near_connection_point_location_y[i] >= 0 &&
                    near_connection_point_location_y[i] < m_connection_points_number_in_line) &&
                   m_connection_points[near_connection_point_location_y[i], near_connection_point_location_x[i]] != DungeonMapConnectionPointType.RoomEntrance &&
                   m_connection_points[near_connection_point_location_y[i], near_connection_point_location_x[i]] != DungeonMapConnectionPointType.RoomExit &&
                   m_connection_points[near_connection_point_location_y[i], near_connection_point_location_x[i]] != DungeonMapConnectionPointType.RoomBoss &&
                   m_connection_points[near_connection_point_location_y[i], near_connection_point_location_x[i]] != DungeonMapConnectionPointType.RoomBonus &&
                   (!IsTwoConnectionPointsConnected(connection_point_location_x, connection_point_location_y, near_connection_point_location_x[i], near_connection_point_location_y[i])))
                {
                    bool is_ok = true;
                    if (m_connection_points[connection_point_location_y, connection_point_location_x] == DungeonMapConnectionPointType.RoomEntrance)
                    {
                        if (m_ladder_entrance_direction == DungeonLadderType.Up && i == 1) is_ok = true;
                        else if (m_ladder_entrance_direction == DungeonLadderType.Down && i == 0) is_ok = true;
                        else if (m_ladder_entrance_direction == DungeonLadderType.Left && i == 3) is_ok = true;
                        else if (m_ladder_entrance_direction == DungeonLadderType.Right && i == 2) is_ok = true;
                        else if (m_ladder_entrance_direction == DungeonLadderType.NoLadder && i == 0) is_ok = true;
                        else is_ok = false;
                    }
                    else if (m_connection_points[connection_point_location_y, connection_point_location_x] == DungeonMapConnectionPointType.RoomExit)
                    {
                        is_ok = false;
                    }
                    else if (m_connection_points[connection_point_location_y, connection_point_location_x] == DungeonMapConnectionPointType.RoomBoss)
                    {
                        if (!was_connected_to_another)
                        {
                            if (m_ladder_exit_direction == DungeonLadderType.Up && i != 0) is_ok = true;
                            else if (m_ladder_exit_direction == DungeonLadderType.Down && i != 1) is_ok = true;
                            else if (m_ladder_exit_direction == DungeonLadderType.Left && i != 2) is_ok = true;
                            else if (m_ladder_exit_direction == DungeonLadderType.Right && i != 3) is_ok = true;
                            else if (m_ladder_exit_direction == DungeonLadderType.NoLadder) is_ok = true;
                            else is_ok = false;
                        }
                        else is_ok = false;
                    }
                    else if (m_connection_points[connection_point_location_y, connection_point_location_x] == DungeonMapConnectionPointType.RoomBonus)
                    {
                        if (GetConnectionPointsConnectedToConnectionPoint(connection_point_location_x, connection_point_location_y).Count != 0) is_ok = false;
                    }
                    if (is_ok)
                    {
                        if (m_connection_points[connection_point_location_y, connection_point_location_x] != DungeonMapConnectionPointType.None &&
                            m_connection_points[near_connection_point_location_y[i], near_connection_point_location_x[i]] != DungeonMapConnectionPointType.None)
                        {
                            ConnectTwoConnectionPoints(connection_point_location_x, connection_point_location_y, near_connection_point_location_x[i], near_connection_point_location_y[i]);
                        }
                        was_connected_to_another = true;
                    }
                }
            }
            if (GetConnectionPointsConnectedToConnectionPoint(connection_point_location_x, connection_point_location_y).Count != 0 && m_connection_points[connection_point_location_y, connection_point_location_x] != DungeonMapConnectionPointType.RoomBonus) return true;
            else return was_connected_to_another;
        }

        /// <summary>Создаёт тоннель (ячейки) между указанными ячейками (ячейки должны находится на одной оси)</summary>
        /// <param name="cell_1_location">Координаты первой ячейки</param>
        /// <param name="cell_2_location">Координаты второй ячейки</param>
        private void CreateTunnelBetweenTwoCells(Point cell_1_location, Point cell_2_location)
        {
            List<Point> created_blocks = new List<Point>();

            int step_x;
            if (cell_1_location.X > cell_2_location.X) step_x = -1;
            else step_x = 1;

            int step_y;
            if (cell_1_location.Y > cell_2_location.Y) step_y = -1;
            else step_y = 1;

            int steps_number;
            if (cell_1_location.X - cell_2_location.X == 0) // если коридоры соединены вертикально
            {
                steps_number = Math.Abs(cell_1_location.Y - cell_2_location.Y);
                for (int i = 0; i < steps_number; i++)
                {
                    created_blocks.Add(new Point(cell_1_location.X, cell_1_location.Y + i * step_y));
                }
            }
            else // если коридоры соединены горизонтально
            {
                steps_number = Math.Abs(cell_1_location.X - cell_2_location.X);
                for (int i = 0; i < steps_number; i++)
                {
                    created_blocks.Add(new Point(cell_1_location.X + i * step_x, cell_1_location.Y));
                }
            }

            for (int i = 0; i < created_blocks.Count; i++)
            {
                //if (!m_is_cells_reserved[created_blocks[i].Y, created_blocks[i].X]) // если ячейка не занята комнатой
                if (m_cells[created_blocks[i].Y, created_blocks[i].X] == DungeonMapCell.Nothing ||
                    m_cells[created_blocks[i].Y, created_blocks[i].X] == DungeonMapCell.Wall ||
                    m_cells[created_blocks[i].Y, created_blocks[i].X] == DungeonMapCell.WallDark ||
                    m_cells[created_blocks[i].Y, created_blocks[i].X] == DungeonMapCell.WallDarkDark)
                {
                    SetCell(created_blocks[i], DungeonMapCell.Floor);
                }
            }
        }

        /// <summary>Возвращает координаты точки соединения, находящейся вне области видимости</summary>
        /// <param name="global_location"></param>
        /// <param name="showing_size"></param>
        /// <returns></returns>
        private Point GetConnectionPointLocationNotInShowingSizeFromGlobalLocation(Point global_location, Size showing_size)
        {
            int y = m_world_key.Next(0, m_connection_points_number_in_line);
            int x = m_world_key.Next(0, m_connection_points_number_in_line);

            if (m_connection_points[y, x] == DungeonMapConnectionPointType.RoomUsusal)
            {
                Point room_location = GetCellLocationRoomLeftUpCornerFromConnectionPoint(new Point(x, y), m_rooms_lengths[y, x]);
                room_location = new Point(room_location.X * block_length + block_length / 2, room_location.Y * block_length + block_length / 2);
                Rectangle rec2 = new Rectangle(room_location, new Size(m_rooms_lengths[y, x] * block_length, m_rooms_lengths[y, x] * block_length));
                Point left_up = new Point(global_location.X - showing_size.Width / 2, global_location.Y - showing_size.Height / 2);
                Rectangle rec = new Rectangle(left_up, showing_size);
                if (!rec.IntersectsWith(rec2))
                {
                    return new Point(x, y);
                }
            }

            return new Point(-1, -1);
        }

        /// <summary>Создаёт монстра в точке соединения (если в точке соединения расположена обычная комната)</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <returns>Монстр (равен null, если не удалось создать)</returns>
        private DungeonMonster CreateMonsterInConnectionPoint(Point connection_point_location)
        {
            if (m_connection_points[connection_point_location.Y, connection_point_location.X] == DungeonMapConnectionPointType.RoomUsusal)
            {
                int i;
                int i2;
                do
                {
                    i = m_world_key.Next(0, 4);
                    i2 = m_world_key.Next(0, 4);
                } while (i == 1 && i2 == 1); // в центре комнаты монстр спавниться не должен, так как на этом месте может находится сундук или блок

                Point room_center_location = GetCellLocationRoomCenterFromConnectionPointLocation(connection_point_location);
                Point spawn_location = new Point(room_center_location.X - 1 + i2, room_center_location.Y - 1 + i);

                double block_x = spawn_location.X * block_length + block_length / 2;
                double block_y = spawn_location.Y * block_length + block_length / 2;
                int texture_id = m_world_key.Next(1, 6);
                Bitmap texture = Properties.Resources.TEXTURE_monster_1;
                if (texture_id == 2) texture = Properties.Resources.TEXTURE_monster_2;
                else if (texture_id == 3) texture = Properties.Resources.TEXTURE_monster_3;
                else if (texture_id == 4) texture = Properties.Resources.TEXTURE_monster_4;
                else if (texture_id == 5) texture = Properties.Resources.TEXTURE_monster_5;
                DungeonMonster monster = new DungeonMonster(texture);
                monster.PlusExp((m_level_id * m_world_key.Next(0, 11 - m_chance_procent / 10)) * 10);
                monster.DistributeAllSkillPoints(ref m_world_key);
                monster.FullStats();
                monster.SetLocation(block_x, block_y);
                EquipMonster(monster);
                return monster;
            }
            return null;
        }

        /// <summary>Создаёт монстра вне области видимости, центр которой расположен в указанных координатах</summary>
        /// <param name="global_location">Координаты центра области видимости</param>
        /// <param name="showing_size">Размер области видимости</param>
        /// <returns>Монстр (равен null, если не удалось создать)</returns>
        private DungeonMonster CreateMonsterFromGlobalLocation(Point global_location, Size showing_size)
        {
            Point monster_spawn_connection_point_location = GetConnectionPointLocationNotInShowingSizeFromGlobalLocation(global_location, showing_size);
            if (monster_spawn_connection_point_location == new Point(-1, -1)) return null;
            return CreateMonsterInConnectionPoint(monster_spawn_connection_point_location);
        }

        /// <summary>Возвращает координаты центра случайной ячейки пола в комнате, расположенной в указанной точке соединения</summary>
        /// <param name="connection_point_location">Координаты точки соединения</param>
        /// <returns>Координаты центра ячейки</returns>
        public Point GetGlobalLocationRandomCellInConnectionPointNotWall(Point connection_point_location)
        {
            Point location = m_rooms_locations[connection_point_location.Y, connection_point_location.X];
            int length = m_rooms_lengths[connection_point_location.Y, connection_point_location.X];

            Point cell_location;
            do
            {
                Random random = new Random();

                int x = location.X + random.Next(0, length);
                int y = location.Y + random.Next(0, length);

                cell_location = new Point(x, y);
            } while (!IsCellContainsFloor(cell_location.X, cell_location.Y));

            return GetGlobalLocationFromCellLocation(cell_location);
        }

        /// <summary>Спавнит монстров (если босс уровня жив)</summary>
        public void TryToSpawnMonsters()
        {
            if (IsBossAlive)
            {
                if (Creatures.Count < m_max_monsters_on_level)
                {
                    m_timer_monsters_spawn.Start();
                    m_timer_monsters_spawn_is_working = true;
                }
            }
        }

        /// <summary>Собыитие для таймера респавна монстров</summary>
        private void m_timer_monsters_spawn_Tick(object sender, EventArgs e)
        {
            if (IsBossAlive)
            {
                if (Creatures.Count < m_max_monsters_on_level)
                {
                    DungeonMonster monster = CreateMonsterFromGlobalLocation(Form.Hero.Location.Point, MainForm.ShowingSize);
                    if (monster != null)
                    {
                        Add(monster);
                    }
                    return;
                }
            }
            m_timer_monsters_spawn.Stop();
            m_timer_monsters_spawn_is_working = false;
        }

        /// <summary>Событие для таймера смены действий монстров</summary>
        private void m_timer_monsters_change_action_Tick(object sender, EventArgs e)
        {
            DungeonHero hero = Form.Hero;

            for (int i = 0; i < Creatures.Count; i++)
            {
                if (Creatures[i] is DungeonMonster)
                {
                    DungeonMonster monster = (Creatures[i] as DungeonMonster);
                    if (monster.IsInPlayerView(hero, new Size(MainForm.ShowingSize.Width + 100, MainForm.ShowingSize.Height + 100)))
                    {
                        Size monster_showing_size;
                        if (monster.ActionStatus == DungeoMonsterActionStatus.Fighting)
                        {
                            monster_showing_size = new Size(MainForm.ShowingSize.Width, MainForm.ShowingSize.Height);
                        }
                        else
                        {
                            monster_showing_size = new Size(MainForm.ShowingSize.Width / 3, MainForm.ShowingSize.Height / 3);
                        }
                        Point monster_location = new Point(monster.Location.Point.X - monster_showing_size.Width / 2, monster.Location.Point.Y - monster_showing_size.Height / 2);
                        Rectangle monster_showing_size_rectangle = new Rectangle(monster_location, monster_showing_size);
                        if (hero.ObjectStatus == DungeonObjectStatus.AddedNotDestroyed && monster_showing_size_rectangle.Contains(hero.Location.Point)) // если игрок в зоне видимости монстра
                        {
                            if (monster.IsAttachedToRoom) // если монстр привязан к комнате
                            {
                                if (monster.ConnectionPointAttached ==
                                    GetConnectionPointLocationFromGlobalLocation(hero.Location.Point)) // если игрок в комнате, к которой привязан монстр
                                {
                                    monster.StartFighting();
                                }
                                else
                                {
                                    if (monster.ActionStatus == DungeoMonsterActionStatus.Fighting) // если раньше монстр сражался
                                    {
                                        monster.StartThinking();
                                    }
                                }
                            }
                            else
                            {
                                monster.StartFighting();
                            }

                            if (monster.ActionStatus == DungeoMonsterActionStatus.Fighting)
                            {
                                if (Creatures[i].Location.GetLengthTo(hero.Location) <= 70) // если игрок рядом на уровне удара
                                {
                                    Creatures[i].Hit();
                                }
                                monster.SetTargetLocation(hero.Location.Point);
                            }
                        }
                        else // если игрока нет в поле зрения
                        {
                            if (monster.ActionStatus == DungeoMonsterActionStatus.Fighting) // если раньше монстр сражался
                            {
                                monster.StartThinking();
                            }
                            else
                            {
                                monster.PickUpItem();
                            }
                        }
                    }
                    else if (monster.IsInPlayerView(hero, new Size(MainForm.ShowingSize.Width * 2, MainForm.ShowingSize.Height * 2)))
                    {
                        if (monster.ActionStatus == DungeoMonsterActionStatus.AFK)
                        {
                            monster.StartThinking();
                        }
                    }
                    else
                    {
                        monster.StopThinking();
                    }
                }
            }
        }

        /// <summary>Рисует все объекты на уровне</summary>
        /// <param name="player_hero">Персонаж игрока (находится в центре экрана)</param>
        /// <param name="showing_size">Размер области видимости</param>
        public void Paint(object sender, PaintEventArgs e, DungeonHero player_hero, Size showing_size)
        {
            Point player_location = player_hero.Location.Point;

            // рисуем блоки
            for (int i = 0; i < m_blocks.Count; i++)
            {
                m_blocks[i].Paint(sender, e, player_location, showing_size);
            }

            // рисуем двери
            for (int i = 0; i < m_doors.Count; i++)
            {
                m_doors[i].Paint(sender, e, player_location, showing_size);
            }

            // рисуем предметы
            List<DungeonItem> container_items = m_container.Items;
            container_items.Sort(new PositionYComparer());
            for (int i = 0; i < container_items.Count; i++)
            {
                container_items[i].Paint(sender, e, player_location, showing_size);
            }

            // рисуем созданий и сундуки вместе, так как создание может находится как за сундуком, так и перед ним
            List<DungeonObject> chests_and_creatures = new List<DungeonObject>();
            for (int i = 0; i < Creatures.Count; i++) chests_and_creatures.Add(Creatures[i]);
            for (int i = 0; i < m_chests.Count; i++) chests_and_creatures.Add(m_chests[i]);
            chests_and_creatures.Sort(new PositionYComparer()); // сортировка списка объектов по их Y-координатам (чтобы объект, расположенный перед другим, рисовался соответственно перед ним, а не за)
            for (int i = 0; i < chests_and_creatures.Count; i++)
            {
                if (chests_and_creatures[i] is DungeonCreature)
                {
                    bool is_in_view = (chests_and_creatures[i] as DungeonCreature).Paint(sender, e, player_location, showing_size);

                    if (is_in_view) // если существо успешно отрисовано
                    {
                        // рисовка HP и имён монстров
                        if (chests_and_creatures[i] is DungeonMonster) // если существо - монстр
                        {
                            Point location;
                            Size size;
                            Rectangle rec;

                            const int between = 5;

                            // фон для здоровья
                            size = new Size(100, 10);
                            location = new DungeonPoint((chests_and_creatures[i] as DungeonCreature).Location.X - player_location.X + (sender as MainForm).Width / 2 - size.Width / 2, (chests_and_creatures[i] as DungeonCreature).Location.Y - player_location.Y + (sender as MainForm).Height / 2 - (chests_and_creatures[i] as DungeonCreature).Size.Height - size.Height - 10).Point;
                            rec = new Rectangle(location, size);
                            (sender as MainForm).DrawRectagle(e, rec, Brushes.Black);

                            // само здоровье
                            size = new Size((int)(size.Width * ((chests_and_creatures[i] as DungeonCreature).TotalHealth / (chests_and_creatures[i] as DungeonCreature).GetStatValueWithEffectValue(DungeonStats.MaxHealth))), size.Height);
                            rec = new Rectangle(location, size);
                            (sender as MainForm).DrawRectagle(e, rec, Brushes.Black, Brushes.Red, 1);

                            // краткое описание существа
                            size = new Size(400, 20);
                            location = new Point(location.X - 150, location.Y - size.Height - between);
                            rec = new Rectangle(location, size);
                            (sender as MainForm).DrawText(e, rec, Brushes.White, (chests_and_creatures[i] as DungeonCreature).Name + " (" + (chests_and_creatures[i] as DungeonCreature).Level + " LVL): " + (int)((chests_and_creatures[i] as DungeonCreature).TotalHealth) + " HP");
                        }

                    }
                }
                else chests_and_creatures[i].Paint(sender, e, player_location, showing_size);
            }

            // рисуем эффекты
            for (int i = 0; i < m_graphic_effects.Count; i++)
            {
                m_graphic_effects[i].Paint(sender, e, player_location, showing_size);
            }
        }

        /// <summary>Проверяет открыта ли указанная ячейка/summary>
        /// <param name="cell_location">Координаты ячейки</param>
        /// <returns>True - если ячейка открыта, false - если нет</returns>
        private bool IsCellFinded(Point cell_location)
        {
            if (cell_location.Y >= 0 &&
                cell_location.Y <= CellsNumberInLine - 1 &&
                cell_location.X >= 0 &&
                cell_location.X <= CellsNumberInLine - 1)
            {
                if (m_is_cell_finded[cell_location.Y, cell_location.X]) return true;
            }
            return false;
        }

        /// <summary>Открывает ячейку</summary>
        /// <param name="cell_location">Координаты ячейки</param>
        private void FindCell(Point cell_location)
        {
            if (cell_location.Y >= 0 &&
                cell_location.Y <= CellsNumberInLine - 1 &&
                cell_location.X >= 0 &&
                cell_location.X <= CellsNumberInLine - 1)
            {
                m_is_cell_finded[cell_location.Y, cell_location.X] = true;
            }
        }

        /// <summary>Открывает все ячейки в области видимости вокруг существа</summary>
        /// <param name="creature">Существо</param>
        /// <param name="showing_size">Область видимости</param>
        public void FindCells(DungeonCreature creature, Size showing_size)
        {
            showing_size = new Size(showing_size.Width / block_length + showing_size.Width % 2, showing_size.Height / block_length + showing_size.Width % 2);
            for (int i = 0; i < showing_size.Height; i++)
            {
                for (int i2 = 0; i2 < showing_size.Width; i2++)
                {
                    Point cell_location = new Point(creature.Location.Point.X / block_length - showing_size.Width / 2 + i2, creature.Location.Point.Y / block_length - showing_size.Height / 2 + i);
                    FindCell(cell_location);
                }
            }
        }

        /// <summary>Возвращает тип указанной ячейки</summary>
        /// <param name="cell_location">Координаты ячейки</param>
        /// <returns>Тип ячейки</returns>
        public DungeonMapCell GetCell(Point cell_location)
        {
            DungeonMapCell map_cell = DungeonMapCell.Nothing;
            if (cell_location.Y >= 0 &&
                cell_location.Y <= CellsNumberInLine - 1 &&
                cell_location.X >= 0 &&
                cell_location.X <= CellsNumberInLine - 1)
            {
                if (IsCellFinded(cell_location))
                {
                    map_cell = m_cells[cell_location.Y, cell_location.X];
                }
                else map_cell = DungeonMapCell.Nothing;
            }
            return map_cell;
        }

        /// <summary>Возвращает тип ячейки расположенной в смещении от заданного существа/summary>
        /// <param name="creature">Существо</param>
        /// <param name="offset_x">Смещение (в ячейках по оси X)</param>
        /// <param name="offset_y">Смещение (в ячейках по оси Y)</param>
        /// <returns>Тип ячейки</returns>
        public DungeonMapCell GetCellWhereIsCreature(DungeonCreature creature, int offset_x = 0, int offset_y = 0)
        {
            Point cell_location = new Point(creature.Location.Point.X / block_length + offset_x, creature.Location.Point.Y / block_length + offset_y);
            return GetCell(cell_location);
        }

        /// <summary>Возобновляет таймер смены действий монстров</summary>
        public void ResumeMonstersActions()
        {
            if (!m_timer_monsters_change_action_is_working)
            {
                m_timer_monsters_change_action.Start();
                m_timer_monsters_change_action_is_working = true;
            }
        }

        /// <summary>Приостанавливает таймер смены действий монстров</summary>
        public void PauseMonstersActions()
        {
            if (m_timer_monsters_change_action_is_working)
            {
                m_timer_monsters_change_action.Stop();
                m_timer_monsters_change_action_is_working = false;
            }
        }

        /// <summary>Возвращает кисть, соответствующую типу указанной ячейки</summary>
        /// <param name="map_cell">Координаты ячейки</param>
        /// <returns>Кисть</returns>
        public static Brush GetBrushForMapCell(DungeonMapCell map_cell)
        {
            Brush brush = Brushes.Black;
            if (map_cell == DungeonMapCell.Floor ||
                map_cell == DungeonMapCell.FloorAndMonster ||
                map_cell == DungeonMapCell.FloorAndMonsterBoss) brush = Brushes.White;
            else if (map_cell == DungeonMapCell.Wall) brush = Brushes.LightGray;
            else if (map_cell == DungeonMapCell.WallDark) brush = Brushes.DarkGray;
            else if (map_cell == DungeonMapCell.WallDarkDark) brush = Brushes.Gray;
            else if (map_cell == DungeonMapCell.EntranceLadderUp ||
                map_cell == DungeonMapCell.EntranceLadderDown ||
                map_cell == DungeonMapCell.EntranceLadderLeft ||
                map_cell == DungeonMapCell.EntranceLadderRight ||
                map_cell == DungeonMapCell.ExitLadderUp ||
                map_cell == DungeonMapCell.ExitLadderDown ||
                map_cell == DungeonMapCell.ExitLadderLeft ||
                map_cell == DungeonMapCell.ExitLadderRight) brush = Brushes.Brown;
            else if (map_cell == DungeonMapCell.FloorAndChest ||
                     map_cell == DungeonMapCell.FloorAndChestBonus) brush = Brushes.Yellow;
            else if (map_cell == DungeonMapCell.FloorAndDoorVertical ||
                     map_cell == DungeonMapCell.FloorAndDoorHorizontal ||
                     map_cell == DungeonMapCell.FloorAndDoorExitVertical ||
                     map_cell == DungeonMapCell.FloorAndDoorExitHorizontal ||
                     map_cell == DungeonMapCell.FloorAndDoorBonusVertical ||
                     map_cell == DungeonMapCell.FloorAndDoorBonusHorizontal) brush = Brushes.Orange;
            return brush;
        }
    }

    /// <summary>
    /// Класс, представляющий метод для сортировки списка объектов подземелья по Y-координате их самой нижней части
    /// </summary>
    public class PositionYComparer : IComparer<DungeonObject>
    {
        /// <summary>Метод для сортировки списка объектов подземелья по Y-координате их самой нижней части</summary>
        /// <param name="obj1">Объект 1</param>
        /// <param name="obj2">Объект 2</param>
        /// <returns>1 - если объект 1 идёт после объекта 2, -1 - если до, 0 - если их позиции равны</returns>
        public int Compare(DungeonObject obj1, DungeonObject obj2)
        {
            double value1 = obj1.Location.Y + obj1.Size.Height / 2;
            double value2 = obj2.Location.Y + obj2.Size.Height / 2;
            if (value1 > value2)
            {
                return 1;
            }
            else if (value1 < value2)
            {
                return -1;
            }
            return 0;
        }
    }
}
