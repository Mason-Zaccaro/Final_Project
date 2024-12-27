using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public class Trap : IGameObject
    {
        public PictureBox TrapPictureBox { get; private set; }

        public Trap(Point position)
        {
            TrapPictureBox = new PictureBox
            {
                Size = new Size(30, 30),
                Location = position,
                BackColor = Color.Red
            };
        }

        public void Update()
        {
            // Ловушка не двигается, поэтому метод пуст
        }

        public void Draw()
        {
            // Отрисовка ловушки (если нужно)
        }

        public Rectangle CollisionBox => TrapPictureBox.Bounds; // Границы коллизии
    }
}