using System;
using System.Drawing;

namespace Dungeon
{
    /// <summary>Блок</summary>
    [Serializable]
    public class DungeonBlock : DungeonObject
    {
        /// <summary>Создаёт блок</summary>
        /// <param name="image">Изображение блока</param>
        /// <param name="collision_type">Тип столкновения с блоком</param>
        public DungeonBlock(Bitmap image, DungeonObjectCollision collision_type = DungeonObjectCollision.NoCollision)
            : base(image, collision_type) { }
    }
}
