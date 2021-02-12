namespace Dungeon
{
    /// <summary>Статус формы (что должна показывать)</summary>
    public enum DungeonFormStatus : byte
    {
        /// <summary>Окно главного меню</summary>
        Menu,

        /// <summary>Окно начала игры</summary>
        NewGameStart,

        /// <summary>Окно начала новой игры (выбор сложности)</summary>
        NewGameDifficulty,

        /// <summary>Окно начала новой игры (выбор персонажа)</summary>
        NewGameCharacter,

        /// <summary>Окно начала новой игры (выбор ключа генерации мира)</summary>
        NewGameLevelGenerate,

        /// <summary>Окно загрузки</summary>
        Loading,

        /// <summary>Окно загрузки главной формы</summary>
        MainLoading,

        /// <summary>Окно загрузки (ожидание нажатия клавиши)</summary>
        LoadingReady,

        /// <summary>Окно загрузки главной формы (ожидание нажатия клавиши)</summary>
        MainLoadingReady,

        /// <summary>Окно сохранения игры</summary>
        Saving,

        /// <summary>Окно игры</summary>
        Game,

        /// <summary>Окно результата игры (победа)</summary>
        GameEndWin,

        /// <summary>Окно результата игры (поражение)</summary>
        GameEndLoose,

        /// <summary>Окно игрового меню</summary>
        InGameMenu,

        /// <summary>Окно подтверждения загрузки (через игровое меню)</summary>
        InGameMenuConfirmLoad,

        /// <summary>Окно подтверждения выхода в меню без сохранения (через игровое меню)</summary>
        InGameMenuConfirmExit,

        /// <summary>Окно правил</summary>
        Rules,

        /// <summary>Окно настроек</summary>
        Settings,

        /// <summary>Окно настроек экрана</summary>
        SettingsScreen,

        /// <summary>Окно дополнительных настроек</summary>
        SettingsOther,

        /// <summary>Окно настроек звука</summary>
        SettingsAudio,

        /// <summary>Окно настроек интерфейса</summary>
        SettingsInterface,

        /// <summary>Окно настроек интерфейса</summary>
        SettingsControls,

        /// <summary>Окно подтверждения выхода</summary>
        ConfirmExit,
    }

    /// <summary>Сложность</summary>
    public enum DungeonDifficulty : byte
    {
        /// <summary>Легко</summary>
        Easy,

        /// <summary>Нормально</summary>
        Normal,

        /// <summary>Сложно</summary>
        Hard,

        /// <summary>Хардкор (очень сложно)</summary>
        Hardcore,
    }

    /// <summary>Статус объекта</summary>
    public enum DungeonObjectStatus : byte
    {
        /// <summary>Статус объекта: создан, но не добавлен на уровень</summary>
        CreatedNotAdded,

        /// <summary>Статус объекта: добавлен на уровень, но не уничтожен</summary>
        AddedNotDestroyed,

        /// <summary>Статус объекта: уничтожен</summary>
        Destroyed,
    }

    /// <summary>Тип столкновения с объектом</summary>
    public enum DungeonObjectCollision : byte
    {
        /// <summary>Тип столкновения с объектом: без столкновения</summary>
        NoCollision,

        /// <summary>Тип столкновения с объектом: статическое столкновение (объект не может быть сдвинут существом)</summary>
        StaticCollision,
    }

    /// <summary>Тип изображения двери</summary>
    public enum DungeonDoorImageType : byte
    {
        /// <summary>Вертикальная дверь</summary>
        Horizontal,

        /// <summary>Горизонтальная дверь</summary>
        Vertical,
    }

    /// <summary>Направление движения существа</summary>
    public enum DungeonCreatureMoveDirection : byte
    {
        /// <summary>Вверх</summary>
        Up,

        /// <summary>Вниз</summary>
        Down,

        /// <summary>Влево</summary>
        Left,

        /// <summary>Вправо</summary>
        Right,
    }

    /// <summary>Направление движения объекта</summary>
    public enum DungeonObjectMoveDirection : byte
    {
        /// <summary>Вверх</summary>
        Up,

        /// <summary>Вниз</summary>
        Down,

        /// <summary>Влево</summary>
        Left,

        /// <summary>Вправо</summary>
        Right,

        /// <summary>Вверх-влево</summary>
        UpLeft,

        /// <summary>Вверх-вправо</summary>
        UpRight,

        /// <summary>Вниз-вправо</summary>
        DownRight,

        /// <summary>Вниз-влево</summary>
        DownLeft,
    }

    /// <summary>Текущее отображение инвентаря</summary>
    public enum DungeonInventoryStatus : byte
    {
        /// <summary>Инвентарь не открыт</summary>
        None,

        /// <summary>Инвентарь открыт в режиме инвентаря</summary>
        Inventory,

        /// <summary>Инвентарь открыт в режиме прокачки</summary>
        Statistic,

        /// <summary>Инвентарь открыт в режиме карты</summary>
        Map,
    }

    /// <summary>Характеристики существа</summary>
    public enum DungeonStats : byte
    {
        /// <summary>Максимальное здоровье</summary>
        MaxHealth,

        /// <summary>Максимальная энергия</summary>
        MaxEnergy,

        /// <summary>Интеллект</summary>
        Intelligence,

        /// <summary>Регенерация (здоровья)</summary>
        Regeneration,

        /// <summary>Восстановление (энергии)</summary>
        Restore,

        /// <summary>Скорость</summary>
        Speed,

        /// <summary>Сила</summary>
        Power,

        /// <summary>Ловкость</summary>
        Mobility,

        /// <summary>Удача</summary>
        Luck,
    }

    /// <summary>Тип ячейки карты</summary>
    public enum DungeonMapCell : byte
    {
        /// <summary>Ячейка ничего не содержит</summary>
        Nothing,

        /// <summary>Ячейка содержит пол</summary>
        Floor,

        /// <summary>Ячейка содержит стену</summary>
        Wall,

        /// <summary>Ячейка содержит стену (тёмную)</summary>
        WallDark,

        /// <summary>Ячейка содержит стену (очень тёмную)</summary>
        WallDarkDark,

        /// <summary>Ячейка содержит лестницу, идущую вверх</summary>
        EntranceLadderUp,

        /// <summary>Ячейка содержит лестницу, идущую вниз</summary>
        EntranceLadderDown,

        /// <summary>Ячейка содержит лестницу, идущую влево</summary>
        EntranceLadderLeft,

        /// <summary>Ячейка содержит лестницу, идущую вправо</summary>
        EntranceLadderRight,

        /// <summary>Ячейка содержит лестницу, идущую сверху</summary>
        ExitLadderUp,

        /// <summary>Ячейка содержит лестницу, идущую снизу</summary>
        ExitLadderDown,

        /// <summary>Ячейка содержит лестницу, идущую слева</summary>
        ExitLadderLeft,

        /// <summary>Ячейка содержит лестницу, идущую справа</summary>
        ExitLadderRight,

        /// <summary>Ячейка содержит пол и сундук</summary>
        FloorAndChest,

        /// <summary>Ячейка содержит пол и сундук</summary>
        FloorAndChestBonus,

        /// <summary>Ячейка содержит пол и вертикальную дверь</summary>
        FloorAndDoorVertical,

        /// <summary>Ячейка содержит пол и вертикальную дверь, ведущую на следующий уровень</summary>
        FloorAndDoorExitVertical,

        /// <summary>Ячейка содержит пол и вертикальную дверь, ведущую в бонусную комнату</summary>
        FloorAndDoorBonusVertical,

        /// <summary>Ячейка содержит пол и горизонтальную дверь</summary>
        FloorAndDoorHorizontal,

        /// <summary>Ячейка содержит пол и горизонтальную дверь, ведущую на следующий уровень</summary>
        FloorAndDoorExitHorizontal,

        /// <summary>Ячейка содержит пол и горизонтальную дверь, ведущую в бонусную комнату</summary>
        FloorAndDoorBonusHorizontal,

        /// <summary>Ячейка содержит пол и монстра</summary>
        FloorAndMonster,

        /// <summary>Ячейка содержит пол и босса</summary>
        FloorAndMonsterBoss,
    }

    /// <summary>Тип лестницы</summary>
    public enum DungeonLadderType : byte
    {
        /// <summary>Лестница идёт вверх</summary>
        Up,

        /// <summary>Лестница идёт вниз</summary>
        Down,

        /// <summary>Лестница идёт влево</summary>
        Left,

        /// <summary>Лестница идёт вправо</summary>
        Right,

        /// <summary>Нет лестницы</summary>
        NoLadder,
    }

    /// <summary>Тип точки соединения карты</summary>
    public enum DungeonMapConnectionPointType : byte
    {
        /// <summary>Нет точки соединения</summary>
        None,

        /// <summary>Пустая точка соединения</summary>
        Corner,

        /// <summary>В точке соединения находится обычная комната</summary>
        RoomUsusal,

        /// <summary>В точке соединения находится комната входа на уровень</summary>
        RoomEntrance,

        /// <summary>В точке соединения находится комната выхода с уровеня</summary>
        RoomExit,

        /// <summary>В точке соединения находится комната с боссом</summary>
        RoomBoss,

        /// <summary>В точке соединения находится бонусная комната</summary>
        RoomBonus,
    }

    /// <summary>Тип комнаты</summary>
    public enum DungeonRoomType : byte
    {
        /// <summary>Обычная комната</summary>
        Usual,

        /// <summary>Комната с лестницей</summary>
        Ladder,

        /// <summary>Комната с боссом</summary>
        Boss,

        /// <summary>Бонусная комната</summary>
        Bonus,
    }

    /// <summary>Тип задания характеристики при создании предмета</summary>
    public enum SetStatType : byte
    {
        /// <summary>Нельзя установить</summary>
        NotSet,

        /// <summary>Можно установить либо положительную, либо отрицательную</summary>
        CanSet,

        /// <summary>Можно установить только положительную</summary>
        CanSetPlus,

        /// <summary>Обязательно установить положительную</summary>
        MustSetPlus,

        /// <summary>Можно установить только отрицательную</summary>
        CanSetMinus,

        /// <summary>Обязательно установить отрицательную</summary>
        MustSetMinus,
    }

    /// <summary>Тип двери</summary>
    public enum DungeonDoorType : byte
    {
        /// <summary>Обычная дверь</summary>
        Usual,

        /// <summary>Дверь, за которой находится переход на следующий уровень</summary>
        Exit,

        /// <summary>Дверь, за которой находится бонусная комната</summary>
        Bonus,
    }

    /// <summary>Текущее действие монстра</summary>
    public enum DungeoMonsterActionStatus : byte
    {
        /// <summary>Нет действия - монстр вне области игрока</summary>
        AFK,

        /// <summary>Думает</summary>
        Thinking,

        /// <summary>Двигается к ячейке (мирно)</summary>
        MovingToCell,

        /// <summary>Сражается с героем</summary>
        Fighting,
    }
}