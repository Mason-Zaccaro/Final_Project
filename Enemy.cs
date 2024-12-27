using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    public class Enemy : IGameObject
    {
        public PictureBox PictureBox { get; private set; }
        public PictureBox player;
        public int speed = 5;

        public Enemy(PictureBox player, Point startPosition)
        {
            this.player = player;
            PictureBox = new PictureBox
            {
                Size = new Size(35, 35),
                Location = startPosition,
                BackColor = Color.Blue
            };
        }

        public void Update()
        {
            // Рассчитываем вектор направления к игроку
            int deltaX = player.Left - PictureBox.Left;
            int deltaY = player.Top - PictureBox.Top;

            // Нормализуем вектор направления
            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            if (distance > 0)
            {
                double directionX = deltaX / distance;
                double directionY = deltaY / distance;

                // Двигаемся по направлению к игроку с постоянной скоростью
                PictureBox.Left += (int)(directionX * speed);
                PictureBox.Top += (int)(directionY * speed);
            }
        }

        public void Draw()
        {
            // Отрисовка врага (если нужно)
        }

        public Rectangle CollisionBox => PictureBox.Bounds; // Границы коллизии
    }
}