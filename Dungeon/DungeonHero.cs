using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Герой</summary>
    [Serializable]
    public class DungeonHero : DungeonCreature
    {
        /// <summary>Текущее время прохождения игры</summary>
        public int WalkthrowTime;

        /// <summary>Текущая пройденная дистанция</summary>
        public double DistanceWalked;

        /// <summary>Текущее количество убитых монстров</summary>
        public int MonstersKilled;

        /// <summary>Текущее количество убитых боссов</summary>
        public int MonstersBossesKilled;

        /// <summary>Текущее количество открытых дверей</summary>
        public int DoorsOpened;

        /// <summary>Текущее количество открытых дверей с кодовым замком</summary>
        public int DoorsCodeOpened;

        /// <summary>Текущее количество открытых сундуков</summary>
        public int ChestsOpened;

        /// <summary>Создаёт героя</summary>
        /// <param name="texture">Текстура героя</param>
        /// <param name="name">Имя героя</param>
        /// <param name="max_health">Здоровье героя</param>
        /// <param name="regeneration">Регенерация (восстановление здоровья) героя</param>
        /// <param name="max_energy">Энергия героя</param>
        /// <param name="restore">Восстановление (возобновление энергии) героя</param>
        /// <param name="power">Сила героя</param>
        /// <param name="mobility">Ловкость героя</param>
        /// <param name="speed">Скорость героя</param>
        /// <param name="intelligence">Интеллект героя</param>
        /// <param name="luck">Удача героя</param>
        public DungeonHero(Bitmap texture, string name = "Герой", double max_health = 100, double regeneration = 0.5, double max_energy = 50, double restore = 0.5, double power = 5, double mobility = 0, double speed = 2.5, double intelligence = 0, double luck = 0)
            : base(texture, name, max_health, regeneration, max_energy, restore, power, mobility, speed, intelligence, luck)
        {
            WalkthrowTime = 0;
            DistanceWalked = 0;
            MonstersKilled = 0;
            MonstersBossesKilled = 0;
            DoorsOpened = 0;
            DoorsCodeOpened = 0;
            ChestsOpened = 0;
        }
    }
}
