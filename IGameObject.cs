using System.Drawing;

namespace MyGame
{
    public interface IGameObject
    {
        void Update(); // Метод для обновления состояния объекта
        void Draw();   // Метод для отрисовки объекта
        Rectangle CollisionBox { get; } // Свойство для получения границ коллизии
    }
}