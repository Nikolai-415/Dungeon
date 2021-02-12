using System.Drawing;

namespace Dungeon
{
    /// <summary>Класс, предоставляющий информацию о характеристиках существа</summary>
    public abstract class DungeonStatsInfo
    {
        /// <summary>Название характеристики</summary>
        /// <param name="stat">Характеристика</param>
        /// <returns>Текст</returns>
        public static string Name(DungeonStats stat)
        {
            if (stat == DungeonStats.MaxHealth) return "Макс. здоровье";
            else if (stat == DungeonStats.MaxEnergy) return "Макс. энергия";
            else if (stat == DungeonStats.Intelligence) return "Интеллект";
            else if (stat == DungeonStats.Regeneration) return "Регенерация";
            else if (stat == DungeonStats.Restore) return "Восстановление";
            else if (stat == DungeonStats.Speed) return "Скорость";
            else if (stat == DungeonStats.Power) return "Сила";
            else if (stat == DungeonStats.Mobility) return "Ловкость";
            else if (stat == DungeonStats.Luck) return "Удача";
            return "error";
        }

        /// <summary>Максимальное значение характеристики при прокачке</summary>
        /// <param name="stat">Характеристика</param>
        /// <returns>Значение</returns>
        public static double Max(DungeonStats stat)
        {
            if (stat == DungeonStats.MaxHealth) return 250;
            else if (stat == DungeonStats.MaxEnergy) return 250;
            else if (stat == DungeonStats.Intelligence) return 250;
            else if (stat == DungeonStats.Regeneration) return 5;
            else if (stat == DungeonStats.Restore) return 5;
            else if (stat == DungeonStats.Speed) return 5;
            else if (stat == DungeonStats.Power) return 50;
            else if (stat == DungeonStats.Mobility) return 50;
            else if (stat == DungeonStats.Luck) return 50;
            return -1;
        }

        /// <summary>Значение, на которое увеличивается характеристика при прокачке</summary>
        /// <param name="stat">Характеристика</param>
        /// <returns>Значение</returns>
        public static double Plus(DungeonStats stat)
        {
            if (stat == DungeonStats.MaxHealth) return 25;
            else if (stat == DungeonStats.MaxEnergy) return 25;
            else if (stat == DungeonStats.Intelligence) return 25;
            else if (stat == DungeonStats.Regeneration) return 0.5;
            else if (stat == DungeonStats.Restore) return 0.5;
            else if (stat == DungeonStats.Speed) return 0.5;
            else if (stat == DungeonStats.Power) return 5;
            else if (stat == DungeonStats.Mobility) return 5;
            else if (stat == DungeonStats.Luck) return 5;
            return -1;
        }

        /// <summary>Цвет, соответствующий характеристике</summary>
        /// <param name="stat">Характеристика</param>
        /// <returns>Цвет</returns>
        public static Color GetColor(DungeonStats stat)
        {
            if (stat == DungeonStats.MaxHealth) return Color.FromArgb(255, 0, 0); // красный
            else if (stat == DungeonStats.MaxEnergy) return Color.FromArgb(0, 0, 255); // синий
            else if (stat == DungeonStats.Intelligence) return Color.FromArgb(255, 165, 0); // оранжевый
            else if (stat == DungeonStats.Regeneration) return Color.FromArgb(255, 76, 91); // светло-красный
            else if (stat == DungeonStats.Restore) return Color.FromArgb(154, 206, 235); // светло-синий
            else if (stat == DungeonStats.Speed) return Color.FromArgb(0, 255, 0); // зелёный
            else if (stat == DungeonStats.Power) return Color.FromArgb(255, 255, 0); // жёлтый
            else if (stat == DungeonStats.Mobility) return Color.FromArgb(139, 0, 255); // фиолетовый
            else if (stat == DungeonStats.Luck) return Color.FromArgb(255, 192, 203); // розовый
            return Color.FromArgb(50, 50, 50); // серый
        }
    }
}
