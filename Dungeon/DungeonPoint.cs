using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Точка с вещественными координатами</summary>
    [Serializable]
    public class DungeonPoint
    {
        /// <summary>X-координата</summary>
        public double X;

        /// <summary>Y-координата</summary>
        public double Y;

        /// <summary>Точка с округлёнными до целых чисел координатами</summary>
        public Point Point
        {
            get
            {
                return new Point((int)X, (int)Y);
            }
        }

        /// <summary>Создаёт точку с вещественными координатами</summary>
        /// <param name="_X">X-координата</param>
        /// <param name="_Y">Y-координата</param>
        public DungeonPoint(double _X = 0, double _Y = 0)
        {
            X = _X;
            Y = _Y;
        }

        /// <summary>Возвращает расстояние от этой точки до заданной</summary>
        /// <param name="point">Вторая точка</param>
        /// <returns>Расстояние между точками</returns>
        public double GetLengthTo(DungeonPoint point)
        {
            return Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));
        }
    }
}
